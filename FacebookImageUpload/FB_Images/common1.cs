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

// Huy file

namespace FacebookImageUpload
{

    public partial class Form1 : Form
    {
        public static Random r;

        public static UserSetting ActiveUser;

        private void CheckUserSetting()
        {

            if (!Properties.Settings.Default["ActiveUser"].ToString().Equals("no"))
            {
                string usrPath = Path.Combine(FB_Image.RelativeDirectory, "UserSetting/" + Properties.Settings.Default["ActiveUser"]);
                if (File.Exists(usrPath))
                {
                    UserSetting a = Common.DeSerializeObject<UserSetting>(Path.Combine(FB_Image.RelativeDirectory, "UserSetting/" + Properties.Settings.Default["ActiveUser"]));
                    if (a != null)
                    {
                        UpdateLoginControl(a);
                        return;
                    }
                }
               

            }
            LoginFacebook();
            
        }

        private void SaveActiveUserOnDisk(UserSetting active)
        {
             if (active != null)
            {
                Common.SerializeObject(active,Path.Combine(FB_Image.RelativeDirectory,"UserSetting/"+active.UserID));
                Properties.Settings.Default["ActiveUser"] = active.UserID;
                Properties.Settings.Default.Save();
            }
        }
        private void SaveInboxOnDisk(List<InboxUser> inbox)
        {
            if (inbox != null)
            {
                string savePath = Path.Combine(FB_Image.RelativeDirectory,"UserSetting/inbox");
                Common.PreparePath(savePath);
                Common.SerializeObject(inbox, savePath);
            }
        }

        private async void LoadBasicInformation()
        {

            var progress = new Progress<int>(s =>
            {
                if (s == 0)
                {
                    UpdateFriendListView(this.listViewUserList, ListInboxUser, true);
                    UpdateFriendListView(listViewFriends, ListInboxUser);
                }
                else if (s == 1)
                {
                    UpdateFriendListView(this.listViewUserList, ListInboxUser, true);
                }
                UpdateFriendListView(this.listViewUserList, ListInboxUser, true);
                UpdateFriendListView(listViewFriends, ListInboxUser);
            });
            await Task.Factory.StartNew(() => { 
                GetFriendList(progress);
                GetImageTagged1(progress, ListInboxUser);

            }, TaskCreationOptions.LongRunning);
            //await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, tbImagePath.Text, messagePath, tagList, albumId), TaskCreationOptions.LongRunning);
            //UpdateFriendListView(this.listViewUserList, ListInboxUser, true);
            //UpdateFriendListView(listViewFriends, ListInboxUser);
           // MessageBox.Show("ok");
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
                message = "Image description",
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
                if (Common.DowloadImageFromLink(source_url, newPath, ImageFormat.Jpeg,overWrite))
                    return true;
                return false;


            }
            catch (Exception e)
            {
                Log(e);
                return false;
            }
        }

        private string Upload_Picture_FB(string filename, FB_Image browseImageUP,string UserAccessToken,string albumID)
        {
            //upload photo
            try
            {
                browseImageUP.FileName = Path.GetFileName(filename); // lấy file name
                browseImageUP.Directory = Path.GetDirectoryName(filename); // get path
                browseImageUP.FileNameWithOutExtension = Path.GetFileNameWithoutExtension(filename); // lấy file name ko có phần mở rộng .jpg
                browseImageUP.FileSize = new FileInfo(filename).Length; // lấy file size
                var image_source = Image.FromFile(filename);
                browseImageUP.Height = image_source.Height;
                browseImageUP.Width = image_source.Width;

                var imgstream = File.OpenRead(filename);
                var fb = new FacebookClient(UserAccessToken);
                dynamic res = fb.Post(albumID + "/photos", new
                {
                    message = "Image description",
                    file = new FacebookMediaStream
                    {
                        ContentType = "image/jpeg",
                        FileName = browseImageUP.FileName,
                    }.SetValue(imgstream)
                });
               return res.id;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }   
        }

