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
        public string Crc32Hash(string filepath)
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;
            using (FileStream fs = File.Open(filepath, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            return hash;
        }
        public void btnJPHide_Click_fn()
        {
            try
            {
                string imageName = Path.GetFileName(tbJpgImage.Text);
                string hiddenFileName = Path.GetFileName(tbHiddenFile.Text);
                string enImageName = "en_" + imageName;
                string path = "Lib\\" + hiddenFileName;
                string hash = Crc32Hash(path);
                string crc = "[crc:" + hash + "]";
                string currentContent = "";

                if (File.Exists(path))
                {
                    currentContent = File.ReadAllText(path);
                }
                File.WriteAllText(path, crc + currentContent);

                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.WorkingDirectory = "Lib";
                proc.StartInfo.Arguments = "/C jphide_modify " + imageName + " " + enImageName + " " + hiddenFileName;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
               
                proc.Start();
                while (!proc.HasExited)
                    ;
                MessageBox.Show("Success");
              
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
                proc.StartInfo.WorkingDirectory = "Lib";
                proc.StartInfo.Arguments = "/C jpseek_modify " + imageName + " " + hiddenFileName;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                while (!proc.HasExited)
                    ;

                string oldpath = "Lib\\" + hiddenFileName;
                string newpath = "Lib\\message.txt";
                string crc = "";
                Regex g = new Regex(@"(\[)(crc)(:).*?(\])");
                using (StreamWriter w = new StreamWriter(newpath))
                {

                    using (StreamReader r = new StreamReader(oldpath))
                    {
                        string line;                   
                        while ((line = r.ReadLine()) != null)
                        {                     
                            Match m = g.Match(line);
                            if (m.Success)
                            {
                                /*w.WriteLine(line);
                            }
                            else
                            {*/
                                string[] words = line.Split(':');
                                crc = words[1].Remove(8);
                                line = line.Remove(0, 14);
                                w.WriteLine(line);
                            }
                        }
                    }


                }
                lbCrc32String.Text = crc;
                string hash = Crc32Hash(newpath);
                if (hash == crc)
                {
                    MessageBox.Show("CRC-32 check successfully!");
                    tbMessage.Text = File.ReadAllText(newpath);
                }
                else
                {
                    MessageBox.Show("CRC-32 check failed!");
                }
               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }

}
