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
            this.label4 = new System.Windows.Forms.Label();
            this.lbAlbumName = new System.Windows.Forms.Label();
            this.lbAlbumId = new System.Windows.Forms.Label();
            this.pBoxAlbumCover = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fdOpenfile = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAlbumCover)).BeginInit();
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
            this.btnCreateAlbum.Location = new System.Drawing.Point(58, 106);
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
            this.groupBox2.Location = new System.Drawing.Point(13, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 171);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Facebook Image Upload";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Album Desc.";
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
            this.btngetAlbumlist.Location = new System.Drawing.Point(58, 135);
            this.btngetAlbumlist.Name = "btngetAlbumlist";
            this.btngetAlbumlist.Size = new System.Drawing.Size(92, 23);
            this.btngetAlbumlist.TabIndex = 7;
            this.btngetAlbumlist.Text = "Get Albums List";
            this.btngetAlbumlist.UseVisualStyleBackColor = true;
            this.btngetAlbumlist.Click += new System.EventHandler(this.btngetAlbumlist_Click);
            // 
            // ListViewalbumList
            // 
            this.ListViewalbumList.Dock = System.Windows.Forms.DockStyle.Right;
            this.ListViewalbumList.Location = new System.Drawing.Point(297, 0);
            this.ListViewalbumList.Name = "ListViewalbumList";
            this.ListViewalbumList.Size = new System.Drawing.Size(409, 413);
            this.ListViewalbumList.TabIndex = 9;
            this.ListViewalbumList.UseCompatibleStateImageBehavior = false;
            this.ListViewalbumList.ItemActivate += new System.EventHandler(this.ListViewalbumList_ItemActivate);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.fdOpenfile);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnUploadImage);
            this.groupBox3.Controls.Add(this.btnDownloadImage);
            this.groupBox3.Controls.Add(this.tbImagePath);
            this.groupBox3.Controls.Add(this.lbAlbumName);
            this.groupBox3.Controls.Add(this.lbAlbumId);
            this.groupBox3.Controls.Add(this.pBoxAlbumCover);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 202);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(229, 196);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Album";
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
            this.lbAlbumId.Size = new System.Drawing.Size(50, 13);
            this.lbAlbumId.TabIndex = 6;
            this.lbAlbumId.Text = "Album ID";
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
            // fdOpenfile
            // 
            this.fdOpenfile.Location = new System.Drawing.Point(196, 111);
            this.fdOpenfile.Name = "fdOpenfile";
            this.fdOpenfile.Size = new System.Drawing.Size(27, 23);
            this.fdOpenfile.TabIndex = 0;
            this.fdOpenfile.Text = "...";
            this.fdOpenfile.UseVisualStyleBackColor = true;
            this.fdOpenfile.Click += new System.EventHandler(this.openfile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 413);
            this.Controls.Add(this.ListViewalbumList);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAlbumCover)).EndInit();
            this.ResumeLayout(false);

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
    }
}

