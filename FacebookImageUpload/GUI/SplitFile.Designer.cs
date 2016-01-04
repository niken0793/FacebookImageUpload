namespace FacebookImageUpload
{
    partial class SplitFile
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
            this.groupbox123 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSplitFileSize = new System.Windows.Forms.ComboBox();
            this.radioSplitAuto = new System.Windows.Forms.RadioButton();
            this.radioSplitFile = new System.Windows.Forms.RadioButton();
            this.radioSplitOne = new System.Windows.Forms.RadioButton();
            this.btnSplitOK = new System.Windows.Forms.Button();
            this.btnSplitCanel = new System.Windows.Forms.Button();
            this.groupbox123.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupbox123
            // 
            this.groupbox123.Controls.Add(this.label1);
            this.groupbox123.Controls.Add(this.cmbSplitFileSize);
            this.groupbox123.Controls.Add(this.radioSplitAuto);
            this.groupbox123.Controls.Add(this.radioSplitFile);
            this.groupbox123.Controls.Add(this.radioSplitOne);
            this.groupbox123.Location = new System.Drawing.Point(12, 13);
            this.groupbox123.Name = "groupbox123";
            this.groupbox123.Size = new System.Drawing.Size(323, 222);
            this.groupbox123.TabIndex = 0;
            this.groupbox123.TabStop = false;
            this.groupbox123.Text = "Option";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File size (Byte):";
            // 
            // cmbSplitFileSize
            // 
            this.cmbSplitFileSize.FormattingEnabled = true;
            this.cmbSplitFileSize.Items.AddRange(new object[] {
            "256",
            "512",
            "768",
            "1024"});
            this.cmbSplitFileSize.Location = new System.Drawing.Point(153, 117);
            this.cmbSplitFileSize.Name = "cmbSplitFileSize";
            this.cmbSplitFileSize.Size = new System.Drawing.Size(121, 21);
            this.cmbSplitFileSize.TabIndex = 3;
            this.cmbSplitFileSize.Tag = "";
            this.cmbSplitFileSize.Text = "Choose Size";
            this.cmbSplitFileSize.SelectedIndexChanged += new System.EventHandler(this.cmbSplitFileSize_SelectedIndexChanged);
            // 
            // radioSplitAuto
            // 
            this.radioSplitAuto.AutoSize = true;
            this.radioSplitAuto.Location = new System.Drawing.Point(19, 175);
            this.radioSplitAuto.Name = "radioSplitAuto";
            this.radioSplitAuto.Size = new System.Drawing.Size(47, 17);
            this.radioSplitAuto.TabIndex = 2;
            this.radioSplitAuto.TabStop = true;
            this.radioSplitAuto.Text = "Auto";
            this.radioSplitAuto.UseVisualStyleBackColor = true;
            this.radioSplitAuto.CheckedChanged += new System.EventHandler(this.radioSplitAuto_CheckedChanged);
            // 
            // radioSplitFile
            // 
            this.radioSplitFile.AutoSize = true;
            this.radioSplitFile.Location = new System.Drawing.Point(19, 117);
            this.radioSplitFile.Name = "radioSplitFile";
            this.radioSplitFile.Size = new System.Drawing.Size(86, 17);
            this.radioSplitFile.TabIndex = 1;
            this.radioSplitFile.TabStop = true;
            this.radioSplitFile.Text = "Split into files";
            this.radioSplitFile.UseVisualStyleBackColor = true;
            this.radioSplitFile.CheckedChanged += new System.EventHandler(this.radioSplitFile_CheckedChanged);
            // 
            // radioSplitOne
            // 
            this.radioSplitOne.AutoSize = true;
            this.radioSplitOne.Location = new System.Drawing.Point(19, 52);
            this.radioSplitOne.Name = "radioSplitOne";
            this.radioSplitOne.Size = new System.Drawing.Size(68, 17);
            this.radioSplitOne.TabIndex = 0;
            this.radioSplitOne.TabStop = true;
            this.radioSplitOne.Text = "All in one";
            this.radioSplitOne.UseVisualStyleBackColor = true;
            this.radioSplitOne.CheckedChanged += new System.EventHandler(this.radioSplitOne_CheckedChanged);
            // 
            // btnSplitOK
            // 
            this.btnSplitOK.Location = new System.Drawing.Point(85, 256);
            this.btnSplitOK.Name = "btnSplitOK";
            this.btnSplitOK.Size = new System.Drawing.Size(62, 27);
            this.btnSplitOK.TabIndex = 1;
            this.btnSplitOK.Text = "OK";
            this.btnSplitOK.UseVisualStyleBackColor = true;
            this.btnSplitOK.Click += new System.EventHandler(this.btnSplitOK_Click);
            // 
            // btnSplitCanel
            // 
            this.btnSplitCanel.Location = new System.Drawing.Point(200, 256);
            this.btnSplitCanel.Name = "btnSplitCanel";
            this.btnSplitCanel.Size = new System.Drawing.Size(62, 27);
            this.btnSplitCanel.TabIndex = 2;
            this.btnSplitCanel.Text = "Cancel";
            this.btnSplitCanel.UseVisualStyleBackColor = true;
            this.btnSplitCanel.Click += new System.EventHandler(this.btnSplitCanel_Click);
            // 
            // SplitFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 313);
            this.Controls.Add(this.btnSplitCanel);
            this.Controls.Add(this.btnSplitOK);
            this.Controls.Add(this.groupbox123);
            this.Name = "SplitFile";
            this.Text = "SplitFile";
            this.groupbox123.ResumeLayout(false);
            this.groupbox123.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupbox123;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSplitFileSize;
        private System.Windows.Forms.RadioButton radioSplitAuto;
        private System.Windows.Forms.RadioButton radioSplitFile;
        private System.Windows.Forms.RadioButton radioSplitOne;
        private System.Windows.Forms.Button btnSplitOK;
        private System.Windows.Forms.Button btnSplitCanel;
    }
}