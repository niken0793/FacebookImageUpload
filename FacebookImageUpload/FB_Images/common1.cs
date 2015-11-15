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
using System.Diagnostics;
using System.Text.RegularExpressions;

// Huy file

namespace FacebookImageUpload
{

    public partial class Form1 : Form
    {
        Random r;
        private string Upload_Picture_FB(string filename, FB_Image browseImageUP,string albumID)
        {
            //upload photo
            try
            {
                browseImageUP.FileName = Path.GetFileName(filename); // lấy file name
                browseImageUP.Directory = Path.GetDirectoryName(filename); // get path
                browseImageUP.FileNameWithOutExtension = Path.GetFileNameWithoutExtension(filename); // lấy file name ko có phần mở rộng .jpg
                browseImageUP.FileSize = new FileInfo(filename).Length; // lấy file size
                var image_source = Image.FromFile(filename);
                browseImageUP.Height = image_source.Height;
                browseImageUP.Width = image_source.Width;

                var imgstream = File.OpenRead(filename);
                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic res = fb.Post(albumID + "/photos", new
                {
                    message = "Image description",
                    file = new FacebookMediaStream
                    {
                        ContentType = "image/jpeg",
                        FileName = browseImageUP.FileName,
                    }.SetValue(imgstream)
                });
               return res.id;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }   
        }

