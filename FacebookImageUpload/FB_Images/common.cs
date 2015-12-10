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
using FacebookImageUpload.FB_Images;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis;
using Google.Apis.Services;

// Đạt file
namespace FacebookImageUpload
{

    public partial class Form1 : Form
    {
        private CoverImageForm coverImageForm;
        private FacebookLoginForm facebookLoginForm;
        private bool isLogin = false;
        public static string AppID = "1499942773583853";
        public static List<InboxUser> ListInboxUser = new List<InboxUser>();
      

        public void openfile_Click_fn(TextBox tb, string filter, bool isPicture=false)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = filter;
                if (open.ShowDialog() == DialogResult.OK)
                {
                    tb.Text = open.FileName;
                    if (isPicture)
                    {
                        pbImage.ImageLocation = open.FileName;
                        lbImageName.Text = Path.GetFileName(open.FileName);
                        lbImageDirectory.Text = Path.GetDirectoryName(open.FileName);
                        lbImageSize.Text = Common.BytesToString(new FileInfo(open.FileName).Length);
                    }
                    else
                    {
                        tbInputMessage.Clear();
                        tbInputMessage.AppendText(File.ReadAllText(open.FileName));
                        tbInputMessage.AppendText(Environment.NewLine);
                    }
                    
                }
            }
            catch (Exception e)
            {
                Log(e);
            }
        }
        public static string Crc32Hash(string filepath)
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;
            using (FileStream fs = File.Open(filepath, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            return hash;
        }


        uint start = 1;
        int gtemp = 0;
        int tcount = 1;

        public void btnImageSearch_Click_fn(IProgress<string> progress)
        {
            string apiKey = "AIzaSyCKOq5EJwqwfQzmdfCW0VE-IX9fFMIZEUM";
            string searchEngineId = "002524252275919064823:dlgwbkge9f0";
            GoogleImage googleImage = new GoogleImage();
            string query = tbKeyWord.Text;
            //start number of result return
            Search images = googleImage.googleImageSearch(apiKey, searchEngineId, query, start);
            foreach (var item in images.Items)
            {
                try
                {
                    //tbGoogleLink.Text += item.Link + "\n";
                    progress.Report(item.Link);

                    using (WebClient webClient = new WebClient())
                    {
                        byte[] data = webClient.DownloadData(item.Link);

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            using (var yourImage = Image.FromStream(mem))
                            {
                                string new_path = FB_Image.BaseDirectory + query +"_" + gtemp + ".jpg";
                                var newImage = googleImage.ScaleImage(yourImage, 960, 720);
                                newImage.Save(new_path, ImageFormat.Jpeg);
                                Common.listFileDelete.Add(new_path);
                                string filename = new_path;
                                string coverImagePath = TestEncodeSuccessRate(null, filename, Path.Combine(FB_Image.RelativeDirectory, "GoogleImage/test_google1.txt"),ActiveUser.PrivateAlbumID, false, true);
                                if (coverImagePath != null)
                                {
                                    File.Copy(FB_Image.BaseDirectory + coverImagePath, "SuccessImage/"+query+"_" + gtemp + ".jpg", false);
                                }
                              
                            }
                        }

                    }


                }
                catch (Exception e)
                {
                    Log(e);
                }
                gtemp += 1;
                if (tcount >= 3)
                {
                    break;
                }

                tcount++;
            }
            Common.DeleteFile(Common.listFileDelete);
            start += 10;
        }


        private void btnCoverImage_Click(object sender, EventArgs e)
        {
            coverImageForm = new CoverImageForm();
            coverImageForm.Show();
            coverImageForm.FormClosed += new FormClosedEventHandler(coverImageForm_FormClosed);
        }

        void coverImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            tbImagePath.Text = coverImageForm.imageLink;
        }

        private void btnFacebookLogin_Click(object sender, EventArgs e)
        {
            LoginFacebook();
           
            
        }

        private void LoginFacebook()
        {
            if (!isLogin)
            {
                string extendPermission = "user_photos,user_posts,user_status,user_likes,user_friends,publish_actions";
                facebookLoginForm = new FacebookLoginForm(AppID, extendPermission);
                DialogResult d = facebookLoginForm.ShowDialog();
                if (d.Equals(DialogResult.OK))
                {
                    // Login

                    FacebookOAuthResult r = facebookLoginForm.AuthResult;
                    UpdateLoginControl(r);
                    SaveActiveUserOnDisk(ActiveUser);

                }
                else
                {
                    isLogin = false;
                }
            }
            else
            {
                Logout(ActiveUser.AccessToken);
                UpdateLoginControl();

            }

            

        }

        private void Logout(string accessToken)
        {
            var webBrowser = new WebBrowser();
            var fb = new FacebookClient();
            var logouUrl = fb.GetLogoutUrl(new { access_token = accessToken, next = "https://www.facebook.com/connect/login_success.html" });
            webBrowser.Navigate(logouUrl);
            Properties.Settings.Default["ActiveUser"] = "no";
            Properties.Settings.Default.Save();

           
        }

        private void UpdateLoginControl(FacebookOAuthResult r)
        {
            if (r != null)
            {
                
                lbAccessTokenExpire.Text = r.Expires.ToString();
                List<string> s = Common.getUserInfo(r.AccessToken, "me", FB_Image.BaseDirectory);
                pBoxUserAvatar.ImageLocation = s[1];
                lbFacebookUserName.Text = s[0];
                bool flag = false;
                //Set private album
                if (File.Exists(Path.Combine(FB_Image.RelativeDirectory, "UserSetting/" +s[2])))
                {
                    UserSetting a = Common.DeSerializeObject<UserSetting>(Path.Combine(FB_Image.RelativeDirectory, "UserSetting/" + s[2]));
                    if (a != null)
                    {
                        Form1.ActiveUser = a;
                    }
                    else
                    {
                        Form1.ActiveUser = new UserSetting(r.AccessToken, s[0], s[2], r.Expires.ToString(), s[1]);
                        Form1.ActiveUser.CheckTime = Common.GetUnixTimesStamp(DateTime.Now);
                    }
                    
                    if (a != null && !string.IsNullOrEmpty(a.PrivateAlbumID) && !string.IsNullOrEmpty(a.PrivateAlbumName))
                    {
                        ActiveUser.PrivateAlbumID = a.PrivateAlbumID;
                        ActiveUser.PrivateAlbumName = a.PrivateAlbumName;
                        lbPrivateAlbum.Text = a.PrivateAlbumName.ToString();
                        flag = true;
                    }


                }
                else
                {
                    Form1.ActiveUser = new UserSetting(r.AccessToken, s[0], s[2], r.Expires.ToString(), s[1]);
                    Form1.ActiveUser.CheckTime = Common.GetUnixTimesStamp(DateTime.Now);
                }

                if (!flag)
                {
                    CreateAlbum a = new CreateAlbum(ActiveUser.AccessToken);
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        if (Form1.ActiveUser != null)
                        {
                            Form1.ActiveUser.PrivateAlbumID = a.AlbumID;
                            Form1.ActiveUser.PrivateAlbumName = a.AlbumName;
                            lbPrivateAlbum.Text = a.AlbumName.ToString();
                        }
                    }
                }
                ActiveUser.AccessToken = r.AccessToken;

                if (ActiveUser.Albums.Count == 0)
                {
                    ActiveUser.Albums = CreateAlbum.GetUserAlbumsForComboBox(ActiveUser.AccessToken, "all");
                }
                cmbInputAlbum.DataSource = ActiveUser.Albums;
                cmbInputAlbum.DisplayMember = "Name";
                cmbInputAlbum.ValueMember = "ID";
              
                Properties.Settings.Default["ActiveUser"] = s[2];
                Properties.Settings.Default.Save();
                btnFacebookLogin.Text = "LogOut";
                isLogin = true;

            }
            else
            {
                lbAccessTokenExpire.Text = "";
                lbFacebookUserName.Text = "Username";
                pBoxUserAvatar.ImageLocation = Path.Combine(Common.ProjectDir,"images/profile.jpg");
                Form1.ActiveUser = null;
                btnFacebookLogin.Text = "Login";
                lbPrivateAlbum.Text = "...";
                isLogin = false;
                cmbInputAlbum.Items.Clear();
            }
            
        }
        private void UpdateLoginControl()
        {
            lbAccessTokenExpire.Text = "";
            lbFacebookUserName.Text = "Username";
            pBoxUserAvatar.ImageLocation = Path.Combine(Common.ProjectDir, "images/profile.jpg");
            Form1.ActiveUser = null;
            btnFacebookLogin.Text = "Login";
            isLogin = false;
            lbPrivateAlbum.Text = "...";

        }

        private void UpdateLoginControl(UserSetting r)
        {
            if (r != null)
            {
               
                Form1.ActiveUser = r;
                ActiveUser.AccessToken = r.AccessToken;
                lbAccessTokenExpire.Text = r.ExpiredTime;
                pBoxUserAvatar.ImageLocation = r.ImgPath;
                lbFacebookUserName.Text = r.UserName;
                lbPrivateAlbum.Text = r.PrivateAlbumName;
                isLogin = true;
                btnFacebookLogin.Text = "LogOut";
                if (ActiveUser.Albums.Count == 0)
                {
                    ActiveUser.Albums = CreateAlbum.GetUserAlbumsForComboBox(ActiveUser.AccessToken, "all");
                }
                cmbInputAlbum.DataSource = ActiveUser.Albums;
                cmbInputAlbum.DisplayMember = "Name";
                cmbInputAlbum.ValueMember = "ID";


            }

        }



        public void CheckTester()
        { 
            
        }
        private void btnTester_Click(object sender, EventArgs e)
        { 
            
        }

        private void btnGetUserList_Click(object sender, EventArgs e)
        {
            GetFriendList();
            GetImageTagged1(null, ListInboxUser);
            UpdateFriendListView(this.listViewUserList, ListInboxUser, true);
            

        }

        private void UpdateFriendListView(ListView listview ,List<InboxUser> user, bool showNewMessage = false)
        {
            listview.Clear();
            ImageList il = new ImageList();
            il.TransparentColor = Color.Blue;
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.ImageSize = new Size(50, 50);
            for (int i = 0; i < user.Count; i++)
            {
                il.Images.Add(Image.FromFile(user[i].ProfilePath));
                il.Images.SetKeyName(i,user[0].UserName);
            }
            listview.View = View.LargeIcon;
            listview.LargeImageList = il;
            for(int j=0;j<user.Count;j++)
            {
                ListViewItem item = new ListViewItem();
                if (showNewMessage)
                {
                    item.Text = GetInboxText(il.Images.Keys[j].ToString(),user[j].CountNew);
                }
                else{
                    item.Text = il.Images.Keys[j].ToString();
                }

                item.Name = user[j].UserID;
                item.ImageIndex = j;
                listview.Items.Add(item);
            }

        }

        private void GetFriendList()
        {
            try
            {
                if (ActiveUser.AccessToken != "")
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Get("me/friends");
                    string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    dynamic json = JObject.Parse(json_string);
                    dynamic data = json["data"];
                    int count = data.Count;
                    int i = 0;
                    List<string> users = new List<string>();
                    while (i < count && data[i] != null)
                    {
                        users.Add((string)data[i]["id"]);
                        i++;
                    }

                    foreach (string user in users.ToArray())
                    {
                        List<string> info = Common.getUserInfo(ActiveUser.AccessToken, user, FB_Image.BaseDirectory + "Test_User\\");

                        InboxUser a = Common.GetInboxByUserID(user, ListInboxUser);
                        if (a == null && info!=null && info.Count==3)
                        {
                            ListInboxUser.Add(new InboxUser(user, info[0],info[1]));
                        }


                    }

                }
                else
                {
                    MessageBox.Show("Please Login to Facebook first", "Login unsuccessful", MessageBoxButtons.OK,
                                           MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                Log(e);
            }

            
        }




       

        private void listViewUserList_ItemActivate(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to use this user to communicate?", "Choose User", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                lbUserNameComm.Text = item.Text;
                lbUserIdComm.Text = item.Name;
                pBoxUserComm.ImageLocation = FB_Image.BaseDirectory + "Test_User\\profilePiture_" + item.Name + ".jpg";
            }
        }

        /// <summary>
        /// Uploading image to facebook and tag friends
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="browseImageUP"></param>
        /// <param name="uids"></param>
        /// <param name="albumID"></param>
        /// <returns></returns>
        private string Upload_Picture_Tag(string filename, FB_Image browseImageUP, List<string> uids,string albumID=null)
        {
            //upload photo
            try
            {
                browseImageUP.FileName = Path.GetFileName(filename); // lấy file name
                browseImageUP.Directory = Path.GetDirectoryName(filename); // get path
                browseImageUP.FileNameWithOutExtension = Path.GetFileNameWithoutExtension(filename); // lấy file name ko có phần mở rộng .jpg
                browseImageUP.FileSize = new FileInfo(filename).Length; // lấy file size
                var image_source = Image.FromFile(filename);
                browseImageUP.Height = image_source.Height;
                browseImageUP.Width = image_source.Width;

                var imgstream = File.OpenRead(filename);
                var fb = new FacebookClient(ActiveUser.AccessToken);
                string id = string.IsNullOrEmpty(albumID)?"me":albumID;

                string paramTag = "";
                if (uids != null && uids.Count > 0)
                {
                    paramTag = "[";
                    foreach (string s in uids)
                    {
                        paramTag += "{\"tag_uid\":\"" + s + "\",\"x\":0,\"y\":0}";
                    }
                    paramTag += "]";
                }
                
               

                dynamic res = fb.Post(id+"/photos", new
                {
                    message = "Image description",
                    file = new FacebookMediaStream
                    {
                        ContentType = "image/jpeg",
                        FileName = browseImageUP.FileName,
                    }.SetValue(imgstream),
                    tags = paramTag
                });
                return res.id;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private string SendImageWithTag(IProgress<string> progress, string filename, string inputText, List<string> uids, string albumID=null)
        {
            try
            {
                string coverImage = Common.CopyFileTo(filename, FB_Image.BaseDirectory);
                string messageFile = Common.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                string encodeFile = JPHideEncode(Path.GetFileName(coverImage), Path.GetFileName(messageFile));
                FB_Image encodeImage = new FB_Image();
                if (progress != null)
                    progress.Report("25|25|Uploading Picture");
                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage, uids,albumID);
                FB_Image downloadImage = new FB_Image();
                encodeImage.CopyTo(downloadImage);
                downloadImage.ImageID = id;
                if (progress != null)
                    progress.Report("50|50|Checking ...");
                string tempFileName = Download_Picture_FB(downloadImage);
                //Decode
                string outputText = Common.AppenFileName(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(tempFileName), outputText);
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                    progress.Report("100|100|Finish");
                if (Common.CompareOutputFile(inputText, outputText, tbInputMessage))
                {
                    return downloadImage.ImageID;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(downloadImage.ImageID);  // xóa ảnh
                    return null;
                }

            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public void GetImageTagged(IProgress<int> progress)
        {
            try
            {
                //string created = "1449376455";
                var fb = new FacebookClient(ActiveUser.AccessToken);
                dynamic res = fb.Get("me/photos?fields=from");
                string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                var json = JObject.Parse(json_string);
                dynamic photos = json["data"];
                int count = photos.Count;
                string source_url = "";
                string json_string_image = "";
                string imagePath = "";
                List<string> imagesId = new List<string>(); 
                int i = 0;
                ImageList successImage = new ImageList();

                while (i < count && photos[i] != null)
                {
                    if ((string)photos[i]["from"]["id"] == lbUserIdComm.Text)
                    {
                        string imageId = (string)photos[i]["id"];
                        dynamic images = fb.Get(imageId+"?fields=images");
                        json_string_image = Newtonsoft.Json.JsonConvert.SerializeObject(images);
                        var json_image = JObject.Parse(json_string_image);

                        source_url = (string)json_image["images"][0]["source"];

                        using (WebClient webClient = new WebClient())
                        {
                            byte[] data = webClient.DownloadData(source_url);

                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                using (var yourImage = Image.FromStream(mem))
                                {
                                    imagePath = FB_Image.BaseDirectory + imageId + ".jpg";
                                    yourImage.Save(imagePath, ImageFormat.Jpeg);
                                }
                            }
                        }
                        successImage.Images.Add(Image.FromFile(imagePath));
                        successImage.Images.SetKeyName(i, imageId);
                        imagesId.Add(imageId);
                    }
                    break;
                }

                FB_Image.Image_Tags_In = successImage;
                FB_Image.Image_TagsID_In = imagesId;
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public void GetImageTagged1(IProgress<int> progress,List<InboxUser> inbox)
        {
            try
            {
                
                string checkTime = ActiveUser.CheckTime.ToString();
                // Make Facebook request
                var fb = new FacebookClient(ActiveUser.AccessToken);
                dynamic res = fb.Get("me/photos?fields=from,id,created_time,images&date_format=U&since=" + checkTime);
                //Hanlde response from graph api
                string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                var json = JObject.Parse(json_string);
                dynamic photos = json["data"];
                int count = photos.Count;

                string source_url = "";
                string imagePath = "";
                int i = 0;

                while (i < count )
                {
                    dynamic currentPhoto = photos[i];
                    string userID = (string)currentPhoto["from"]["id"];
                    InboxUser userInbox = Common.GetInboxByUserID(userID,inbox);
                    if (userInbox == null)
                    {
                        i++;
                        continue;
                    }
                    dynamic images = currentPhoto["images"];
                    int countImages = images.Count;
                    source_url = (string)images[0]["source"];
                    string imageId = (string)currentPhoto["id"];

                    foreach(dynamic image in images)
                    {
                        if (((string)image["width"]).Equals("960"))
                        {
                            source_url = (string)image["source"];
                        }
                    }
                    imagePath = FB_Image.BaseDirectory + imageId + ".jpg";
                    Common.DowloadImageFromLink(source_url, imagePath, ImageFormat.Jpeg);
                    string content = Common.GetMessageFromImage(imagePath);
                    userInbox.Messages.Add(new FB_Message(content, new FB_Image(imageId, Path.GetFileName(imagePath), Path.GetDirectoryName(imagePath))));
                    userInbox.CountNew++;
                    i++;
                    
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private List<FB_Message> GetNewMessageFromImage(IProgress<string> progress,string imageID)
        {
            List<FB_Message> ListMessages = new List<FB_Message>();
            FB_Message message = new FB_Message();
            FB_Image messageImage = new FB_Image();
            messageImage.FileName = imageID + ".jpg";
            messageImage.Directory = FB_Image.BaseDirectory + imageID + ".jpg";
            messageImage.ImageID = imageID;

            message.Image = messageImage;
            if (message.Image.FileName != "")
            {
                string output = JPSeekDecode(message.Image.FileName, Path.GetFileNameWithoutExtension(message.Image.FileName) + ".txt");
                if (output != null && output != "")
                {
                    // check CRC
                    message.Content = File.ReadAllText(Path.Combine(FB_Image.BaseDirectory, output));
                }

            }
            ListMessages.Add(message);

            return ListMessages;

        }
    }

}
