using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookImageUpload.FB_Images
{
    public class FB_Image
    {
        public static string AccessToken = "CAAVUMKQz7ZB0BAJG21S5gyhrBOpK7qvIZCViGPfbwMYckHWhXy8nPYcI5ZBxQOMZCjz5ieT9IydD6hDiE7sLEn6taU3K7ztbmZCgcHohnLqQw3vZAJPdXs5LjefrOEy4fIxQSPWXMe57n1TBCtmqUiWHdr4JSyq5ujBtQXWYlg5pdZBpQJncIvfU2rvPIkslQZCaZAs9ZAf8CRpQZDZD";
        public static int Temp = 0;
        public static string BaseDirectory = @"E:\Tai lieu UIT\Khoa luan\Test\";
        public static string DefaultAlbumID = "";
        public static int ImageSize = 960;
        public static float RatioMax = 1.1F;
        public static float RatioMin = 0.9F;
        string imageID;
        string fileName;
        string fileNameWithOutExtension;
        string directory;
        string albumID;
        int height;
        int width;
        long upFileSize;
        long downFileSize;
 
        public string ImageID
        {
            get { return imageID; }
            set { imageID = value; }
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
        public long UpFileSize
        {
            get { return upFileSize; }
            set { upFileSize = value; }
        }
        public long DownFileSize
        {
            get { return downFileSize; }
            set { downFileSize = value; }
        }

        public FB_Image()
        {
            this.albumID = "0";
            this.imageID = "";
            this.fileName = "";
            this.fileNameWithOutExtension = "";
            this.directory = "";
            this.height = this.width = 480;
            this.upFileSize = this.downFileSize = 0;
            
        }

        public FB_Image(string paramImageID, string paramFileName,string paramFileNameNo, string paramDirectory,
            string paramAlbum, int paramHeight, int paramWidth, long paramUp, long paramDown)
        {
            imageID = paramImageID;
            fileName = paramFileName;
            fileNameWithOutExtension = paramFileNameNo;
            directory = paramDirectory;
            albumID = paramAlbum;
            height = paramHeight;
            width = paramWidth;
            upFileSize = paramUp;
            downFileSize = paramDown;
        }

       

    }
}
