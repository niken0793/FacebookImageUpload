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
using System.IO;
using System.Net;
using System.Drawing.Imaging;
// Test code commit
namespace FacebookImageUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string _imageId = ""; // lấy id của image đã up để query đường dẫn và độ phân giải ảnh
        string _accessToken = "CAAVUMKQz7ZB0BAJG21S5gyhrBOpK7qvIZCViGPfbwMYckHWhXy8nPYcI5ZBxQOMZCjz5ieT9IydD6hDiE7sLEn6taU3K7ztbmZCgcHohnLqQw3vZAJPdXs5LjefrOEy4fIxQSPWXMe57n1TBCtmqUiWHdr4JSyq5ujBtQXWYlg5pdZBpQJncIvfU2rvPIkslQZCaZAs9ZAf8CRpQZDZD";
        int image_source_height, image_source_width; // lấy resolution của ảnh gốc
        string _fileName = "", _fileNamewithoutExtension = ""; // lấy tên file của ảnh
        int temp = 0; // biến để đặt tên file tải về
        long _upFileSize, _downFileSize;  // lấy dung lượng up và down để chia ra tỉ lệ
        string _directory = @"E:\Tai lieu UIT\Khoa luan\Test\";  // đường dẫn thư mục download
        string _albumid = ""; // lấy album id để up ảnh
        private void openfile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog(); 
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    imagePath.Text = open.FileName;
                    _fileName = Path.GetFileName(imagePath.Text); // lấy file name
                    _fileNamewithoutExtension = Path.GetFileNameWithoutExtension(imagePath.Text); // lấy file name ko có phần mở rộng .jpg
                    _upFileSize = new FileInfo(imagePath.Text).Length; // lấy file size
                    var image_source = Image.FromFile(imagePath.Text); 
                    image_source_height = image_source.Height; // lấy width
                    image_source_width = image_source.Width;   // lấy height
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
