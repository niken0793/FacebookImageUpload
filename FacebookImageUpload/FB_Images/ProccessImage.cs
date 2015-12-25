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
using FacebookImageUpload.FB_Images;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BitMiracle.LibJpeg;
using BitMiracle;
using BitMiracle.LibJpeg.Classic;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace FacebookImageUpload
{


    public class ProcessImage
    {
        public static void saveJpeg(string oldPath, string NewPath, long quality)
        {
            // Encoder parameter for image quality
            Bitmap img = new Image<Bgr, byte>(oldPath).ToBitmap();
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(NewPath, jpegCodec, encoderParams);
        }

        public static ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        public static string PrintDctTable(jvirt_array<JBLOCK>[] JBlock, int startRow, int numRow, int numOfCol = 5)
        {
            string t = "";
            try
            {
                if (JBlock.Length >= 3 && JBlock[0] != null)
                {
                    var b = JBlock[0].Access(startRow, numRow);
                    for (int i = 0; i < numRow; i++)
                    {

                        for (int j = 0; j < numOfCol; j++)
                        {
                            t += "Block " + (startRow + i).ToString() + ": " + j.ToString() + " " + Environment.NewLine;
                            for (int m = 0; m < 64; m++)
                            {
                                t += " " + b[i][j].data[m] + " ";
                            }
                            t += Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return t;
        }
        public static List<DiffDCT> CompareDctFiles(string fullPathA, string fullPathB, bool zero = false)
        {
            List<DiffDCT> result;
            var a = new Bitmap(fullPathA);
            int width = a.Width / 8;
            int height = a.Height / 8;
            a.Dispose();
            jvirt_array<JBLOCK>[] blockA = ReadCoefFromImage(fullPathA);
            jvirt_array<JBLOCK>[] blockB = ReadCoefFromImage(fullPathB);
            result = CompareTwoDct(blockA, blockB, height, width,zero);
            return result;
        }
        public static jvirt_array<BitMiracle.LibJpeg.Classic.JBLOCK>[] ReadCoefFromImage(string fullPath)
        {
            var img = new Bitmap(fullPath);
            var width = img.Width;
            var height = img.Height;
            img.Dispose();
            BitMiracle.LibJpeg.Classic.jpeg_decompress_struct oJpegDecompress = new BitMiracle.LibJpeg.Classic.jpeg_decompress_struct();
            System.IO.FileStream oFileStreamImage = new System.IO.FileStream(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            oJpegDecompress.jpeg_stdio_src(oFileStreamImage);
            oJpegDecompress.jpeg_read_header(true);
            BitMiracle.LibJpeg.Classic.jvirt_array<BitMiracle.LibJpeg.Classic.JBLOCK>[] JBlock = oJpegDecompress.jpeg_read_coefficients();
            oJpegDecompress.jpeg_finish_decompress();
            oFileStreamImage.Close();
            return JBlock;
        }

        public static List<DiffDCT> CompareTwoDct(jvirt_array<JBLOCK>[] ABlock, jvirt_array<JBLOCK>[] BBlock, int h, int w,bool zero = false)
        {
            try
            {
                List<DiffDCT> diff = new List<DiffDCT>();
                if (ABlock.Length >= 3 && ABlock[0] != null)
                {
                    var a = ABlock[0].Access(0, h);
                    var b = BBlock[0].Access(0, h);
                    for (int i = 0; i < h; i++)
                    {

                        for (int j = 0; j < w; j++)
                        {
                            bool flag = false;
                            string s = "";
                            DiffDCT dif = new DiffDCT(new Point(i, j), s);
                            if (!zero)
                            {
                                for (int m = 0; m < 64; m++)
                                {

                                    if (a[i][j].data[m] != b[i][j].data[m])
                                    {
                                        //point.Add(new Point(i, j));
                                        //break;
                                        //if (m == 0 && fixDCT && a[i][j].data[m] % 2 == 0)
                                        //    tbTesting.AppendText(String.Format("{0} -> {1}", a[i][j].data[m], b[i][j].data[m])+Environment.NewLine);
                                        if (m == 0)
                                            dif.zero = 1;
                                        s += String.Format(" {0} : {1} -> {2} ", m, a[i][j].data[m], b[i][j].data[m]);
                                        flag = true;

                                    }
                                }

                                dif.s = s;
                                if (flag)
                                {
                                    diff.Add(dif);
                                }
                            }
                            else
                            {
                                if (a[i][j].data[0] != b[i][j].data[0])
                                {
                                    s += String.Format(" {0} : {1} -> {2} ", 0, a[i][j].data[0], b[i][j].data[0]);
                                    dif.s = s;
                                    diff.Add(dif);

                                }
                            }


                        }
                    }
                }
                return diff;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }

        }

        public static string DecImageDCT(string fullPath, List<DiffDCT> points)
        {

            string suffix = "fixDCT";
            var img = new Bitmap(fullPath);
            var width = img.Width;
            var height = img.Height;
            int hd = height / 8;
            int wd = width / 8;
            img.Dispose();
            BitMiracle.LibJpeg.Classic.jpeg_decompress_struct oJpegDecompress = new BitMiracle.LibJpeg.Classic.jpeg_decompress_struct();
            System.IO.FileStream oFileStreamImage = new System.IO.FileStream(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            oJpegDecompress.jpeg_stdio_src(oFileStreamImage);
            oJpegDecompress.jpeg_read_header(true);

            // View DCT

            BitMiracle.LibJpeg.Classic.jvirt_array<BitMiracle.LibJpeg.Classic.JBLOCK>[] JBlock = oJpegDecompress.jpeg_read_coefficients();
            var block = JBlock[0].Access(0, hd); // accessing the element

            if (points.Count > 0)
            {
                foreach (DiffDCT i in points)
                {
                    short t = block[i.Points.X][i.Points.Y].data[0];
                    if ((t >= 0 && t % 2 == 1) || (t < 0 && t % 2 == 0))
                    {
                        t++;
                    }
                    else if ((t >= 0 && t % 2 == 0) || (t < 0 && t % 2 == 1))
                    {
                        t--;
                    }
                    block[i.Points.X][i.Points.Y].data[0] = t;
                }
            }

            oJpegDecompress.jpeg_finish_decompress();
            oFileStreamImage.Close();
            ////
            string filenameNew = Common.AppendFileName(fullPath, suffix);
            System.IO.FileStream objFileStreamMegaMap = System.IO.File.Create(filenameNew);
            BitMiracle.LibJpeg.Classic.jpeg_compress_struct oJpegCompress = new BitMiracle.LibJpeg.Classic.jpeg_compress_struct();
            oJpegCompress.jpeg_stdio_dest(objFileStreamMegaMap);
            oJpegDecompress.jpeg_copy_critical_parameters(oJpegCompress);
            oJpegCompress.Image_height = height;
            oJpegCompress.Image_width = width;
            oJpegCompress.jpeg_write_coefficients(JBlock);
            oJpegCompress.jpeg_finish_compress();
            objFileStreamMegaMap.Close();
            oJpegDecompress.jpeg_abort_decompress();
            return filenameNew;
        }

        public static string IncImageDCT(string fullPath)
        {
            string suffix = "testDCT";
            var img = new Bitmap(fullPath);
            var width = img.Width;
            var height = img.Height;
            int hd = height / 8;
            int wd = width / 8;
            img.Dispose();
            BitMiracle.LibJpeg.Classic.jpeg_decompress_struct oJpegDecompress = new BitMiracle.LibJpeg.Classic.jpeg_decompress_struct();
            System.IO.FileStream oFileStreamImage = new System.IO.FileStream(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            oJpegDecompress.jpeg_stdio_src(oFileStreamImage);
            oJpegDecompress.jpeg_read_header(true);
            BitMiracle.LibJpeg.Classic.jvirt_array<BitMiracle.LibJpeg.Classic.JBLOCK>[] JBlock = oJpegDecompress.jpeg_read_coefficients();
            var block = JBlock[0].Access(0, hd); // accessing the element
            for (int i = 0; i < hd; i++)
            {
                for (int j = 0; j < wd; j++)
                {
                    short t = block[i][j].data[0];
                    if ((t >= 0 && t % 2 == 1) || (t < 0 && t % 2 == 0))
                    {
                        t--;
                    }
                    else if ((t >= 0 && t % 2 == 0) || (t < 0 && t % 2 == 1))
                    {
                        t++;
                    }
                    block[i][j].data[0] = t;
                }
            }
            oJpegDecompress.jpeg_finish_decompress();
            oFileStreamImage.Close();
            ////
            string filenameNew = Common.AppendFileName(fullPath, suffix);
            System.IO.FileStream objFileStreamMegaMap = System.IO.File.Create(filenameNew);
            BitMiracle.LibJpeg.Classic.jpeg_compress_struct oJpegCompress = new BitMiracle.LibJpeg.Classic.jpeg_compress_struct();
            oJpegCompress.jpeg_stdio_dest(objFileStreamMegaMap);
            oJpegDecompress.jpeg_copy_critical_parameters(oJpegCompress);
            oJpegCompress.Image_height = height;
            oJpegCompress.Image_width = width;
            oJpegCompress.jpeg_write_coefficients(JBlock);
            oJpegCompress.jpeg_finish_compress();
            objFileStreamMegaMap.Close();
            oJpegDecompress.jpeg_abort_decompress();
            return filenameNew;
        }

        public static int Ratio = 72;


        public static string ReSaveFileToMinimal(string fullPath, string newDir )
        {
            try
            {


                string file1 = Common.AppendFileName(fullPath, "resave1");
                string file2 = Common.AppendFileName(fullPath, "resave2");
                File.Copy(fullPath, file1, true);
                int vstart = 5;
                while (vstart > 0)
                {
                    saveJpeg(file1, file1, Ratio);
                    vstart--;
                }
                bool flag = true;
                while (flag)
                {

                    File.Copy(file1, file2, true);
                    saveJpeg(file1, file1, Ratio);
                    List<DiffDCT> r = CompareDctFiles(file1, file2);
                    if (r.Count == 0)
                    {
                        flag = false;
                    }
                }


                if (newDir != null)
                    File.Copy(file1, Path.Combine(newDir, Path.GetFileName(file1)),true);
                return file1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public static string ReSaveFileToMinimal(string fullPath, string newDir, string token, string albumID)
        {
            try
            {


                string file1 = Common.AppendFileName(fullPath, "resave1");
                string file2 = Common.AppendFileName(fullPath, "resave2");
                File.Copy(fullPath, file1, true);
                int vstart = 5;
                while (vstart > 0)
                {
                    saveJpeg(file1, file1, Ratio);
                    vstart--;
                }
                bool flag = true;
                while (flag)
                {

                    File.Copy(file1, file2, true);
                    saveJpeg(file1, file1, Ratio);
                    List<DiffDCT> r = CompareDctFiles(file1, file2);
                    if (r.Count == 0)
                    {
                        flag = false;
                    }
                }

                string id = Form1.UploadFB(file1, token, albumID);
                if (!Form1.DownloadFB(id, token, file1,true))
                {
                    return null;
                }
                List<DiffDCT> diff = CompareDctFiles(file1, file2,true);
                if (diff.Count == 0)
                {
                    if (newDir != null)
                        File.Copy(file2, Path.Combine(newDir, Path.GetFileName(file2)), true);
                    return file1;
                }
                else
                {

                    vstart = 3;
                    while (vstart > 0)
                    {
                        saveJpeg(file1, file1, Ratio);
                        vstart--;
                    }
                    flag = true;
                    while (flag)
                    {

                        File.Copy(file1, file2, true);
                        saveJpeg(file1, file1, Ratio);
                        List<DiffDCT> r = CompareDctFiles(file1, file2);
                        if (r.Count == 0)
                        {
                            flag = false;
                        }
                    }

                    if (newDir != null)
                        File.Copy(file1, Path.Combine(newDir, Path.GetFileName(file1)), true);
                    return file1;
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public static string FixFailImage(string fullPath)
        {
            try
            {
                int step = 3;
                string s = fullPath;
                while (step > 0)
                {
                    string testFile = IncImageDCT(s);
                    string testFileCopy = Common.AppendFileName(testFile, "copy");
                    saveJpeg(testFile, testFileCopy, Ratio);
                    List<DiffDCT> a = CompareDctFiles(testFile, testFileCopy, true);
                    if (a.Count > 0)
                    {
                        string fixFile = DecImageDCT(fullPath, a);
                        s = fixFile;
                    }
                    else
                    {
                        return s;
                    }
                    step--;

                }
                return null;


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }

        }




    }

   public class DiffDCT
    {
        public Point Points;
        public string s = "";
        public int zero;

        public DiffDCT()
        {
            zero = 0;
        }

        public DiffDCT(Point p, string ss)
            : this()
        {
            Points = p;
            s = ss;
        }
    }
}
