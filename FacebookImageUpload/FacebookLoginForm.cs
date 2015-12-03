using System;
using System.Dynamic;
using System.Linq;
using System.Windows.Forms;
using Facebook;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FacebookImageUpload.FB_Images;
using System.Drawing.Imaging;

namespace FacebookImageUpload
{
    public partial class FacebookLoginForm : Form
    {

        private readonly Uri loginUrl;
        protected readonly FacebookClient fb;
        public FacebookOAuthResult AuthResult { get; private set; }


      

        public FacebookLoginForm(string appId, string extendedPermissions)
            : this(new FacebookClient(), appId, extendedPermissions)
        {
        }
        public FacebookLoginForm(FacebookClient paramFB, string appId, string extendedPermissions)
        {
            if (paramFB == null)
                throw new ArgumentNullException("fb");
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");

            fb = paramFB;
            loginUrl = GenerateLoginUrl(appId, extendedPermissions);

            InitializeComponent();
        }

        private Uri GenerateLoginUrl(string appId, string extendedPermissions)
        {
            
            dynamic parameters = new ExpandoObject();
            parameters.client_id = appId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";
            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";
            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";
            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(extendedPermissions))
                parameters.scope = extendedPermissions;
            // when the Form is loaded navigate to the login url.
            return fb.GetLoginUrl(parameters);
        }


        private void FacebookLoginForm_Load(object sender, EventArgs e)
        {

            webBrowserFacebook.Navigate(loginUrl.AbsoluteUri);
        }
        private void webBrowserFacebook_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
          
                FacebookOAuthResult oauthResult;
                if (fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
                {

                    AuthResult = oauthResult;
                    DialogResult = AuthResult.IsSuccess ? DialogResult.OK : DialogResult.No;

                }
                else
                {
                    AuthResult = null;
                }
            
        }

    }
}
