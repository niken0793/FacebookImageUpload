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
using Facebook;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
namespace FacebookImageUpload
{
    // test commit code
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string _imageId = "";
        string _accessToken = "CAAVUMKQz7ZB0BAJG21S5gyhrBOpK7qvIZCViGPfbwMYckHWhXy8nPYcI5ZBxQOMZCjz5ieT9IydD6hDiE7sLEn6taU3K7ztbmZCgcHohnLqQw3vZAJPdXs5LjefrOEy4fIxQSPWXMe57n1TBCtmqUiWHdr4JSyq5ujBtQXWYlg5pdZBpQJncIvfU2rvPIkslQZCaZAs9ZAf8CRpQZDZD";
        int image_source_height, image_source_width;
        string _fileName = "", _fileNamewithoutExtension = "";
        int temp = 0;
        long _upFileSize, _downFileSize;
        string _directory = @"E:\Tai lieu UIT\Khoa luan\Test\";
        string _albumid = "";
        private void openfile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    imagePath.Text = open.FileName;
                    _fileName = Path.GetFileName(imagePath.Text);
                    _fileNamewithoutExtension = Path.GetFileNameWithoutExtension(imagePath.Text);
                    _upFileSize = new FileInfo(imagePath.Text).Length;
                    var image_source = Image.FromFile(imagePath.Text);
                    image_source_height = image_source.Height;
                    image_source_width = image_source.Width;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }

        private void createAlbum_Click(object sender, EventArgs e)
        {
            //create album
            dynamic albumPost = new ExpandoObject();
            albumPost.message = "Album desc";
            albumPost.name = "Stegano";
            var fb = new FacebookClient(_accessToken);
            dynamic result = fb.Post("me/albums", albumPost);
            _albumid = result.id;

        }
        private void uploadImage_Click(object sender, EventArgs e)
        {
            //upload photo
            var imgstream = File.OpenRead(imagePath.Text);
            var fb = new FacebookClient(_accessToken);
            dynamic res = fb.Post(_albumid + "/photos", new
            {
                message = "Image description",
                file = new FacebookMediaStream{
                    ContentType = "image/jpeg",
                    FileName = _fileName,
                }.SetValue(imgstream)
            });
            _imageId = res.id;        
        }

        private void downloadImage_Click(object sender, EventArgs e)
        {
            temp++;
            var fb = new FacebookClient(_accessToken);
            dynamic res = fb.Get(_imageId + "?fields=images");
            string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            var json = JObject.Parse(json_string);
            int i =0;
            string source_url = "";
            while ((string)json["images"][i].ToString()!=null)
            {
                int image_height = Int32.Parse((string)json["images"][i]["height"]);
                int image_width = Int32.Parse((string)json["images"][i]["width"]);
                if (image_height == image_source_height && image_width == image_source_width)
                {
                    source_url = (string)json["images"][i]["source"];
                    break;
                }
                i++;

            }
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(source_url);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        yourImage.Save(_directory + _fileNamewithoutExtension + "_" + temp + ".jpg", ImageFormat.Jpeg);
                        _downFileSize = new FileInfo(_directory + _fileNamewithoutExtension + "_" + temp + ".jpg").Length;
                    }
                }

            }
            long ratio = _upFileSize / _downFileSize ;
            MessageBox.Show(ratio.ToString());
        }

    }
}
