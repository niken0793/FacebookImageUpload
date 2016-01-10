using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Facebook; // PM
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using FacebookImageUpload.FB_Images;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ECC;

// Huy file

namespace FacebookImageUpload
{

    public partial class Form1 : Form
    {
        public static Random r;

        public static UserSetting ActiveUser;
        public static AppSetting MyAppSetting = new AppSetting();

        private void CheckUserSetting()
        {

            string appPath = Path.Combine(FB_Image.RelativeDirectory,FB_Image.UserSettingDir, FB_Image.AppSettingFile);
            if (File.Exists(appPath))
            {
                AppSetting tempApp = MyHelper.DeSerializeObject<AppSetting>(appPath);
                if (tempApp != null && !String.IsNullOrEmpty(tempApp.ActiveUser))
                {
                    string usrPath = Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserSettingDir, tempApp.ActiveUser);
                    MyAppSetting = tempApp;
                    if (File.Exists(usrPath))
                    {
                        UserSetting a = MyHelper.DeSerializeObject<UserSetting>(Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserSettingDir, tempApp.ActiveUser));
                        if (a != null)
                        {
                            UpdateLoginControl(a);
                            return;
                        }
                    }
                    
                }
            }
          
            LoginFacebook();
            
        }

        private void SaveActiveUserOnDisk(UserSetting active)
        {
             if (active != null)
            {
                MyHelper.SerializeObject(active, Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserSettingDir , active.UserID));
            }
        }
        private void SaveAppSettingOnDisk(AppSetting app)
        {
            if (app != null)
            {
                MyHelper.SerializeObject(app, Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserSettingDir,FB_Image.AppSettingFile));
            }
        }

        private void SaveInboxOnDisk(List<InboxUser> inbox)
        {
            if (inbox != null)
            {
                string savePath = Path.Combine(FB_Image.RelativeDirectory,FB_Image.UserSettingDir,"inbox");
                MyHelper.PreparePath(savePath);
                MyHelper.SerializeObject(inbox, savePath);
            }
        }

        private async void LoadFriendList()
        {
            try
            {
                if (ActiveUser != null && !String.IsNullOrEmpty(ActiveUser.AccessToken))
                {
                    MyHelper.EnableControl(false, btnShowFriendList, btnGetImageList, listViewFriends, listViewMessage, listViewInbox);
                    var progressFriend = new Progress<string>(s =>
                    {

                        MyHelper.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing);
                        UpdateFriendListView(this.listViewInbox, ListInboxUser, true);
                        UpdateFriendListView(listViewFriends, ListInboxUser);


                    });

                    await Task.Factory.StartNew(() =>
                    {
                        GetFriendList(progressFriend);

                    }, TaskCreationOptions.LongRunning);



                    MyHelper.EnableControl(true, btnShowFriendList, btnGetImageList, listViewFriends, listViewMessage, listViewInbox);
                    MyHelper.ResetStatusTrip(pbStatus, lbStatusBar, lbDoing);
                }
            }
            catch (Exception e)
            {
                Log(e);
                MyHelper.EnableControl(true, btnShowFriendList, btnGetImageList, listViewFriends, listViewMessage, listViewInbox);
            }

        }

        private async void LoadMessage()
        {

            try
            {
                if (ActiveUser != null && !String.IsNullOrEmpty(ActiveUser.AccessToken))
                {
                    MyHelper.EnableControl(false, btnGetImageList, listViewMessage, listViewInbox);
                    var progressMessage = new Progress<string>(s =>
                    {
                        UpdateFriendListView(this.listViewInbox, ListInboxUser, true);
                        MyHelper.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing);


                    });

                    await Task.Factory.StartNew(() =>
                    {

                        GetImageTagged1(progressMessage, ListInboxUser);
                    }, TaskCreationOptions.LongRunning);

                    MyHelper.EnableControl(true, btnGetImageList, listViewMessage, listViewInbox);
                    MyHelper.ResetStatusTrip(pbStatus, lbStatusBar, lbDoing);
                }
            }
            catch (Exception e)
            {
                Form1.Log(e);
                MyHelper.EnableControl(true, btnGetImageList, listViewMessage, listViewInbox);
            }
        }




       public static string UploadFB(string fullPath, string token, string albumID)
        {
            var imgstream = File.OpenRead(fullPath);
            var fb = new FacebookClient(ActiveUser.AccessToken);
            if (String.IsNullOrEmpty(albumID))
            {
                albumID = "me";
            }
            dynamic res = fb.Post(albumID + "/photos", new
            {
                //message = "Image description",
                file = new FacebookMediaStream
                {
                    ContentType = "image/jpeg",
                    FileName = fullPath,
                }.SetValue(imgstream)
            });
            return res.id;
        }

       public static bool DownloadFB(string photoID, string token, string newPath,bool overWrite=false)
        {
            try
            {
                var fb = new FacebookClient(ActiveUser.AccessToken);
                dynamic res = fb.Get(photoID+ "?fields=images");  // query đường dẫn + độ phân giải ảnh
                string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res); // parse response sang json
                dynamic json = JObject.Parse(json_string);
                dynamic imagesJson = json["images"];
                int count = imagesJson.Count;
                int i = 0;
                string source_url = "";

                /* lấy độ phân giải trùng với độ phân giải của ảnh đã up */
                while (i < count && imagesJson[i] != null)
                {
                    int image_height = Int32.Parse((string)imagesJson[i]["height"]);
                    int image_width = Int32.Parse((string)imagesJson[i]["width"]);
                    if (image_height == FB_Image.ImageSize || image_width == FB_Image.ImageSize)
                    {
                        source_url = imagesJson[i]["source"];
                        break;
                    }
                    i++;

                }
                if (i >= count)
                {
                    source_url = imagesJson[0]["source"];
                }
                if (MyHelper.DowloadImageFromLink(source_url, newPath, ImageFormat.Jpeg,overWrite))
                    return true;
                return false;


            }
            catch (Exception e)
            {
                Log(e);
                return false;
            }
        }



        public string ProcessUserImage(string fullPath)
        {
            string minimalFile = ProcessImage.ReSaveFileToMinimal(fullPath, null);
            minimalFile = ProcessImage.FixFailImage(minimalFile);
            return minimalFile;
        }
        public string ProcessUserImage(string fullPath,string token, string albumID)
        {
            string minimalFile = ProcessImage.ReSaveFileToMinimal(fullPath, null,token,albumID);
            minimalFile = ProcessImage.FixFailImage(minimalFile);
            return minimalFile;
        }



        public string TestEncode(IProgress<string> progress,string filename,string inputText,string albumID,bool googleSearch = false)
        {
            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;

                //Reduce size ratio of picture
                if (progress != null)
                    progress.Report("15|Preparing Image");
                string coverImageFileName = ProcessUserImage(filename);
                string messageFile = MyHelper.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                if (progress != null)
                    progress.Report("50|Uploading Image");
                string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName), Path.GetFileName(inputText));
                if (string.IsNullOrEmpty(encodeFile) || cancelSearching)
                {
                    return null;
                }


                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile), null, albumID);
                if (string.IsNullOrEmpty(id) || cancelSearching)
                {
                    return null;
                }
                string downloadFile = MyHelper.AppendFileName(Path.Combine(FB_Image.BaseDirectory, encodeFile), "_download");
                if (!DownloadFB(id, ActiveUser.AccessToken, downloadFile, true))
                    return null;
                if (progress != null)
                    progress.Report("75|Checking");

                //Decode
                string outputText = MyHelper.AppendFileNameNoLimit(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(downloadFile), outputText,null,true);
                if (outputText == null || cancelSearching)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                    progress.Report("100|Finish");
                if (MyHelper.CompareOutputFile(inputText, outputText))
                {

                    if (!googleSearch)
                    {
                        return id;
                    }
                    else
                    {
                        return coverImageFileName;
                    }
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public string SendNoTestImageWithTag(IProgress<string> progress, string filename, string inputText, string albumID,List<string> uids)
        {

            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;

                //Reduce size ratio of picture
                if (progress != null)
                    progress.Report("15|15|Preparing Image");
                string coverImageFileName = MyHelper.CopyFileTo(filename, FB_Image.BaseDirectory);
                coverImageFileName = ProcessUserImage(coverImageFileName);
                string messageFile = MyHelper.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                if (progress != null)
                    progress.Report("50|Uploading Image");
                string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName), Path.GetFileName(inputText));
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }
                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile),uids ,albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }
                if (progress != null)
                    progress.Report("75|Checking");
                string downloadFile = MyHelper.AppendFileName(Path.Combine(FB_Image.BaseDirectory, encodeFile), "_download");
                if (!DownloadFB(id, ActiveUser.AccessToken, downloadFile, true))
                    return null;


                //Decode
                string outputText = MyHelper.AppendFileNameNoLimit(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(downloadFile), outputText,null,true);
                if (outputText == null)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                    progress.Report("100|Finish");
                if (MyHelper.CompareOutputFile(inputText, outputText))
                {           
                       return coverImageFileName;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(id);  // xóa ảnh
                    return null;
                }
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }




        public static string JPHideEncode(string filename,string input,string password = null)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(input) != "")
                {
                    throw new Exception("No need for full path");
                }
                MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, filename));
                MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, input));
               // input=Path.GetFileName(InsertCrc32(input));
                MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, input));
                string enImageName = Path.GetFileNameWithoutExtension(filename)+"_encode"+Path.GetExtension(filename);
                MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, enImageName));
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                string commandArguments =null;
                if (String.IsNullOrEmpty(password))
                {
                    commandArguments  = String.Format("/C jphide_modify \"{0}\" \"{1}\" \"{2}\"", filename, enImageName, input);
                }else{
                    commandArguments = String.Format("/C jphide_pass \"{0}\" \"{1}\" \"{2}\" \"{3}\"", filename, enImageName, input,password);
                }
                 
                //proc.StartInfo.Arguments = "/C jphide_modify " + filename + " " + enImageName + " " + input;
                proc.StartInfo.Arguments = commandArguments;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;

                proc.Start();
                while (!proc.HasExited)
                    ;
                proc.Dispose();
                return enImageName;
              
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }
        public void OpenFolder(string filePath)
        {
            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
        }

        public static string InsertCrc32(string input)
        {

            string input_path = Path.Combine(FB_Image.BaseDirectory, input);
            string hash = Crc32Hash(input_path);
            string crc = "[crc:" + hash + "]";
            if (File.Exists(input_path))
            {
                string currentContent = File.ReadAllText(input_path);
                string newinput = Path.Combine(FB_Image.BaseDirectory, MyHelper.AppendFileNameNoLimit(input, "_" + (r.Next(13377)).ToString()));
                currentContent = crc + currentContent;
                byte[] bContent = AddErrorCorrection(File.ReadAllBytes(currentContent));
                File.WriteAllBytes(newinput, bContent);
                return newinput;
            }
            else
            {
                return null;
            }
        }

        public static List<string> SplitFileIntoPart(string fullPath, int n = 1)
        {
            try
            {
                byte[] bResult = null;
                byte[] bContent = null;
                List<byte[]> bListResult = new List<byte[]>();
                List<string> listFile = new List<string>();
                string id = MyHelper.GetUnixTimesStamp(DateTime.Now).ToString();
                if (n <= 0)
                {

                    string header = string.Format("{0}|00|0",id);
                    bContent = Encoding.UTF8.GetBytes(header + File.ReadAllText(fullPath));
                    bResult = AddErrorCorrection(bContent);
                    string newFile = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath) + "_one.txt");
                    File.WriteAllBytes(Path.Combine(FB_Image.BaseDirectory,newFile), bResult);
                    listFile.Add(newFile);
                    return listFile;

                }
                else
                {
                    bContent = Encoding.UTF8.GetBytes(File.ReadAllText(fullPath));
                    int dataLen = (BLOCK_LENGTH - EC) * n - HEADER_LEN;
                    int numOfFile = bContent.Length % dataLen == 0 ? bContent.Length / dataLen : bContent.Length / dataLen + 1;
                    int index = 0;
                    for (int i = 0; i < numOfFile; i++)
                    {
                        byte[] temp = new byte[dataLen + HEADER_LEN];
                        string offset = i < 10 ? "0" + i.ToString() : i.ToString();
                        string flag = i == numOfFile - 1 ? "0" : "1";
                        string header = String.Format("{0}|{1}|{2}", id, offset, flag);
                        byte[] bHeader = Encoding.UTF8.GetBytes(header);
                        int copyLen = dataLen;
                        Array.Copy(bHeader, 0, temp, 0, HEADER_LEN);
                        if (i == numOfFile - 1 && bContent.Length % dataLen != 0)
                            copyLen = bContent.Length % dataLen;
                        if (i == 0 && bContent.Length < dataLen)
                            copyLen = bContent.Length;
                        Array.Copy(bContent, index, temp, HEADER_LEN, copyLen);
                        index += dataLen;
                        byte[] tempResult = AddErrorCorrection(temp, EC);
                        bListResult.Add(tempResult);

                    }

                }
                if (bListResult.Count > 0)
                {
                    int j = 0;
                    foreach (byte[] item in bListResult)
                    {
                        string newFile = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath) +"_" +j.ToString()+".txt");
                        //File.WriteAllText(newFile, Encoding.UTF8.GetString(item));
                        File.WriteAllBytes(newFile, item);
                        j++;
                        listFile.Add(newFile);
                       
                    }
                    return listFile;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }



        }

        public const int BLOCK_LENGTH = 255;
        public const int EC = 32;
        public const int HEADER_LEN = 15;

        public static byte[] AddErrorCorrection(byte[] content, int ec = EC)
        {
            byte[] b = content;
            int dataLen =b.Length > BLOCK_LENGTH ? BLOCK_LENGTH - ec:b.Length;
            int blockLen = dataLen + ec;
            int numOfBlock = b.Length % dataLen==0 ? b.Length / dataLen : b.Length / dataLen + 1;
            int t = numOfBlock;
            int index = 0;
            List<int[]> arrayList = new List<int[]>();
            for (int i = 0; i < numOfBlock; i++)
            {
                int tempBlockLen = 0;
                int copyLen = dataLen;
                if (numOfBlock == 1)
                {
                    tempBlockLen = blockLen;
                    copyLen = dataLen;
                }
                else if (numOfBlock > 1 && i == numOfBlock - 1 && b.Length % dataLen != 0)
                {
                    tempBlockLen = b.Length % dataLen +ec;
                    copyLen = b.Length % dataLen;
                }
                else
                {
                    tempBlockLen = BLOCK_LENGTH;
                }
                int[] temp = new int[tempBlockLen];
                Array.Copy(b, index, temp, 0, copyLen);
                arrayList.Add(temp);
                index += dataLen;
            }
            ReedSolomonEncoder encoder = new ReedSolomonEncoder(GenericGF.QR_CODE_FIELD_256);
            for (int i = 0; i < numOfBlock; i++)
            {
                encoder.encode(arrayList[i], ec);
            }
            byte[] bContent = ConvertIntListToByteArray(arrayList);
            return bContent;
        }
        public static byte[] ConvertIntListToByteArray(List<int[]> list)
        {
            byte[] result=null;
            int index = 0;
            int resultLength=0;
            if (list.Count > 0)
            {
                foreach (int[] i in list)
                    resultLength += i.Length;
                result = new byte[resultLength];

                for (int i = 0; i < list.Count; i++)
                {
                    byte[] temp = new byte[list[i].Length];
                    CopyInto2Byte(list[i], temp);
                    Array.Copy(temp, 0, result, index, list[i].Length);
                    index += list[i].Length;
                }

            }
            return result;

        }

        public static void CopyInto2Byte(int[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = (byte)a[i];
            }
        }

        public static string JPSeekDecode(string filename, string output,string password=null, bool isSent = false)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(output) != "")
                {
                    throw new Exception("No need for full path");
                }
                string imageName = filename;
                MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, filename));
                string hiddenFileName = output;
                MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, hiddenFileName));
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                string commandArguments = String.Format("/C jpseek_modify \"{0}\" \"{1}\" ", imageName, hiddenFileName);
                if (!String.IsNullOrEmpty(password))
                {
                    commandArguments = String.Format("/C jpseek_passs \"{0}\" \"{1}\" \"{2}\"", imageName, hiddenFileName,password);
                }
                //proc.StartInfo.Arguments = "/C jpseek_modify " + imageName + " " + hiddenFileName;
                proc.StartInfo.Arguments = commandArguments;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                while (!proc.HasExited)
                    ;
                proc.Dispose();
                if(isSent)
                    hiddenFileName = CheckCrc32(hiddenFileName);  
                if (hiddenFileName != null)
                {
                    MyHelper.ListFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, hiddenFileName));
                    return Path.GetFileName(hiddenFileName);
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public static string CheckCrc32(string filename)
        {

            string oldpath = Path.Combine(FB_Image.BaseDirectory, filename);
            string oldfile = MyHelper.AppendFileName(oldpath, "_correction");
            byte[] result = CorrectError(oldpath);
            if (result != null)
            {
                File.WriteAllBytes(oldfile, result);
                return oldfile;
            }
            else
            {
                return null;
            }


        }




        public static byte[] CorrectError(string fileName, int ec=32)
        {
            byte[] bContent = File.ReadAllBytes(fileName);
            int blockLen = bContent.Length < BLOCK_LENGTH ? bContent.Length : BLOCK_LENGTH;
            int numOfBlock = bContent.Length % blockLen == 0 ? bContent.Length / blockLen : bContent.Length / blockLen + 1;
            List<int[]> list = new List<int[]>();
            int index = 0;
            
            for (int i = 0; i < numOfBlock; i++)
            {
                int[] temp = null;
                int tempBlockLen = 0;
                if (numOfBlock > 1 && i == numOfBlock - 1 && bContent.Length % blockLen != 0)
                {
                    tempBlockLen = bContent.Length % blockLen;
                }
                else
                {
                    tempBlockLen = blockLen;
                }
                temp = new int[tempBlockLen];
                Array.Copy(bContent, index, temp, 0, tempBlockLen);
                list.Add(temp);
                index += tempBlockLen;

            }

            if (list.Count > 0)
            {
                ReedSolomonDecoder decoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
                bool flag = true;
                int size=0;
                for (int i = 0; i < list.Count; i++)
                {
                    size += list[i].Length - ec;
                    if (!decoder.decode(list[i], ec))
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                    return null;

                byte[] result = new byte[size];
                index = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    int len = list[i].Length;
                    byte[] temp = new byte[len];
                    CopyInto2Byte(list[i], temp);
                    Array.Copy(temp, 0, result, index, len - ec);
                    index += len - ec;
                }
                return result;


            }
            else
            {
                return null;
            }

        }
        public static string CorrectErrorString(byte[] content, int ec = 32)
        {
            try
            {
                byte[] bContent = content;
                int blockLen = bContent.Length < BLOCK_LENGTH ? bContent.Length : BLOCK_LENGTH;
                int numOfBlock = bContent.Length % blockLen == 0 ? bContent.Length / blockLen : bContent.Length / blockLen + 1;
                List<int[]> list = new List<int[]>();
                int index = 0;

                for (int i = 0; i < numOfBlock; i++)
                {
                    int[] temp = null;
                    int tempBlockLen = 0;
                    if (numOfBlock > 1 && i == numOfBlock - 1 && bContent.Length % blockLen != 0)
                    {
                        tempBlockLen = bContent.Length % blockLen;
                    }
                    else
                    {
                        tempBlockLen = blockLen;
                    }
                    temp = new int[tempBlockLen];
                    Array.Copy(bContent, index, temp, 0, tempBlockLen);
                    list.Add(temp);
                    index += tempBlockLen;

                }

                if (list.Count > 0)
                {
                    ReedSolomonDecoder decoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
                    bool flag = true;
                    int size = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        size += list[i].Length - ec;
                        if (!decoder.decode(list[i], ec))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                        return null;

                    byte[] result = new byte[size];
                    index = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        int len = list[i].Length;
                        byte[] temp = new byte[len];
                        CopyInto2Byte(list[i], temp);
                        Array.Copy(temp, 0, result, index, len - ec);
                        index += len - ec;
                    }
                    return Encoding.UTF8.GetString(result);


                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }

        }








        private void cmbSelectTextType_SelectedIndexChanged_Handle(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            if (c.SelectedIndex == c.Items.IndexOf("From File"))
            {
                btnSelectFile.Enabled = true;
                tbMessagePath.Enabled = true;
                isFromFile = true;
            }
            else if (c.SelectedIndex == c.Items.IndexOf("From Text"))
            {
                btnSelectFile.Enabled = false;
                tbMessagePath.Enabled = false;
                isFromFile = false;
            }

        }

        private void tbInputMessage_TextChanged_Hanlde(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            lbMessageLength.Text = t.Text.Length.ToString();
        }


        InboxUser currentInbox = null;
        ListViewItem currentInboxLVItem = null;
        private void ChangeUserInbox(object sender, EventArgs e)
        {
            ListView lvUser = (ListView)sender;
            if (lvUser.SelectedItems.Count == 1 )
            {
                InboxUser inbox = MyHelper.GetInboxByUserID(lvUser.SelectedItems[0].Name, ListInboxUser);
                UpdateMessageListView(listViewMessage, inbox);
                currentInbox = inbox;
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                currentInboxLVItem = item;
                lbUserNameComm.Text = inbox.UserName;
                lbUserIdComm.Text = item.Name;
                pBoxUserComm.ImageLocation = Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserImageDir, item.Name + ".jpg");

            }
         
            
        }

        private void ChangeUserMessage(object sender, EventArgs e)
        {
            
            ListView lvMessage = (ListView)sender;
            if (lvMessage.SelectedItems.Count ==1 )
            {
                if (currentInbox != null)
                {
                    int i = lvMessage.SelectedItems[0].Index;
                    if (i < currentInbox.Messages.Count)
                    {
                        if (currentInbox.Messages[i].IsFull)
                        {
                            tbInbox.Text = currentInbox.Messages[i].Content;
                        }
                        else
                        {
                            string s = currentInbox.Messages[i].GetContent();
                            if (s != null)
                                tbInbox.Text = s;
                        }
                        if (!currentInbox.Messages[i].IsRead)
                        {
                            currentInbox.Messages[i].IsRead = true;
                            if(currentInbox.CountNew>0)
                                currentInbox.CountNew--;

                            currentInboxLVItem.Text = GetInboxText(currentInbox.UserName, currentInbox.CountNew);
                            UpdateNewMessageLabel(currentInboxLVItem, lbReceiverNewMessage, currentInbox);

                        }
                        
                    }

                }
                

            }
        }
        private void UpdateNewMessageLabel(ListViewItem lvItem, Label label, InboxUser inbox)
        {
            if(lvItem != null)
                lvItem.Text = GetInboxText(inbox.UserName, inbox.CountNew);
            lbReceiverNewMessage.Text = String.Format("{0} in {1}", inbox.CountNew, inbox.Messages.Count);
        }

        private void UpdateMessageListView(ListView listview, InboxUser inbox)
        {
            listview.Clear();
            ImageList il = new ImageList();
            il.TransparentColor = Color.Blue;
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.ImageSize = new Size(50, 50);
            List<FB_Message> messages = inbox.Messages;
            for (int i = 0; i < messages.Count; i++)
            {
                il.Images.Add(Image.FromFile(messages[i].Image.FullPath));
                il.Images.SetKeyName(i, "Message_"+i);
            }
            listview.View = View.LargeIcon;
            listview.LargeImageList = il;
            for (int j = 0; j < messages.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                    item.Text = il.Images.Keys[j].ToString();
                item.Name = j.ToString();
                item.ImageIndex = j;
                listview.Items.Add(item);
            }
            if (listview.Items.Count > 0)
            {
                listview.Items[listview.Items.Count - 1].EnsureVisible();
            }
        }

        private void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = this.getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }


   
        public static void Log(Exception message)
        {
            try
            {
                string error_string = "";
                DateTime date = DateTime.Now;
                String timeStamp = date.ToString("dd-MM-yyyy HH:mm:ss");
                error_string += ("[" + timeStamp + "] " + message.Message + "\r\n");
                error_string += ("[" + timeStamp + "] " + message.TargetSite + "\r\n");
                error_string += ("[" + timeStamp + "] " + message.StackTrace + "\r\n");
                error_string += ("[" + timeStamp + "] " + message.Source + "\r\n");
                error_string += "\r\n";

                string logFile = date.Day + "-" + date.Month + "-" + date.Year + ".txt";
                logFile = Path.Combine(FB_Image.RelativeDirectory, FB_Image.LogDirectory, logFile);
                Directory.CreateDirectory(Path.Combine(FB_Image.RelativeDirectory, FB_Image.LogDirectory));
                bool flag = File.Exists(logFile);
                if (!flag)
                {
                    File.Create(logFile).Dispose();
                }
                using (var des = File.AppendText(logFile))
                {
                    des.Write(error_string);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }


   
}
