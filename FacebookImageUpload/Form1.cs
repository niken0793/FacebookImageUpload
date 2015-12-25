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
            cmbSelectTextType.SelectedIndex = 0;

            inbox = User.inboxUserA;
            outbox = User.inboxUserB;
            r = new Random();

            CheckUserSetting();
            lbCheckTime.Text = Common.UnixTimeStampToDateTime(ActiveUser.CheckTime).ToString();
			LoadBasicInformation();
        }

        FB_Image browseImage = new FB_Image();
        int temp = 0; // biến để đặt tên file tải về
        private List<string> inbox;
        private List<string> outbox;
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
            if (lbFriendID.Text != "...")
            {
                string messagePath = "";
                if (cmbSelectTextType.SelectedIndex == cmbSelectTextType.Items.IndexOf("From File"))
                {
                    messagePath = tbMessagePath.Text;
                }
                else if (cmbSelectTextType.SelectedIndex == cmbSelectTextType.Items.IndexOf("From Text"))
                {
                    string tempMessagePath = Path.Combine(FB_Image.RelativeDirectory, "temp", "temp_message.txt");
                    File.WriteAllText(tempMessagePath, tbInputMessage.Text);
                    messagePath = tempMessagePath;

                }

                string albumId = cmbInputAlbum.SelectedValue.ToString();
                var progress = new Progress<string>(s => { Common.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing); });
                List<string> tagList = new List<string>();
                tagList.Add(lbFriendID.Text);

                if (cbIsTested.Checked)
                {
                    string flag = "";
                    await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, tbImagePath.Text, messagePath, tagList, albumId), TaskCreationOptions.LongRunning);
                    if (flag != null)
                    {
                        MessageBox.Show("The message has been sent");
                    }
                    else
                    {
                        MessageBox.Show("This image is not ready for sending your message");
                    }
                }
                else
                {
                    string flag = "";
                    await Task.Factory.StartNew(() => flag = SendNoTestImageWithTag(progress, tbImagePath.Text, messagePath, albumId, tagList));
                    if (flag != null)
                    {
                        MessageBox.Show("The message has been sent");
                    }
                    else
                    {
                        MessageBox.Show("This image is not ready for sending your message");
                    }
                }
                Common.ResetStatusTrip(pbStatus, lbStatusBar, lbDoing);
                // Common.DeleteFile(Common.listFileDelete);
            }
            else
            {
                MessageBox.Show("Please choose Receiver from Receiver Tab", "Choose Receiver", MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
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
        private async void btnImageSearch_Click(object sender, EventArgs e)
        {
            string progressString;
            try
            {
                var progress = new Progress<string>
                    (s =>
                    {
                        progressString = s;
                        tbGoogleLink.AppendText(progressString + Environment.NewLine);
                    });
                await Task.Factory.StartNew(() => searchImageFromGoogle(progress), TaskCreationOptions.LongRunning);
                MessageBox.Show("Searching is finish");

            }
            catch (Exception ex)
            {
                Log(ex);
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
            tbCheckTime.Text = Common.GetUnixTimesStamp(dtpCheckTime.Value).ToString();
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
        }

        private void btnChangeCheckTime_Click(object sender, EventArgs e)
        {
            ActiveUser.CheckTime =long.Parse(tbCheckTime.Text);
            
        }

        private void listViewFriends_ItemActivate(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to use this user to communicate?", "Choose User", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                lbFriendName.Text = item.Text;
                lbFriendID.Text = item.Name;
                pbFriend.ImageLocation = FB_Image.BaseDirectory + "Test_User\\profilePiture_" + item.Name + ".jpg";
            }
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
                            string newfile = Common.AppendFileName(i, "down");
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
                    var progress = new Progress<string>(s => { Common.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing); });
                    string messagePath = FB_Image.RelativeDirectory + "/GoogleImage/200.txt";
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
            if (a.ShowDialog() == DialogResult.OK)
            {
                if (Form1.ActiveUser != null)
                {
                    Form1.ActiveUser.PrivateAlbumID = a.AlbumID;
                    Form1.ActiveUser.PrivateAlbumName = a.AlbumName;
                    lbPrivateAlbum.Text = a.AlbumName.ToString();
                }
            }
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
                            string newpath = Common.AppendFileName(i, "fb");
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

    }

}
