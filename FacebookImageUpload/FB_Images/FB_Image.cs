using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace FacebookImageUpload.FB_Images
{
    public class FB_Image
    {
        //public static string UserAccessToken = "";
        public static int Temp = 0;
    
        public static string RelativeDirectory =
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string LogDirectory = "Log/";
        public static string DefaultImage = "images/default.jpg";

        public const string UserSettingDir = "UserSetting";
        public const string UserImageDir = "UserSetting\\images";
        public static string BaseDirectory =Path.Combine(FB_Image.RelativeDirectory, "Temp");
        public const string SuccessImageDir = "SuccessImage";
        public const string LogDir = "Log";
        public const string OutputDir = "Output";
        public const string TestInputDir = "TestInput";


        public const string AppSettingFile = "appsetting.xml";


        public static string DefaultAlbumID = "";
        public static int ImageSize = 960;
        public static string DefaultPassword = "123";




        // property
        string imageID;
        string fileName;
        string fileNameWithOutExtension;
        string directory;
        string albumID;
        string fullPath;
        int height;
        int width;
        long fileSize;
 
        public string ImageID
        {
            get { return imageID; }
            set { imageID = value; }
        }
        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public string FileNameWithOutExtension
        {
            get { return fileNameWithOutExtension; }
            set { fileNameWithOutExtension = value; }
        }

        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }
        public string AlbumID
        {
            get { return albumID; }
            set { albumID = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public FB_Image()
        {
            this.albumID = "";
            this.imageID = "";
            this.fileName = "";
            this.fileNameWithOutExtension = "";
            this.directory = "";
            this.height = this.width = 480;
            this.fileSize = 0;
            
        }

        public FB_Image(string paramImageID, string paramFullPath)
        {
            imageID = paramImageID;
            fullPath = paramFullPath;
        }

        public FB_Image(string paramImageID, string paramFileName,string paramFileNameNo, string paramDirectory,
            string paramAlbum, int paramHeight, int paramWidth, long paramUp)
        {
            imageID = paramImageID;
            fileName = paramFileName;
            fileNameWithOutExtension = paramFileNameNo;
            directory = paramDirectory;
            albumID = paramAlbum;
            height = paramHeight;
            width = paramWidth;
            fileSize = paramUp;
        }


        // method
        public void CopyTo(FB_Image paramImage)
        {
            paramImage.FileName = this.FileName;
            paramImage.FileNameWithOutExtension = this.FileNameWithOutExtension;
            paramImage.FileSize = this.FileSize;
            paramImage.Directory = this.Directory;
            paramImage.Height = this.Height;
            paramImage.Width = this.Width;
            paramImage.ImageID = this.ImageID;
            paramImage.AlbumID = this.AlbumID;
        }

        public override string ToString()
        {
            return String.Format(
                    @"FileName : {0} 
Directoty : {1} 
ID :  {2}
Size : {3} 
Height,Width {4} - {5}

",
                     this.FileName, this.Directory, this.ImageID, this.FileSize, this.Height, this.Width);
                                  
        }
  

    }
}
