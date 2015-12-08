using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookImageUpload.FB_Images
{
    [Serializable]
    public class UserSetting
    {


        private string accessToken;

        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
        private string privateAlbumID;

        public string PrivateAlbumID
        {
            get { return privateAlbumID; }
            set { privateAlbumID = value; }
        }

        private string privateAlbumName;

        public string PrivateAlbumName
        {
            get { return privateAlbumName; }
            set { privateAlbumName = value; }
        }


        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string expiredTime;

        public string ExpiredTime
        {
            get { return expiredTime; }
            set { expiredTime = value; }
        }

        private string imgPath;

        public string ImgPath
        {
            get { return imgPath; }
            set { imgPath = value; }
        }


        public UserSetting()
        {
        }
        public UserSetting(string pToken, string pUserName, string pUserID)
        {
            accessToken = pToken;
            userName = pUserName;
            UserID = pUserID;
        }

        public UserSetting(string pToken, string pUserName, string pUserID, string pETime, string pImage)
        {
            accessToken = pToken;
            userName = pUserName;
            UserID = pUserID;
            expiredTime = pETime;
            imgPath = pImage;
        }

       
    }
}
