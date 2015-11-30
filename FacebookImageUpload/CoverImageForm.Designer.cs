namespace FacebookImageUpload
{
    partial class CoverImageForm
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
            this.tbCoverImageKeyword = new System.Windows.Forms.TextBox();
            this.btnCoverImageSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewSuccessImage = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbCoverImageKeyword
            // 
            this.tbCoverImageKeyword.Location = new System.Drawing.Point(12, 28);
            this.tbCoverImageKeyword.Name = "tbCoverImageKeyword";
            this.tbCoverImageKeyword.Size = new System.Drawing.Size(364, 20);
            this.tbCoverImageKeyword.TabIndex = 21;
            // 
            // btnCoverImageSearch
            // 
            this.btnCoverImageSearch.Location = new System.Drawing.Point(382, 28);
            this.btnCoverImageSearch.Name = "btnCoverImageSearch";
            this.btnCoverImageSearch.Size = new System.Drawing.Size(123, 23);
            this.btnCoverImageSearch.TabIndex = 22;
            this.btnCoverImageSearch.Text = "Search";
            this.btnCoverImageSearch.UseVisualStyleBackColor = true;
            this.btnCoverImageSearch.Click += new System.EventHandler(this.btnCoverImageSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewSuccessImage);
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 347);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Success Image";
            // 
            // listViewSuccessImage
            // 
            this.listViewSuccessImage.Location = new System.Drawing.Point(5, 19);
            this.listViewSuccessImage.MultiSelect = false;
            this.listViewSuccessImage.Name = "listViewSuccessImage";
            this.listViewSuccessImage.Size = new System.Drawing.Size(493, 322);
            this.listViewSuccessImage.TabIndex = 19;
            this.listViewSuccessImage.UseCompatibleStateImageBehavior = false;
            this.listViewSuccessImage.ItemActivate += new System.EventHandler(this.listViewSuccessImage_ItemActivate);
            // 
            // CoverImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 432);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCoverImageSearch);
            this.Controls.Add(this.tbCoverImageKeyword);
            this.Name = "CoverImageForm";
            this.Text = "CoverImageForm";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCoverImageKeyword;
        private System.Windows.Forms.Button btnCoverImageSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewSuccessImage;
    }
}