        private string Download_Picture_FB(FB_Image browseImageDown)
        {
            try
            {
                temp++; // biến để đặt tên
                var fb = new FacebookClient(ActiveUser.AccessToken);
                dynamic res = fb.Get(browseImageDown.ImageID + "?fields=images");  // query đường dẫn + độ phân giải ảnh
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

                /*------*/
                /* download ảnh */
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(source_url);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var yourImage = Image.FromStream(mem))
                        {
                            string new_path = FB_Image.BaseDirectory + browseImageDown.FileNameWithOutExtension + "_" + temp + ".jpg";
                            yourImage.Save(new_path, ImageFormat.Jpeg);
                            browseImageDown.FileSize = new FileInfo(FB_Image.BaseDirectory + browseImageDown.FileNameWithOutExtension + "_" + temp + ".jpg").Length;
                            string filename = new_path;
                            browseImageDown.FileName = Path.GetFileName(new_path);
                            browseImageDown.Directory = Path.GetDirectoryName(new_path);
                            return filename;
                            

                        }
                    }

                }
             
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }
        public string TestEncodeSuccessRate(IProgress<string> progress,string filename,string inputText,string albumID,bool test = true,bool googleSearch = false)
        {
            
            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;
                if (test)
                {
                    inputText = Path.Combine(FB_Image.BaseDirectory, "test.txt");
                    albumID = FB_Image.Album_Test;
                }

                //Reduce size ratio of picture
                if(progress != null)
                    progress.Report("15|15|Preparing Image");
                List<FB_Image> listFile = AutoUploadAndDownload(filename, null);
                if (listFile == null || listFile.Count < 2)
                {
                    return null;
                }
                FB_Image coverImage = listFile[listFile.Count - 1];
                string coverImageFileName = coverImage.FileName;
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                if (progress != null)
                progress.Report("50|50|Uploading Image");
                string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName), Path.GetFileName(inputText));
                FB_Image encodeImage = new FB_Image();
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }

                string id = Upload_Picture_FB(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,ActiveUser.AccessToken, albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }
                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                progress.Report("75|75|Checking");
                string tempFileName = Download_Picture_FB(downloadImage);
                if (tempFileName == null)
                    return null;
     

                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                if (outputText == null)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {
                    if (!googleSearch)
                    {
                        return downloadImage.ImageID;
                    }
                    else
                    {
                        return coverImageFileName;
                    }
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
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
                List<FB_Image> listFile = AutoUploadAndDownload(filename, null);
                if (listFile == null || listFile.Count < 2)
                {
                    return null;
                }

                FB_Image coverImage = listFile[listFile.Count - 1];
                string coverImageFileName = coverImage.FileName;
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                if (progress != null)
                    progress.Report("50|50|Uploading Image");
                string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName), Path.GetFileName(inputText));
                FB_Image encodeImage = new FB_Image();
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }
                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,uids ,albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }

                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                    progress.Report("75|75|Checking");
                string tempFileName = Download_Picture_FB(downloadImage);
                if (tempFileName == null)
                    return null;


                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                if (outputText == null)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                    progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {           
                       return coverImageFileName;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
                    return null;
                }
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }


        private string SendMessageWithTestedSource(IProgress<string> progress,string filename, string inputText, string albumID)
        {
            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;
                string coverImage = Common.CopyFileTo(filename, FB_Image.BaseDirectory);
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                string encodeFile = JPHideEncode(Path.GetFileName(coverImage), Path.GetFileName(messageFile));
                FB_Image encodeImage = new FB_Image();
                if (progress != null)
                progress.Report("25|25|Uploading Picture");
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }

                string id = Upload_Picture_FB(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,ActiveUser.AccessToken, albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }


                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                progress.Report("50|50|Checking ...");
                string tempFileName = Download_Picture_FB(downloadImage);
                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {
                    return downloadImage.ImageID;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
                    return null;
                }
               
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public static string JPHideEncode(string filename,string input)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(input) != "")
                {
                    throw new Exception("No need for full path");
                }
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, filename));
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, input));
                input=Path.GetFileName(InsertCrc32(input));
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, input));
                string enImageName = Path.GetFileNameWithoutExtension(filename)+"_encode"+Path.GetExtension(filename);
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, enImageName));
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jphide_modify " + filename + " " + enImageName + " " + input;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;

                proc.Start();
                while (!proc.HasExited)
                    ;
                return enImageName;
              
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public static string InsertCrc32(string input)
        {

            string input_path = Path.Combine(FB_Image.BaseDirectory, input);
            string hash = Crc32Hash(input_path);
            string crc = "[crc:" + hash + "]";
            if (File.Exists(input_path))
            {
                string currentContent = File.ReadAllText(input_path);
                string newinput = Path.Combine(FB_Image.BaseDirectory, Common.AppenFileName(input, "_" + (r.Next(1337)).ToString()));
                File.WriteAllText(newinput, crc + currentContent);
                return newinput;
            }
            else
            {
                return null;
            }
        }

        public  static string JPSeekDecode(string filename,string output)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(output) != "")
                {
                    throw new Exception("No need for full path");
                }
                string imageName = filename;
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, filename));
                string hiddenFileName = output;
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, hiddenFileName));
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jpseek_modify " + imageName + " " + hiddenFileName;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                while (!proc.HasExited)
                    ;

                hiddenFileName = CheckCrc32(hiddenFileName);  
                if (hiddenFileName != null)
                {
                    Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, hiddenFileName));
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
            string newpath = Path.Combine(FB_Image.BaseDirectory, Common.AppenFileName(filename, "_check"));
            string crc = "";
            Regex g = new Regex(@"(\[)(crc)(:).*?(\])");

            using (StreamWriter w = new StreamWriter(newpath))
            {

                using (StreamReader r = new StreamReader(oldpath))
                {
                    string line;
                    line = r.ReadToEnd();

                    Match m = g.Match(line);
                    if (m.Success)
                    {
                        string[] words = line.Split(':');
                        crc = words[1].Remove(8);
                        line = line.Remove(0, 14);
                        w.Write(line);
                    }
                }


            }
            string hash = Crc32Hash(newpath);
            if (hash == crc)
            {
                return newpath;
            }
            else
            {
                return null;
            }
        }

        public List<FB_Image> AutoUploadAndDownload(string filename,IProgress<int> progress = null)
        {
            string tempName = filename;
            List<FB_Image> uploadedImage = new List<FB_Image>();      
            FB_Image currentImage = new FB_Image();
            float ratio = 15;
            float ratioMax = 1.005F;
            while (ratio > ratioMax)
            {
                string id = Upload_Picture_FB(tempName, currentImage,ActiveUser.AccessToken,ActiveUser.PrivateAlbumID);
                if (currentImage.ImageID.Equals(""))
                {
                    FB_Image temp = new FB_Image();
                    currentImage.CopyTo(temp);
                    uploadedImage.Add(temp);
                }

                FB_Image downImage = new FB_Image();
                currentImage.CopyTo(downImage);
                downImage.ImageID = id;
                tempName= Download_Picture_FB(downImage);
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory,tempName));
                ratio = ((float)currentImage.FileSize / (float)downImage.FileSize);
                FB_Image k = new FB_Image();
                downImage.CopyTo(k);
                uploadedImage.Add(k);
                downImage.CopyTo(currentImage);
                if (progress != null)
                {
                    float t = ratio;
                    if (ratio > 9)
                    {
                        progress.Report(10);
                    }
                    else if (ratio >= 2)
                    {
                        progress.Report((int)(ratio * 10));
                    }
                    else if (ratio >= 1 && ratio < 2)
                    {
                        progress.Report((int)((ratio - 1) * 100));
                    }
                }

            }
            if(progress != null)
                progress.Report(100);

            return uploadedImage;
        }
        public List<FB_Image> AutoUploadAndDownload(string filename, IProgress<int> progress = null, int count = 5)
        {
            string tempName = filename;
            List<FB_Image> uploadedImage = new List<FB_Image>();
            FB_Image currentImage = new FB_Image();
            float ratio = 15;
            float ratioMax = 1.005F;
            while (ratio > ratioMax)
            {
                string id = Upload_Picture_FB(tempName, currentImage, ActiveUser.AccessToken, ActiveUser.PrivateAlbumID);
                if (currentImage.ImageID.Equals(""))
                {
                    FB_Image temp = new FB_Image();
                    currentImage.CopyTo(temp);
                    uploadedImage.Add(temp);
                }

                FB_Image downImage = new FB_Image();
                currentImage.CopyTo(downImage);
                downImage.ImageID = id;
                tempName = Download_Picture_FB(downImage);
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, tempName));
                ratio = ((float)currentImage.FileSize / (float)downImage.FileSize);
                FB_Image k = new FB_Image();
                downImage.CopyTo(k);
                uploadedImage.Add(k);
                downImage.CopyTo(currentImage);
                if (progress != null)
                {
                    float t = ratio;
                    if (ratio > 9)
                    {
                        progress.Report(10);
                    }
                    else if (ratio >= 2)
                    {
                        progress.Report((int)(ratio * 10));
                    }
                    else if (ratio >= 1 && ratio < 2)
                    {
                        progress.Report((int)((ratio - 1) * 100));
                    }
                }

            }
            if (progress != null)
                progress.Report(100);

            return uploadedImage;
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
        private void ChangeUserInbox(object sender, EventArgs e)
        {
            ListView lvUser = (ListView)sender;
            if (lvUser.SelectedItems.Count == 1 )
            {
                InboxUser inbox = Common.GetInboxByUserID(lvUser.SelectedItems[0].Name, ListInboxUser);
                UpdateMessageListView(listViewTagImage, inbox);
                currentInbox = inbox;
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                lbUserNameComm.Text = item.Text;
                lbUserIdComm.Text = item.Name;
                pBoxUserComm.ImageLocation = FB_Image.BaseDirectory + "Test_User\\profilePiture_" + item.Name + ".jpg";

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
                        tbInbox.Text = currentInbox.Messages[i].Content;
                    }

                }
                

            }
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
                il.Images.Add(Image.FromFile(Path.Combine(messages[i].Image.Directory,messages[i].Image.FileName)));
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
