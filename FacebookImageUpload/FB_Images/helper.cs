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

namespace FacebookImageUpload.FB_Images
{
    public class MyHelper
    {

        public static string ProjectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;



        public static List<string> ListFileDelete = new List<string>();



        //public static bool CheckingProgramDir()
        //{
        //    string jphide_pass = Path.Combine(FB_Image.BaseDirectory, "jphide_pass.exe");
        //    string jpseek_pass = Path.Combine(FB_Image.BaseDirectory, "jpseek_pass.exe");
        //    string jphide_modify = Path.Combine(FB_Image.BaseDirectory, "jphide_modify.exe");
        //    string jpseek_modify = Path.Combine(FB_Image.BaseDirectory, "jpseek_modify.exe");
        //    List<string> listDir = new List<string>();
        //    listDir.AddRange(new string[] {
        //      FB_Image.UserSettingDir,
        //      FB_Image.UserImageDir,
        //      FB_Image.BaseDirectory,
        //      FB_Image.SuccessImageDir,
        //      FB_Image.LogDir,
        //      FB_Image.OutputDir,
        //      FB_Image.TestInputDir,
        //    });

        //    List<string> listFile = new List<string>();


        //    foreach (string item in listDir)
        //    {
                
        //    }

        //}

        public static void ShowToolTip(string message, Control control, int time, int x = 0, int y = 0)
        {
            ToolTip t = new ToolTip();
            t.Show(message, control, x, y, time);
        }

        public static bool CreateProgramDir()
        {
            try
            {
                List<string> listDir = new List<string>();
                listDir.AddRange(new string[] {
              FB_Image.UserSettingDir,
              FB_Image.UserImageDir,
              FB_Image.BaseDirectory,
              FB_Image.SuccessImageDir,
              FB_Image.LogDir,
              FB_Image.OutputDir,
              FB_Image.TestInputDir
            });
                foreach (string dir in listDir)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }

                string jphide_pass = Path.Combine(FB_Image.BaseDirectory,"jphide_pass.exe");
                string jpseek_pass = Path.Combine( FB_Image.BaseDirectory,"jpseek_pass.exe");
                string jphide_modify = Path.Combine( FB_Image.BaseDirectory,"jphide_modify.exe");
                string jpseek_modify = Path.Combine( FB_Image.BaseDirectory,"jpseek_modify.exe");

                if (!File.Exists(jphide_pass))
                {
                    File.WriteAllBytes(jphide_pass, Properties.Resources.jphide_pass);
                }
                if (!File.Exists(jpseek_pass))
                {
                    File.WriteAllBytes(jpseek_pass, Properties.Resources.jpseek_pass);
                }
                if (!File.Exists(jphide_modify))
                {
                    File.WriteAllBytes(jphide_modify, Properties.Resources.jphide_modify);
                }
                if (!File.Exists(jpseek_modify))
                {
                    File.WriteAllBytes(jpseek_modify, Properties.Resources.jpseek_modify);
                }




                return true;
            }
            catch (Exception e)
            {
                Form1.Log(e);
                return false;
            }
        }

        public static string GetInboxText(string name, int count)
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

        /// <summary>
        /// Return user information base on user token return Username, Path to profile pic, UserID
        /// </summary>
        /// <param name="userAccessToken"></param>
        /// <param name="uid"></param>
        /// <param name="path"></param>
        /// <returns>Username, Path to profile pic, UserID</returns>
        public static List<string> getUserInfo(string userAccessToken, string uid, string path)
        {
            string userAvatarPath = "";
            List<string> userInfo = new List<string>();
            var user = new FacebookClient(userAccessToken);
            dynamic res = user.Get(uid + "?fields=picture,name");
            string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            var json = JObject.Parse(json_string);
            string source_url = "";
            dynamic me = user.Get(uid);
            string userName = me.name;
            string userId = me.id;
            userAvatarPath = Path.Combine(path, userId + ".jpg");

            source_url = (string)json["picture"]["data"]["url"];
            if (!File.Exists(userAvatarPath))
            {
                DowloadImageFromLink(source_url, userAvatarPath, ImageFormat.Jpeg);
            }
            userInfo.Add(userName);
            userInfo.Add(userAvatarPath);
            userInfo.Add(userId);
            return userInfo;
        }

