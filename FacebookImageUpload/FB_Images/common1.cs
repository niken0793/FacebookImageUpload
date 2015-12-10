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
        public static Random r;

        public static UserSetting ActiveUser;

        private void CheckUserSetting()
        {

            if (!Properties.Settings.Default["ActiveUser"].ToString().Equals("no"))
            {
                string usrPath = Path.Combine(FB_Image.RelativeDirectory, "UserSetting/" + Properties.Settings.Default["ActiveUser"]);
                if (File.Exists(usrPath))
                {
                    UserSetting a = Common.DeSerializeObject<UserSetting>(Path.Combine(FB_Image.RelativeDirectory, "UserSetting/" + Properties.Settings.Default["ActiveUser"]));
                    if (a != null)
                    {
                        UpdateLoginControl(a);
                        return;
                    }
                }
               

            }
            LoginFacebook();
        }

        private void SaveActiveUserOnDisk(UserSetting active)
        {
             if (active != null)
            {
                Common.SerializeObject(active,Path.Combine(FB_Image.RelativeDirectory,"UserSetting/"+active.UserID));
                Properties.Settings.Default["ActiveUser"] = active.UserID;
                Properties.Settings.Default.Save();
            }
        }
        





        private string Upload_Picture_FB(string filename, FB_Image browseImageUP,string UserAccessToken,string albumID)
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
                var fb = new FacebookClient(UserAccessToken);
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
                var fb = new FacebookClient(ActiveUser.AccessToken);
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
        public string TestEncodeSuccessRate(IProgress<string> progress,string filename,string inputText,string albumID,bool test = true,bool googleSearch = false)
        {
            
            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;
                if (test)
                {
                    inputText = Path.Combine(FB_Image.BaseDirectory, "test.txt");
                    albumID = FB_Image.Album_Test;
                }

                //Reduce size ratio of picture
                if(progress != null)
                    progress.Report("15|15|Preparing Image");
                List<FB_Image> listFile = AutoUploadAndDownload(filename, null);
                if (listFile == null || listFile.Count < 2)
                {
                    return null;
                }
                FB_Image coverImage = listFile[listFile.Count - 1];
                string coverImageFileName = coverImage.FileName;
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                if (progress != null)
                progress.Report("50|50|Uploading Image");
                string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName), Path.GetFileName(inputText));
                FB_Image encodeImage = new FB_Image();
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }

                string id = Upload_Picture_FB(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,ActiveUser.AccessToken, albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }
                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                progress.Report("75|75|Checking");
                string tempFileName = Download_Picture_FB(downloadImage);
                if (tempFileName == null)
                    return null;
     

                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                if (outputText == null)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {
                    if (!googleSearch)
                    {
                        return downloadImage.ImageID;
                    }
                    else
                    {
                        return coverImageFileName;
                    }
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
                    return null;
                }
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public string SendNoTestImageWithTag(IProgress<string> progress, string filename, string inputText, string albumID,List<string> uids)
        {

            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;

                //Reduce size ratio of picture
                if (progress != null)
                    progress.Report("15|15|Preparing Image");
                List<FB_Image> listFile = AutoUploadAndDownload(filename, null);
                if (listFile == null || listFile.Count < 2)
                {
                    return null;
                }

                FB_Image coverImage = listFile[listFile.Count - 1];
                string coverImageFileName = coverImage.FileName;
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                if (progress != null)
                    progress.Report("50|50|Uploading Image");
                string encodeFile = JPHideEncode(Path.GetFileName(coverImageFileName), Path.GetFileName(inputText));
                FB_Image encodeImage = new FB_Image();
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }
                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,uids ,albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }

                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                    progress.Report("75|75|Checking");
                string tempFileName = Download_Picture_FB(downloadImage);
                if (tempFileName == null)
                    return null;


                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                if (outputText == null)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                    progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {           
                       return coverImageFileName;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
                    return null;
                }
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }


        private string SendMessageWithTestedSource(IProgress<string> progress,string filename, string inputText, string albumID)
        {
            try
            {
                if (String.IsNullOrEmpty(ActiveUser.AccessToken))
                    return null;
                string coverImage = Common.CopyFileTo(filename, FB_Image.BaseDirectory);
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                string encodeFile = JPHideEncode(Path.GetFileName(coverImage), Path.GetFileName(messageFile));
                FB_Image encodeImage = new FB_Image();
                if (progress != null)
                progress.Report("25|25|Uploading Picture");
                if (string.IsNullOrEmpty(encodeFile))
                {
                    return null;
                }

                string id = Upload_Picture_FB(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage,ActiveUser.AccessToken, albumID);
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }


                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                progress.Report("50|50|Checking ...");
                string tempFileName = Download_Picture_FB(downloadImage);
                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {
                    return downloadImage.ImageID;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
                    return null;
                }
               
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public static string JPHideEncode(string filename,string input)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(input) != "")
                {
                    throw new Exception("No need for full path");
                }
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, filename));
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, input));
                input=Path.GetFileName(InsertCrc32(input));
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, input));
                string enImageName = Path.GetFileNameWithoutExtension(filename)+"_encode"+Path.GetExtension(filename);
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, enImageName));
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jphide_modify " + filename + " " + enImageName + " " + input;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;

                proc.Start();
                while (!proc.HasExited)
                    ;
                return enImageName;
              
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public static string InsertCrc32(string input)
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



        public  static string JPSeekDecode(string filename,string output)
        {
            try
            {
                if (Path.GetDirectoryName(filename) != "" && Path.GetDirectoryName(output) != "")
                {
                    throw new Exception("No need for full path");
                }
                string imageName = filename;
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, filename));
                string hiddenFileName = output;
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, hiddenFileName));
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
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory, hiddenFileName));
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

        public static string CheckCrc32(string filename)
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
                string id = Upload_Picture_FB(tempName, currentImage,ActiveUser.AccessToken,ActiveUser.PrivateAlbumID);
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
                Common.listFileDelete.Add(Path.Combine(FB_Image.BaseDirectory,tempName));
                ratio = ((float)currentImage.FileSize / (float)downImage.FileSize);
                FB_Image k = new FB_Image();
                downImage.CopyTo(k);
                uploadedImage.Add(k);
                downImage.CopyTo(currentImage);
                if (progress != null)
                {
                    float t = ratio;
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


        public void GetAlbumList_1(IProgress<int> progress, List<string> inboxAlbums = null, int limit = 5)
        {
            try
            {

                var fb = new FacebookClient(ActiveUser.AccessToken);
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
                            info.NewNupdatedTime = long.Parse(updatedTime);
                            info.NewNumber = newCount - info.Count;
                            info.Count = newCount;
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

                var fb = new FacebookClient(ActiveUser.AccessToken);
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

        private List<FB_Message> GetNewMessageFromAlbum(IProgress<string> progress,string albumID)
        {
            AlbumInfo album = Common.GetAlbumInfoByID(FB_Image.List_AlbumInfo_In, albumID);
            if (album.NewNumber > 0)
            {
                string limit = "&limit=" + album.NewNumber;
                string since = "&since=" + album.UpdatedTime;
                string dateFormat = "&date_format=U";
                List<FB_Message> ListMessages = new List<FB_Message>();

                if (progress != null)
                    progress.Report("15|15|Getting inbox information");
                var fb = new FacebookClient(ActiveUser.AccessToken);
                dynamic albums = fb.Get(albumID+"/photos?pretty=1"+limit+since+dateFormat); // Get album information
                string json_string = JsonConvert.SerializeObject(albums); // parse response sang json
                var json = JObject.Parse(json_string);
                dynamic photos = json["data"];
                int count = photos.Count;
                int p = 70 / count;
                int i = 1;
                if (progress != null)
                    progress.Report("30|30|Getting messages");
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
                            if (progress != null)
                                progress.Report((i*p).ToString()+"|"+(i*p).ToString()+"|Getting messages");
                            i++;
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
                    if (progress != null)
                        progress.Report("100|100|Finish");
                    album.UpdatedTime = album.NewNupdatedTime;
                    album.NewNumber = 0;
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

        /*public void LoadingAlbumList()
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
                    if(File.Exists(e.Path))
                        FB_Image.Album_PhotoList.Images.Add(Image.FromFile(e.Path));
                    else
                        FB_Image.Album_PhotoList.Images.Add(Image.FromFile(FB_Image.DefaultImage));
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

        }*/

        /*public void LoadingAlbumList_In()
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
                    if (File.Exists(e.Path))
                        FB_Image.Album_PhotoList_In.Images.Add(Image.FromFile(e.Path));
                    else
                        FB_Image.Album_PhotoList_In.Images.Add(Image.FromFile(FB_Image.DefaultImage));
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

        }*/

        private void cmbSelectTextType_SelectedIndexChanged_Handle(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            if (c.SelectedIndex == c.Items.IndexOf("From File"))
            {
                btnSelectFile.Enabled = true;
                tbMessagePath.Enabled = true;
                isFromFile = true;
            }
            else if (c.SelectedIndex == c.Items.IndexOf("From Text"))
            {
                btnSelectFile.Enabled = false;
                tbMessagePath.Enabled = false;
                isFromFile = false;
            }

        }

        private void tbInputMessage_TextChanged_Hanlde(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            lbMessageLength.Text = t.Text.Length.ToString();
        }


        InboxUser currentInbox = null;
        private void ChangeUserInbox(object sender, EventArgs e)
        {
            ListView lvUser = (ListView)sender;
            if (lvUser.SelectedItems.Count == 1 )
            {
                InboxUser inbox = Common.GetInboxByUserID(lvUser.SelectedItems[0].Name, ListInboxUser);
                UpdateMessageListView(listViewTagImage, inbox);
                currentInbox = inbox;

            }
         
            
        }

        private void ChangeUserMessage(object sender, EventArgs e)
        {
            ListView lvMessage = (ListView)sender;
            if (lvMessage.SelectedItems.Count ==1 )
            {
                if (currentInbox != null)
                {
                    int i = lvMessage.SelectedItems[0].Index;
                    if (i < currentInbox.Messages.Count)
                    {
                        tbInbox.Text = currentInbox.Messages[i].Content;
                    }

                }
                

            }
        }

        private void UpdateMessageListView(ListView listview, InboxUser inbox)
        {
            listview.Clear();
            ImageList il = new ImageList();
            il.TransparentColor = Color.Blue;
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.ImageSize = new Size(50, 50);
            List<FB_Message> messages = inbox.Messages;
            for (int i = 0; i < messages.Count; i++)
            {
                il.Images.Add(Image.FromFile(Path.Combine(messages[i].Image.Directory,messages[i].Image.FileName)));
                il.Images.SetKeyName(i, "Message_"+i);
            }
            listview.View = View.LargeIcon;
            listview.LargeImageList = il;
            for (int j = 0; j < messages.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                    item.Text = il.Images.Keys[j].ToString();
                item.Name = j.ToString();
                item.ImageIndex = j;
                listview.Items.Add(item);
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
