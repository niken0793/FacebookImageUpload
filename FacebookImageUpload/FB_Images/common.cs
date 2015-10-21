﻿using System;
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

// Đạt file
namespace FacebookImageUpload
{
    public class common
    {

    }

    public partial class Form1 : Form
    {
        public void openfile_Click_fn()
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    tbImagePath.Text = open.FileName;
                    browseImage.FileName = Path.GetFileName(tbImagePath.Text); // lấy file name
                    browseImage.FileNameWithOutExtension = Path.GetFileNameWithoutExtension(tbImagePath.Text); // lấy file name ko có phần mở rộng .jpg
                    browseImage.UpFileSize = new FileInfo(tbImagePath.Text).Length; // lấy file size
                    var image_source = Image.FromFile(tbImagePath.Text);
                    browseImage.Height = image_source.Height;
                    browseImage.Width = image_source.Width;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
    }

}
