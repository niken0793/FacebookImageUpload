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
using Facebook; // PM
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using FacebookImageUpload.FB_Images;

namespace FacebookImageUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FB_Image browseImage = new FB_Image();
        string _accessToken = "CAAVUMKQz7ZB0BAJG21S5gyhrBOpK7qvIZCViGPfbwMYckHWhXy8nPYcI5ZBxQOMZCjz5ieT9IydD6hDiE7sLEn6taU3K7ztbmZCgcHohnLqQw3vZAJPdXs5LjefrOEy4fIxQSPWXMe57n1TBCtmqUiWHdr4JSyq5ujBtQXWYlg5pdZBpQJncIvfU2rvPIkslQZCaZAs9ZAf8CRpQZDZD";
        int temp = 0; // biến để đặt tên file tải về
        string _albumid = ""; // lấy album id để up ảnh

        private void openfile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog(); 
                open.Filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    tbImagePath.Text = open.FileName;
                    browseImage.FileName =  Path.GetFileName(tbImagePath.Text); // lấy file name
                    browseImage.FileNameWithOutExtension = Path.GetFileNameWithoutExtension(tbImagePath.Text); // lấy file name ko có phần mở rộng .jpg
                    browseImage.UpFileSize = new FileInfo(tbImagePath.Text).Length; // lấy file size
                    var image_source = Image.FromFile(tbImagePath.Text); 
                    browseImage.Height = image_source.Height;
                    browseImage.Width = image_source.Width;
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
            dynamic albumPost = new ExpandoObject();  // tạo đối tượng
            albumPost.message = "Album desc";  // truyền tham số 
            albumPost.name = "Stegano";
            var fb = new FacebookClient(_accessToken);
            dynamic result = fb.Post("me/albums", albumPost); //request facebook api dạng: /{album-id}?field=message,name
            _albumid = result.id; // lấy album id

        }
        private void uploadImage_Click(object sender, EventArgs e)
        {
            //upload photo
            var imgstream = File.OpenRead(tbImagePath.Text);
            var fb = new FacebookClient(_accessToken);
            dynamic res = fb.Post(_albumid + "/photos", new
            {
                message = "Image description",
                file = new FacebookMediaStream{  
                    ContentType = "image/jpeg",
                    FileName = browseImage.FileName,
                }.SetValue(imgstream)
            });
            browseImage.ImageID = res.id;
            MessageBox.Show("Picture ID is :" + res.id);
        }

        private void downloadImage_Click(object sender, EventArgs e)
        {
            temp++; // biến để đặt tên
            var fb = new FacebookClient(_accessToken);
            dynamic res = fb.Get(browseImage.ImageID + "?fields=images");  // query đường dẫn + độ phân giải ảnh
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
                if (image_height == browseImage.Height && image_width == browseImage.Width)
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
                        yourImage.Save(FB_Image.BaseDirectory + browseImage.FileNameWithOutExtension + "_" + temp + ".jpg", ImageFormat.Jpeg);
                        browseImage.DownFileSize = new FileInfo(FB_Image.BaseDirectory + browseImage.FileNameWithOutExtension + "_" + temp + ".jpg").Length;
                    }
                }

            }
            long ratio = browseImage.UpFileSize / browseImage.DownFileSize; // lấy tỉ lệ
            MessageBox.Show(ratio.ToString());

          
        }

        private void getAlbumlist_Click(object sender, EventArgs e)
        {
            var fb = new FacebookClient(_accessToken);
            dynamic albums = fb.Get("me?fields=albums");
            string json_string = JsonConvert.SerializeObject(albums); // parse response sang json
            var json = JObject.Parse(json_string);
            int i = 0;
            ImageList photoList = new ImageList();
            photoList.TransparentColor = Color.Blue;
            photoList.ColorDepth = ColorDepth.Depth32Bit;
            photoList.ImageSize = new Size(50, 50);

            Dictionary<int, string> dic_albumId = new Dictionary<int, string>();

            foreach (var obj in json["albums"]["data"])
            {
                string albumName = (string)obj["name"];

                dynamic coverPhotos = fb.Get(obj["id"].ToString() + "?fields=cover_photo");
                string coverPhotos_json_string = JsonConvert.SerializeObject(coverPhotos); // parse response sang json
                var coverPhotos_json = JObject.Parse(coverPhotos_json_string);

                string cover_photo_id = (string)coverPhotos_json["id"];
                dynamic cover_link = fb.Get(cover_photo_id + "?fields=picture");
                string cover_link_json_string = JsonConvert.SerializeObject(cover_link); // parse response sang json
                var cover_link_json = JObject.Parse(cover_link_json_string);

                /* download ảnh */
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData((string)cover_link_json["picture"]["data"]["url"]);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var yourImage = Image.FromStream(mem))
                        {
                            yourImage.Save(FB_Image.BaseDirectory + obj["id"].ToString() + ".jpg", ImageFormat.Jpeg);
                            string img_url = FB_Image.BaseDirectory + obj["id"].ToString() + ".jpg";
                            photoList.Images.Add(Image.FromFile(img_url));
                            photoList.Images.SetKeyName(i, albumName);
                            dic_albumId.Add(i, obj["id"].ToString());
                            i++;
                        }
                    }

                }


            }

            this.albumList.View = View.LargeIcon;
            this.albumList.LargeImageList = photoList;

            for (int j = 0; j < photoList.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = photoList.Images.Keys[j].ToString();
                item.Name = dic_albumId[j];
                item.ImageIndex = j;
                this.albumList.Items.Add(item);
            }
        }

        private void albumList_ItemActivate(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to choose this Album for upload?", "Choose Album", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                pictureBox1.Image = (Image)item.ImageList.Images[item.ImageIndex];
                albumName.Text = item.Text.ToString();
                albumId.Text = item.Name.ToString();
            }
        }
    }
}
