using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook; // PM
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FacebookImageUpload
{
    public partial class CreateAlbum : Form
    {
        private string acesstoken;
        public CreateAlbum()
        {
            InitializeComponent();
            
        }
        public CreateAlbum(string paramToken):this()
        {
            acesstoken = paramToken;
            if (!string.IsNullOrEmpty(paramToken))
            {
                Dictionary<string,string> albuminfo = new Dictionary<string,string>();
                List<PrivateAlbum> albums = GetUserPrivateAlbums(acesstoken);
                cmbPrivateAlbum.DataSource = albums;
                cmbPrivateAlbum.DisplayMember = "Name";
                cmbPrivateAlbum.ValueMember = "ID";
            }
            else
            {
                MessageBox.Show("You need to login first", "Login requied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public static List<PrivateAlbum> GetUserPrivateAlbums(string accessToken, int limit = 15)
        {
            List<PrivateAlbum> albums = new List<PrivateAlbum>();
            var fb = new FacebookClient(accessToken);
            string paramLimit = "limit="+limit;
            dynamic res = fb.Get("me/albums?" + paramLimit);  // query đường dẫn + độ phân giải ảnh
            string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res); // parse response sang json
            dynamic json = JObject.Parse(json_string);
            dynamic albumJson = json["data"];
            int count = albumJson.Count;
            for(int i =0; i<count ; i++)
            {
                if (albumJson[i]["privacy"] == "custom")
                {
                    albums.Add(new PrivateAlbum((string)albumJson[i]["id"],(string)albumJson[i]["name"]));

                }
            }

            return albums;

        }


    }


  public  class PrivateAlbum
    {
        public string ID {get;set;}
        public string Name {get;set;}

        public PrivateAlbum(string paramID, string paramName)
        {
            ID = paramID;
            Name = paramName;
        }
    }
   



}
