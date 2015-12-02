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
        Form1 mainform = new Form1();

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

                        List<string> userInfo = mainform.getUserInfo(userAccessToken,"me",FB_Image.BaseDirectory);
                        userName = userInfo[0];
                        userAvatarPath = userInfo[1];
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
