using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookImageUpload.FB_Images
{
    public class FB_Message
    {
        private string sender;

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        private string receiver;

        public string Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }
        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        private string crc;

        public string Crc
        {
            get { return crc; }
            set { crc = value; }
        }

        private FB_Image image;

        public FB_Image Image
        {
            get { return image; }
            set { image = value; }
        }


        public FB_Message()
        {
            sender = "";
            receiver = "";
            content = "";
            crc = "";
        }
        public FB_Message(string paramSender, string paramReceiver, string paramContent,FB_Image paramImage)
        {
            sender = paramSender;
            receiver = paramReceiver;
            content = paramContent;
            image = paramImage;
        }

        public FB_Message(string paramContent, FB_Image paramImage)
        {
            content = paramContent;
            image = paramImage;
        }

    }
}
