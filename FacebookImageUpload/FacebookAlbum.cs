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
        public string createAlbum(string _accessToken, string description = null, string name = null)
        {
            dynamic albumPost = new ExpandoObject();  // tạo đối tượng
            albumPost.message = description;  // truyền tham số 
            albumPost.name = name;
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
        public string id;
        public string name;
        public string path;

        public AlbumInfo()
        {
        }
        public AlbumInfo(string paramID, string paramName, string paramPath)
        {
            id = paramID;
            name = paramName;
            path = paramPath;
        }
    }
}
