﻿namespace FacebookImageUpload
{
    partial class FacebookLoginForm
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
            this.webBrowserFacebook = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserFacebook
            // 
            this.webBrowserFacebook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserFacebook.Location = new System.Drawing.Point(0, 0);
            this.webBrowserFacebook.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserFacebook.Name = "webBrowserFacebook";
            this.webBrowserFacebook.ScriptErrorsSuppressed = true;
            this.webBrowserFacebook.Size = new System.Drawing.Size(590, 368);
            this.webBrowserFacebook.TabIndex = 5;
            this.webBrowserFacebook.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserFacebook_Navigated);
            // 
            // FacebookLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(590, 368);
            this.Controls.Add(this.webBrowserFacebook);
            this.Name = "FacebookLoginForm";
            this.Text = "FacebookLoginForm";
            this.Load += new System.EventHandler(this.FacebookLoginForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserFacebook;

    }
}