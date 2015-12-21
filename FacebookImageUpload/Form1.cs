﻿using System;
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

            //FacebookAlbum f = new FacebookAlbum();
            //tbInputMessage.Text = f.createAlbum(FB_Image.AccessToken, "test albumss", "{\"value\":\"SELF\"}", "self");

            CheckUserSetting();
            lbCheckTime.Text = Common.UnixTimeStampToDateTime(ActiveUser.CheckTime).ToString();
            //lbTimeNow.Text = DateTime.Now.ToString();
			LoadBasicInformation();
            //if (CheckTester())
            //{
            //    btnTester.Enabled = false;
            //}
            //else 
            //{
            //    btnTester.Enabled = true;
            //}
        }

        FB_Image browseImage = new FB_Image();
        int temp = 0; // biến để đặt tên file tải về
        private List<string> inbox;
        private List<string> outbox;
        private bool isFromFile = true;


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

        private void createAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookAlbum album = new FacebookAlbum();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void uploadImage_Click(object sender, EventArgs e)
        {
            if(lbFriendID.Text!="...")
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
                    await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, tbImagePath.Text, messagePath, tagList,albumId), TaskCreationOptions.LongRunning);
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
                    await Task.Factory.StartNew(() =>flag = SendNoTestImageWithTag(progress, tbImagePath.Text, messagePath, albumId,tagList));
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


        /*private async void btngetAlbumlist_Click(object sender, EventArgs e)
        {
            btngetAlbumlist.Enabled = false;
            pbStatus.Maximum = 100;
            pbStatus.Step = 1;
            //List<string> inboxAlbums = new List<string> { "1661869480692559", "1658361464376694", "1661205730758934" };
            //List<string> inboxAlbums = null;
            List<string> inboxAlbums = outbox;
            this.ListViewalbumList.Items.Clear();
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; lbStatusBar.Text = s.ToString(); });
                await Task.Factory.StartNew(() => GetAlbumList_Outbox(progress, inboxAlbums, 5), TaskCreationOptions.LongRunning);

                this.ListViewalbumList.View = View.LargeIcon;
                this.ListViewalbumList.LargeImageList = FB_Image.Album_PhotoList;

                for (int j = 0; j < FB_Image.Album_PhotoList.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Name = FB_Image.List_AlbumID[j];
                    item.Text = FB_Image.Album_PhotoList.Images.Keys[j].ToString();
                    item.ImageIndex = j;
                    this.ListViewalbumList.Items.Add(item);
                }
                string path = Path.Combine(FB_Image.RelativeDirectory, FB_Image.AlbumDirectory);
                Common.SerializeObject(FB_Image.List_AlbumInfo, path);



            }
            catch (Exception ex)
            {
                Log(ex);
            }
            btngetAlbumlist.Enabled = true;
        }*/


        /*private void ListViewalbumList_ItemActivate(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to choose this Album for upload?", "Choose Album", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                pBoxAlbumCover.Image = (Image)item.ImageList.Images[item.ImageIndex];
                lbAlbumName.Text = item.Text.ToString();
                tbAlbumID.Text = item.Name.ToString();
            }
        }*/


        private void cmbSelectTextType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectTextType_SelectedIndexChanged_Handle(sender, e);
        }

        private void tbMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isFromFile)
                e.Handled = true;
        }

        /*
         * Tab Receiver
         * 
         *  */

       /* private async void btnGetAlbumInbox_Click(object sender, EventArgs e)
        {
            btnGetAlbumInbox.Enabled = false;
            pbStatus.Maximum = 100;
            pbStatus.Step = 1;
            //List<string> inboxAlbums = new List<string> { "1661869480692559", "1658361464376694", "1661205730758934" };
            //List<string> inboxAlbums = null;
            List<string> inboxAlbums = inbox;
            this.ListViewalbumList_In.Items.Clear();
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; lbStatusBar.Text = s.ToString(); });
                await Task.Factory.StartNew(() => GetAlbumList_1(progress, inboxAlbums, 5), TaskCreationOptions.LongRunning);

                this.ListViewalbumList_In.View = View.LargeIcon;
                this.ListViewalbumList_In.LargeImageList = FB_Image.Album_PhotoList_In;

                for (int j = 0; j < FB_Image.Album_PhotoList_In.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Name = FB_Image.List_AlbumID_In[j];
                    item.Text = GetInboxText(FB_Image.Album_PhotoList_In.Images.Keys[j].ToString(), FB_Image.List_AlbumInfo_In[j].NewNumber);
                    item.ImageIndex = j;
                    this.ListViewalbumList_In.Items.Add(item);
                }
                string path = Path.Combine(FB_Image.RelativeDirectory, FB_Image.AlbumDirectory_In);
                Common.SerializeObject(FB_Image.List_AlbumInfo_In, path);



            }
            catch (Exception ex)
            {
                Log(ex);
            }
            btnGetAlbumInbox.Enabled = true;

        }*/

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


        /*private async void btnGetMessage_Click(object sender, EventArgs e)
        {

            var a = ListViewalbumList_In.SelectedItems;
            if (a.Count > 0)
            {

                foreach (ListViewItem i in a)
                {
                    string albumID = i.Name;
                    string text = i.Text;
                    if (text.IndexOf('-') > 0)
                        text = text.Substring(0, text.IndexOf('-'));
                    List<FB_Message> listMessage = null;
                    var progress = new Progress<string>(s => { });
                    await Task.Factory.StartNew(() => { listMessage = GetNewMessageFromAlbum(progress, albumID); }, TaskCreationOptions.LongRunning);
                    if (listMessage == null)
                    {
                        tbInbox.AppendText("No ouput" + Environment.NewLine);
                        return;
                    }
                    foreach (FB_Message m in listMessage)
                    {
                        if (m.Content != "")
                        {
                            tbInbox.AppendText("Output of " + m.Image.FileName + " : " + Environment.NewLine);
                            tbInbox.AppendText(m.Content + Environment.NewLine);
                        }
                        else
                        {
                            tbInbox.AppendText("Output of " + m.Image.FileName + " : " + Environment.NewLine);
                            tbInbox.AppendText("No ouput" + Environment.NewLine);
                        }

                    }

                    i.Text = text;
                }

                MessageBox.Show("get message is finished");
            }
        }*/


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

        private void tbInputMessage_TextChanged(object sender, EventArgs e)
        {
            tbInputMessage_TextChanged_Hanlde(sender, e);
        }

        private async void btnGetImageTagged_Click(object sender, EventArgs e)
        {
            btnGetImageTagged.Enabled = false;
            pbStatus.Maximum = 100;
            pbStatus.Step = 1;
            List<string> inboxImage = inbox;
            //this.listViewTagImage.Items.Clear();
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; lbStatusBar.Text = s.ToString(); });
                await Task.Factory.StartNew(() => GetImageTagged1(progress,ListInboxUser), TaskCreationOptions.LongRunning);

                //this.listViewTagImage.View = View.LargeIcon;
                //this.listViewTagImage.LargeImageList = FB_Image.Image_Tags_In;

                //for (int j = 0; j < FB_Image.Image_Tags_In.Images.Count; j++)
                //{
                //    ListViewItem item = new ListViewItem();
                //    item.Name = FB_Image.Image_TagsID_In[j];
                //    item.Text = FB_Image.Image_Tags_In.Images.Keys[j].ToString();
                //    item.ImageIndex = j;
                //    this.listViewTagImage.Items.Add(item);
                //}
                if (ListInboxUser.Count > 0)
                {
                    foreach (InboxUser i in ListInboxUser)
                    {
                        if (i.Messages.Count > 0)
                        {
                            foreach (FB_Message m in i.Messages)
                            {
                                tbInbox.AppendText("Message: "+ m.Content  + Environment.NewLine);
                                tbInbox.AppendText("Sender: " + i.UserName + Environment.NewLine);
                            }
                        }
                    }
                }
                //ActiveUser.CheckTime = Common.GetUnixTimesStamp(DateTime.Now);
                SaveActiveUserOnDisk(ActiveUser);


            }
            catch (Exception ex)
            {
                Log(ex);
            }
            btnGetImageTagged.Enabled = true;
        }

        private async void btnGetImageMessage_Click(object sender, EventArgs e)
        {
            var a = listViewTagImage.SelectedItems;
            if (a.Count > 0)
            {

                foreach (ListViewItem i in a)
                {
                    List<FB_Message> listMessage = null;
                    var progress = new Progress<string>(s => { });
                    await Task.Factory.StartNew(() => { listMessage = GetNewMessageFromImage(progress, i.Text); }, TaskCreationOptions.LongRunning);
                    if (listMessage == null)
                    {
                        tbInbox.AppendText("No ouput" + Environment.NewLine);
                        return;
                    }
                    foreach (FB_Message m in listMessage)
                    {
                        if (m.Content != "")
                        {
                            tbInbox.AppendText("Output of " + m.Image.FileName + " : " + Environment.NewLine);
                            tbInbox.AppendText(m.Content + Environment.NewLine);
                        }
                        else
                        {
                            tbInbox.AppendText("Output of " + m.Image.FileName + " : " + Environment.NewLine);
                            tbInbox.AppendText("No ouput" + Environment.NewLine);
                        }

                    }

                }

                MessageBox.Show("get message is finished");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            //CreateAlbum a = new CreateAlbum(ActiveUser.AccessToken);
            //if (a.ShowDialog() == DialogResult.OK)
            //{
            //    if (Form1.ActiveUser != null)
            //    {
            //        Form1.ActiveUser.PrivateAlbumID = a.AlbumID;
            //        Form1.ActiveUser.PrivateAlbumName = a.AlbumName;
            //        lbPrivateAlbum.Text = a.AlbumName;
            //    }
            //}

           // tbInputMessage.AppendText("Check Time" +ActiveUser.CheckTime.ToString() + Environment.NewLine);
            //foreach (PrivateAlbum i in ActiveUser.Albums)
            //{
            //    tbInputMessage.AppendText(i.Name.ToString() + " : " + i.ID.ToString()+Environment.NewLine);
            //}
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

        //private void btnTester_Click(object sender, EventArgs e)
        //{
        //    //tbInputMessage.Text = ActiveUser.CheckTime.ToString();
        //    lbCheckTime.Text = Common.UnixTimeStampToDateTime(ActiveUser.CheckTime).ToString();
        //}


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

        private void button2_Click(object sender, EventArgs e)
        {
            //AutoUploadAndDownload(tbImagePath.Text, null,5);
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp";
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                //Load the Image
                Image<Bgr, Byte> img = new Image<Bgr, byte>(Openfile.FileName);

                SaveFileDialog SaveFile = new SaveFileDialog();
                if (SaveFile.ShowDialog() == DialogResult.OK)
                {
                    saveJpeg(SaveFile.FileName, img.ToBitmap(), 72);
                }
            }
        }

        private void btnUpFolder_Click(object sender, EventArgs e)
        {
            
        }

        private async void btnTestEncode_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (DialogResult.OK == f.ShowDialog())
            {
                string dir = f.SelectedPath;
                string[] files = Directory.GetFiles(dir, "*.jpg");
                if (files.Length > 0)
                {
                    int count = 0;
                    var progress = new Progress<string>(s => { Common.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing); });
                    string messagePath = FB_Image.RelativeDirectory + "/GoogleImage/10.txt";
                    foreach (string i in files)
                    {
                            string flag = "";
                            await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, i, messagePath, null, ActiveUser.PrivateAlbumID), TaskCreationOptions.LongRunning);
                            if (flag != null)
                            {
                                tbSetting.AppendText(Path.GetFileName(i) + " : success"+Environment.NewLine);
                            }
                            else
                            {
                                tbSetting.AppendText(Path.GetFileName(i) + " : fail"+Environment.NewLine);
                                count++;
                            }

                        
                    }
                    tbSetting.AppendText("Fail: " + count.ToString());
                    MessageBox.Show("Finish");

                }

            }
        }





        //private void btnFacebookLogin_Click(object sender, EventArgs e)
        //{
        //    LoginFacebook();
        //}





      


    }

}
