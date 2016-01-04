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
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.tbAlbumName = new System.Windows.Forms.TextBox();
            this.btnCreatePrivateAlbum = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.tbCreateAlbumName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCreateAlbumType = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbPrivateAlbum
            // 
            this.cmbPrivateAlbum.FormattingEnabled = true;
            this.cmbPrivateAlbum.Location = new System.Drawing.Point(9, 85);
            this.cmbPrivateAlbum.Name = "cmbPrivateAlbum";
            this.cmbPrivateAlbum.Size = new System.Drawing.Size(207, 21);
            this.cmbPrivateAlbum.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(396, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Choose or create a private album for handling images processing";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "List of your private albums:";
            // 
            // btnChoosePrivateAlbum
            // 
            this.btnChoosePrivateAlbum.Location = new System.Drawing.Point(63, 192);
            this.btnChoosePrivateAlbum.Name = "btnChoosePrivateAlbum";
            this.btnChoosePrivateAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnChoosePrivateAlbum.TabIndex = 7;
            this.btnChoosePrivateAlbum.Text = "Choose";
            this.btnChoosePrivateAlbum.UseVisualStyleBackColor = true;
            this.btnChoosePrivateAlbum.Click += new System.EventHandler(this.btnChoosePrivateAlbum_Click);
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 222;
            this.lineShape1.X2 = 222;
            this.lineShape1.Y1 = 74;
            this.lineShape1.Y2 = 251;
            // 
            // tbAlbumName
            // 
            this.tbAlbumName.Location = new System.Drawing.Point(254, 86);
            this.tbAlbumName.Name = "tbAlbumName";
            this.tbAlbumName.Size = new System.Drawing.Size(180, 20);
            this.tbAlbumName.TabIndex = 11;
            // 
            // btnCreatePrivateAlbum
            // 
            this.btnCreatePrivateAlbum.Location = new System.Drawing.Point(308, 192);
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
            this.label1.Location = new System.Drawing.Point(251, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Album Name:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-2, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(479, 294);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbAlbumName);
            this.tabPage1.Controls.Add(this.cmbPrivateAlbum);
            this.tabPage1.Controls.Add(this.btnCreatePrivateAlbum);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnChoosePrivateAlbum);
            this.tabPage1.Controls.Add(this.shapeContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(471, 268);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Choose Private Album";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.btnCreate);
            this.tabPage2.Controls.Add(this.cmbCreateAlbumType);
            this.tabPage2.Controls.Add(this.tbCreateAlbumName);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(471, 268);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Create Album";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer2.Size = new System.Drawing.Size(465, 262);
            this.shapeContainer2.TabIndex = 12;
            this.shapeContainer2.TabStop = false;
            // 
            // tbCreateAlbumName
            // 
            this.tbCreateAlbumName.Location = new System.Drawing.Point(13, 52);
            this.tbCreateAlbumName.Name = "tbCreateAlbumName";
            this.tbCreateAlbumName.Size = new System.Drawing.Size(272, 20);
            this.tbCreateAlbumName.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Album Name:";
            // 
            // cmbCreateAlbumType
            // 
            this.cmbCreateAlbumType.FormattingEnabled = true;
            this.cmbCreateAlbumType.Items.AddRange(new object[] {
            "Private",
            "Public",
            "Friends"});
            this.cmbCreateAlbumType.Location = new System.Drawing.Point(13, 100);
            this.cmbCreateAlbumType.Name = "cmbCreateAlbumType";
            this.cmbCreateAlbumType.Size = new System.Drawing.Size(136, 21);
            this.cmbCreateAlbumType.TabIndex = 29;
            this.cmbCreateAlbumType.Text = "Choose type of album";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(13, 150);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 30;
            this.btnCreate.Text = "Create Album";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Album Type:";
            // 
            // CreateAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 294);
            this.Controls.Add(this.tabControl1);
            this.Name = "CreateAlbum";
            this.Text = "CreateAlbum";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateAlbum_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPrivateAlbum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnChoosePrivateAlbum;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.TextBox tbAlbumName;
        private System.Windows.Forms.Button btnCreatePrivateAlbum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tbCreateAlbumName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCreateAlbumType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCreate;
    }
}