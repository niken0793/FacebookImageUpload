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

namespace FacebookImageUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadingAlbumList();

            cmbSelectTextType.SelectedIndex = 0;
            tbInputMessage.Enabled = false;
            r = new Random();
        }

        FB_Image browseImage = new FB_Image();
        int temp = 0; // biến để đặt tên file tải về



        private void openfile_Click(object sender, EventArgs e)
        {
            string filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
            openfile_Click_fn(tbImagePath,filter);
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filter = "Text Files(*.txt)|*.txt";
            openfile_Click_fn(tbMessagePath,filter);
        }

        private void createAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookAlbum album = new FacebookAlbum();
                tbAlbumID.Text = album.createAlbum(FB_Image.AccessToken, tbAlbumDesc.Text, tbAlbumName.Text);
                lbAlbumName.Text = album.getName(FB_Image.AccessToken);
                pBoxAlbumCover.Image = FacebookImageUpload.Properties.Resources._default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void uploadImage_Click(object sender, EventArgs e)
        {
            string messagePath = "";
            tbMessage.AppendText("Input Text: " + Environment.NewLine);
            if (cmbSelectTextType.SelectedIndex == cmbSelectTextType.Items.IndexOf("From File"))
            {
                messagePath = tbMessagePath.Text;
                tbMessage.AppendText(File.ReadAllText(tbMessagePath.Text));
                tbMessage.AppendText(Environment.NewLine);
            }
            else if (cmbSelectTextType.SelectedIndex == cmbSelectTextType.Items.IndexOf("From Text"))
            {
                string tempMessagePath = Path.Combine(FB_Image.RelativeDirectory, "temp", "temp_message.txt");
                File.WriteAllText(tempMessagePath, tbInputMessage.Text);
                messagePath = tempMessagePath;

            }
            if (cbIsTested.Checked)
            {
                string flag = SendMessageWithTestedSource(tbImagePath.Text, messagePath, tbAlbumID.Text);
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
                string flag = TestEncodeSuccessRate(tbImagePath.Text, messagePath, tbAlbumID.Text,false);
                if (flag != null)
                {
                    MessageBox.Show("The message has been sent");
                }
                else
                {
                    MessageBox.Show("This image is not ready for sending your message");
                }
            }
            
            
            
        }


        private async void btngetAlbumlist_Click(object sender, EventArgs e)
        {
            btngetAlbumlist.Enabled = false;
            pbStatus.Maximum = 100;
            pbStatus.Step = 1;
            //List<string> inboxAlbums = new List<string> { "1661869480692559", "1658361464376694", "1661205730758934" };
            List<string> inboxAlbums = null;
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; lbImagePath.Text = s.ToString(); });
                await Task.Factory.StartNew(() => GetAlbumList_1(progress,inboxAlbums,5), TaskCreationOptions.LongRunning);
                
                this.ListViewalbumList.View = View.LargeIcon;
                this.ListViewalbumList.LargeImageList = FB_Image.Album_PhotoList;

                for (int j = 0; j < FB_Image.Album_PhotoList.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Name = FB_Image.List_AlbumID[j];
                   
                    if (FB_Image.List_AlbumInfo[j].NewNumber > 0)
                    {
                        item.Text = string.Format("{0} ({1})", FB_Image.Album_PhotoList.Images.Keys[j].ToString(), FB_Image.List_AlbumInfo[j].NewNumber);
                    }
                    else
                    {
                        item.Text = FB_Image.Album_PhotoList.Images.Keys[j].ToString();
                    }
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

        }

        private void ListViewalbumList_ItemActivate(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to choose this Album for upload?", "Choose Album", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                pBoxAlbumCover.Image = (Image)item.ImageList.Images[item.ImageIndex];
                lbAlbumName.Text = item.Text.ToString();
                tbAlbumID.Text = item.Name.ToString();
            }
        }

        private async void btnAuto_Click(object sender, EventArgs e)
        {

           
            pbStatus.Value = 0;
            List<FB_Image> listFile = new List<FB_Image>();
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; });
                await Task.Factory.StartNew(() => listFile= AutoUploadAndDownload(tbImagePath.Text,progress), TaskCreationOptions.LongRunning);

            }
            catch (Exception ex)
            {
                Log(ex);
            }

          
            if (listFile.Count > 0)
            {
                foreach (FB_Image i in listFile)
                {
                    tbMessage.AppendText(i.ToString());
                }
            }


        }


        private  void btnTask_Click(object sender, EventArgs e)
        {

            //var a = ListViewalbumList.SelectedItems;
            //if (a.Count > 0)
            //{
            //    foreach (ListViewItem i in a)
            //    {
            //        string albumID = i.Name;
            //        List<FB_Message> listMessage = GetNewMessageFromAlbum(albumID);
            //        foreach (FB_Message m in listMessage)
            //        {
            //            if (m.Content != "")
            //            {
            //                tbMessage.AppendText("Output of " + m.Image.FileName + " : " + Environment.NewLine);
            //                tbMessage.AppendText(m.Content + Environment.NewLine);
            //            }
            //            else
            //            {
            //                tbMessage.AppendText("Output of " + m.Image.FileName + " : " + Environment.NewLine);
            //                tbMessage.AppendText("No ouput" + Environment.NewLine);
            //            }
                        
            //        }

            //    }
            //    MessageBox.Show("get message is finished");
            //}

            //string crc1 = Crc32Hash(Path.Combine(FB_Image.BaseDirectory, "test.txt"));
            //string crc2 = Crc32Hash(Path.Combine(FB_Image.BaseDirectory, "test1.txt"));

            string input = InsertCrc32("test.txt");
            tbMessage.AppendText(File.ReadAllText(input) + Environment.NewLine);
            string output = CheckCrc32(input);
            if (output != null)
                tbMessage.AppendText(File.ReadAllText(output));

            
        }

        private void cmbSelectTextType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectTextType_SelectedIndexChangedHandle(sender, e);
        }



    }
}