        public static void AddToDictionary(Dictionary<string, List<FB_Message>> inbox, string userID, FB_Message message)
        {
            List<FB_Message> userInbox = inbox[userID];
            if (userInbox != null)
            {
                userInbox.Add(message);
            }
            else
            {
                userInbox = new List<FB_Message>();
                userInbox.Add(message);
                inbox.Add(userID, userInbox);
            }
        }
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static long GetUnixTimesStamp(DateTime time)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((time.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static long GetCheckTimeFromInbox(List<InboxUser> inboxs)
        {
            if (inboxs != null && inboxs.Count > 0)
            {
                var min = inboxs.Min(r => r.CheckTime);
                return (long)min;
            }
            return 0;
        }

        public static InboxUser GetInboxByUserID(string userID, List<InboxUser> inboxs)
        {
            if (inboxs != null && inboxs.Count > 0)
            {
                var i = inboxs.Where(r => r.UserID.Equals(userID));
                if (i.Count() > 0)
                {
                    return i.First();
                }

            }
            return null;
        }

        public static string AppendFileName(string fullPath, string suffix)
        {
            string s = "";
            string dir = Path.GetDirectoryName(fullPath);
            string filename = Path.GetFileNameWithoutExtension(fullPath);
            string exten = Path.GetExtension(fullPath);
            s = Path.Combine(dir, filename + "_" + suffix + exten);
            return s;
        }

        public static bool DowloadImageFromLink(string link, string pathToSave, ImageFormat format, bool overWrite = false)
        {
            try
            {
                if (!File.Exists(pathToSave) || overWrite)
                {

                    using (WebClient webClient = new WebClient())
                    {
                        byte[] data = webClient.DownloadData(link);

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            using (var yourImage = Image.FromStream(mem))
                            {
                                MyHelper.PrepareDirectory(pathToSave);
                                yourImage.Save(pathToSave, format);
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                Form1.Log(e);
                return false;
            }
        }

        public static string GetMessageFromImage(string imagePath)
        {
            string dir = Path.GetDirectoryName(imagePath);
            string filename = Path.GetFileName(imagePath);
            string fileNameNO = Path.GetFileNameWithoutExtension(imagePath);
            string content;

            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(fileNameNO))
            {
                if (!dir.Equals((FB_Image.BaseDirectory.TrimEnd('\\'))))
                {
                    File.Copy(imagePath, Path.Combine(FB_Image.BaseDirectory, filename), true);
                }
                string output = Form1.JPSeekDecode(filename, fileNameNO + ".txt");
                if (!string.IsNullOrEmpty(output))
                {
                    content = File.ReadAllText(Path.Combine(FB_Image.BaseDirectory, output));
                    return content;
                }
            }
            return "";
        }

        public static byte[] GetByteFromImage(string imagePath)
        {
            string dir = Path.GetDirectoryName(imagePath);
            string filename = Path.GetFileName(imagePath);
            string fileNameNO = Path.GetFileNameWithoutExtension(imagePath);
            byte[] content;

            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(fileNameNO))
            {
                if (!dir.Equals((FB_Image.BaseDirectory.TrimEnd('\\'))))
                {
                    File.Copy(imagePath, Path.Combine(FB_Image.BaseDirectory, filename), true);
                }
                string output = Form1.JPSeekDecode(filename, fileNameNO + ".txt");
                if (!string.IsNullOrEmpty(output))
                {
                    content = File.ReadAllBytes(Path.Combine(FB_Image.BaseDirectory, output));
                    return content;
                }
            }
            return null;
        }




        public static void ShowProgressBar(string progress, ToolStripProgressBar pb, ToolStripLabel lbPercent, ToolStripLabel lbDoing)
        {
            try
            {
                if (progress != null && progress.Length > 0)
                {
                    string[] a = progress.Split('|');
                    if (a.Length == 2)
                    {
                        if (pb != null)
                        {
                            pb.Value = Int16.Parse(a[0]);

                        }
                        if (lbPercent != null)
                        {
                            lbPercent.Text = a[0] + " %";
                        }
                        if (lbDoing != null)
                        {
                            lbDoing.Text = a[1];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Form1.Log(e);
            }
        }

        public static void EnableControl(bool enable = true, params Control[] args)
        {
            foreach (Control item in args)
            {
                if (item != null && item.IsHandleCreated)
                {
                    item.Invoke(new Action(()=>item.Enabled = enable));
                    //item.Enabled = enable;
                }
            }
        }




        public static void ResetStatusTrip(ToolStripProgressBar pb, ToolStripLabel lbPercent, ToolStripLabel lbDoing)
        {
            if (pb != null)
            {
                pb.Value = 0;

            }
            if (lbPercent != null)
            {
                lbPercent.Text = "...";
            }
            if (lbDoing != null)
            {
                lbDoing.Text = "Facebook Image Stegano";
            }
        }


        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        public static void DeleteFile(string path)
        {
            if (path != null && path != "" && File.Exists(path))
            {
                File.Delete(path);
            }
        }


        public static void AddToDeleleList(params string[] files)
        {
            if (files.Length > 0)
            {
                ListFileDelete.AddRange(files);
            }
        }

        public static void DeleteFile(List<string> pathes)
        {
            try
            {
                List<string> errorFile = new List<string>();
                if (pathes != null && pathes.Count > 0)
                {
                    foreach (string path in pathes)
                    {
                        try
                        {
                            if (path != null && path != "" && File.Exists(path))
                            {
                                File.Delete(path);

                            }
                        }
                        catch (Exception ee)
                        {
                            Form1.Log(ee);
                            errorFile.Add(path);
                        }
                    }
                    pathes.Clear();
                    pathes.AddRange(errorFile);


                }
            }
            catch (Exception e)
            {
                Form1.Log(e);

            }
        }

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
        public static void PrepareDirectory(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            string file = Path.GetFileName(filename);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }


        public static bool CompareOutputFile(string path_file_in, string path_file_out)
        {
            if (File.Exists(path_file_in) && File.Exists(path_file_out))
            {
                //string s_in = File.ReadAllText(path_file_in);
                string s_in = Form1.CorrectErrorString(File.ReadAllBytes(path_file_in));
                string s_out = File.ReadAllText(path_file_out);
                if (s_in.IndexOf(s_out) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static AlbumInfo GetAlbumInfoByID(List<AlbumInfo> albums, string id)
        {
            if (albums == null || albums.Count == 0 || id == "")
                return null;
            foreach (AlbumInfo i in albums)
            {
                if (i.Id.Equals(id))
                    return i;
            }
            return null;
        }
        public static string CopyFileTo(string source, string dir)
        {
            string t1 = Path.GetDirectoryName(source);
            string t2 = Path.GetFileName(source);
            string asciiFileName = ConvertVN(t2);
            string newFullPath = Path.Combine(dir, asciiFileName);
            if (!t1.Equals(dir.Trim('\\')))
            {
                File.Copy(source, newFullPath, true);
            }
            else if (!t2.Equals(asciiFileName))
            {
                if (File.Exists(newFullPath))
                    File.Delete(newFullPath);
                File.Move(source, newFullPath);
            }
            return newFullPath;
        }

        public static string ConvertVN(string chucodau)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = chucodau.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(chucodau[index]);
                chucodau = chucodau.Replace(chucodau[index], ReplText[index2]);
            }
            return chucodau;
        }


        public static string AppendFileNameNoLimit(string fileName, string suffix)
        {
            if (fileName != null && suffix != null && fileName != "" && suffix != "")
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                string exten = Path.GetExtension(fileName);
                return name + suffix + exten;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject, string fileName, bool isEncrypt = false)
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
                    if (isEncrypt)
                    {
                        string cipher = File.ReadAllText(fileName).ToSecureString().EncryptString();
                        File.WriteAllText(fileName, cipher);
                    }
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
        public static T DeSerializeObject<T>(string fileName, bool isDecrypt = false)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                string attributeXml = string.Empty;
                XmlDocument xmlDocument = new XmlDocument();

                if (File.Exists(fileName))
                {
                    if (isDecrypt)
                    {
                        xmlDocument.Load(GenerateStreamFromString( File.ReadAllText(fileName).DecryptString().ToInsecureString()));
                    }
                    else
                    {
                        xmlDocument.Load(fileName);
                    }
                }
                else
                {
                    return default(T);
                }
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

        public static Stream GenerateStreamFromString( string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream,Encoding.Unicode);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
