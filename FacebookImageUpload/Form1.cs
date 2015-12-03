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
            //LoadingAlbumList();
            //LoadingAlbumList_In();

            cmbSelectTextType.SelectedIndex = 0;

            inbox = User.inboxUserA;
            outbox = User.inboxUserB;
            r = new Random();
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
            if(lbUserIdComm.Text!="userid")
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

                var progress = new Progress<string>(s => { Common.ShowProgressBar(s, pbStatus, lbStatusBar, lbDoing); });
                //await Task.Factory.StartNew(() => GetAlbumList_1(progress, inboxAlbums, 5), TaskCreationOptions.LongRunning);

                if (cbIsTested.Checked)
                {
                    string flag = "";
                    //await Task.Factory.StartNew(() => flag=SendMessageWithTestedSource(progress,tbImagePath.Text, messagePath, tbAlbumID.Text), TaskCreationOptions.LongRunning);
                    await Task.Factory.StartNew(() => flag = SendImageWithTag(progress, tbImagePath.Text, messagePath, lbUserIdComm.Text), TaskCreationOptions.LongRunning);
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
                    await Task.Factory.StartNew(() => TestEncodeSuccessRate(progress, tbImagePath.Text, messagePath, "106782166359188", false));
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
                Common.DeleteFile(Common.listFileDelete);
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

        /*private void btnChangeUser_Click(object sender, EventArgs e)
        {

            if (cmbChangeUser.SelectedItem.Equals("User A"))
            {
                inbox = User.inboxUserA;
                outbox = User.inboxUserB;
            }
            else
            {
                inbox = User.inboxUserB;
                outbox = User.inboxUserA;
            }

        }*/

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
                await Task.Factory.StartNew(() => btnImageSearch_Click_fn(progress), TaskCreationOptions.LongRunning);
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
            this.listViewTagImage.Items.Clear();
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; lbStatusBar.Text = s.ToString(); });
                await Task.Factory.StartNew(() => GetImageTagged(progress), TaskCreationOptions.LongRunning);

                this.listViewTagImage.View = View.LargeIcon;
                this.listViewTagImage.LargeImageList = FB_Image.Image_Tags_In;

                for (int j = 0; j < FB_Image.Image_Tags_In.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Name = FB_Image.Image_TagsID_In[j];
                    item.Text = FB_Image.Image_Tags_In.Images.Keys[j].ToString();
                    item.ImageIndex = j;
                    this.listViewTagImage.Items.Add(item);
                }

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

    }

}
