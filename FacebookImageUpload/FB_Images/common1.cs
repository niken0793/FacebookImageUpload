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
using FacebookImageUpload.FB_Images;
using System.Xml;
using System.Xml.Serialization;

// Huy file

namespace FacebookImageUpload
{

    public partial class Form1 : Form
    {
        private void Upload_Picture_FB(string filename, FB_Image browseImage)
        {
            //upload photo
            try
            {
                browseImage.FileName = Path.GetFileName(filename); // lấy file name
                browseImage.FileNameWithOutExtension = Path.GetFileNameWithoutExtension(filename); // lấy file name ko có phần mở rộng .jpg
                browseImage.UpFileSize = new FileInfo(filename).Length; // lấy file size
                var image_source = Image.FromFile(filename);
                browseImage.Height = image_source.Height;
                browseImage.Width = image_source.Width;

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

        private void Download_Picture_FACEBOOK(ref string filename, FB_Image browseImage)
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
                        filename = new_path;

                    }
                }

            }
        }

        public void AutoUploadAndDownload(IProgress<int> progress, string filename, FB_Image browseImage, ref float ratio)
        {
            while (ratio > FB_Image.RatioMax)
            {
                Upload_Picture_FB(filename, browseImage);
                Download_Picture_FACEBOOK(ref filename, browseImage);
                ratio = ((float)browseImage.UpFileSize / (float)browseImage.DownFileSize);
                float t = ratio;
                lbImagePath.Invoke(new Action(() => lbImagePath.Text = "Your ratio is: " + t.ToString()));
                if (ratio > 9)
                {
                    progress.Report(10);
                }
                else if(ratio>=2)
                {
                    progress.Report((int)(ratio * 10));
                }
                else if (ratio >= 1 && ratio < 2)
                {
                    progress.Report((int)((ratio - 1) * 100));
                }

            }
            progress.Report(100);
        }


        public void GetAlbumList(IProgress<int> progress,int limit =5)
        {
            try
            {

                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic albums = fb.Get("me?fields=albums.limit(5)"); // Get album information
                string json_string = JsonConvert.SerializeObject(albums); // parse response sang json
                var json = JObject.Parse(json_string);
                int i = 0;
                ImageList photoList = new ImageList(); // ImageList
                photoList.TransparentColor = Color.Blue;
                photoList.ColorDepth = ColorDepth.Depth32Bit;
                photoList.ImageSize = new Size(50, 50);
                dynamic a_albums = json["albums"]["data"];
                int count = a_albums.Count;
                List<string> list_albumID = new List<string>();

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
                                FB_Image.List_AlbumInfo.Add(new AlbumInfo(obj["id"].ToString(), albumName, img_url));
                                list_albumID.Add(obj["id"].ToString());
                                progress.Report((i * 100 / count));
                                i++;
                            }
                        }

                    }


                }

                FB_Image.Album_PhotoList = photoList;
                FB_Image.List_AlbumID = list_albumID;

                progress.Report(100);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public void LoadingAlbumList()
        {
            string filename = Path.Combine(FB_Image.RelativeDirectory,FB_Image.AlbumDirectory);
            FB_Image.List_AlbumInfo = Common.DeSerializeObject<List<AlbumInfo>>(filename);
            if (FB_Image.List_AlbumInfo.Count > 0)
            {
                FB_Image.List_AlbumID = new List<string>();
                FB_Image.Album_PhotoList = new ImageList();
                FB_Image.Album_PhotoList.TransparentColor = Color.Blue;
                FB_Image.Album_PhotoList.ColorDepth = ColorDepth.Depth32Bit;
                FB_Image.Album_PhotoList.ImageSize = new Size(50, 50);
                for (int i = 0; i < FB_Image.List_AlbumInfo.Count; i++)
                {
                    AlbumInfo e = FB_Image.List_AlbumInfo[i];
                    FB_Image.List_AlbumID.Add(e.id);
                    FB_Image.Album_PhotoList.Images.Add(Image.FromFile(e.path));
                    FB_Image.Album_PhotoList.Images.SetKeyName(i, e.name);

                }
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



            }

        }

   
        public static void Log(Exception message)
        {
            try
            {
                string error_string = "";
                DateTime date = DateTime.Now;
                String timeStamp = date.ToString("dd-MM-yyyy HH:mm:ss");
                error_string += ("[" + timeStamp + "] " + message.Message + "\r\n");
                error_string += ("[" + timeStamp + "] " + message.TargetSite + "\r\n");
                error_string += ("[" + timeStamp + "] " + message.StackTrace + "\r\n");
                error_string += ("[" + timeStamp + "] " + message.Source + "\r\n");
                error_string += "\r\n";

                string logFile = date.Day + "-" + date.Month + "-" + date.Year + ".txt";
                logFile = Path.Combine(FB_Image.RelativeDirectory, FB_Image.LogDirectory, logFile);
                Directory.CreateDirectory(Path.Combine(FB_Image.RelativeDirectory, FB_Image.LogDirectory));
                bool flag = File.Exists(logFile);
                if (!flag)
                {
                    File.Create(logFile).Dispose();
                }
                using (var des = File.AppendText(logFile))
                {
                    des.Write(error_string);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }


    public class Common
    {

        public static void PreparePath(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            string file = Path.GetFileName(filename);
            if (Directory.Exists(directory))
            {
                if (!File.Exists(filename))
                {
                    File.Create(filename).Dispose();
                }
            }
            else
            {
                Directory.CreateDirectory(directory);
                File.Create(filename).Dispose();

            }
            
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                PreparePath(fileName);
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Form1.Log(ex);
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                PreparePath(fileName);
                string attributeXml = string.Empty;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                Form1.Log(ex);
            }

            return objectOut;
        }
    }
}
