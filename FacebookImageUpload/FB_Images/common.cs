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

// Đạt file
namespace FacebookImageUpload
{
    public class common
    {

    }

    public partial class Form1 : Form
    {
        public void openfile_Click_fn(TextBox tb, string filter)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = filter;
                if (open.ShowDialog() == DialogResult.OK)
                {
                    tb.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
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

       

    }

}
