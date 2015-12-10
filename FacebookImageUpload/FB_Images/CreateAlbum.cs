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
        public string AlbumID { get; set; }
        public string AlbumName { get; set; }
        public CreateAlbum()
        {
            InitializeComponent();
            
        }
        public CreateAlbum(string paramToken):this()
        {
            acesstoken = paramToken;
            if (!string.IsNullOrEmpty(paramToken))
            {
                //Dictionary<string,string> albuminfo = new Dictionary<string,string>();
                List<PrivateAlbum> albums = GetUserAlbumsForComboBox(acesstoken);
                cmbPrivateAlbum.DataSource = albums;
                cmbPrivateAlbum.DisplayMember = "Name";
                cmbPrivateAlbum.ValueMember = "ID";
               // NumOfAlbum = albuminfo.Count;
               

            }
            else
            {
                MessageBox.Show("You need to login first", "Login required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }

        public static List<PrivateAlbum> GetUserAlbumsForComboBox(string accessToken,string pricvacy = "custom", int limit = 15)
        {
            List<PrivateAlbum> albums = new List<PrivateAlbum>();
            var fb = new FacebookClient(accessToken);
            string paramLimit = "limit="+limit;
            dynamic res = fb.Get("me/albums?" + paramLimit);  // query đường dẫn + độ phân giải ảnh
            string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res); // parse response sang json
            dynamic json = JObject.Parse(json_string);
            dynamic albumJson = json["data"];
            int count = albumJson.Count;
            if (pricvacy.Equals("all"))
            {
                albums.Add(new PrivateAlbum("me", "No Albums"));
            }
            for (int i = 0; i < count; i++)
            {
                if (pricvacy.Equals("all"))
                {
                    albums.Add(new PrivateAlbum((string)albumJson[i]["id"], (string)albumJson[i]["name"]));
                }
                else
                {
                    if (albumJson[i]["privacy"]==pricvacy)
                    {
                        albums.Add(new PrivateAlbum((string)albumJson[i]["id"], (string)albumJson[i]["name"]));

                    }
                }

            }

            return albums;

        }

        public static string CreatePrivateAlbum(string accessToken, string albumName, string privacy = "SELF")
        {
            FacebookAlbum f = new FacebookAlbum();
            string albumID = null;
            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(albumName) && !string.IsNullOrEmpty(privacy))
            {
                albumID = f.createAlbum(accessToken, albumName, "{\"value\":\"" + privacy + "\"}");
                return albumID;
            }
            else
            {
                MessageBox.Show("You need to enter album's name", "Name required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            
            
        }
    

        private void btnCreatePrivateAlbum_Click(object sender, EventArgs e)
        {
            string albumID = CreatePrivateAlbum(this.acesstoken, tbAlbumName.Text);
            this.AlbumName = tbAlbumName.Text;
            this.AlbumID = albumID;
            if (!string.IsNullOrEmpty(albumID))
            {
                MessageBox.Show("Your new private albums is successfully created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Fail to create new private album", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnChoosePrivateAlbum_Click(object sender, EventArgs e)
        {
            this.AlbumID = cmbPrivateAlbum.SelectedValue.ToString();
            this.AlbumName = ((PrivateAlbum)cmbPrivateAlbum.SelectedItem).Name;
            DialogResult = DialogResult.OK;
        }

        private void CreateAlbum_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(AlbumID))
            {
                DialogResult = DialogResult.No;
            }
        }


    }


  public  class PrivateAlbum
    {
        public string ID {get;set;}
        public string Name {get;set;}

        public PrivateAlbum()
        {
        }

        public PrivateAlbum(string paramID, string paramName)
        {
            ID = paramID;
            Name = paramName;
        }
    }
   



}
