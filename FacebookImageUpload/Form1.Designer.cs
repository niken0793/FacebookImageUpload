namespace FacebookImageUpload
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUploadImage = new System.Windows.Forms.Button();
            this.tbImagePath = new System.Windows.Forms.TextBox();
            this.ListViewalbumList = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbIsTested = new System.Windows.Forms.CheckBox();
            this.cmbSelectTextType = new System.Windows.Forms.ComboBox();
            this.tbInputMessage = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.tbMessagePath = new System.Windows.Forms.TextBox();
            this.tbAlbumID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fdOpenfile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAlbumName = new System.Windows.Forms.Label();
            this.pBoxAlbumCover = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSender = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btngetAlbumlist = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.btnTask = new System.Windows.Forms.Button();
            this.lbStatusBar = new System.Windows.Forms.Label();
            this.tabReceiver = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnGetAlbumInbox = new System.Windows.Forms.Button();
            this.ListViewalbumList_In = new System.Windows.Forms.ListView();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnChangeUser = new System.Windows.Forms.Button();
            this.cmbChangeUser = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAlbumDesc = new System.Windows.Forms.TextBox();
            this.tbAlbumName = new System.Windows.Forms.TextBox();
            this.btnCreateAlbum = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGetMessage = new System.Windows.Forms.Button();
            this.tbInbox = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAlbumCover)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabSender.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabReceiver.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUploadImage
            // 
            this.btnUploadImage.Location = new System.Drawing.Point(0, 203);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(75, 23);
            this.btnUploadImage.TabIndex = 2;
            this.btnUploadImage.Text = "Send Message";
            this.btnUploadImage.UseVisualStyleBackColor = true;
            this.btnUploadImage.Click += new System.EventHandler(this.uploadImage_Click);
            // 
            // tbImagePath
            // 
            this.tbImagePath.Location = new System.Drawing.Point(6, 111);
            this.tbImagePath.Name = "tbImagePath";
            this.tbImagePath.Size = new System.Drawing.Size(184, 20);
            this.tbImagePath.TabIndex = 3;
            // 
            // ListViewalbumList
            // 
            this.ListViewalbumList.Location = new System.Drawing.Point(6, 19);
            this.ListViewalbumList.Name = "ListViewalbumList";
            this.ListViewalbumList.Size = new System.Drawing.Size(363, 240);
            this.ListViewalbumList.TabIndex = 9;
            this.ListViewalbumList.UseCompatibleStateImageBehavior = false;
            this.ListViewalbumList.ItemActivate += new System.EventHandler(this.ListViewalbumList_ItemActivate);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbIsTested);
            this.groupBox3.Controls.Add(this.cmbSelectTextType);
            this.groupBox3.Controls.Add(this.tbInputMessage);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btnSelectFile);
            this.groupBox3.Controls.Add(this.tbMessagePath);
            this.groupBox3.Controls.Add(this.tbAlbumID);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.fdOpenfile);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnUploadImage);
            this.groupBox3.Controls.Add(this.tbImagePath);
            this.groupBox3.Controls.Add(this.lbAlbumName);
            this.groupBox3.Controls.Add(this.pBoxAlbumCover);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(15, 215);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(557, 241);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sending";
            // 
            // cbIsTested
            // 
            this.cbIsTested.AutoSize = true;
            this.cbIsTested.Location = new System.Drawing.Point(470, 65);
            this.cbIsTested.Name = "cbIsTested";
            this.cbIsTested.Size = new System.Drawing.Size(59, 17);
            this.cbIsTested.TabIndex = 17;
            this.cbIsTested.Text = "Tested";
            this.cbIsTested.UseVisualStyleBackColor = true;
            // 
            // cmbSelectTextType
            // 
            this.cmbSelectTextType.FormattingEnabled = true;
            this.cmbSelectTextType.Items.AddRange(new object[] {
            "From File",
            "From Text"});
            this.cmbSelectTextType.Location = new System.Drawing.Point(342, 62);
            this.cmbSelectTextType.Name = "cmbSelectTextType";
            this.cmbSelectTextType.Size = new System.Drawing.Size(121, 21);
            this.cmbSelectTextType.TabIndex = 16;
            this.cmbSelectTextType.Text = "From file or text";
            this.cmbSelectTextType.SelectedIndexChanged += new System.EventHandler(this.cmbSelectTextType_SelectedIndexChanged);
            // 
            // tbInputMessage
            // 
            this.tbInputMessage.Location = new System.Drawing.Point(266, 111);
            this.tbInputMessage.Multiline = true;
            this.tbInputMessage.Name = "tbInputMessage";
            this.tbInputMessage.Size = new System.Drawing.Size(235, 82);
            this.tbInputMessage.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Message Path :";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(196, 157);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(55, 23);
            this.btnSelectFile.TabIndex = 13;
            this.btnSelectFile.Text = "Browse";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbMessagePath
            // 
            this.tbMessagePath.Location = new System.Drawing.Point(4, 160);
            this.tbMessagePath.Name = "tbMessagePath";
            this.tbMessagePath.Size = new System.Drawing.Size(184, 20);
            this.tbMessagePath.TabIndex = 12;
            // 
            // tbAlbumID
            // 
            this.tbAlbumID.Location = new System.Drawing.Point(124, 64);
            this.tbAlbumID.Name = "tbAlbumID";
            this.tbAlbumID.Size = new System.Drawing.Size(175, 20);
            this.tbAlbumID.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Cover Image Path :";
            // 
            // fdOpenfile
            // 
            this.fdOpenfile.Location = new System.Drawing.Point(196, 109);
            this.fdOpenfile.Name = "fdOpenfile";
            this.fdOpenfile.Size = new System.Drawing.Size(55, 23);
            this.fdOpenfile.TabIndex = 0;
            this.fdOpenfile.Text = "Browse";
            this.fdOpenfile.UseVisualStyleBackColor = true;
            this.fdOpenfile.Click += new System.EventHandler(this.openfile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Album ID:";
            // 
            // lbAlbumName
            // 
            this.lbAlbumName.AutoSize = true;
            this.lbAlbumName.Location = new System.Drawing.Point(131, 30);
            this.lbAlbumName.Name = "lbAlbumName";
            this.lbAlbumName.Size = new System.Drawing.Size(67, 13);
            this.lbAlbumName.TabIndex = 7;
            this.lbAlbumName.Text = "Album Name";
            // 
            // pBoxAlbumCover
            // 
            this.pBoxAlbumCover.Image = global::FacebookImageUpload.Properties.Resources._default;
            this.pBoxAlbumCover.Location = new System.Drawing.Point(10, 30);
            this.pBoxAlbumCover.Name = "pBoxAlbumCover";
            this.pBoxAlbumCover.Size = new System.Drawing.Size(50, 50);
            this.pBoxAlbumCover.TabIndex = 5;
            this.pBoxAlbumCover.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Album Name:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSender);
            this.tabControl.Controls.Add(this.tabReceiver);
            this.tabControl.Controls.Add(this.tabSetting);
            this.tabControl.Location = new System.Drawing.Point(-1, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1008, 517);
            this.tabControl.TabIndex = 11;
            // 
            // tabSender
            // 
            this.tabSender.Controls.Add(this.groupBox1);
            this.tabSender.Controls.Add(this.tbMessage);
            this.tabSender.Controls.Add(this.pbStatus);
            this.tabSender.Controls.Add(this.btnTask);
            this.tabSender.Controls.Add(this.lbStatusBar);
            this.tabSender.Controls.Add(this.groupBox3);
            this.tabSender.Location = new System.Drawing.Point(4, 22);
            this.tabSender.Name = "tabSender";
            this.tabSender.Padding = new System.Windows.Forms.Padding(3);
            this.tabSender.Size = new System.Drawing.Size(1000, 491);
            this.tabSender.TabIndex = 0;
            this.tabSender.Text = "Sender";
            this.tabSender.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btngetAlbumlist);
            this.groupBox1.Controls.Add(this.ListViewalbumList);
            this.groupBox1.Location = new System.Drawing.Point(600, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(377, 307);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Outbox Album";
            // 
            // btngetAlbumlist
            // 
            this.btngetAlbumlist.Location = new System.Drawing.Point(142, 274);
            this.btngetAlbumlist.Name = "btngetAlbumlist";
            this.btngetAlbumlist.Size = new System.Drawing.Size(92, 23);
            this.btngetAlbumlist.TabIndex = 17;
            this.btngetAlbumlist.Text = "Get Albums List";
            this.btngetAlbumlist.UseVisualStyleBackColor = true;
            this.btngetAlbumlist.Click += new System.EventHandler(this.btngetAlbumlist_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(15, 29);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMessage.Size = new System.Drawing.Size(551, 159);
            this.tbMessage.TabIndex = 16;
            this.tbMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMessage_KeyPress);
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(15, 479);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(551, 12);
            this.pbStatus.TabIndex = 15;
            // 
            // btnTask
            // 
            this.btnTask.Location = new System.Drawing.Point(783, 387);
            this.btnTask.Name = "btnTask";
            this.btnTask.Size = new System.Drawing.Size(75, 23);
            this.btnTask.TabIndex = 14;
            this.btnTask.Text = "Task";
            this.btnTask.UseVisualStyleBackColor = true;
            this.btnTask.Click += new System.EventHandler(this.btnTask_Click);
            // 
            // lbStatusBar
            // 
            this.lbStatusBar.AutoSize = true;
            this.lbStatusBar.Location = new System.Drawing.Point(583, 475);
            this.lbStatusBar.Name = "lbStatusBar";
            this.lbStatusBar.Size = new System.Drawing.Size(0, 13);
            this.lbStatusBar.TabIndex = 13;
            // 
            // tabReceiver
            // 
            this.tabReceiver.Controls.Add(this.tbInbox);
            this.tabReceiver.Controls.Add(this.groupBox4);
            this.tabReceiver.Location = new System.Drawing.Point(4, 22);
            this.tabReceiver.Name = "tabReceiver";
            this.tabReceiver.Padding = new System.Windows.Forms.Padding(3);
            this.tabReceiver.Size = new System.Drawing.Size(1000, 491);
            this.tabReceiver.TabIndex = 1;
            this.tabReceiver.Text = "Receiver";
            this.tabReceiver.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnGetMessage);
            this.groupBox4.Controls.Add(this.btnGetAlbumInbox);
            this.groupBox4.Controls.Add(this.ListViewalbumList_In);
            this.groupBox4.Location = new System.Drawing.Point(629, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(335, 266);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Inbox Album";
            // 
            // btnGetAlbumInbox
            // 
            this.btnGetAlbumInbox.Location = new System.Drawing.Point(29, 223);
            this.btnGetAlbumInbox.Name = "btnGetAlbumInbox";
            this.btnGetAlbumInbox.Size = new System.Drawing.Size(92, 23);
            this.btnGetAlbumInbox.TabIndex = 18;
            this.btnGetAlbumInbox.Text = "Refresh";
            this.btnGetAlbumInbox.UseVisualStyleBackColor = true;
            this.btnGetAlbumInbox.Click += new System.EventHandler(this.btnGetAlbumInbox_Click);
            // 
            // ListViewalbumList_In
            // 
            this.ListViewalbumList_In.Location = new System.Drawing.Point(0, 19);
            this.ListViewalbumList_In.Name = "ListViewalbumList_In";
            this.ListViewalbumList_In.Size = new System.Drawing.Size(335, 181);
            this.ListViewalbumList_In.TabIndex = 10;
            this.ListViewalbumList_In.UseCompatibleStateImageBehavior = false;
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.groupBox5);
            this.tabSetting.Controls.Add(this.groupBox2);
            this.tabSetting.Location = new System.Drawing.Point(4, 22);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabSetting.Size = new System.Drawing.Size(1000, 491);
            this.tabSetting.TabIndex = 2;
            this.tabSetting.Text = "Setting";
            this.tabSetting.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnChangeUser);
            this.groupBox5.Controls.Add(this.cmbChangeUser);
            this.groupBox5.Location = new System.Drawing.Point(342, 17);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(185, 103);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "User";
            // 
            // btnChangeUser
            // 
            this.btnChangeUser.Location = new System.Drawing.Point(50, 74);
            this.btnChangeUser.Name = "btnChangeUser";
            this.btnChangeUser.Size = new System.Drawing.Size(75, 23);
            this.btnChangeUser.TabIndex = 1;
            this.btnChangeUser.Text = "Change User";
            this.btnChangeUser.UseVisualStyleBackColor = true;
            this.btnChangeUser.Click += new System.EventHandler(this.btnChangeUser_Click);
            // 
            // cmbChangeUser
            // 
            this.cmbChangeUser.FormattingEnabled = true;
            this.cmbChangeUser.Items.AddRange(new object[] {
            "User A",
            "User B"});
            this.cmbChangeUser.Location = new System.Drawing.Point(18, 35);
            this.cmbChangeUser.Name = "cmbChangeUser";
            this.cmbChangeUser.Size = new System.Drawing.Size(136, 21);
            this.cmbChangeUser.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbAlbumDesc);
            this.groupBox2.Controls.Add(this.tbAlbumName);
            this.groupBox2.Controls.Add(this.btnCreateAlbum);
            this.groupBox2.Location = new System.Drawing.Point(19, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 149);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Facebook Image Upload";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Album Name";
            // 
            // tbAlbumDesc
            // 
            this.tbAlbumDesc.Location = new System.Drawing.Point(79, 64);
            this.tbAlbumDesc.Name = "tbAlbumDesc";
            this.tbAlbumDesc.Size = new System.Drawing.Size(143, 20);
            this.tbAlbumDesc.TabIndex = 12;
            // 
            // tbAlbumName
            // 
            this.tbAlbumName.Location = new System.Drawing.Point(79, 32);
            this.tbAlbumName.Name = "tbAlbumName";
            this.tbAlbumName.Size = new System.Drawing.Size(143, 20);
            this.tbAlbumName.TabIndex = 11;
            // 
            // btnCreateAlbum
            // 
            this.btnCreateAlbum.Location = new System.Drawing.Point(33, 107);
            this.btnCreateAlbum.Name = "btnCreateAlbum";
            this.btnCreateAlbum.Size = new System.Drawing.Size(92, 23);
            this.btnCreateAlbum.TabIndex = 5;
            this.btnCreateAlbum.Text = "Create Album";
            this.btnCreateAlbum.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1007, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStripMenuItem1});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.settingToolStripMenuItem.Text = "Tool";
            // 
            // settingToolStripMenuItem1
            // 
            this.settingToolStripMenuItem1.Name = "settingToolStripMenuItem1";
            this.settingToolStripMenuItem1.Size = new System.Drawing.Size(108, 22);
            this.settingToolStripMenuItem1.Text = "Setting";
            // 
            // btnGetMessage
            // 
            this.btnGetMessage.Location = new System.Drawing.Point(162, 223);
            this.btnGetMessage.Name = "btnGetMessage";
            this.btnGetMessage.Size = new System.Drawing.Size(92, 23);
            this.btnGetMessage.TabIndex = 19;
            this.btnGetMessage.Text = "Get Message";
            this.btnGetMessage.UseVisualStyleBackColor = true;
            this.btnGetMessage.Click += new System.EventHandler(this.btnGetMessage_Click);
            // 
            // tbInbox
            // 
            this.tbInbox.Location = new System.Drawing.Point(9, 22);
            this.tbInbox.Multiline = true;
            this.tbInbox.Name = "tbInbox";
            this.tbInbox.Size = new System.Drawing.Size(597, 266);
            this.tbInbox.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 536);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAlbumCover)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabSender.ResumeLayout(false);
            this.tabSender.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabReceiver.ResumeLayout(false);
            this.tabReceiver.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUploadImage;
        private System.Windows.Forms.TextBox tbImagePath;
        private System.Windows.Forms.ListView ListViewalbumList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAlbumName;
        private System.Windows.Forms.PictureBox pBoxAlbumCover;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button fdOpenfile;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSender;
        private System.Windows.Forms.TabPage tabReceiver;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem1;
        private System.Windows.Forms.Label lbStatusBar;
        private System.Windows.Forms.Button btnTask;
        private System.Windows.Forms.ProgressBar pbStatus;

        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbAlbumID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox tbMessagePath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbInputMessage;
        private System.Windows.Forms.ComboBox cmbSelectTextType;
        private System.Windows.Forms.CheckBox cbIsTested;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAlbumDesc;
        private System.Windows.Forms.TextBox tbAlbumName;
        private System.Windows.Forms.Button btnCreateAlbum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btngetAlbumlist;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnGetAlbumInbox;
        private System.Windows.Forms.ListView ListViewalbumList_In;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnChangeUser;
        private System.Windows.Forms.ComboBox cmbChangeUser;
        private System.Windows.Forms.Button btnGetMessage;
        private System.Windows.Forms.TextBox tbInbox;
    }
}

