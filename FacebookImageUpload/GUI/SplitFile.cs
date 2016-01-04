using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacebookImageUpload
{
    public partial class SplitFile : Form
    {

        public enum SplitType { 
            AllInOne,
            Split,
            Auto
        }
        private int fileSize;
        SplitType type;

        public SplitType Type
        {
            get { return type; }
            set { type = value; }
        }

        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public SplitFile()
        {
            InitializeComponent();
            FileSize = 1;
            type = SplitType.AllInOne;
        }

        private void radioSplitFile_CheckedChanged(object sender, EventArgs e)
        {
            cmbSplitFileSize.Enabled = true;
            type = SplitType.Split;

        }

        private void radioSplitOne_CheckedChanged(object sender, EventArgs e)
        {
            cmbSplitFileSize.Enabled = false;
            type = SplitType.AllInOne;
        }

        private void radioSplitAuto_CheckedChanged(object sender, EventArgs e)
        {
            cmbSplitFileSize.Enabled = false;
            type = SplitType.Auto;
        }

        private void cmbSplitFileSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            FileSize = c.SelectedIndex + 1;
        }

        private void btnSplitOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnSplitCanel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
