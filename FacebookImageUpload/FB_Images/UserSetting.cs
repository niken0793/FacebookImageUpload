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

        private long  checkTime;

        public long CheckTime
        {
            get { return checkTime; }
            set { checkTime = value; }
        }

        private string imgPath;

        public string ImgPath
        {
            get { return imgPath; }
            set { imgPath = value; }
        }

        private List<PrivateAlbum> albums;

        public List<PrivateAlbum> Albums
        {
            get { return albums; }
            set { albums = value; }
        }


        public UserSetting()
        {
            albums = new List<PrivateAlbum>();
        }
        public UserSetting(string pToken, string pUserName, string pUserID):this()
        {
            accessToken = pToken;
            userName = pUserName;
            UserID = pUserID;
        }

        public UserSetting(string pToken, string pUserName, string pUserID, string pETime, string pImage):this()
        {
            accessToken = pToken;
            userName = pUserName;
            UserID = pUserID;
            expiredTime = pETime;
            imgPath = pImage;
        }

       
    }

    [Serializable]
    public class AppSetting
    {
        private string activeUser;

        public string ActiveUser
        {
            get { return activeUser; }
            set { activeUser = value; }
        }

        public AppSetting()
        {

        }

        public AppSetting(string pUser)
        {
            activeUser = pUser;
        }
    }
}
