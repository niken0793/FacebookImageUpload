namespace FacebookImageUpload
{
    partial class CreateAlbum
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
            this.cmbPrivateAlbum = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnChoosePrivateAlbum = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.tbAlbumName = new System.Windows.Forms.TextBox();
            this.btnCreatePrivateAlbum = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbPrivateAlbum
            // 
            this.cmbPrivateAlbum.FormattingEnabled = true;
            this.cmbPrivateAlbum.Location = new System.Drawing.Point(15, 62);
            this.cmbPrivateAlbum.Name = "cmbPrivateAlbum";
            this.cmbPrivateAlbum.Size = new System.Drawing.Size(207, 21);
            this.cmbPrivateAlbum.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "List of your private albums:";
            // 
            // btnChoosePrivateAlbum
            // 
            this.btnChoosePrivateAlbum.Location = new System.Drawing.Point(69, 108);
            this.btnChoosePrivateAlbum.Name = "btnChoosePrivateAlbum";
            this.btnChoosePrivateAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnChoosePrivateAlbum.TabIndex = 7;
            this.btnChoosePrivateAlbum.Text = "Choose";
            this.btnChoosePrivateAlbum.UseVisualStyleBackColor = true;
            this.btnChoosePrivateAlbum.Click += new System.EventHandler(this.btnChoosePrivateAlbum_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(466, 249);
            this.shapeContainer1.TabIndex = 8;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 242;
            this.lineShape1.X2 = 242;
            this.lineShape1.Y1 = 9;
            this.lineShape1.Y2 = 167;
            // 
            // tbAlbumName
            // 
            this.tbAlbumName.Location = new System.Drawing.Point(260, 63);
            this.tbAlbumName.Name = "tbAlbumName";
            this.tbAlbumName.Size = new System.Drawing.Size(180, 20);
            this.tbAlbumName.TabIndex = 11;
            // 
            // btnCreatePrivateAlbum
            // 
            this.btnCreatePrivateAlbum.Location = new System.Drawing.Point(312, 108);
            this.btnCreatePrivateAlbum.Name = "btnCreatePrivateAlbum";
            this.btnCreatePrivateAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnCreatePrivateAlbum.TabIndex = 10;
            this.btnCreatePrivateAlbum.Text = "Create Album";
            this.btnCreatePrivateAlbum.UseVisualStyleBackColor = true;
            this.btnCreatePrivateAlbum.Click += new System.EventHandler(this.btnCreatePrivateAlbum_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(257, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Album Name:";
            // 
            // CreateAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 249);
            this.Controls.Add(this.tbAlbumName);
            this.Controls.Add(this.btnCreatePrivateAlbum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChoosePrivateAlbum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbPrivateAlbum);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "CreateAlbum";
            this.Text = "CreateAlbum";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateAlbum_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPrivateAlbum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnChoosePrivateAlbum;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.TextBox tbAlbumName;
        private System.Windows.Forms.Button btnCreatePrivateAlbum;
        private System.Windows.Forms.Label label1;
    }
}