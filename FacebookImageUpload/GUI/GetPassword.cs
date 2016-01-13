using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookImageUpload;
using FacebookImageUpload.FB_Images;

namespace FacebookImageUpload.GUI
{
    public partial class GetPassword : Form
    {

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private bool isSavePass;

        public bool IsSavePass
        {
            get { return isSavePass; }
            set { isSavePass = value; }
        }

        public GetPassword()
        {
            InitializeComponent();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            password = FB_Images.FB_Image.DefaultPassword;
            isSavePass = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbPass.Text) || !(string.IsNullOrWhiteSpace(tbPass.Text)))
            {
                password = tbPass.Text;
                isSavePass = cbSavePass.Checked;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MyHelper.ShowToolTip("Your password is empty!",tbPass, 2000,150,-30);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
