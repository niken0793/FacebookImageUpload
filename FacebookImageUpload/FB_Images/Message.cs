using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
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
        private bool isFull;

        public bool IsFull
        {
            set { isFull = value; }
            get { return isFull; }
            
        }

        private bool isOld;

        public bool IsOld
        {
            get { return isOld; }
            set { isOld = value; }
        }

        private bool isSent;

        public bool IsSent
        {
            get { return isSent; }
            set { isSent = value; }
        }

        private List<MessagePart> part;

        public List<MessagePart> Part
        {
            get { return part; }
            set { part = value; }
        }


        private FB_Image image;

        public FB_Image Image
        {
            get { return image; }
            set { image = value; }
        }


        public FB_Message()
        {
            part = new List<MessagePart>();
        }


        public FB_Message(string paramContent, FB_Image paramImage):this()
        {
            content = paramContent;
            image = paramImage;
        }
        public FB_Message(string paramContent, FB_Image paramImage, string paramID,long pCreatedTime)
            : this()
        {
            content = paramContent;
            image = paramImage;
            id = paramID;
            createdDate = pCreatedTime;
        }

        public FB_Message(string paramContent, FB_Image paramImage, long paramCreatedDate, bool paramSent)
            : this(paramContent, paramImage)
        {
            createdDate = paramCreatedDate;
            isSent = paramSent;
        }

        public string GetContent(string pass)
        {
            try
            {
                if (this.Part.Count > 0)
                {
                    SimplerAES aes = new SimplerAES(pass);
                    part.Sort(delegate(MessagePart a, MessagePart b) { return a.Offset.CompareTo(b.Offset); });
                    string s = "";
                    int start = part[0].Offset;
                    int end = part[part.Count - 1].Offset;
                    if (part[0].Offset == 0 && part[0].Flag == 0)
                    {
                        isFull = true;
                        content = part[0].Content;
                        string tempString  = aes.Decrypt(content.Trim('\0'));
                        if (tempString != null)
                        {
                            content = tempString;
                        }
                        else
                        {
                            isOld = true;
                        }
                        

                        return content;
                    }
                    int j = 0;
                    if (start != 0)
                        s += "[....]";
                    for (int i = start; i <= end; i++)
                    {

                        if (part[j].Offset == i)
                        {
                            s += part[j].Content;
                            j++;
                        }
                        else
                        {
                            s += "[....]";
                        }
                    }
                    if (part[part.Count - 1].Flag == 1)
                    {
                        s += "[....]";
                    }
                    if (part.Count == part[part.Count - 1].Offset + 1 && part[part.Count - 1].Flag == 0)
                    {
                        isFull = true;
                        content = s.Trim('\0');
                        string tempString = aes.Decrypt(s.Trim('\0'));
                        if (tempString != null)
                        {
                            content = tempString;
                        }
                        else
                        {
                            isOld = true;
                        }
                        return content;
                    }


                    content = s;
                    return s;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Form1.Log(e);
                return null;
            }
        }

    }

    public class MessagePart
    {
        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        private int offset;

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private int flag;

        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }


        public MessagePart()
        { }
        public MessagePart(string pContent, int pOffset, int pFlag, string pImage)
        {
            content = pContent;
            offset = pOffset;
            flag = pFlag;
            imagePath = pImage;
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

        private bool isSavePass;

        public bool IsSavePass
        {
            get { return isSavePass; }
            set { isSavePass = value; }
        }

        private string password="123";

        public string Password
        {
            get { return password; }
            set { password = value; }
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

        public bool AddMessageToInbox(string content, string imageId, string imagePath, long createdTime, bool isSent = false)
        {
            try
            {
                if (content.Length >= Form1.HEADER_LEN)
                {
                    string header = content.Substring(0, Form1.HEADER_LEN);
                    string[] headerComponent = header.Split('|');
                    if (headerComponent.Length == 3)
                    {
                        string id = headerComponent[0];
                        int offset = Int16.Parse(headerComponent[1]);
                        int flag = Int16.Parse( headerComponent[2]);
                        string subContent = content.Substring(Form1.HEADER_LEN, content.Length - Form1.HEADER_LEN);
                        FB_Message m = GetMessageByID(messages, id);
                        if (m != null)
                        {
                            
                            m.Part.Add(new MessagePart(subContent, offset, flag,imagePath));
                            return false;
                        }
                        else
                        {
                            MessagePart part = new MessagePart(subContent, offset, flag,imagePath);
                            FB_Message mes = new FB_Message(content, new FB_Image(imageId, imagePath), id,createdTime);
                            mes.Part.Add(part);
                            messages.Add(mes);
                            return true;
                        }
                        
                    }
                    

                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

        }

        public void PrintLogMessages(RichTextBox tb)
        {
            if (this.Messages.Count > 0)
            {
                string content;
                for (int i = 0; i < Messages.Count; i++)
                {
                    content = "";
                    if (Messages[i].IsSent)
                    {
                        content = String.Format("{0} :{1}{2}{3}{4}{5}", "Me", Environment.NewLine, 
                            Messages[i].Content,Environment.NewLine, 
                            MyHelper.UnixTimeStampToDateTime(Messages[i].CreatedDate).ToString(), Environment.NewLine);
                        tb.SelectionAlignment = HorizontalAlignment.Right;
                        tb.AppendText(content + Environment.NewLine);
                        //tb.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        if (Messages[i].IsFull)
                        {
                            content = String.Format("{0} :{1}{2}{3}{4}{5}",this.UserName, Environment.NewLine,
                                Messages[i].Content, Environment.NewLine, 
                                MyHelper.UnixTimeStampToDateTime(Messages[i].CreatedDate).ToString(), Environment.NewLine);
                            tb.SelectionAlignment = HorizontalAlignment.Left;
                            tb.AppendText(content + Environment.NewLine);
                            //tb.AppendText(Environment.NewLine);
                        }
                        
                    }
                }
            }
        }
        public static FB_Message GetMessageByID(List<FB_Message> listMessage, string id)
        {
            if (listMessage != null && listMessage.Count > 0)
            {
                for (int i = 0; i < listMessage.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listMessage[i].ID)&& listMessage[i].ID.Equals(id))
                    {
                        return listMessage[i];
                    }
                }
            }
            return null;
        }


    }
}
