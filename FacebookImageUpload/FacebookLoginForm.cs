using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using FacebookImageUpload.FB_Images;
using System.Drawing.Imaging;

namespace FacebookImageUpload
{
    public partial class FacebookLoginForm : Form
    {
        public string userAccessToken = "";
        public string userAccessTokenExpire = "";
        public string userName = "";
        public string userAvatarPath = "";

        public FacebookLoginForm()
        {
            InitializeComponent();
        }

        private void FacebookLoginForm_Load(object sender, EventArgs e)
        {
            dynamic parameters = new ExpandoObject();

            parameters.client_id = "1499942773583853";
            parameters.response_type = "token";
            parameters.display = "popup";
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";
            parameters.scope = "user_photos,user_posts,user_status,user_likes,user_friends,publish_actions";

            var fb = new FacebookClient();

            Uri loginUri = fb.GetLoginUrl(parameters);

            webBrowserFacebook.Navigate(loginUri.AbsoluteUri);
        }
        private void webBrowserFacebook_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowserFacebook.Visible)
            {
                var fb = new FacebookClient();

                FacebookOAuthResult oauthResult;
                if (fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
                {
                    if (oauthResult.IsSuccess)
                    {
                        userAccessToken = oauthResult.AccessToken;
                        userAccessTokenExpire = oauthResult.Expires.ToString();

                        var user = new FacebookClient(userAccessToken);
                        dynamic me = user.Get("me");
                        userName = me.name;

                        dynamic res = user.Get("/me?fields=picture");
                        string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                        var json = JObject.Parse(json_string);
                        string source_url = "";

                        source_url = (string)json["picture"]["data"]["url"];

                        using (WebClient webClient = new WebClient())
                        {
                            byte[] data = webClient.DownloadData(source_url);

                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                using (var yourImage = Image.FromStream(mem))
                                {
                                    userAvatarPath = FB_Image.BaseDirectory + "profilePiture.jpg";
                                    yourImage.Save(userAvatarPath, ImageFormat.Jpeg);
                                }
                            }

                        }

                        this.Close();
                    }
                    else
                    {
                        userAccessToken = "";
                        MessageBox.Show("Couldn't log into Facebook!", "Login unsuccessful", MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                    }

                }
            }
        }

    }
}
