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
using Facebook; // PM
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
namespace FacebookImageUpload
{
    class FacebookAlbum
    {
        public string _albumid = "";
        public string createAlbum(string _accessToken, string name = null,string privacy=null, string description = "Testing album (private)")
        {
            dynamic albumPost = new ExpandoObject();  // tạo đối tượng
            //albumPost.message = description;  // truyền tham số 
            albumPost.name = name;
            albumPost.privacy = privacy;
            var fb = new FacebookClient(_accessToken);
            dynamic result = fb.Post("me/albums", albumPost); //request facebook api dạng: /{album-id}?field=message,name
            this._albumid = result.id;
            return _albumid;
        }
        public string getName(string _accessToken)
        {
            var fb = new FacebookClient(_accessToken);
            dynamic result = fb.Get(this._albumid);
            return result.name;
        }
    }

    [Serializable]
    public class AlbumInfo
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private long createdTime;

        public long CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }
        private long updatedTime;

        public long UpdatedTime
        {
            get { return updatedTime; }
            set { updatedTime = value; }
        }

        private long newNupdatedTime;

        public long NewNupdatedTime
        {
            get { return newNupdatedTime; }
            set { newNupdatedTime = value; }
        }

        private int newNumber;

        public int NewNumber
        {
            get { return newNumber; }
            set { newNumber = value; }
        }

        public AlbumInfo()
        {
        }
        public AlbumInfo(string paramID, string paramName
            , string paramPath,int paramCount, long paramCreate, long paramUpdate)
        {
            id = paramID;
            name = paramName;
            path = paramPath;
            count = paramCount;
            createdTime = paramCreate;
            updatedTime = paramUpdate;
        }
        public AlbumInfo(string paramID, string paramName
           , string paramPath, int paramCount, long paramCreate, long paramUpdate, int paramNew)
        {
            id = paramID;
            name = paramName;
            path = paramPath;
            count = paramCount;
            createdTime = paramCreate;
            updatedTime = paramUpdate;
            newNumber = paramNew;
        }
        public AlbumInfo(string paramID, string paramName
            , string paramPath)
        {
            id = paramID;
            name = paramName;
            path = paramPath;
            count = 0;
            createdTime = 0;
            updatedTime = 0;
        }

        

    }
}
