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
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
        public void fdJpgImageOpen_Click_fn()
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg)|*.jpg; *.jpeg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    tbJpgImage.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
        public void fdHiddenFile_Click_fn()
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.txt)|*.txt";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    tbHiddenFile.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
        public void btnJPHide_Click_fn()
        {
            try
            {
                string imageName = Path.GetFileName(tbJpgImage.Text);
                string hiddenFileName = Path.GetFileName(tbHiddenFile.Text);
                string enImageName = "en_" + imageName;
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jphide " + imageName + " " + enImageName + " " + hiddenFileName;
               
                proc.Start();
                timerEncodeInput.Enabled = true;
                timerEncodeInput.Interval = 5 * 1000;
                timerEncodeInput.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        int count = 0;
        public void timerPassInput_Tick_fn()
        {
            if (count < 2)
            {
                SendKeys.Send(tbPassPhrase.Text);
                SendKeys.Send("{ENTER}");
                count++;
            }
            else
            {
                timerEncodeInput.Stop();
                MessageBox.Show("Image encoded successfully!");
                count = 0;
            }
        }
        public void btnJPSeek_Click_fn()
        {
            try
            {
                string imageName = Path.GetFileName(tbJpgImage.Text);
                string hiddenFileName = Path.GetFileName(tbHiddenFile.Text);
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = FB_Image.BaseDirectory;
                proc.StartInfo.Arguments = "/C jpseek " + imageName + " " + hiddenFileName;
                proc.Start();
                timerDecodeInput.Enabled = true;
                timerDecodeInput.Interval = 5 * 1000;
                timerDecodeInput.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void timerDecodeInput_Tick_fn()
        {
            if (count < 1)
            {
                SendKeys.Send(tbPassPhrase.Text);
                SendKeys.Send("{ENTER}");
                count++;
            }
            else
            {
                timerDecodeInput.Stop();
                MessageBox.Show("Message decoded successfully!");
                count = 0;
            }
        }
    }

}
