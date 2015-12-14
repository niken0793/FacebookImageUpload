﻿using System;
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

        private long createdDate;

        public long CreatedDate
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

        private bool isSent;

        public bool IsSent
        {
            get { return isSent; }
            set { isSent = value; }
        }

        private FB_Image image;

        public FB_Image Image
        {
            get { return image; }
            set { image = value; }
        }


        public FB_Message()
        {
        }


        public FB_Message(string paramContent, FB_Image paramImage):this()
        {
            content = paramContent;
            image = paramImage;
        }
        public FB_Message(string paramContent, FB_Image paramImage, long paramCreatedDate, bool paramSent)
            : this(paramContent, paramImage)
        {
            createdDate = paramCreatedDate;
            isSent = paramSent;
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
        private string profilePath;

        public string ProfilePath
        {
            get { return profilePath; }
            set { profilePath = value; }
        }

        public InboxUser()
        {
            messages = new List<FB_Message>();
        }

        public InboxUser(string paramID, string paramName, string paramPath):this()
        {
            userID = paramID;
            userName = paramName;
            profilePath = paramPath;
        }

        public InboxUser(string paramID, string paramName):this()
        {
            userID = paramID;
            userName = paramName;
        }


    }
}
