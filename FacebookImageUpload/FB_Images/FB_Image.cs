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
        public static string AccessToken = "CAAVUMKQz7ZB0BAJG21S5gyhrBOpK7qvIZCViGPfbwMYckHWhXy8nPYcI5ZBxQOMZCjz5ieT9IydD6hDiE7sLEn6taU3K7ztbmZCgcHohnLqQw3vZAJPdXs5LjefrOEy4fIxQSPWXMe57n1TBCtmqUiWHdr4JSyq5ujBtQXWYlg5pdZBpQJncIvfU2rvPIkslQZCaZAs9ZAf8CRpQZDZD";
        public static string Album_Test = "1661607290718778";
        public static int Temp = 0;
        public static string BaseDirectory = @"E:\Tai lieu UIT\Khoa luan\Test\";
        public static string RelativeDirectory =
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string LogDirectory = "Log/";
        public static string DefaultImage = "images/default.jpg";
      
        public static string DefaultAlbumID = "";
        public static int ImageSize = 960;
        public static float RatioMax = 1.005F;
        public static float RatioMin = 0.9F;

        // Outbox Album Info
        public static string AlbumDirectory = "Album_Save/album.xml";
        public static List<string> List_AlbumID;
        public static List<AlbumInfo> List_AlbumInfo = new List<AlbumInfo>();
        public static ImageList Album_PhotoList;

        // Inbox Album Info
        public static string AlbumDirectory_In = "Album_Save/album_in.xml";
        public static List<string> List_AlbumID_In;
        public static List<AlbumInfo> List_AlbumInfo_In = new List<AlbumInfo>();
        public static ImageList Album_PhotoList_In;
        public static ImageList Image_Tags_In;
        public static List<string> Image_TagsID_In;


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

        public FB_Image(string paramImageID, string paramFileName, string paramDirectory)
        {
            imageID = paramImageID;
            fileName = paramFileName;
            directory = paramDirectory;
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
