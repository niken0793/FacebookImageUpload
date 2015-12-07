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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreatePrivateAlbum = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmbPrivateAlbum = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.label3 = new System.Windows.Forms.Label();
            this.btnChoosePrivateAlbum = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Album Name:";
            // 
            // btnCreatePrivateAlbum
            // 
            this.btnCreatePrivateAlbum.Location = new System.Drawing.Point(15, 95);
            this.btnCreatePrivateAlbum.Name = "btnCreatePrivateAlbum";
            this.btnCreatePrivateAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnCreatePrivateAlbum.TabIndex = 1;
            this.btnCreatePrivateAlbum.Text = "Create Album";
            this.btnCreatePrivateAlbum.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(165, 20);
            this.textBox1.TabIndex = 2;
            // 
            // cmbPrivateAlbum
            // 
            this.cmbPrivateAlbum.FormattingEnabled = true;
            this.cmbPrivateAlbum.Location = new System.Drawing.Point(207, 56);
            this.cmbPrivateAlbum.Name = "cmbPrivateAlbum";
            this.cmbPrivateAlbum.Size = new System.Drawing.Size(162, 21);
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
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(399, 204);
            this.shapeContainer1.TabIndex = 5;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.Info;
            this.lineShape1.BorderWidth = 2;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.SelectionColor = System.Drawing.Color.DeepSkyBlue;
            this.lineShape1.X1 = 193;
            this.lineShape1.X2 = 192;
            this.lineShape1.Y1 = 39;
            this.lineShape1.Y2 = 148;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "List of your private albums:";
            // 
            // btnChoosePrivateAlbum
            // 
            this.btnChoosePrivateAlbum.Location = new System.Drawing.Point(216, 95);
            this.btnChoosePrivateAlbum.Name = "btnChoosePrivateAlbum";
            this.btnChoosePrivateAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnChoosePrivateAlbum.TabIndex = 7;
            this.btnChoosePrivateAlbum.Text = "Choose";
            this.btnChoosePrivateAlbum.UseVisualStyleBackColor = true;
            // 
            // CreateAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 204);
            this.Controls.Add(this.btnChoosePrivateAlbum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbPrivateAlbum);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCreatePrivateAlbum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "CreateAlbum";
            this.Text = "CreateAlbum";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreatePrivateAlbum;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cmbPrivateAlbum;
        private System.Windows.Forms.Label label2;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnChoosePrivateAlbum;
    }
}