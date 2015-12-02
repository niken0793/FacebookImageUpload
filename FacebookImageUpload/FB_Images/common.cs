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
        public string Crc32Hash(string filepath)
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
                                string coverImagePath = TestEncodeSuccessRate(null, filename, Path.Combine(FB_Image.RelativeDirectory, "GoogleImage/test_google1.txt"), "1663699240509583", false, true);
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
            facebookLoginForm = new FacebookLoginForm();
            facebookLoginForm.Show();
            facebookLoginForm.FormClosed += new FormClosedEventHandler(facebookLoginForm_FormClosed);
        }
        void facebookLoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnFacebookLogin.Enabled = false;
            tbUserAccessToken.Text = facebookLoginForm.userAccessToken;
            FB_Image.UserAccessToken = facebookLoginForm.userAccessToken;
            lbFacebookUserName.Text = facebookLoginForm.userName;
            lbAccessTokenExpire.Text = facebookLoginForm.userAccessTokenExpire;
            pBoxUserAvatar.ImageLocation = facebookLoginForm.userAvatarPath;
        }

        public void CheckTester()
        { 
            
        }
        private void btnTester_Click(object sender, EventArgs e)
        { 
            
        }

        private void btnGetUserList_Click(object sender, EventArgs e)
        {
            if (FB_Image.UserAccessToken != "")
            {
                var fb = new FacebookClient(FB_Image.UserAccessToken);
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
         
                // Add User to List view
                ImageList successImage = new ImageList();
                int k = 0;
            
                foreach (string user in users.ToArray())
                {
                    List<string> info = getUserInfo(FB_Image.UserAccessToken,user, FB_Image.BaseDirectory + "Test_User\\");

                    try
                    {
                        successImage.Images.Add(Image.FromFile(info[1]));
                        successImage.Images.SetKeyName(k, info[0]);
                        k += 1;
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                }
                this.listViewUserList.View = View.LargeIcon;
                successImage.TransparentColor = Color.Blue;
                successImage.ColorDepth = ColorDepth.Depth32Bit;
                successImage.ImageSize = new Size(50, 50);
                this.listViewUserList.LargeImageList = successImage;
                for (int j = 0; j < successImage.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = successImage.Images.Keys[j].ToString();
                    item.Name = users[j];
                    item.ImageIndex = j;
                    this.listViewUserList.Items.Add(item);
                }
                btnGetUserList.Enabled = false;
            }
            else 
            {
                MessageBox.Show("Please Login to Facebook first", "Login unsuccessful", MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
            }
            

        }

        public List<string> getUserInfo(string userAccessToken,string uid,string path)
        {
            string userAvatarPath = "";
            List<string> userInfo = new List<string>();
            var user = new FacebookClient(userAccessToken);
            dynamic me = user.Get(uid);
            string userName = me.name;
            string userId = me.id;
                    
            dynamic res = user.Get(uid+"?fields=picture");
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
                        userAvatarPath = path + "profilePiture_" + userId + ".jpg";
                        yourImage.Save(userAvatarPath, ImageFormat.Jpeg);
                    }
                }
            }

            userInfo.Add(userName);
            userInfo.Add(userAvatarPath);
            return userInfo;
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

        private string Upload_Picture_Tag(string filename, FB_Image browseImageUP, string uid)
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
                var fb = new FacebookClient(FB_Image.UserAccessToken);
                dynamic res = fb.Post("me/photos", new
                {
                    message = "Image description",
                    file = new FacebookMediaStream
                    {
                        ContentType = "image/jpeg",
                        FileName = browseImageUP.FileName,
                    }.SetValue(imgstream),
                    tags = "[{\"tag_uid\":\""+uid+"\",\"x\":0,\"y\":0}]"
                });
                return res.id;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private string SendImageWithTag(IProgress<string> progress, string filename, string inputText, string uid)
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
                string id = Upload_Picture_Tag(Path.Combine(FB_Image.BaseDirectory, encodeFile), encodeImage, uid);
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
                    var fb = new FacebookClient(FB_Image.AccessToken);
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
                var fb = new FacebookClient(FB_Image.UserAccessToken);

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
