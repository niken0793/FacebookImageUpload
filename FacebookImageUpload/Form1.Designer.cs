﻿namespace FacebookImageUpload
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
            this.btnDownloadImage = new System.Windows.Forms.Button();
            this.btnCreateAlbum = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAlbumDesc = new System.Windows.Forms.TextBox();
            this.tbAlbumName = new System.Windows.Forms.TextBox();
            this.btngetAlbumlist = new System.Windows.Forms.Button();
            this.ListViewalbumList = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAuto = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.fdOpenfile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAlbumName = new System.Windows.Forms.Label();
            this.lbAlbumId = new System.Windows.Forms.Label();
            this.pBoxAlbumCover = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabManual = new System.Windows.Forms.TabPage();
            this.btnTask = new System.Windows.Forms.Button();
            this.lbImagePath = new System.Windows.Forms.Label();
            this.lbImageName = new System.Windows.Forms.Label();
            this.tabAuto = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAlbumCover)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabManual.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUploadImage
            // 
            this.btnUploadImage.Location = new System.Drawing.Point(6, 158);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(75, 23);
            this.btnUploadImage.TabIndex = 2;
            this.btnUploadImage.Text = "Upload";
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
            // btnDownloadImage
            // 
            this.btnDownloadImage.Location = new System.Drawing.Point(148, 158);
            this.btnDownloadImage.Name = "btnDownloadImage";
            this.btnDownloadImage.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadImage.TabIndex = 4;
            this.btnDownloadImage.Text = "Download";
            this.btnDownloadImage.UseVisualStyleBackColor = true;
            this.btnDownloadImage.Click += new System.EventHandler(this.downloadImage_Click);
            // 
            // btnCreateAlbum
            // 
            this.btnCreateAlbum.Location = new System.Drawing.Point(6, 135);
            this.btnCreateAlbum.Name = "btnCreateAlbum";
            this.btnCreateAlbum.Size = new System.Drawing.Size(92, 23);
            this.btnCreateAlbum.TabIndex = 5;
            this.btnCreateAlbum.Text = "Create Album";
            this.btnCreateAlbum.UseVisualStyleBackColor = true;
            this.btnCreateAlbum.Click += new System.EventHandler(this.createAlbum_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbAlbumDesc);
            this.groupBox2.Controls.Add(this.tbAlbumName);
            this.groupBox2.Controls.Add(this.btngetAlbumlist);
            this.groupBox2.Controls.Add(this.btnCreateAlbum);
            this.groupBox2.Location = new System.Drawing.Point(3, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 177);
            this.groupBox2.TabIndex = 7;
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
            // btngetAlbumlist
            // 
            this.btngetAlbumlist.Location = new System.Drawing.Point(118, 135);
            this.btngetAlbumlist.Name = "btngetAlbumlist";
            this.btngetAlbumlist.Size = new System.Drawing.Size(92, 23);
            this.btngetAlbumlist.TabIndex = 7;
            this.btngetAlbumlist.Text = "Get Albums List";
            this.btngetAlbumlist.UseVisualStyleBackColor = true;
            this.btngetAlbumlist.Click += new System.EventHandler(this.btngetAlbumlist_Click);
            // 
            // ListViewalbumList
            // 
            this.ListViewalbumList.Location = new System.Drawing.Point(335, 21);
            this.ListViewalbumList.Name = "ListViewalbumList";
            this.ListViewalbumList.Size = new System.Drawing.Size(303, 346);
            this.ListViewalbumList.TabIndex = 9;
            this.ListViewalbumList.UseCompatibleStateImageBehavior = false;
            this.ListViewalbumList.ItemActivate += new System.EventHandler(this.ListViewalbumList_ItemActivate);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAuto);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.fdOpenfile);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnUploadImage);
            this.groupBox3.Controls.Add(this.btnDownloadImage);
            this.groupBox3.Controls.Add(this.tbImagePath);
            this.groupBox3.Controls.Add(this.lbAlbumName);
            this.groupBox3.Controls.Add(this.lbAlbumId);
            this.groupBox3.Controls.Add(this.pBoxAlbumCover);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(2, 217);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 241);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Album";
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(80, 203);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(75, 23);
            this.btnAuto.TabIndex = 11;
            this.btnAuto.Text = "Auto";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "File Path :";
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
            // lbAlbumId
            // 
            this.lbAlbumId.AutoSize = true;
            this.lbAlbumId.Location = new System.Drawing.Point(116, 67);
            this.lbAlbumId.Name = "lbAlbumId";
            this.lbAlbumId.Size = new System.Drawing.Size(103, 13);
            this.lbAlbumId.TabIndex = 6;
            this.lbAlbumId.Text = "1658361464376694";
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
            this.tabControl.Controls.Add(this.tabManual);
            this.tabControl.Controls.Add(this.tabAuto);
            this.tabControl.Location = new System.Drawing.Point(-1, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(784, 533);
            this.tabControl.TabIndex = 11;
            // 
            // tabManual
            // 
            this.tabManual.Controls.Add(this.pbStatus);
            this.tabManual.Controls.Add(this.btnTask);
            this.tabManual.Controls.Add(this.lbImagePath);
            this.tabManual.Controls.Add(this.lbImageName);
            this.tabManual.Controls.Add(this.groupBox3);
            this.tabManual.Controls.Add(this.ListViewalbumList);
            this.tabManual.Controls.Add(this.groupBox2);
            this.tabManual.Location = new System.Drawing.Point(4, 22);
            this.tabManual.Name = "tabManual";
            this.tabManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabManual.Size = new System.Drawing.Size(776, 507);
            this.tabManual.TabIndex = 0;
            this.tabManual.Text = "Manual";
            this.tabManual.UseVisualStyleBackColor = true;
            // 
            // btnTask
            // 
            this.btnTask.Location = new System.Drawing.Point(543, 408);
            this.btnTask.Name = "btnTask";
            this.btnTask.Size = new System.Drawing.Size(75, 23);
            this.btnTask.TabIndex = 14;
            this.btnTask.Text = "Task";
            this.btnTask.UseVisualStyleBackColor = true;
            this.btnTask.Click += new System.EventHandler(this.btnTask_Click);
            // 
            // lbImagePath
            // 
            this.lbImagePath.AutoSize = true;
            this.lbImagePath.Location = new System.Drawing.Point(370, 430);
            this.lbImagePath.Name = "lbImagePath";
            this.lbImagePath.Size = new System.Drawing.Size(35, 13);
            this.lbImagePath.TabIndex = 13;
            this.lbImagePath.Text = "label8";
            // 
            // lbImageName
            // 
            this.lbImageName.AutoSize = true;
            this.lbImageName.Location = new System.Drawing.Point(370, 397);
            this.lbImageName.Name = "lbImageName";
            this.lbImageName.Size = new System.Drawing.Size(72, 13);
            this.lbImageName.TabIndex = 11;
            this.lbImageName.Text = "lbImageName";
            // 
            // tabAuto
            // 
            this.tabAuto.Location = new System.Drawing.Point(4, 22);
            this.tabAuto.Name = "tabAuto";
            this.tabAuto.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuto.Size = new System.Drawing.Size(776, 507);
            this.tabAuto.TabIndex = 1;
            this.tabAuto.Text = "Auto";
            this.tabAuto.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(783, 24);
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
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(335, 463);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(140, 23);
            this.pbStatus.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 556);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAlbumCover)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabManual.ResumeLayout(false);
            this.tabManual.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUploadImage;
        private System.Windows.Forms.TextBox tbImagePath;
        private System.Windows.Forms.Button btnDownloadImage;
        private System.Windows.Forms.Button btnCreateAlbum;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAlbumDesc;
        private System.Windows.Forms.TextBox tbAlbumName;
        private System.Windows.Forms.Button btngetAlbumlist;
        private System.Windows.Forms.ListView ListViewalbumList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAlbumName;
        private System.Windows.Forms.Label lbAlbumId;
        private System.Windows.Forms.PictureBox pBoxAlbumCover;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button fdOpenfile;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabManual;
        private System.Windows.Forms.TabPage tabAuto;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Label lbImagePath;
        private System.Windows.Forms.Label lbImageName;
        private System.Windows.Forms.Button btnTask;
        private System.Windows.Forms.ProgressBar pbStatus;
    }
}