        private string Download_Picture_FB(FB_Image browseImageDown)
        {
            try
            {
                temp++; // biến để đặt tên
                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic res = fb.Get(browseImageDown.ImageID + "?fields=images");  // query đường dẫn + độ phân giải ảnh
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
                    if (image_height == FB_Image.ImageSize || image_width == FB_Image.ImageSize)
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
                            string new_path = FB_Image.BaseDirectory + browseImageDown.FileNameWithOutExtension + "_" + temp + ".jpg";
                            yourImage.Save(new_path, ImageFormat.Jpeg);
                            browseImageDown.FileSize = new FileInfo(FB_Image.BaseDirectory + browseImageDown.FileNameWithOutExtension + "_" + temp + ".jpg").Length;
                            string filename = new_path;
                            browseImageDown.FileName = Path.GetFileName(new_path);
                            browseImageDown.Directory = Path.GetDirectoryName(new_path);
                            return filename;
                            

                        }
                    }

                }
             
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }
        public string TestEncodeSuccessRate(string filename,string inputText,string albumID,bool test = true)
        {
            if (test)
            {
                inputText = Path.Combine(FB_Image.BaseDirectory, "test.txt");
                albumID = FB_Image.Album_Test;
            }

            //Reduce size ratio of picture
            List<FB_Image> listFile = AutoUploadAndDownload(filename, null);
            FB_Image coverImage = listFile[listFile.Count - 1];
            string coverImageFileName = coverImage.FileName;
            string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

            //Encode
            string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName),Path.GetFileName(inputText));
            FB_Image encodeImage = new FB_Image();
            string id = Upload_Picture_FB(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,albumID);
            FB_Image downloadImage = new FB_Image();
            encodeImage.CopyTo(downloadImage);
            downloadImage.ImageID = id;
            string tempFileName = Download_Picture_FB(downloadImage);
            //Decode
            string outputText = Common.AppenFileName(inputText, "_ouput");
            if (outputText == null)
                outputText = "output_test.txt";
            JPSeekDecode(Path.GetFileName(tempFileName),outputText);
            outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

            //compare 2 file
            if (Common.CompareOutputFile(inputText, outputText, tbMessage))
            {
                return downloadImage.ImageID;
            }
            else
            {
                return null;
            }    
        }

        private string SendMessageWithTestedSource(string filename, string inputText, string albumID)
        {

            string coverImage = Common.CopyFileTo(filename, FB_Image.BaseDirectory);
            string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

            //Encode
            string encodeFile = JPHideEncode(Path.GetFileName(coverImage),Path.GetFileName(messageFile));
            FB_Image encodeImage = new FB_Image();
            string id = Upload_Picture_FB(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage, albumID);
            FB_Image downloadImage = new FB_Image();
            encodeImage.CopyTo(downloadImage);
            downloadImage.ImageID = id;
            string tempFileName = Download_Picture_FB(downloadImage);
            //Decode
            string outputText = Common.AppenFileName(inputText,"_ouput");
            if(outputText == null)
                outputText="output_test.txt";
            outputText= JPSeekDecode(Path.GetFileName(tempFileName),outputText);
            outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

            //compare 2 file
            if (Common.CompareOutputFile(inputText, outputText, tbMessage))
            {
                return downloadImage.ImageID;
            }
            else
            {
                return null;
            }    
        }

        public string JPHideEncode(string filename,string input)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(input) != "")
                {
                    throw new Exception("No need for full path");
                }

                input=Path.GetFileName(InsertCrc32(input));
                
                string enImageName = Path.GetFileNameWithoutExtension(filename)+"_encode"+Path.GetExtension(filename);
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jphide_modify " + filename + " " + enImageName + " " + input;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;

                proc.Start();
                while (!proc.StandardError.EndOfStream)
                {
                    string line = proc.StandardError.ReadLine();
                    //tbMessage.AppendText(line);
                }
                return enImageName;
              
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        private string InsertCrc32(string input)
        {

            string input_path = Path.Combine(FB_Image.BaseDirectory, input);
            string hash = Crc32Hash(input_path);
            string crc = "[crc:" + hash + "]";
            if (File.Exists(input_path))
            {
                string currentContent = File.ReadAllText(input_path);
                string newinput = Path.Combine(FB_Image.BaseDirectory, Common.AppenFileName(input, "_" + (r.Next(1337)).ToString()));
                File.WriteAllText(newinput, crc + currentContent);
                return newinput;
            }
            else
            {
                return null;
            }
        }

        public string JPSeekDecode(string filename,string output)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(output) != "")
                {
                    throw new Exception("No need for full path");
                }
                string imageName = filename;
                string hiddenFileName = output;
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jpseek_modify " + imageName + " " + hiddenFileName;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                while (!proc.HasExited)
                    ;

                hiddenFileName = CheckCrc32(hiddenFileName);
                if (hiddenFileName != null)
                    return Path.GetFileName(hiddenFileName);
                else
                    return null;
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public string CheckCrc32(string filename)
        {
            string oldpath = Path.Combine(FB_Image.BaseDirectory, filename);
            string newpath = Path.Combine(FB_Image.BaseDirectory, Common.AppenFileName(filename, "_check"));
            string crc = "";
            Regex g = new Regex(@"(\[)(crc)(:).*?(\])");

            using (StreamWriter w = new StreamWriter(newpath))
            {

                using (StreamReader r = new StreamReader(oldpath))
                {
                    string line;
                    line = r.ReadToEnd();

                    Match m = g.Match(line);
                    if (m.Success)
                    {
                        string[] words = line.Split(':');
                        crc = words[1].Remove(8);
                        line = line.Remove(0, 14);
                        w.Write(line);
                    }
                }


            }
            string hash = Crc32Hash(newpath);
            if (hash == crc)
            {
                tbMessage.Text = File.ReadAllText(newpath);
                return newpath;
            }
            else
            {
                return null;
            }
        }

        public List<FB_Image> AutoUploadAndDownload(string filename,IProgress<int> progress = null)
        {
            string tempName = filename;
            List<FB_Image> uploadedImage = new List<FB_Image>();      
            FB_Image currentImage = new FB_Image();
            float ratio = 15;
            while (ratio > FB_Image.RatioMax)
            {
                string id = Upload_Picture_FB(tempName, currentImage,FB_Image.Album_Test);
                if (currentImage.ImageID.Equals(""))
                {
                    FB_Image temp = new FB_Image();
                    currentImage.CopyTo(temp);
                    uploadedImage.Add(temp);
                }

                FB_Image downImage = new FB_Image();
                currentImage.CopyTo(downImage);
                downImage.ImageID = id;
                tempName= Download_Picture_FB(downImage);
                ratio = ((float)currentImage.FileSize / (float)downImage.FileSize);
                FB_Image k = new FB_Image();
                downImage.CopyTo(k);
                uploadedImage.Add(k);
                downImage.CopyTo(currentImage);
                if (progress != null)
                {
                    float t = ratio;
                    lbStatusBar.Invoke(new Action(() => lbStatusBar.Text = "Your ratio is: " + t.ToString()));
                    if (ratio > 9)
                    {
                        progress.Report(10);
                    }
                    else if (ratio >= 2)
                    {
                        progress.Report((int)(ratio * 10));
                    }
                    else if (ratio >= 1 && ratio < 2)
                    {
                        progress.Report((int)((ratio - 1) * 100));
                    }
                }

            }
            if(progress != null)
                progress.Report(100);

            return uploadedImage;
        }


        /*
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
                FB_Image.List_AlbumInfo = new List<AlbumInfo>();

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

        /*            using (WebClient webClient = new WebClient())
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
        
         */


        public void GetAlbumList_1(IProgress<int> progress, List<string> inboxAlbums = null, int limit = 5)
        {
            try
            {

                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic albums = fb.Get("me/albums?fields=count,created_time,updated_time,name,cover_photo,picture&limit=10&date_format=U"); // Get album information
                string json_string = JsonConvert.SerializeObject(albums); // parse response sang json
                var json = JObject.Parse(json_string);
                int i = 0;
                ImageList photoList = new ImageList(); // ImageList
                photoList.TransparentColor = Color.Blue;
                photoList.ColorDepth = ColorDepth.Depth32Bit;
                photoList.ImageSize = new Size(50, 50);
                dynamic a_albums = json["data"];
                int count = a_albums.Count;
                List<string> list_albumID = new List<string>();

                foreach (var obj in json["data"])
                {
                    string albumName = (string)obj["name"];
                    string albumID = (string )obj["id"];
                    string createdTime = (string)obj["created_time"];
                    string updatedTime = (string)obj["updated_time"];
                    createdTime= createdTime.TrimEnd(')');
                    updatedTime= updatedTime.TrimEnd(')');
                    
                    string img_url ="";
                    if (inboxAlbums != null)        // chỉ kiểm tra các album đã quy định trước
                    {
                        var match = inboxAlbums.Where(x => x.Equals(albumID));
                        if (match.Count()==0)
                            continue;
                    }

                    var match1 = FB_Image.List_AlbumInfo_In.Where(x => x.Id.Equals(albumID)); // album đã có thông tin hay chưa
                    AlbumInfo info = null;
                    if (match1.Count() == 1)
                    {
                        info = match1.First();
                    }
             
                    /* download ảnh */
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] data = webClient.DownloadData((string)obj["picture"]["data"]["url"]);

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            using (var yourImage = Image.FromStream(mem))
                            {
                                img_url = FB_Image.BaseDirectory + obj["id"].ToString()+(r.Next()).ToString() + ".jpg";
                                yourImage.Save(img_url, ImageFormat.Jpeg);
                               
                                photoList.Images.Add(Image.FromFile(img_url));
                                photoList.Images.SetKeyName(i, albumName);
                            }
                        }

                    }


                    // Xử lý thông tin albums
                    if (info == null)
                    {
                        FB_Image.List_AlbumInfo_In.Add(
                            new AlbumInfo(
                                obj["id"].ToString(),
                                albumName,
                                img_url,
                                Int32.Parse(obj["count"].ToString()),
                                long.Parse(createdTime),
                                long.Parse(updatedTime),
                                Int32.Parse(obj["count"].ToString())
                                ));
                    }
                    else
                    {
                        int newCount = Int32.Parse(obj["count"].ToString());
                        if (info.Count < newCount)
                        {
                            info.NewNupdatedTime = long.Parse(createdTime);
                            info.NewNumber = newCount - info.Count;
                        }
                        else
                        {
                            info.NewNupdatedTime = long.Parse(updatedTime);
                            info.NewNumber = 0;
                        }
                    }
                    list_albumID.Add(obj["id"].ToString());
                    progress.Report((i * 100 / count));
                    i++;


                }

                FB_Image.Album_PhotoList_In = photoList;
                FB_Image.List_AlbumID_In = list_albumID;

                progress.Report(100);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public void GetAlbumList_Outbox(IProgress<int> progress, List<string> listAlbums = null, int limit = 5)
        {
            try
            {

                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic albums = fb.Get("me/albums?fields=count,created_time,updated_time,name,cover_photo,picture&limit=10&date_format=U)"); // Get album information
                string json_string = JsonConvert.SerializeObject(albums); // parse response sang json
                var json = JObject.Parse(json_string);
                int i = 0;
                ImageList photoList = new ImageList(); // ImageList
                photoList.TransparentColor = Color.Blue;
                photoList.ColorDepth = ColorDepth.Depth32Bit;
                photoList.ImageSize = new Size(50, 50);
                dynamic a_albums = json["data"];
                int count = a_albums.Count;
                List<string> list_albumID = new List<string>();
                FB_Image.List_AlbumInfo = new List<AlbumInfo>();

                foreach (var obj in json["data"])
                {
                    string albumName = (string)obj["name"];
                    string albumID = (string)obj["id"];
                    if (listAlbums != null)        // chỉ kiểm tra các album đã quy định trước
                    {
                        var match = listAlbums.Where(x => x.Equals(albumID));
                        if (match.Count() == 0)
                            continue;
                    }
                    string img_url = "";

                    /* download ảnh */
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] data = webClient.DownloadData((string)obj["picture"]["data"]["url"]);

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            using (var yourImage = Image.FromStream(mem))
                            {
                                img_url = FB_Image.BaseDirectory + obj["id"].ToString()+(r.Next()).ToString() + ".jpg";
                                yourImage.Save(img_url, ImageFormat.Jpeg);
                                
                                photoList.Images.Add(Image.FromFile(img_url));
                                photoList.Images.SetKeyName(i, albumName);
                            }
                        }

                    }


                    // Xử lý thông tin albums
                        FB_Image.List_AlbumInfo.Add(
                            new AlbumInfo(
                                obj["id"].ToString(),
                                albumName,
                                img_url
                                ));
                        list_albumID.Add(obj["id"].ToString());
                    progress.Report((i * 100 / count));
                    i++;
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

        private List<FB_Message> GetNewMessageFromAlbum(string albumID)
        {
            AlbumInfo album = Common.GetAlbumInfoByID(FB_Image.List_AlbumInfo_In, albumID);
            if (album.NewNumber > 0)
            {
                string limit = "&limit=" + album.NewNumber;
                string since = "&since=" + album.CreatedTime;
                string dateFormat = "&date_format=U";
                List<FB_Message> ListMessages = new List<FB_Message>();

                var fb = new FacebookClient(FB_Image.AccessToken);
                dynamic albums = fb.Get(albumID+"/photos?pretty=1"+limit+since+dateFormat); // Get album information
                string json_string = JsonConvert.SerializeObject(albums); // parse response sang json
                var json = JObject.Parse(json_string);
                dynamic photos = json["data"];
                int count = photos.Count;
                if (count > 0)
                {
                    foreach (var obj in photos)
                    {
                        FB_Message message = new FB_Message();
                        FB_Image messageImage = new FB_Image();
                        dynamic images = obj["images"];
                        int images_count = images.Count;
                        if (images_count > 0)
                        {
                        
                            foreach (var m in obj["images"])
                            {
                                if (((string)m["width"]).Equals("960"))
                                {
                                    string img_path;

                                    /* download ảnh */
                                    using (WebClient webClient = new WebClient())
                                    {
                                        byte[] data = webClient.DownloadData((string)m["source"]);

                                        using (MemoryStream mem = new MemoryStream(data))
                                        {
                                            using (var yourImage = Image.FromStream(mem))
                                            {
                                                img_path = FB_Image.BaseDirectory + obj["id"].ToString() + ".jpg";
                                                yourImage.Save(img_path, ImageFormat.Jpeg);
                                                messageImage.FileName = Path.GetFileName(img_path);
                                                messageImage.Directory = Path.GetDirectoryName(img_path);
                                                messageImage.ImageID = obj["id"].ToString();
                                                break;

                                            }
                                        }

                                    }


                                }



                            }

                        }

                        message.Image = messageImage;
                        if (message.Image.FileName != "")
                        {
                            string output = JPSeekDecode(message.Image.FileName, Path.GetFileNameWithoutExtension(message.Image.FileName) + ".txt");
                            if (output != null && output != "")
                            {
                                // check CRC
                                message.Content = File.ReadAllText(Path.Combine(FB_Image.BaseDirectory,output));
                            }
                            
                        }
                        ListMessages.Add(message);
                    }
                    return ListMessages;

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public void LoadingAlbumList()
        {
            string filename = Path.Combine(FB_Image.RelativeDirectory,FB_Image.AlbumDirectory);
            FB_Image.List_AlbumInfo = Common.DeSerializeObject<List<AlbumInfo>>(filename);
            if (FB_Image.List_AlbumInfo != null && FB_Image.List_AlbumInfo.Count > 0)
            {
                FB_Image.List_AlbumID = new List<string>();
                FB_Image.Album_PhotoList = new ImageList();
                FB_Image.Album_PhotoList.TransparentColor = Color.Blue;
                FB_Image.Album_PhotoList.ColorDepth = ColorDepth.Depth32Bit;
                FB_Image.Album_PhotoList.ImageSize = new Size(50, 50);
                for (int i = 0; i < FB_Image.List_AlbumInfo.Count; i++)
                {
                    AlbumInfo e = FB_Image.List_AlbumInfo[i];
                    FB_Image.List_AlbumID.Add(e.Id);
                    FB_Image.Album_PhotoList.Images.Add(Image.FromFile(e.Path));
                    FB_Image.Album_PhotoList.Images.SetKeyName(i, e.Name);

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
            else
            {
                FB_Image.List_AlbumInfo = new List<AlbumInfo>();
            }

        }

        public void LoadingAlbumList_In()
        {
            string filename = Path.Combine(FB_Image.RelativeDirectory, FB_Image.AlbumDirectory_In);
            FB_Image.List_AlbumInfo_In = Common.DeSerializeObject<List<AlbumInfo>>(filename);
            if (FB_Image.List_AlbumInfo_In != null && FB_Image.List_AlbumInfo_In.Count > 0)
            {
                FB_Image.List_AlbumID_In = new List<string>();
                FB_Image.Album_PhotoList_In = new ImageList();
                FB_Image.Album_PhotoList_In.TransparentColor = Color.Blue;
                FB_Image.Album_PhotoList_In.ColorDepth = ColorDepth.Depth32Bit;
                FB_Image.Album_PhotoList_In.ImageSize = new Size(50, 50);
                for (int i = 0; i < FB_Image.List_AlbumInfo_In.Count; i++)
                {
                    AlbumInfo e = FB_Image.List_AlbumInfo_In[i];
                    FB_Image.List_AlbumID_In.Add(e.Id);
                    FB_Image.Album_PhotoList_In.Images.Add(Image.FromFile(e.Path));
                    FB_Image.Album_PhotoList_In.Images.SetKeyName(i, e.Name);

                }
                this.ListViewalbumList_In.View = View.LargeIcon;
                this.ListViewalbumList_In.LargeImageList = FB_Image.Album_PhotoList_In;

                for (int j = 0; j < FB_Image.Album_PhotoList_In.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = FB_Image.Album_PhotoList_In.Images.Keys[j].ToString();
                    item.Name = FB_Image.List_AlbumID_In[j];
                    item.ImageIndex = j;
                    this.ListViewalbumList_In.Items.Add(item);
                }



            }
            else
            {
                FB_Image.List_AlbumInfo_In = new List<AlbumInfo>();
            }

        }

        private void cmbSelectTextType_SelectedIndexChangedHandle(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            if (c.SelectedIndex == c.Items.IndexOf("From File"))
            {
                tbInputMessage.Enabled = false;
                btnSelectFile.Enabled = true;
                tbMessagePath.Enabled = true;
            }
            else if (c.SelectedIndex == c.Items.IndexOf("From Text"))
            {
                tbInputMessage.Enabled = true; 
                btnSelectFile.Enabled = false;
                tbMessagePath.Enabled = false;
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


   
}
