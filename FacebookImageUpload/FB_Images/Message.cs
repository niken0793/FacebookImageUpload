using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookImageUpload.FB_Images
{
    public class FB_Message
    {

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

        private string createdDate;

        public string CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private bool isRead;

        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }



        private FB_Image image;

        public FB_Image Image
        {
            get { return image; }
            set { image = value; }
        }


        public FB_Message()
        {
            isRead = false;
        }


        public FB_Message(string paramContent, FB_Image paramImage):this()
        {
            content = paramContent;
            image = paramImage;
        }

    }

     [Serializable]
    public class InboxUser
    {
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private long checkTime;

        public long CheckTime
        {
            get { return checkTime; }
            set { checkTime = value; }
        }
        private List<FB_Message> messages;

        public List<FB_Message> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
        private int countNew;

        public int CountNew
        {
            get { return countNew; }
            set { countNew = value; }
        }

        public InboxUser()
        {
            messages = new List<FB_Message>();
        }

        public InboxUser(string paramID, string paramName, long paramCheckTime):this()
        {
            userID = paramID;
            userName = paramName;
            checkTime = paramCheckTime;
        }

        public InboxUser(string paramID, string paramName):this()
        {
            userID = paramID;
            userName = paramName;
        }


    }
}
