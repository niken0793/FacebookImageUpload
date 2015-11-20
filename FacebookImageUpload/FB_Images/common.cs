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



        public void btnCrc32_Click_fn()
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;

            using (FileStream fs = File.Open("D:\\malwarescanner.zip", FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            MessageBox.Show("CRC-32 is " + hash);
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

        public void LoadSuccessImage()
        {
            DirectoryInfo dir = new DirectoryInfo(@"SuccessImage");
            ImageList successImage = new ImageList();
            int i = 0;
            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    successImage.Images.Add(Image.FromFile(file.FullName));
                    successImage.Images.SetKeyName(i, file.Name);
                    i += 1;
                }
                catch
                {

                }
            }
            this.listViewSuccessImage.View = View.LargeIcon;
            successImage.TransparentColor = Color.Blue;
            successImage.ColorDepth = ColorDepth.Depth32Bit;
            successImage.ImageSize = new Size(50, 50);
            this.listViewSuccessImage.LargeImageList = successImage;
            for (int j = 0; j < successImage.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = successImage.Images.Keys[j].ToString();
                item.ImageIndex = j;
                this.listViewSuccessImage.Items.Add(item);
            }
        }

       

    }

}
