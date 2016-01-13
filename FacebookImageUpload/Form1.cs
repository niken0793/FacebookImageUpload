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
using System.Diagnostics;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using FacebookImageUpload.FB_Images;
using FacebookImageUpload.GUI;
using System.Threading;

// project test sync
//ss

namespace FacebookImageUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            MyHelper.CreateProgramDir();
            cmbSelectTextType.SelectedIndex = 0;
            r = new Random();
            CheckUserSetting();

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (IsHandleCreated)
            {
                //LoadFriendList();
                //LoadMessage();
            }
        }

        FB_Image browseImage = new FB_Image();
        int temp = 0; // biến để đặt tên file tải về
        private bool isFromFile = true;


        #region Sender Tab

        private void openfile_Click(object sender, EventArgs e)
        {
            string filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
            openfile_Click_fn(tbImagePath, filter, true);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filter = "Text Files(*.txt)|*.txt";
            openfile_Click_fn(tbMessagePath, filter);
        }


        private async void uploadImage_Click(object sender, EventArgs e)
        {

            int size = GetSplitSize();
            if (size == -1)
                return;
            if (lbFriendID.Text != "...")
            {
                string messagePath = GetMessageInput(cmbSelectTextType, tbMessagePath, tbInputMessage);
                string encodeFile =tbImagePath.Text;
                List<string> tagList = new List<string>();
                tagList.Add(lbFriendID.Text);
                if (sentPass == null)
                    return;
                SimplerAES aes = new SimplerAES(sentPass);
                messagePath = aes.EncryptFile(messagePath);
                List<string> a = SplitFileIntoPart(messagePath, size);
                List<string> coverFile = new List<string>();
                if (!GetMoreImage(encodeFile, a, coverFile))
                    return;

                List<string> FailFile = new List<string>();

                string albumId = cmbInputAlbum.SelectedValue.ToString();
                var progress = new Progress<string>(s => { MyHelper.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing); });

                int count=0;
                for (int i = 0; i < a.Count; i++)
                {
                    //string newFile = Common.AppendFileName(encodeFile, i.ToString());
                    //File.Copy(encodeFile, newFile, true);
                    string flag = "";
                    if(Path.GetDirectoryName(coverFile[i]).Equals(Path.Combine(FB_Image.RelativeDirectory,"SuccessImage")))
                        await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, coverFile[i], a[i], tagList, albumId), TaskCreationOptions.LongRunning);
                    else
                        await Task.Factory.StartNew(() => flag = SendNoTestImageWithTag(progress, coverFile[i], a[i], albumId, tagList), TaskCreationOptions.LongRunning);
                    
                    
                    if (flag != null)
                    {
                        
                        count++;
                    }
                    else
                    {
                        FailFile.Add(a[i]);   
                    }
                }
                if (FailFile.Count > 0)
                {
                    MessageBox.Show("These images are not ready for sending your message");
                }
                else
                {
                    MessageBox.Show("The message has been sent");
                }

                MyHelper.ResetStatusTrip(pbStatus, lbStatusBar, lbDoing);
                // Common.DeleteFile(Common.listFileDelete);
            }
            else
            {
                MessageBox.Show("Please choose a Friend", "Choose A Friend", MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
            }

        }

        private void btnCoverImage_Click(object sender, EventArgs e)
        {
            coverImageForm = new CoverImageForm();
            coverImageForm.StartPosition = FormStartPosition.CenterParent;
            if (coverImageForm.ShowDialog() == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(coverImageForm.imageLink))
                {
                    tbImagePath.Text = coverImageForm.imageLink;
                    pbImage.ImageLocation = coverImageForm.imageLink;
                    lbImageName.Text = Path.GetFileName(coverImageForm.imageLink);
                    lbImageDirectory.Text = Path.GetDirectoryName(coverImageForm.imageLink);
                    lbImageSize.Text = MyHelper.BytesToString(new FileInfo(coverImageForm.imageLink).Length);
                }
            }

        }


        private void cmbSelectTextType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectTextType_SelectedIndexChanged_Handle(sender, e);
        }
        private void tbMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isFromFile)
                e.Handled = true;
        }
        private string GetInboxText(string name, int count)
        {
            string text;
            if (count > 0)
            {
                text = string.Format("{0} - ({1})", name, count);
            }
            else
            {
                text = name;
            }
            return text;
        }
        private void tbInputMessage_TextChanged(object sender, EventArgs e)
        {
            tbInputMessage_TextChanged_Hanlde(sender, e);
        }

        #endregion


        #region Google Image
        bool cancelSearching = false;
        bool isStart = false;
        private async void btnImageSearch_Click(object sender, EventArgs e)
        {
 
            string progressString;
            try
            {
                
                if (!isStart)
                {
                    btnImageSearch.Text = "Cancel";
                    isStart = true;
                    cancelSearching = false;
                    tbGoogleLink.Clear();
                    var progress = new Progress<string>
                    (s =>
                    {
                        progressString = s;
                        tbGoogleLink.AppendText(progressString + Environment.NewLine);
                    });
                    await Task.Factory.StartNew(() => GetProcessImageFromGoogleImage(progress), TaskCreationOptions.LongRunning);
                    MessageBox.Show("Searching is finish");
                }
                else
                {
                    cancelSearching = true;
                    btnImageSearch.Text = "Search";
                }



            }
            catch (Exception ex)
            {
                Log(ex);
                cancelSearching = false;
            }
            
        }
        #endregion


        #region Receiver tab
        private void listViewUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUserInbox(sender, e);
        }

        private void listViewTagImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUserMessage(sender, e);
        }

        private void dtpCheckTime_ValueChanged(object sender, EventArgs e)
        {
            tbCheckTime.Text = MyHelper.GetUnixTimesStamp(dtpCheckTime.Value).ToString();
        }

        private void btnFacebookLogin_Click(object sender, EventArgs e)
        {
            LoginFacebook();

        }
        private void btnGetUserList_Click(object sender, EventArgs e)
        {
            LoadMessage();

        }

        #endregion 


        #region Setting tab

        private void button1_Click_1(object sender, EventArgs e)
        {

            listViewFriends.Visible = !listViewFriends.Visible;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            SaveActiveUserOnDisk(ActiveUser);
            SaveInboxOnDisk(ListInboxUser);
            SaveAppSettingOnDisk(MyAppSetting);
        }

        private void btnChangeCheckTime_Click(object sender, EventArgs e)
        {
            ActiveUser.CheckTime =long.Parse(tbCheckTime.Text);
            
        }

        private string sentPass = null;
        private void listViewFriends_ItemActivate(object sender, EventArgs e)
        {

            ListViewItem item = ((ListView)sender).SelectedItems[0];
            lbFriendName.Text = item.Text;
            lbFriendID.Text = item.Name;
            pbFriend.ImageLocation = Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserImageDir, item.Name + ".jpg");
            string pass = GetMessagePassword(lbFriendID.Text);
            sentPass = pass;
            
        }

        private void btnUpFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog f = new FolderBrowserDialog();
                
                if (DialogResult.OK == f.ShowDialog())
                {
                    string dir = f.SelectedPath;
                    string[] files = Directory.GetFiles(dir, "*.jpg");
                    if (files.Length > 0)
                    {
                       

                        foreach (string i in files)
                        {
                            string id = UploadFB(i, ActiveUser.AccessToken, ActiveUser.PrivateAlbumID);
                            string newfile = MyHelper.AppendFileName(i, "down");
                            DownloadFB(id, ActiveUser.AccessToken, newfile);
                        }

                        MessageBox.Show("Finish");

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        private async void btnTestEncode_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (DialogResult.OK == f.ShowDialog())
            {
                string dir = f.SelectedPath;
                string failDir = dir + "/fail";
                if (!Directory.Exists(failDir))
                    Directory.CreateDirectory(failDir);
                string successDir = dir + "/success";
                if (!Directory.Exists(successDir))
                    Directory.CreateDirectory(successDir);
                string[] files = Directory.GetFiles(dir, "*.jpg");
                if (files.Length > 0)
                {
                    int count = 0;
                    int success = 0;
                    var progress = new Progress<string>(s => { MyHelper.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing); });
                    string messagePath =Path.Combine( FB_Image.RelativeDirectory,FB_Image.TestInputDir,"200.txt");
                    foreach (string i in files)
                    {
                            string flag = "";
                            await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, i, messagePath, null, ActiveUser.PrivateAlbumID), TaskCreationOptions.LongRunning);
                            if (flag != null)
                            {
                                tbSetting.AppendText(Path.GetFileName(i) + " : success"+Environment.NewLine);
                                File.Copy(i, Path.Combine(successDir, Path.GetFileName(i)),true);
                                success++;
                            }
                            else
                            {
                                tbSetting.AppendText(Path.GetFileName(i) + " : fail"+Environment.NewLine);
                                File.Copy(i, Path.Combine(failDir, Path.GetFileName(i)),true);
                                count++;
                            }

                        
                    }
                    tbSetting.AppendText("Success: " + success.ToString()+ Environment.NewLine);
                    tbSetting.AppendText("Fail: " + count.ToString()+Environment.NewLine);
                    tbSetting.AppendText("All: " + files.Length.ToString() + Environment.NewLine);
                    MessageBox.Show("Finish");

                }

            }
        }

        private  void btnAutoTest_Click(object sender, EventArgs e)
        {
            AutoTest();
        }

        private void AutoTest()
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (DialogResult.OK == f.ShowDialog())
            {
                string dir = f.SelectedPath;
                string[] files = Directory.GetFiles(dir, "*.jpg");
                string newDir = dir + "/resave";
                if (files.Length > 0)
                {

                    if (!Directory.Exists(newDir))
                        Directory.CreateDirectory(newDir);

                    tbSetting.AppendText("Resaveing........." + Environment.NewLine);
                    foreach (string i in files)
                    {
                        if (ActiveUser != null && !String.IsNullOrEmpty(ActiveUser.AccessToken) && !String.IsNullOrEmpty(ActiveUser.PrivateAlbumID))
                        {
                            ProcessImage.ReSaveFileToMinimal(i, newDir, ActiveUser.AccessToken, ActiveUser.PrivateAlbumID);
                        }
                        else
                        {
                            ProcessImage.ReSaveFileToMinimal(i, newDir);
                        }
                        tbSetting.AppendText(i + Environment.NewLine);

                    }
                    MessageBox.Show("Finish");

                }

                string fixDir = newDir + "/fix";
                if (!Directory.Exists(fixDir))
                    Directory.CreateDirectory(fixDir);
                files = Directory.GetFiles(newDir, "*.jpg");
                if (files.Length > 0)
                {
                    tbSetting.AppendText("Fixing........." + Environment.NewLine);
                    //progress.Report("Fixing........." + Environment.NewLine);
                    foreach (string i in files)
                    {
                        string s = ProcessImage.FixFailImage(i);
                        if (s != null)
                        {
                            File.Copy(s, Path.Combine(fixDir, Path.GetFileName(s)),true);

                        }
                        tbSetting.AppendText(i + Environment.NewLine);
                        // progress.Report(i + Environment.NewLine);
                    }

                }
            }
           
        }

        private void btnShowPrivateAlbum_Click(object sender, EventArgs e)
        {
           
                CreateAlbum a = new CreateAlbum(ActiveUser.AccessToken);
                a.StartPosition = FormStartPosition.CenterParent;
                if (a.ShowDialog() == DialogResult.OK)
                {
                    if (Form1.ActiveUser != null)
                    {
                        Form1.ActiveUser.PrivateAlbumID = a.AlbumID;
                        Form1.ActiveUser.PrivateAlbumName = a.AlbumName;
                        lbPrivateAlbum.Text = a.AlbumName.ToString();
                        lbLoginPrivateAlbum.Text = a.AlbumName.ToString();

                        ActiveUser.Albums = CreateAlbum.GetUserAlbumsForComboBox(ActiveUser.AccessToken, "all");
                        cmbInputAlbum.DataSource = ActiveUser.Albums;
                        cmbInputAlbum.DisplayMember = "Name";
                        cmbInputAlbum.ValueMember = "ID";
                    }
                }
              




            //string file = InsertCrc32("100.txt");
            //string newFile = CheckCrc32(file);
            //if (newFile != null)
            //    tbSetting.Text = File.ReadAllText(newFile);


            //byte[] a = Encoding.UTF8.GetBytes("1451237243|01|0");
            //byte[] b = Encoding.UTF8.GetBytes("1951237293|99|1");
            //string hash = Crc32Hash(Path.Combine(FB_Image.BaseDirectory, "100.txt"));
            //string hash1 = Crc32HashFromString(File.ReadAllText(Path.Combine(FB_Image.BaseDirectory, "100.txt")));

            //string encodeFile = Path.Combine(FB_Image.BaseDirectory, "a.jpg");
            //List<string> a = SplitFileIntoPart(Path.Combine(FB_Image.BaseDirectory, "fill.txt"), 1);
            //List<string> encodedFile = new List<string>();
            //for (int i = 0; i < a.Count; i++)
            //{
            //    string newFile = Common.AppendFileName(encodeFile, i.ToString());
            //    File.Copy(encodeFile, newFile, true);
            //    string s = JPHideEncode(Path.GetFileName(newFile), Path.GetFileName(a[i]));
            //    encodedFile.Add(s);
            //}

            //List<string> listOut = new List<string>();
            //for (int i = 0; i < encodedFile.Count; i++)
            //{
            //    string output = Path.GetFileNameWithoutExtension(encodedFile[i]) + "_out.txt";
            //    string s = JPSeekDecode(encodedFile[i], output);
            //    if (s != null)
            //    {
            //        s = Path.Combine(FB_Image.BaseDirectory, s);
            //        string t = Encoding.UTF8.GetString(CorrectError(s));
            //        listOut.Add(t);
            //    }

            //}

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbSetting.Clear();
        }

        private void btnDiff_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (DialogResult.OK == f.ShowDialog())
            {
                string dir = f.SelectedPath;
                string[] files = Directory.GetFiles(dir, "*.jpg");
                int diff = 0;
                int sameZero = 0;
                int same = 0;
                if (files.Length > 0)
                {

                    foreach( string i in files)
                    {
                        try
                        {
                            string newpath = MyHelper.AppendFileName(i, "fb");
                            string id = Form1.UploadFB(i, ActiveUser.AccessToken, ActiveUser.PrivateAlbumID);
                            if (Form1.DownloadFB(id, ActiveUser.AccessToken, newpath))
                            {
                                List<DiffDCT> a = ProcessImage.CompareDctFiles(i, newpath,true);
                                if (a.Count > 0)
                                {


                                    //int zero = 0;
                                    //foreach (DiffDCT dif in a)
                                    //{
                                    //    zero += dif.zero;
                                    //}
                                    //if (zero == 0)
                                    //{
                                    //    sameZero++;
                                    //    tbSetting.AppendText(Path.GetFileName(i) + " : same zero" + Environment.NewLine);
                                    //}
                                    //else
                                    //{
                                        tbSetting.AppendText(Path.GetFileName(i) + " : diff" + Environment.NewLine);
                                        diff++;
                                    //}
                                }
                                else
                                {
                                    tbSetting.AppendText(Path.GetFileName(i) + " : same" + Environment.NewLine);
                                    same++;
                                }
                            }
                            else
                            {
                                tbSetting.AppendText(Path.GetFileName(i) + " : fail to download" + Environment.NewLine);
                            }
                            //Thread.Sleep(1000);
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    tbSetting.AppendText(String.Format("Diff: {0}, same: {1}, sameZero {2} All: {3}", diff, same,sameZero, files.Length));
                    MessageBox.Show("Finish");
                        

                }
            }
        }

        #endregion

      


        #region manual
        string[] manualListFile = null;
        private void btnManualBrowseImage_Click(object sender, EventArgs e)
        {
            string filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
            //openfile_Click_fn(tbManualImage, filter, true);
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = filter;
            if (radioManualEncode.Checked)
            {
                if (of.ShowDialog() == DialogResult.OK)
                {
                    tbManualImage.Text = of.FileName;
                    pbManual.ImageLocation = of.FileName;
                    lbManualImageName.Text = Path.GetFileName(of.FileName);
                    lbManualImageDir.Text = Path.GetDirectoryName(of.FileName);
                    lbManualImageSize.Text = MyHelper.BytesToString(new FileInfo(of.FileName).Length);
                }

            }
            else
            {
                of.Multiselect = true;
                if (of.ShowDialog() == DialogResult.OK && of.FileNames.Length >= 1)
                {
                    manualListFile = of.FileNames;
                    foreach (string item in of.FileNames)
                        tbManualImage.AppendText(item + Environment.NewLine);
                    pbManual.ImageLocation = of.FileNames[0];
                    lbManualImageName.Text = Path.GetFileName(of.FileNames[0]);
                    lbManualImageDir.Text = Path.GetDirectoryName(of.FileNames[0]);
                    lbManualImageSize.Text = MyHelper.BytesToString(new FileInfo(of.FileNames[0]).Length);
                }
            }
        }


        private void btnManualBrowseFile_Click(object sender, EventArgs e)
        {
            string filter = "Text Files(*.txt)|*.txt";
            //openfile_Click_fn(tbManualText, filter);
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = filter;
            if (open.ShowDialog() == DialogResult.OK)
            {
                tbManualText.Text = open.FileName;
                tbManualStatus.Clear();
                tbManualStatus.AppendText(File.ReadAllText(open.FileName));
                tbManualStatus.AppendText(Environment.NewLine);
            }

            

        }

        private void cmbManualFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            if (c.SelectedIndex == c.Items.IndexOf("From File"))
            {
                btnManualBrowseFile.Enabled = true;
                tbManualText.Enabled = true;
                isFromFile = true;
            }
            else if (c.SelectedIndex == c.Items.IndexOf("From Text"))
            {
                btnManualBrowseFile.Enabled = false;
                tbManualText.Enabled = false;
                isFromFile = false;
            }
        }

        InboxUser manualInbox;
        private void btnManualEncode_Click(object sender, EventArgs e)
        {
            string pass = GetMessagePasswordNoID();
            if (pass == null)
                return;
            if (radioManualEncode.Checked)
            {
                string messagePath = GetMessageInput(cmbManualFileType, tbManualText, tbManualStatus);
                if (String.IsNullOrEmpty(tbManualImage.Text))
                {
                    return;
                }

                int size = GetSplitSize();
                if (size < 0)
                    return;
                string encodeFile = tbManualImage.Text;
                SimplerAES aes = new SimplerAES(pass);
                messagePath = aes.EncryptFile(messagePath);
                List<string> a = SplitFileIntoPart(messagePath, size);
                List<string> coverFile = new List<string>();
                if (!GetMoreImage(encodeFile, a, coverFile))
                    return;
                List<string> FailFile = new List<string>();
                string flag = "";
                int count = 0;
                string successFile = "";
                for (int i = 0; i < a.Count; i++)
                {
                    //string newFile = Common.AppendFileName(encodeFile, i.ToString());
                    //File.Copy(encodeFile, newFile, true);
                    bool isTest = Path.GetDirectoryName(coverFile[i]).Equals(Path.Combine(FB_Image.RelativeDirectory, "SuccessImage"));
                    flag = EncodeImage(coverFile[i], a[i], null, isTest);

                    if (flag != null)
                    {
                        string outputdir = Path.Combine(Path.Combine(FB_Image.RelativeDirectory, FB_Image.OutputDir));
                        if (!Directory.Exists(outputdir))
                        {
                            Directory.CreateDirectory(outputdir);
                        }
                        successFile = MyHelper.CopyFileTo(Path.Combine(FB_Image.BaseDirectory, flag), outputdir);
                        count++;
                    }
                    else
                    {
                        FailFile.Add(a[i]);
                    }
                }
                if (FailFile.Count > 0)
                {
                    MessageBox.Show("These images are not ready for sending your message");
                }
                else
                {
                    OpenFolder(successFile);
                    MessageBox.Show("Encoding Success");
                }
            }
            else
            {
                if (manualListFile != null && manualListFile.Length >= 1)
                {
                    manualInbox = new InboxUser("1", "Manual");
                    foreach (string item in manualListFile)
                    {
                        byte[] content = MyHelper.GetByteFromImage(item);
                        if (content != null)
                        {
                            string s_content = CorrectErrorString(content);
                            if (s_content != null)
                            {
                                //userInbox.Messages.Add(new FB_Message(content, new FB_Image(imageId,imagePath),createdTime,isSent));
                                bool flag = manualInbox.AddMessageToInbox(s_content, "1", item, 0);
                                if (flag)
                                    manualInbox.CountNew++;
                            }
                        }
                    }
                    if (manualInbox.Messages.Count >= 0)
                    {
                        foreach (FB_Message mes in manualInbox.Messages)
                        {
                            string s = "";
                            if (mes.Part.Count >= 1)
                            {
                                s += "Images path:" + Environment.NewLine;
                                foreach (MessagePart p in mes.Part)
                                {
                                    s += p.ImagePath + Environment.NewLine;
                                }
                                s += "Message: ";
                                string content = mes.GetContent(pass);
                                if (content != null)
                                    s += content + Environment.NewLine;
                                else
                                    s += "No Output" + Environment.NewLine;
                                s += "---------------------------------" + Environment.NewLine;
                                tbManualStatus.AppendText(s);

                            }
                        }
                    }

                }
            }



        }

        private static bool GetMoreImage(string encodeFile, List<string> a, List<string> coverFile)
        {
            coverFile.Add(encodeFile);

            if (a.Count > 1)
            {
                OpenFileDialog opf = new OpenFileDialog();
                string filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
                opf.Title = "Choose: " + (a.Count - 1).ToString() + " files";
                opf.Filter = filter;
                opf.Multiselect = true;
                do
                {
                    if (opf.ShowDialog() == DialogResult.Cancel)
                    {
                        return false;
                    }


                } while (opf.FileNames.Length != a.Count - 1);

                coverFile.AddRange(opf.FileNames);

            }
            return true;
        }

        private static int GetSplitSize()
        {
            int size;

            SplitFile sp = new SplitFile();
            sp.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == sp.ShowDialog())
            {
                if (sp.Type == SplitFile.SplitType.Split)
                {
                    size = sp.FileSize;
                }
                else
                {
                    size = 0;
                }
            }
            else
            {
                size = -1;
            }
            return size;
        }

        private string GetMessageInput(ComboBox cmbType, TextBox tbInputPath, TextBox tbInput)
        {
            string messagePath = "";
            if (cmbType.SelectedIndex == cmbType.Items.IndexOf("From File"))
            {
                messagePath = tbInputPath.Text;
            }
            else if (cmbType.SelectedIndex == cmbType.Items.IndexOf("From Text"))
            {
                string tempMessagePath = Path.Combine(FB_Image.RelativeDirectory, FB_Image.TestInputDir, "temp_message.txt");
                File.WriteAllText(tempMessagePath, tbInput.Text);
                messagePath = tempMessagePath;

            }
            else
            {
                messagePath = tbInputPath.Text;
            }
            return messagePath;
        }

        private string GetMessagePassword(string friendID)
        {
            InboxUser a = MyHelper.GetInboxByUserID(friendID, ListInboxUser);
            if (a != null)
            {
                if (!string.IsNullOrEmpty(a.Password) && a.IsSavePass)
                {
                    return a.Password;
                }
                else
                {
                    GetPassword getPass = new GetPassword();
                    getPass.StartPosition = FormStartPosition.CenterParent;
                    if (getPass.ShowDialog() == DialogResult.OK)
                    {
                        a.Password = getPass.Password;
                        a.IsSavePass = getPass.IsSavePass;
                        return a.Password;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null; ;

        }
        private string GetMessagePasswordNoID()
        {

            GetPassword getPass = new GetPassword();
            getPass.StartPosition = FormStartPosition.CenterParent;
            if (getPass.ShowDialog() == DialogResult.OK)
            {
                return getPass.Password;
            }
            else
            {
                return null;
            }

        }


        private void btnManualDecode_Click(object sender, EventArgs e)
        {
            tbManualStatus.Clear();
            if (!String.IsNullOrEmpty(tbManualImage.Text))
            {
                string output = DecodeImage(tbManualImage.Text, null);
                if (output != null)
                {
                    tbManualStatus.Text = File.ReadAllText(Path.Combine(FB_Image.BaseDirectory, output));
                }
                else
                {
                    tbManualStatus.Text = "No output";
                }
            }



        }

        private void tbManualStatus_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            lbManualLength.Text = t.Text.Length.ToString();
        }

        private void btnManualClear_Click(object sender, EventArgs e)
        {
            tbManualStatus.Clear();
        }


        private void radioManualEncode_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = true;
            tbManualText.Visible = flag;
            cmbManualFileType.Visible = flag;
            btnManualBrowseFile.Visible = flag;
            lbManualMessage.Visible = flag;
            tbManualImage.Size = new System.Drawing.Size(tbManualImage.Size.Width, 20);
            MyHelper.EnableControl(flag, tbManualText, cmbManualFileType, btnManualBrowseFile, lbManualMessage);
            tbManualStatus.Clear();
            tbManualText.Clear();
            tbManualImage.Clear();
            btnManualEncode.Text = "Encode";
        }

        private void radioManualDecode_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = false;
            tbManualText.Visible = flag;
            cmbManualFileType.Visible = flag;
            btnManualBrowseFile.Visible = flag;
            lbManualMessage.Visible = flag;
            tbManualImage.Size = new System.Drawing.Size(tbManualImage.Size.Width, 80);
            MyHelper.EnableControl(flag, tbManualText, cmbManualFileType, btnManualBrowseFile, lbManualMessage);
            tbManualStatus.Clear();
            tbManualImage.Clear();
            tbManualText.Clear();
            btnManualEncode.Text = "Decode";
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //LoadFriendList();
            //GetPassword a = new GetPassword();
            //a.ShowDialog();
            //string cipher = "123".ToSecureString().EncryptString();
            //string plain = cipher.DecryptString().ToInsecureString();
            LoadFriendList();
            LoadMessage();
        }

        private void listViewMessage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (((ListView)sender).FocusedItem.Bounds.Contains(e.Location))
                {
                    //cmnLVMessage.Show(Cursor.Position);
                }
            }
        }
        #endregion




    }

}
