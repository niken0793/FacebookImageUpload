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
    public class Common
    {
        public static List<string> listFileDelete = new List<string>();




        public static List<string> getUserInfo(string userAccessToken, string uid, string path)
        {
            string userAvatarPath = "";
            List<string> userInfo = new List<string>();
            var user = new FacebookClient(userAccessToken);
            dynamic me = user.Get(uid);
            string userName = me.name;
            string userId = me.id;

            dynamic res = user.Get(uid + "?fields=picture");
            string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            var json = JObject.Parse(json_string);
            string source_url = "";

            source_url = (string)json["picture"]["data"]["url"];

            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(source_url);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        userAvatarPath = path + "profilePiture_" + userId + ".jpg";
                        yourImage.Save(userAvatarPath, ImageFormat.Jpeg);
                    }
                }
            }

            userInfo.Add(userName);
            userInfo.Add(userAvatarPath);
            return userInfo;
        }


        public static void ShowProgressBar(string progress,ToolStripProgressBar pb, ToolStripLabel lbPercent,ToolStripLabel lbDoing)
        {
            if (progress != null && progress.Length > 0)
            {
                string[] a = progress.Split('|');
                if (a.Length == 3)
                {
                    if (pb != null)
                    {
                        pb.Value = Int16.Parse(a[0]);

                    }
                    if(lbPercent!= null)
                    {
                        lbPercent.Text = a[1] +" %";
                    }
                    if(lbDoing != null)
                    {
                        lbDoing.Text = a[2];
                    }
                }
            }
        }

        public static void ResetStatusTrip(ToolStripProgressBar pb, ToolStripLabel lbPercent, ToolStripLabel lbDoing)
        {
             if (pb != null)
                    {
                        pb.Value = 0;

                    }
                    if(lbPercent!= null)
                    {
                        lbPercent.Text = "...";
                    }
                    if(lbDoing != null)
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
            return (Math.Sign(byteCount) * num).ToString() + " "+suf[place];
        }

        public static void DeleteFile(string path)
        {
            if (path != null && path != "" && File.Exists(path))
            {
                File.Delete(path);
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

        public static bool CompareOutputFile(string path_file_in, string path_file_out, TextBox tb)
        {
            if (File.Exists(path_file_in) && File.Exists(path_file_out))
            {
                string s_in = File.ReadAllText(path_file_in);
                string s_out = File.ReadAllText(path_file_out);
                if (tb != null)
                {
                }
                File.Delete(path_file_out);
                if (s_in.Equals(s_out))
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
                if(i.Id.Equals(id))
                    return i;
            }
            return null;
        }
        public static string CopyFileTo(string source,string dir)
        {
            string t1 = Path.GetDirectoryName(source);
            string t2 = Path.GetFileName(source);
            if (!t1.Equals(FB_Image.BaseDirectory))
            {
                File.Copy(source, Path.Combine(dir, t2),true);
                return Path.Combine(dir, t2);
            }else{
                return source;
            }
        }

        public static string AppenFileName(string fileName, string suffix)
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
