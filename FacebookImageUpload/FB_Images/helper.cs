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
                    tb.AppendText("Output: " + Environment.NewLine);
                    tb.AppendText(s_out);
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
