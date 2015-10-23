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

// Huy file

namespace FacebookImageUpload
{

    public partial class Form1 : Form
    {
        public void Upload_Picture_FB(string filename, FB_Image browseImage)
        {
            //upload photo
            try
            {
                var imgstream = File.OpenRead(filename);
                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic res = fb.Post(lbAlbumId.Text + "/photos", new
                {
                    message = "Image description",
                    file = new FacebookMediaStream
                    {
                        ContentType = "image/jpeg",
                        FileName = browseImage.FileName,
                    }.SetValue(imgstream)
                });
                browseImage.ImageID = res.id;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        public void Download_Picture_FACEBOOK(FB_Image browseImage)
        {
            temp++; // biến để đặt tên
            var fb = new FacebookClient(FB_Image.AccessToken);
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
                if (image_height == FB_Image.ImageSize|| image_width == FB_Image.ImageSize)
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
                        string new_path = FB_Image.BaseDirectory + browseImage.FileNameWithOutExtension + "_" + temp + ".jpg";
                        yourImage.Save(new_path, ImageFormat.Jpeg);
                        browseImage.DownFileSize = new FileInfo(FB_Image.BaseDirectory + browseImage.FileNameWithOutExtension + "_" + temp + ".jpg").Length;
                        tbImagePath.Text = new_path;
                    }
                }

            }
        }
    }
}
