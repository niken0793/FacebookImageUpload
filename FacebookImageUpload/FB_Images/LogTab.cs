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
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis;
using Google.Apis.Services;

namespace FacebookImageUpload
{
    public partial class Form1 : Form
    {
       
        InboxUser currentLogInbox;
        private void ChangeLogUserInbox(object sender, EventArgs e)
        {
            ListView lvUser = (ListView)sender;
            if (lvUser.SelectedItems.Count == 1)
            {
                tbLogInbox.Clear();
                InboxUser inbox = MyHelper.GetInboxByUserID(lvUser.SelectedItems[0].Name, ListInboxUser);
                currentLogInbox = inbox;
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                currentInboxLVItem = item;
                lbLogUserName.Text = inbox.UserName;
                lbLogUserId.Text = item.Name;
                pbLogUserImage.ImageLocation = Path.Combine(FB_Image.RelativeDirectory, FB_Image.UserImageDir, item.Name + ".jpg");
                //receivePass = GetMessagePassword(lbUserIdComm.Text);
                currentLogInbox.PrintLogMessages(tbLogInbox);

            }


        }
    }
}
