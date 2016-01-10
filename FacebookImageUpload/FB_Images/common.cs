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
                        lbImageSize.Text = MyHelper.BytesToString(new FileInfo(open.FileName).Length);
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

        public static string 
            Crc32Hash(string filepath)
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;
            using (FileStream fs = File.Open(filepath, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            return hash;
        }

        public static string Crc32HashFromString(string content)
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;
            foreach (byte b in crc32.ComputeHash(Encoding.UTF8.GetBytes(content))) hash += b.ToString("x2").ToLower();
            return hash;
        }

        uint start = 1;
        int gtemp = 0;
        

        public void GetProcessImageFromGoogleImage(IProgress<string> progress)
        {
            //string apiKey = "AIzaSyCKOq5EJwqwfQzmdfCW0VE-IX9fFMIZEUM";
            //string searchEngineId = "002524252275919064823:dlgwbkge9f0";
            
            string apiKey = "AIzaSyCOQh0JLePZur6o26W1lwI5mpj5pa3M5oA";
            string searchEngineId = "001783529186805515716:jqccojujq9c";
            GoogleImage googleImage = new GoogleImage();
            string query = tbKeyWord.Text;
            //start number of result return
            int count = 0;
            int numOfImage = 5;
            while (count <numOfImage)
            {
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
                                    string new_path = Path.Combine(FB_Image.BaseDirectory , query + "_" + gtemp + ".jpg");
                                    var newImage = googleImage.ScaleImage(yourImage, 960, 720);
                                    newImage.Save(new_path, ImageFormat.Jpeg);
                                    MyHelper.ListFileDelete.Add(new_path);
                                    string filename = new_path;
                                    string testMessage = SplitFileIntoPart(Path.Combine(FB_Image.RelativeDirectory,FB_Image.TestInputDir,"200.txt"),0)[0];
                                    string coverImagePath = TestEncode(null, filename, testMessage, ActiveUser.PrivateAlbumID, true);
                                    if (coverImagePath != null)
                                    {
                                        File.Copy(coverImagePath, Path.Combine(FB_Image.RelativeDirectory,FB_Image.SuccessImageDir, query + "_" + gtemp + ".jpg"), true);
                                        count++;
                                        progress.Report("success"+Environment.NewLine);
                                    }
                                    else
                                    {
                                        progress.Report("Fail"+Environment.NewLine);
                                    }

                                }
                            }

                        }
                        if (cancelSearching)
                        {
                            isStart = false;
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Log(e);
                    }
                    gtemp += 1;

                }
                MyHelper.DeleteFile(MyHelper.ListFileDelete);
                start += 10;
            }
        }

        public void searchImageFromGoogle(IProgress<string> progress)
        {
            //string apiKey = "AIzaSyCKOq5EJwqwfQzmdfCW0VE-IX9fFMIZEUM";
            //string searchEngineId = "002524252275919064823:dlgwbkge9f0";
            string apiKey = "AIzaSyCOQh0JLePZur6o26W1lwI5mpj5pa3M5oA";
            string searchEngineId = "001783529186805515716:jqccojujq9c";
            GoogleImage googleImage = new GoogleImage();
            string query = tbKeyWord.Text;
            //start number of result return
            int count = 0;
            while (count < 10)
            {
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
                                    string new_path = Path.Combine(FB_Image.RelativeDirectory, FB_Image.SuccessImageDir,query + "_" + gtemp + ".jpg");
                                    var newImage = googleImage.ScaleImage(yourImage, 960, 720);
                                    newImage.Save(new_path, ImageFormat.Jpeg);

                                }
                            }

                        }


                    }
                    catch (Exception e)
                    {
                        Log(e);
                    }
                    gtemp += 1;

                }
                MyHelper.DeleteFile(MyHelper.ListFileDelete);
                start += 10;
            }
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
                    if (MyAppSetting == null)
                        MyAppSetting = new AppSetting();
                    MyAppSetting.ActiveUser = ActiveUser.UserID;
                    SaveAppSettingOnDisk(MyAppSetting);

                }
                else
                {
                    isLogin = false;
                }
                LoadFriendList();
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
            MyAppSetting.ActiveUser = "0";
           
        }

        private void UpdateLoginControl(FacebookOAuthResult r)
        {
            if (r != null)
            {
                
                List<string> s = MyHelper.getUserInfo(r.AccessToken, "me", FB_Image.UserImageDir);
                pBoxUserAvatar.ImageLocation = s[1];
                lbLoginUserName.Text = s[0];
                lbLoginID.Text = s[2];
                bool flag = false;
                //Set private album
                if (File.Exists(Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserSettingDir,s[2])))
                {
                    UserSetting a = MyHelper.DeSerializeObject<UserSetting>(Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserSettingDir , s[2]));
                    if (a != null)
                    {
                        Form1.ActiveUser = a;
                    }
                    else
                    {
                        Form1.ActiveUser = new UserSetting(r.AccessToken, s[0], s[2], r.Expires.ToString(), s[1]);
                        Form1.ActiveUser.CheckTime = MyHelper.GetUnixTimesStamp(DateTime.Now);
                    }
                    
                    if (a != null && !string.IsNullOrEmpty(a.PrivateAlbumID) && !string.IsNullOrEmpty(a.PrivateAlbumName))
                    {
                        ActiveUser.PrivateAlbumID = a.PrivateAlbumID;
                        ActiveUser.PrivateAlbumName = a.PrivateAlbumName;
                        lbPrivateAlbum.Text = a.PrivateAlbumName.ToString();
                        lbLoginPrivateAlbum.Text = a.PrivateAlbumName.ToString();
                        flag = true;
                    }


                }
                else
                {
                    Form1.ActiveUser = new UserSetting(r.AccessToken, s[0], s[2], r.Expires.ToString(), s[1]);
                    Form1.ActiveUser.CheckTime = MyHelper.GetUnixTimesStamp(DateTime.Now);
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
                            lbLoginPrivateAlbum.Text = a.AlbumName.ToString();
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
              
                btnFacebookLogin.Text = "LogOut";
                isLogin = true;

            }
            else
            {
                lbLoginUserName.Text = "...";
                lbLoginID.Text = "...";
                pBoxUserAvatar.Image = Properties.Resources.profile;
                Form1.ActiveUser = null;
                btnFacebookLogin.Text = "Login";
                lbPrivateAlbum.Text = "...";
                lbLoginPrivateAlbum.Text = "...";
                isLogin = false;
                cmbInputAlbum.Items.Clear();
            }
            
        }
        private void UpdateLoginControl()
        {
            lbLoginUserName.Text = "Username";
            lbLoginID.Text = "...";
            pBoxUserAvatar.Image = Properties.Resources.profile;
            Form1.ActiveUser = null;
            btnFacebookLogin.Text = "Login";
            isLogin = false;
            lbPrivateAlbum.Text = "...";
            lbLoginPrivateAlbum.Text = "...";
            currentInbox = null;
            listViewFriends.Clear();
            listViewMessage.Clear();
            listViewInbox.Clear();

        }

        private void UpdateLoginControl(UserSetting r)
        {
            if (r != null)
            {
               
                Form1.ActiveUser = r;
                ActiveUser.AccessToken = r.AccessToken;
                pBoxUserAvatar.ImageLocation = r.ImgPath;
                lbLoginUserName.Text = r.UserName;
                lbLoginID.Text = r.UserID;
                lbPrivateAlbum.Text = r.PrivateAlbumName;
                lbLoginPrivateAlbum.Text = r.PrivateAlbumName;
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






        private void UpdateFriendListView(ListView listview ,List<InboxUser> user, bool showNewMessage = false)
        {
            try
            {
                listview.Clear();
                ImageList il = new ImageList();
                il.TransparentColor = Color.Blue;
                il.ColorDepth = ColorDepth.Depth32Bit;
                il.ImageSize = new Size(50, 50);
                for (int i = 0; i < user.Count; i++)
                {
                    il.Images.Add(Image.FromFile(user[i].ProfilePath));
                    il.Images.SetKeyName(i, user[i].UserName);
                }
                listview.View = View.LargeIcon;
                listview.LargeImageList = il;
                for (int j = 0; j < user.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    if (showNewMessage)
                    {
                        item.Text = GetInboxText(il.Images.Keys[j].ToString(), user[j].CountNew);
                        UpdateNewMessageLabel(null, lbReceiverNewMessage, user[j]);
                    }
                    else
                    {
                        item.Text = il.Images.Keys[j].ToString();
                    }

                    item.Name = user[j].UserID;
                    item.ImageIndex = j;
                    listview.Items.Add(item);

                }
            }
            catch (Exception e)
            {
                Form1.Log(e);
            }

        }

        private void GetFriendList(IProgress<string> progress)
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
                    i=0;
                    ListInboxUser.Clear();
                    foreach (string user in users.ToArray())
                    {
                        List<string> info = MyHelper.getUserInfo(ActiveUser.AccessToken, user, FB_Image.UserImageDir);

                        InboxUser a = MyHelper.GetInboxByUserID(user, ListInboxUser);
                        if (a == null && info!=null && info.Count==3)
                        {
                            ListInboxUser.Add(new InboxUser(user, info[0],info[1]));
                            if (progress != null)
                            {
                                string per = (i / count).ToString();
                                progress.Report(String.Format("{0}|{1}",per,"Getting Friends List"));
                            }
                        }
                        i++;


                    }
                    progress.Report(String.Format("{0}|{1}", 100, "Getting Friends List"));

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


        /// <summary>
        /// Uploading image to facebook and tag friends
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="browseImageUP"></param>
        /// <param name="uids"></param>
        /// <param name="albumID"></param>
        /// <returns></returns>
        private string Upload_Picture_Tag(string filename, List<string> uids,string albumID=null)
        {
            //upload photo
            try
            {
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
                        FileName = filename,
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

        private string EncodeImage(string imageFile, string inputText, string password = null, bool isTest= false)
        {
            string coverImage = MyHelper.CopyFileTo(imageFile, FB_Image.BaseDirectory);
            string messageFile = MyHelper.CopyFileTo(inputText, FB_Image.BaseDirectory);
            if (!isTest)
            {
                coverImage = ProcessUserImage(coverImage);
            }

            string encodeFile = JPHideEncode(Path.GetFileName(coverImage), Path.GetFileName(messageFile),password);
            return encodeFile;

        }

        private string DecodeImage(string imageFile, string password = null)
        {
            string coverImage = MyHelper.CopyFileTo(imageFile, FB_Image.BaseDirectory);
            string outputText = Path.GetFileNameWithoutExtension(imageFile)+"_output.txt";
             outputText = JPSeekDecode(Path.GetFileName(imageFile), outputText);
             return outputText;
        }


                

        private string SendImageWithTag(IProgress<string> progress, string filename, string inputText, List<string> uids, string albumID=null)
        {
            try
            {
                string coverImage = MyHelper.CopyFileTo(filename, FB_Image.BaseDirectory);
                string messageFile = MyHelper.CopyFileTo(inputText, FB_Image.BaseDirectory);

                //Encode
                string encodeFile = JPHideEncode(Path.GetFileName(coverImage), Path.GetFileName(messageFile));
                if (progress != null)
                    progress.Report("25|Uploading Picture");
                if (encodeFile == null)
                    return null;
                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile), uids,albumID);
                if (progress != null)
                    progress.Report("50|Checking ...");
                string downloadFile = MyHelper.AppendFileName(Path.Combine(FB_Image.BaseDirectory, encodeFile), "_download");
                if (!DownloadFB(id, ActiveUser.AccessToken, downloadFile, true))
                    return null;
                //Decode
                string outputText = MyHelper.AppendFileNameNoLimit(inputText, "_ouput");
                if (outputText == null)
                    outputText = "output_test.txt";
                outputText = JPSeekDecode(Path.GetFileName(downloadFile), outputText,null,true);
                if (outputText == null)
                    return null;
                outputText = Path.Combine(FB_Image.BaseDirectory, outputText);

                //compare 2 file
                if (progress != null)
                    progress.Report("100|Finish");
                if (MyHelper.CompareOutputFile(inputText, outputText))
                {
                    return id;
                }
                else
                {
                    var fb = new FacebookClient(ActiveUser.AccessToken);
                    dynamic res = fb.Delete(id);  // xóa ảnh
                    return null;
                }

            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public void GetImageTagged1(IProgress<string> progress,List<InboxUser> inbox)
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
                int i = count-1;

                while (i >= 0 )
                {
                    dynamic currentPhoto = photos[i];
                    string userID = (string)currentPhoto["from"]["id"];
                    long createdTime = long.Parse((string)currentPhoto["created_time"]);
                   // bool isSent = false;
                    InboxUser userInbox = MyHelper.GetInboxByUserID(userID,inbox);
                    if (userInbox == null)
                    {
                        i--;
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
                            break;
                        }
                    }
                    imagePath = Path.Combine(FB_Image.BaseDirectory , imageId + ".jpg");
                    MyHelper.DowloadImageFromLink(source_url, imagePath, ImageFormat.Jpeg);
                    byte[] content = MyHelper.GetByteFromImage(imagePath);
                    if(content != null)
                    {
                       string  s_content = CorrectErrorString(content);
                       if (s_content != null)
                       {
                           //userInbox.Messages.Add(new FB_Message(content, new FB_Image(imageId,imagePath),createdTime,isSent));
                           bool flag = userInbox.AddMessageToInbox(s_content, imageId, imagePath, createdTime);
                           if (flag)
                               userInbox.CountNew++;
                       }
                    }
                    i--;
                    
                    if (progress != null)
                    {
                        progress.Report(String.Format("{0}|{1}",(count-i)/count,"Getting Messages"));
                    }
                    
                }
                ActiveUser.CheckTime = MyHelper.GetUnixTimesStamp(DateTime.Now);
                progress.Report(String.Format("{0}|{1}", 100, "Getting Messages"));
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }


        //public bool CheckTester()
        //{
        //    var app = new FacebookClient(Form1.AppToken);
        //    dynamic res = app.Get(Form1.AppID + "/roles");
        //    string json_string = Newtonsoft.Json.JsonConvert.SerializeObject(res);
        //    var json = JObject.Parse(json_string);
        //    dynamic roles = json["data"];
        //    int count = roles.Count;
        //    int i = 0;
        //    while (i < count )
        //    {
        //        dynamic currentUser = roles[i];
        //        string userID = (string)currentUser["user"];
        //        if(userID == ActiveUser.UserID)
        //        {
        //            return true;
        //        }
        //        i++;
        //    }

        //    return false;
        //}
        //private void btnTester_Click(object sender, EventArgs e)
        //{
        //    dynamic userRole = new ExpandoObject();
        //    userRole.user = ActiveUser.UserID;
        //    userRole.role = "testers";
        //    var app = new FacebookClient(FB_Image.AccessToken);
        //    dynamic res = app.Post(Form1.AppID + "/roles", userRole);
        //    if (res.success == true) {
        //        MessageBox.Show("Join as tester successfully", "Tester", MessageBoxButtons.OK,
        //                                   MessageBoxIcon.Information);
        //        btnTester.Enabled = false;
        //    }
        //}
    }

}
