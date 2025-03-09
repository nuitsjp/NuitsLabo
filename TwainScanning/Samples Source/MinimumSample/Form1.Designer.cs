namespace MinimumSample
{
    partial class Simple
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
            this.btnOpenSource = new System.Windows.Forms.Button();
            this.btnAcquire = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnImgSavePath = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenSource
            // 
            this.btnOpenSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenSource.Location = new System.Drawing.Point(354, 12);
            this.btnOpenSource.Name = "btnOpenSource";
            this.btnOpenSource.Size = new System.Drawing.Size(116, 25);
            this.btnOpenSource.TabIndex = 0;
            this.btnOpenSource.Text = "Open Source";
            this.btnOpenSource.UseVisualStyleBackColor = true;
            this.btnOpenSource.Click += new System.EventHandler(this.btnOpenSource_Click);
            // 
            // btnAcquire
            // 
            this.btnAcquire.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcquire.Location = new System.Drawing.Point(354, 43);
            this.btnAcquire.Name = "btnAcquire";
            this.btnAcquire.Size = new System.Drawing.Size(118, 25);
            this.btnAcquire.TabIndex = 1;
            this.btnAcquire.Text = "Acquire";
            this.btnAcquire.UseVisualStyleBackColor = true;
            this.btnAcquire.Click += new System.EventHandler(this.btnAcquire_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(336, 160);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(12, 178);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(336, 20);
            this.tbPath.TabIndex = 3;
            // 
            // btnImgSavePath
            // 
            this.btnImgSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImgSavePath.Location = new System.Drawing.Point(354, 177);
            this.btnImgSavePath.Name = "btnImgSavePath";
            this.btnImgSavePath.Size = new System.Drawing.Size(28, 20);
            this.btnImgSavePath.TabIndex = 4;
            this.btnImgSavePath.Text = "...";
            this.btnImgSavePath.UseVisualStyleBackColor = true;
            this.btnImgSavePath.Click += new System.EventHandler(this.btnImgSavePath_Click);
            // 
            // Simple
            // 
#if !TWCore
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
#endif
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 210);
            this.Controls.Add(this.btnImgSavePath);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnAcquire);
            this.Controls.Add(this.btnOpenSource);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Simple";
            this.ShowIcon = false;
#if !TWCore
            this.Text = "Terminalworks - TwainScanning - Minimum Sample - ";
#else
            this.Text = "Terminalworks - TwainScanningCore - Minimum Sample - "; 
#endif
            this.Load += new System.EventHandler(this.Simple_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenSource;
        private System.Windows.Forms.Button btnAcquire;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnImgSavePath;
    }
}

