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
            //LoadingAlbumList();
        }

        FB_Image browseImage = new FB_Image();
        int temp = 0; // biến để đặt tên file tải về



        private void openfile_Click(object sender, EventArgs e)
        {

            openfile_Click_fn();
           
        }

        private void createAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookAlbum album = new FacebookAlbum();
                lbAlbumId.Text = album.createAlbum(FB_Image.AccessToken, tbAlbumDesc.Text, tbAlbumName.Text);
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
            #region comment1
            //upload photo
            //try
            //{
            //    var imgstream = File.OpenRead(tbImagePath.Text);
            //    var fb = new FacebookClient(FB_Image.AccessToken);
            //    dynamic res = fb.Post(lbAlbumId.Text + "/photos", new
            //    {
            //        message = "Image description",
            //        file = new FacebookMediaStream
            //        {
            //            ContentType = "image/jpeg",
            //            FileName = browseImage.FileName,
            //        }.SetValue(imgstream)
            //    });
            //    browseImage.ImageID = res.id;
            //    MessageBox.Show("Picture ID is :" + res.id);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            #endregion
            Upload_Picture_FB(tbImagePath.Text, browseImage);
            MessageBox.Show("Your picture ID is: " + browseImage.ImageID);
            
            
        }

        private void downloadImage_Click(object sender, EventArgs e)
        {

            #region commnet1
            //temp++; // biến để đặt tên
            //var fb = new FacebookClient(FB_Image.AccessToken);
            //dynamic res = fb.Get(browseImage.ImageID + "?fields=images");  // query đường dẫn + độ phân giải ảnh
            //string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res); // parse response sang json
            //dynamic json = JObject.Parse(json_string);
            //dynamic imagesJson = json["images"];
            //int count = imagesJson.Count;
            //int i = 0;
            //string source_url = "";

            ///* lấy độ phân giải trùng với độ phân giải của ảnh đã up */
            //while (i < count && imagesJson[i] != null)
            //{
            //    int image_height = Int32.Parse((string)imagesJson[i]["height"]);
            //    int image_width = Int32.Parse((string)imagesJson[i]["width"]);
            //    if (image_height == browseImage.Height && image_width == browseImage.Width)
            //    {
            //        source_url = imagesJson[i]["source"];
            //        break;
            //    }
            //    i++;

            //}
            //if (i >= count)
            //{
            //    source_url = imagesJson[0]["source"];
            //}
           
            ///*------*/
            ///* download ảnh */
            //using (WebClient webClient = new WebClient())
            //{
            //    byte[] data = webClient.DownloadData(source_url);

            //    using (MemoryStream mem = new MemoryStream(data))
            //    {
            //        using (var yourImage = Image.FromStream(mem))
            //        {
            //            yourImage.Save(FB_Image.BaseDirectory + browseImage.FileNameWithOutExtension + "_" + temp + ".jpg", ImageFormat.Jpeg);
            //            browseImage.DownFileSize = new FileInfo(FB_Image.BaseDirectory + browseImage.FileNameWithOutExtension + "_" + temp + ".jpg").Length;
            //        }
            //    }

            //}
            #endregion
            string file = tbImagePath.Text;

            Download_Picture_FACEBOOK(ref file, browseImage);
            float ratio = (float)(browseImage.UpFileSize / browseImage.DownFileSize); // lấy tỉ lệ
            MessageBox.Show(ratio.ToString());        
        }

        private async void btngetAlbumlist_Click(object sender, EventArgs e)
        {
            btngetAlbumlist.Enabled = false;
            pbStatus.Maximum = 100;
            pbStatus.Step = 1;
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; lbImagePath.Text = s.ToString(); });
                await Task.Factory.StartNew(() => GetAlbumList(progress), TaskCreationOptions.LongRunning);
                
                this.ListViewalbumList.View = View.LargeIcon;
                this.ListViewalbumList.LargeImageList = FB_Image.Album_PhotoList;

                for (int j = 0; j < FB_Image.Album_PhotoList.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = FB_Image.Album_PhotoList.Images.Keys[j].ToString();
                    item.Name = FB_Image.List_AlbumID[j];
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
                lbAlbumId.Text = item.Name.ToString();
            }
        }

        private async void btnAuto_Click(object sender, EventArgs e)
        {
            float ratio = 5.0F;
            btnAuto.Enabled = false;
            pbStatus.Value = 0;
            try
            {
                var progress = new Progress<int>(s => { pbStatus.Value = s; });
                await Task.Factory.StartNew(() => AutoUploadAndDownload(progress,tbImagePath.Text,browseImage,ref ratio), TaskCreationOptions.LongRunning);

            }
            catch (Exception ex)
            {
                Log(ex);
            }

            btnAuto.Enabled = true;
            MessageBox.Show("Ratio is " + ratio);
            lbImageName.Text = browseImage.FileName;
            lbImagePath.Text = browseImage.DownFileSize.ToString();

        }

        private static StringBuilder sortOutput = null;
       private static StreamWriter sortStreamWriter = null;
        private  void btnTask_Click(object sender, EventArgs e)
        {

            Process sortProcess;
            sortProcess = new Process();
            sortProcess.StartInfo.FileName = "cmd.exe";
            sortProcess.StartInfo.WorkingDirectory = Path.Combine(FB_Image.RelativeDirectory,"Lib");
            sortProcess.StartInfo.Arguments = "/C jphide cipher1.jpg cipher3.jpg hello.txt";

            // Set UseShellExecute to false for redirection.
            sortProcess.StartInfo.UseShellExecute = false;

            // Redirect the standard output of the sort command.  
            // This stream is read asynchronously using an event handler.
            sortProcess.StartInfo.RedirectStandardOutput = true;
            sortProcess.StartInfo.CreateNoWindow = true;
            sortOutput = new StringBuilder("");

            // Set our event handler to asynchronously read the sort output.
            sortProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

            // Redirect standard input as well.  This stream
            // is used synchronously.
            sortProcess.StartInfo.RedirectStandardInput = true;
            
            // Start the process.
            sortProcess.Start();
            // Use a stream writer to synchronously write the sort input.
          

            // Start the asynchronous read of the sort output stream.
            sortProcess.BeginOutputReadLine();
            sortStreamWriter = sortProcess.StandardInput;
            sortStreamWriter.WriteLine("abc");
            sortStreamWriter.WriteLine("\n");



            
        }
        private static int n = 0;
        private static void SortOutputHandler(object sendingProcess,
           DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                n = 1;
                sortOutput.Append(outLine.Data);
                MessageBox.Show(sortOutput.ToString());
                sortStreamWriter.WriteLine("123");
            }
        }

        private void btnJPHide_Click(object sender, EventArgs e)
        {
            btnJPHide_Click_fn();
        }

        private void fdJpgImageOpen_Click(object sender, EventArgs e)
        {
            fdJpgImageOpen_Click_fn();
        }

        private void fdHiddenFile_Click(object sender, EventArgs e)
        {
            fdHiddenFile_Click_fn();
        }



        private void btnJPSeek_Click(object sender, EventArgs e)
        {
            btnJPSeek_Click_fn();
        }


        private void btnStegoRun_Click(object sender, EventArgs e)
        {

        }

        private void btnCrc32_Click(object sender, EventArgs e)
        {
            btnCrc32_Click_fn();
        }

    }
}
