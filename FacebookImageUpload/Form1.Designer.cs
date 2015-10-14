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
            this.openfile = new System.Windows.Forms.Button();
            this.uploadImage = new System.Windows.Forms.Button();
            this.imagePath = new System.Windows.Forms.TextBox();
            this.downloadImage = new System.Windows.Forms.Button();
            this.createAlbum = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openfile
            // 
            this.openfile.Location = new System.Drawing.Point(177, 26);
            this.openfile.Name = "openfile";
            this.openfile.Size = new System.Drawing.Size(27, 23);
            this.openfile.TabIndex = 0;
            this.openfile.Text = "...";
            this.openfile.UseVisualStyleBackColor = true;
            this.openfile.Click += new System.EventHandler(this.openfile_Click);
            // 
            // uploadImage
            // 
            this.uploadImage.Location = new System.Drawing.Point(129, 38);
            this.uploadImage.Name = "uploadImage";
            this.uploadImage.Size = new System.Drawing.Size(75, 23);
            this.uploadImage.TabIndex = 2;
            this.uploadImage.Text = "Upload";
            this.uploadImage.UseVisualStyleBackColor = true;
            this.uploadImage.Click += new System.EventHandler(this.uploadImage_Click);
            // 
            // imagePath
            // 
            this.imagePath.Location = new System.Drawing.Point(23, 40);
            this.imagePath.Name = "imagePath";
            this.imagePath.Size = new System.Drawing.Size(162, 20);
            this.imagePath.TabIndex = 3;
            // 
            // downloadImage
            // 
            this.downloadImage.Location = new System.Drawing.Point(80, 218);
            this.downloadImage.Name = "downloadImage";
            this.downloadImage.Size = new System.Drawing.Size(75, 23);
            this.downloadImage.TabIndex = 4;
            this.downloadImage.Text = "Download";
            this.downloadImage.UseVisualStyleBackColor = true;
            this.downloadImage.Click += new System.EventHandler(this.downloadImage_Click);
            // 
            // createAlbum
            // 
            this.createAlbum.Location = new System.Drawing.Point(10, 37);
            this.createAlbum.Name = "createAlbum";
            this.createAlbum.Size = new System.Drawing.Size(92, 23);
            this.createAlbum.TabIndex = 5;
            this.createAlbum.Text = "Create Album";
            this.createAlbum.UseVisualStyleBackColor = true;
            this.createAlbum.Click += new System.EventHandler(this.createAlbum_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.openfile);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 74);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Browse";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.createAlbum);
            this.groupBox2.Controls.Add(this.uploadImage);
            this.groupBox2.Location = new System.Drawing.Point(13, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 88);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Facebook Image Upload";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 372);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.downloadImage);
            this.Controls.Add(this.imagePath);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openfile;
        private System.Windows.Forms.Button uploadImage;
        private System.Windows.Forms.TextBox imagePath;
        private System.Windows.Forms.Button downloadImage;
        private System.Windows.Forms.Button createAlbum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

