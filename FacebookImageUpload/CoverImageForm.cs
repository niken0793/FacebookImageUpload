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
using System.Threading.Tasks;
using Facebook; // PM
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using System.Diagnostics;
using FacebookImageUpload.FB_Images;
using System.Text.RegularExpressions;

namespace FacebookImageUpload
{
    public partial class CoverImageForm : Form
    {
        public string imageLink = "";
        public List<string> images = new List<string>();

        public CoverImageForm()
        {
            InitializeComponent();
            LoadSuccessImage("");
        }
        public void LoadSuccessImage(string pattern)
        {
            dynamic files = new ExpandoObject();

            if (pattern == "")
            {
                pattern = ".*jpg";
                files = Directory.GetFiles(Path.Combine(FB_Image.RelativeDirectory,FB_Image.SuccessImageDir))
               .Where(path => Regex.Match(path, pattern).Success);
            }
            else
            {
                files = Directory.GetFiles(Path.Combine(FB_Image.RelativeDirectory, FB_Image.SuccessImageDir))
               .Where(path => Regex.Match(path, pattern).Success);
            }
            this.listViewSuccessImage.Clear();
            ImageList successImage = new ImageList();
            int i = 0;
            images.Clear();
            foreach (string file in files)
            {
                try
                {
                    images.Add(file);
                    successImage.Images.Add(Image.FromFile(file));
                    successImage.Images.SetKeyName(i, Path.GetFileName(file));
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

        private void listViewSuccessImage_ItemActivate(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to use this Image as Cover Image?", "Choose Cover Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                imageLink = Path.Combine(FB_Image.RelativeDirectory,FB_Image.SuccessImageDir, item.Text);
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void btnCoverImageSearch_Click(object sender, EventArgs e)
        {
            string pattern = tbCoverImageKeyword.Text;
            LoadSuccessImage(pattern);
        }
    }
}
