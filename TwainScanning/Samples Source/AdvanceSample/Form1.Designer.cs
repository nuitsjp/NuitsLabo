namespace TwainDllTest
{
    partial class AdvanceSampleForm
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
            this.lbSources = new System.Windows.Forms.ListBox();
            this.lblDefSource = new System.Windows.Forms.Label();
            this.lblSources = new System.Windows.Forms.Label();
            this.btnAsync = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cbMultiPageTiff = new System.Windows.Forms.CheckBox();
            this.cbMultiPagePDF = new System.Windows.Forms.CheckBox();
            this.cbCloseDlgAfterScan = new System.Windows.Forms.CheckBox();
            this.lbTransferMechanism = new System.Windows.Forms.ListBox();
            this.lblTransferMechanism = new System.Windows.Forms.Label();
            this.tbDefSource = new System.Windows.Forms.TextBox();
            this.tbMPPDF = new System.Windows.Forms.TextBox();
            this.tbMPTiff = new System.Windows.Forms.TextBox();
            this.cbxPixelType = new System.Windows.Forms.ComboBox();
            this.cbxResolution = new System.Windows.Forms.ComboBox();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.pixelTypeLabel = new System.Windows.Forms.Label();
            this.cbDuplex = new System.Windows.Forms.CheckBox();
            this.cbADF = new System.Windows.Forms.CheckBox();
            this.cbUI = new System.Windows.Forms.CheckBox();
            this.tbFileTransfer = new System.Windows.Forms.TextBox();
            this.lblFileTrans = new System.Windows.Forms.Label();
            this.lblImgSavePath = new System.Windows.Forms.Label();
            this.tbImgPath = new System.Windows.Forms.TextBox();
            this.capability = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnImgSavePath = new System.Windows.Forms.Button();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.btnTiff = new System.Windows.Forms.Button();
            this.btnPDFPath = new System.Windows.Forms.Button();
            this.capValue = new System.Windows.Forms.ListBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.comboFileTransferFormat = new System.Windows.Forms.ComboBox();
            this.lblFileFormat = new System.Windows.Forms.Label();
            this.lblErrorMsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.capValueToSet = new System.Windows.Forms.TextBox();
            this.setCapValue = new System.Windows.Forms.Button();
            this.numberOfScansBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pageSizeLabel = new System.Windows.Forms.Label();
            this.cbxPageSize = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnProtectPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSources
            // 
            this.lbSources.FormattingEnabled = true;
            this.lbSources.Location = new System.Drawing.Point(9, 39);
            this.lbSources.Name = "lbSources";
            this.lbSources.Size = new System.Drawing.Size(388, 95);
            this.lbSources.TabIndex = 1;
            this.lbSources.SelectedIndexChanged += new System.EventHandler(this.lbSources_SelectedIndexChanged);
            // 
            // lblDefSource
            // 
            this.lblDefSource.AutoSize = true;
            this.lblDefSource.Location = new System.Drawing.Point(7, 143);
            this.lblDefSource.Name = "lblDefSource";
            this.lblDefSource.Size = new System.Drawing.Size(81, 13);
            this.lblDefSource.TabIndex = 3;
            this.lblDefSource.Text = "Default Source:";
            // 
            // lblSources
            // 
            this.lblSources.AutoSize = true;
            this.lblSources.Location = new System.Drawing.Point(7, 23);
            this.lblSources.Name = "lblSources";
            this.lblSources.Size = new System.Drawing.Size(63, 13);
            this.lblSources.TabIndex = 4;
            this.lblSources.Text = "All Sources:";
            // 
            // btnAsync
            // 
            this.btnAsync.Location = new System.Drawing.Point(8, 744);
            this.btnAsync.Name = "btnAsync";
            this.btnAsync.Size = new System.Drawing.Size(85, 23);
            this.btnAsync.TabIndex = 10;
            this.btnAsync.Text = "Acquire";
            this.btnAsync.UseVisualStyleBackColor = true;
            this.btnAsync.Click += new System.EventHandler(this.btnAsync_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(428, 710);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(561, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // cbMultiPageTiff
            // 
            this.cbMultiPageTiff.AutoSize = true;
            this.cbMultiPageTiff.Location = new System.Drawing.Point(10, 421);
            this.cbMultiPageTiff.Name = "cbMultiPageTiff";
            this.cbMultiPageTiff.Size = new System.Drawing.Size(88, 17);
            this.cbMultiPageTiff.TabIndex = 16;
            this.cbMultiPageTiff.Text = "MultiPageTiff";
            this.cbMultiPageTiff.UseVisualStyleBackColor = true;
            // 
            // cbMultiPagePDF
            // 
            this.cbMultiPagePDF.AutoSize = true;
            this.cbMultiPagePDF.Location = new System.Drawing.Point(9, 365);
            this.cbMultiPagePDF.Name = "cbMultiPagePDF";
            this.cbMultiPagePDF.Size = new System.Drawing.Size(47, 17);
            this.cbMultiPagePDF.TabIndex = 19;
            this.cbMultiPagePDF.Text = "PDF";
            this.cbMultiPagePDF.UseVisualStyleBackColor = true;
            // 
            // cbCloseDlgAfterScan
            // 
            this.cbCloseDlgAfterScan.AutoSize = true;
            this.cbCloseDlgAfterScan.Checked = true;
            this.cbCloseDlgAfterScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCloseDlgAfterScan.Location = new System.Drawing.Point(208, 272);
            this.cbCloseDlgAfterScan.Name = "cbCloseDlgAfterScan";
            this.cbCloseDlgAfterScan.Size = new System.Drawing.Size(133, 17);
            this.cbCloseDlgAfterScan.TabIndex = 20;
            this.cbCloseDlgAfterScan.Text = "Close dialog after scan";
            this.cbCloseDlgAfterScan.UseVisualStyleBackColor = true;
            // 
            // lbTransferMechanism
            // 
            this.lbTransferMechanism.FormattingEnabled = true;
            this.lbTransferMechanism.Location = new System.Drawing.Point(9, 198);
            this.lbTransferMechanism.Name = "lbTransferMechanism";
            this.lbTransferMechanism.Size = new System.Drawing.Size(388, 56);
            this.lbTransferMechanism.TabIndex = 22;
            this.lbTransferMechanism.SelectedIndexChanged += new System.EventHandler(this.lbTransferMechanism_SelectedIndexChanged);
            // 
            // lblTransferMechanism
            // 
            this.lblTransferMechanism.AutoSize = true;
            this.lblTransferMechanism.Location = new System.Drawing.Point(6, 182);
            this.lblTransferMechanism.Name = "lblTransferMechanism";
            this.lblTransferMechanism.Size = new System.Drawing.Size(105, 13);
            this.lblTransferMechanism.TabIndex = 23;
            this.lblTransferMechanism.Text = "Transfer mechanism:";
            // 
            // tbDefSource
            // 
            this.tbDefSource.Location = new System.Drawing.Point(9, 159);
            this.tbDefSource.Name = "tbDefSource";
            this.tbDefSource.Size = new System.Drawing.Size(388, 20);
            this.tbDefSource.TabIndex = 24;
            // 
            // tbMPPDF
            // 
            this.tbMPPDF.Location = new System.Drawing.Point(9, 384);
            this.tbMPPDF.Name = "tbMPPDF";
            this.tbMPPDF.Size = new System.Drawing.Size(349, 20);
            this.tbMPPDF.TabIndex = 25;
            this.tbMPPDF.TextChanged += new System.EventHandler(this.tbMPPDF_TextChanged);
            // 
            // tbMPTiff
            // 
            this.tbMPTiff.Location = new System.Drawing.Point(10, 441);
            this.tbMPTiff.Name = "tbMPTiff";
            this.tbMPTiff.Size = new System.Drawing.Size(349, 20);
            this.tbMPTiff.TabIndex = 26;
            this.tbMPTiff.TextChanged += new System.EventHandler(this.tbMPTiff_TextChanged);
            // 
            // cbxPixelType
            // 
            this.cbxPixelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPixelType.FormattingEnabled = true;
            this.cbxPixelType.Location = new System.Drawing.Point(8, 319);
            this.cbxPixelType.Name = "cbxPixelType";
            this.cbxPixelType.Size = new System.Drawing.Size(78, 21);
            this.cbxPixelType.TabIndex = 30;
            this.cbxPixelType.SelectedIndexChanged += new System.EventHandler(this.cbxPixelType_SelectedIndexChanged);
            // 
            // cbxResolution
            // 
            this.cbxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxResolution.FormattingEnabled = true;
            this.cbxResolution.Location = new System.Drawing.Point(210, 319);
            this.cbxResolution.Name = "cbxResolution";
            this.cbxResolution.Size = new System.Drawing.Size(81, 21);
            this.cbxResolution.TabIndex = 31;
            this.cbxResolution.SelectedIndexChanged += new System.EventHandler(this.cbxResolution_SelectedIndexChanged);
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(210, 303);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(57, 13);
            this.resolutionLabel.TabIndex = 33;
            this.resolutionLabel.Text = "Resolution";
            // 
            // pixelTypeLabel
            // 
            this.pixelTypeLabel.AutoSize = true;
            this.pixelTypeLabel.Location = new System.Drawing.Point(7, 303);
            this.pixelTypeLabel.Name = "pixelTypeLabel";
            this.pixelTypeLabel.Size = new System.Drawing.Size(56, 13);
            this.pixelTypeLabel.TabIndex = 32;
            this.pixelTypeLabel.Text = "Pixel Type";
            // 
            // cbDuplex
            // 
            this.cbDuplex.AutoSize = true;
            this.cbDuplex.Location = new System.Drawing.Point(70, 272);
            this.cbDuplex.Name = "cbDuplex";
            this.cbDuplex.Size = new System.Drawing.Size(59, 17);
            this.cbDuplex.TabIndex = 34;
            this.cbDuplex.Text = "Duplex";
            this.cbDuplex.UseVisualStyleBackColor = true;
            this.cbDuplex.CheckedChanged += new System.EventHandler(this.cbDuplex_CheckedChanged);
            // 
            // cbADF
            // 
            this.cbADF.AutoSize = true;
            this.cbADF.Location = new System.Drawing.Point(9, 272);
            this.cbADF.Name = "cbADF";
            this.cbADF.Size = new System.Drawing.Size(47, 17);
            this.cbADF.TabIndex = 35;
            this.cbADF.Text = "ADF";
            this.cbADF.UseVisualStyleBackColor = true;
            this.cbADF.CheckedChanged += new System.EventHandler(this.cbADF_CheckedChanged);
            // 
            // cbUI
            // 
            this.cbUI.AutoSize = true;
            this.cbUI.Checked = true;
            this.cbUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUI.Location = new System.Drawing.Point(135, 272);
            this.cbUI.Name = "cbUI";
            this.cbUI.Size = new System.Drawing.Size(67, 17);
            this.cbUI.TabIndex = 37;
            this.cbUI.Text = "Show UI";
            this.cbUI.UseVisualStyleBackColor = true;
            // 
            // tbFileTransfer
            // 
            this.tbFileTransfer.Location = new System.Drawing.Point(91, 544);
            this.tbFileTransfer.Name = "tbFileTransfer";
            this.tbFileTransfer.Size = new System.Drawing.Size(264, 20);
            this.tbFileTransfer.TabIndex = 46;
            this.tbFileTransfer.TextChanged += new System.EventHandler(this.tbFileTransfer_TextChanged);
            // 
            // lblFileTrans
            // 
            this.lblFileTrans.AutoSize = true;
            this.lblFileTrans.Location = new System.Drawing.Point(88, 528);
            this.lblFileTrans.Name = "lblFileTrans";
            this.lblFileTrans.Size = new System.Drawing.Size(84, 13);
            this.lblFileTrans.TabIndex = 48;
            this.lblFileTrans.Text = "FileTransferPath";
            // 
            // lblImgSavePath
            // 
            this.lblImgSavePath.AutoSize = true;
            this.lblImgSavePath.Location = new System.Drawing.Point(7, 474);
            this.lblImgSavePath.Name = "lblImgSavePath";
            this.lblImgSavePath.Size = new System.Drawing.Size(88, 13);
            this.lblImgSavePath.TabIndex = 51;
            this.lblImgSavePath.Text = "Image Save path";
            // 
            // tbImgPath
            // 
            this.tbImgPath.Location = new System.Drawing.Point(9, 491);
            this.tbImgPath.Name = "tbImgPath";
            this.tbImgPath.Size = new System.Drawing.Size(349, 20);
            this.tbImgPath.TabIndex = 49;
            this.tbImgPath.TextChanged += new System.EventHandler(this.tbImgPath_TextChanged);
            // 
            // capability
            // 
            this.capability.FormattingEnabled = true;
            this.capability.Location = new System.Drawing.Point(8, 599);
            this.capability.Name = "capability";
            this.capability.Size = new System.Drawing.Size(184, 134);
            this.capability.TabIndex = 52;
            this.capability.SelectedIndexChanged += new System.EventHandler(this.capability_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 581);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Supported caps:";
            // 
            // btnImgSavePath
            // 
            this.btnImgSavePath.Location = new System.Drawing.Point(364, 489);
            this.btnImgSavePath.Name = "btnImgSavePath";
            this.btnImgSavePath.Size = new System.Drawing.Size(28, 21);
            this.btnImgSavePath.TabIndex = 29;
            this.btnImgSavePath.Text = "...";
            this.btnImgSavePath.UseVisualStyleBackColor = true;
            this.btnImgSavePath.Click += new System.EventHandler(this.btnImgSavePath_Click);
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(364, 543);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(28, 22);
            this.btnFilePath.TabIndex = 50;
            this.btnFilePath.Text = "...";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // btnTiff
            // 
            this.btnTiff.Location = new System.Drawing.Point(365, 438);
            this.btnTiff.Name = "btnTiff";
            this.btnTiff.Size = new System.Drawing.Size(28, 22);
            this.btnTiff.TabIndex = 47;
            this.btnTiff.Text = "...";
            this.btnTiff.UseVisualStyleBackColor = true;
            this.btnTiff.Click += new System.EventHandler(this.btnTiff_Click);
            // 
            // btnPDFPath
            // 
            this.btnPDFPath.Location = new System.Drawing.Point(364, 382);
            this.btnPDFPath.Name = "btnPDFPath";
            this.btnPDFPath.Size = new System.Drawing.Size(28, 22);
            this.btnPDFPath.TabIndex = 28;
            this.btnPDFPath.Text = "...";
            this.btnPDFPath.UseVisualStyleBackColor = true;
            this.btnPDFPath.Click += new System.EventHandler(this.btnPDFPath_Click);
            // 
            // capValue
            // 
            this.capValue.FormattingEnabled = true;
            this.capValue.Location = new System.Drawing.Point(199, 599);
            this.capValue.Name = "capValue";
            this.capValue.Size = new System.Drawing.Size(198, 95);
            this.capValue.TabIndex = 54;
            this.capValue.SelectedIndexChanged += new System.EventHandler(this.capValue_SelectedIndexChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(432, 39);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(561, 655);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // comboFileTransferFormat
            // 
            this.comboFileTransferFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFileTransferFormat.FormattingEnabled = true;
            this.comboFileTransferFormat.Location = new System.Drawing.Point(8, 543);
            this.comboFileTransferFormat.Name = "comboFileTransferFormat";
            this.comboFileTransferFormat.Size = new System.Drawing.Size(74, 21);
            this.comboFileTransferFormat.TabIndex = 55;
            this.comboFileTransferFormat.SelectedIndexChanged += new System.EventHandler(this.comboFileTransferFormat_SelectedIndexChanged);
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.AutoSize = true;
            this.lblFileFormat.Location = new System.Drawing.Point(7, 527);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new System.Drawing.Size(58, 13);
            this.lblFileFormat.TabIndex = 56;
            this.lblFileFormat.Text = "File Format";
            // 
            // lblErrorMsg
            // 
            this.lblErrorMsg.AutoSize = true;
            this.lblErrorMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblErrorMsg.Location = new System.Drawing.Point(429, 736);
            this.lblErrorMsg.Name = "lblErrorMsg";
            this.lblErrorMsg.Size = new System.Drawing.Size(0, 16);
            this.lblErrorMsg.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(198, 697);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "Value to set:";
            // 
            // capValueToSet
            // 
            this.capValueToSet.Location = new System.Drawing.Point(198, 713);
            this.capValueToSet.Name = "capValueToSet";
            this.capValueToSet.Size = new System.Drawing.Size(157, 20);
            this.capValueToSet.TabIndex = 59;
            // 
            // setCapValue
            // 
            this.setCapValue.Location = new System.Drawing.Point(361, 713);
            this.setCapValue.Name = "setCapValue";
            this.setCapValue.Size = new System.Drawing.Size(36, 20);
            this.setCapValue.TabIndex = 60;
            this.setCapValue.Text = "Set";
            this.setCapValue.UseVisualStyleBackColor = true;
            this.setCapValue.Click += new System.EventHandler(this.setCapValue_Click);
            // 
            // numberOfScansBox
            // 
            this.numberOfScansBox.Location = new System.Drawing.Point(297, 320);
            this.numberOfScansBox.Name = "numberOfScansBox";
            this.numberOfScansBox.Size = new System.Drawing.Size(100, 20);
            this.numberOfScansBox.TabIndex = 61;
            this.numberOfScansBox.Text = "-1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 304);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Number of scans:";
            // 
            // pageSizeLabel
            // 
            this.pageSizeLabel.AutoSize = true;
            this.pageSizeLabel.Location = new System.Drawing.Point(92, 302);
            this.pageSizeLabel.Name = "pageSizeLabel";
            this.pageSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.pageSizeLabel.TabIndex = 64;
            this.pageSizeLabel.Text = "Page Size";
            // 
            // cbxPageSize
            // 
            this.cbxPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPageSize.FormattingEnabled = true;
            this.cbxPageSize.Location = new System.Drawing.Point(92, 319);
            this.cbxPageSize.Name = "cbxPageSize";
            this.cbxPageSize.Size = new System.Drawing.Size(112, 21);
            this.cbxPageSize.TabIndex = 63;
            this.cbxPageSize.SelectedIndexChanged += new System.EventHandler(this.cbxPageSize_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(99, 744);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 23);
            this.btnCancel.TabIndex = 65;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnProtectPDF
            // 
            this.btnProtectPDF.Location = new System.Drawing.Point(317, 409);
            this.btnProtectPDF.Name = "btnProtectPDF";
            this.btnProtectPDF.Size = new System.Drawing.Size(75, 23);
            this.btnProtectPDF.TabIndex = 66;
            this.btnProtectPDF.Text = "Protect PDF";
            this.btnProtectPDF.UseVisualStyleBackColor = true;
            this.btnProtectPDF.Click += new System.EventHandler(this.btnProtectPDF_Click);
            // 
            // AdvanceSampleForm
            // 
#if !TWCore
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
#endif
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 779);
            this.Controls.Add(this.btnProtectPDF);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pageSizeLabel);
            this.Controls.Add(this.cbxPageSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numberOfScansBox);
            this.Controls.Add(this.setCapValue);
            this.Controls.Add(this.capValueToSet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblErrorMsg);
            this.Controls.Add(this.lblFileFormat);
            this.Controls.Add(this.comboFileTransferFormat);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.capValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.capability);
            this.Controls.Add(this.lblImgSavePath);
            this.Controls.Add(this.btnFilePath);
            this.Controls.Add(this.tbImgPath);
            this.Controls.Add(this.lblFileTrans);
            this.Controls.Add(this.btnTiff);
            this.Controls.Add(this.tbFileTransfer);
            this.Controls.Add(this.cbUI);
            this.Controls.Add(this.cbADF);
            this.Controls.Add(this.cbDuplex);
            this.Controls.Add(this.resolutionLabel);
            this.Controls.Add(this.pixelTypeLabel);
            this.Controls.Add(this.cbxResolution);
            this.Controls.Add(this.cbxPixelType);
            this.Controls.Add(this.btnImgSavePath);
            this.Controls.Add(this.btnPDFPath);
            this.Controls.Add(this.tbMPTiff);
            this.Controls.Add(this.tbMPPDF);
            this.Controls.Add(this.tbDefSource);
            this.Controls.Add(this.lblTransferMechanism);
            this.Controls.Add(this.lbTransferMechanism);
            this.Controls.Add(this.cbCloseDlgAfterScan);
            this.Controls.Add(this.cbMultiPagePDF);
            this.Controls.Add(this.cbMultiPageTiff);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnAsync);
            this.Controls.Add(this.lblSources);
            this.Controls.Add(this.lblDefSource);
            this.Controls.Add(this.lbSources);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "AdvanceSampleForm";
#if !TWCore
            this.Text = "Terminalworks - TwainScanning - Advance Sample - ";
#else
            this.Text = "Terminalworks - TwainScanningCore - Advance Sample - ";
#endif
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvanceSampleForm_FormClosing);
            this.Load += new System.EventHandler(this.AdvanceSampleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbSources;
        private System.Windows.Forms.Label lblSources;
        private System.Windows.Forms.Button btnAsync;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox cbMultiPageTiff;
        private System.Windows.Forms.CheckBox cbMultiPagePDF;
        private System.Windows.Forms.CheckBox cbCloseDlgAfterScan;
        private System.Windows.Forms.ListBox lbTransferMechanism;
        private System.Windows.Forms.Label lblTransferMechanism;
        private System.Windows.Forms.TextBox tbDefSource;
        private System.Windows.Forms.TextBox tbMPPDF;
        private System.Windows.Forms.TextBox tbMPTiff;
        private System.Windows.Forms.ComboBox cbxPixelType;
        private System.Windows.Forms.ComboBox cbxResolution;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label pixelTypeLabel;
        private System.Windows.Forms.CheckBox cbDuplex;
        private System.Windows.Forms.CheckBox cbADF;
        private System.Windows.Forms.CheckBox cbUI;
        private System.Windows.Forms.TextBox tbFileTransfer;
        private System.Windows.Forms.Label lblFileTrans;
        private System.Windows.Forms.Label lblImgSavePath;
        private System.Windows.Forms.TextBox tbImgPath;
        private System.Windows.Forms.ListBox capability;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnImgSavePath;
        private System.Windows.Forms.Button btnFilePath;
        private System.Windows.Forms.Button btnTiff;
        private System.Windows.Forms.Button btnPDFPath;
        private System.Windows.Forms.ListBox capValue;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox comboFileTransferFormat;
        private System.Windows.Forms.Label lblFileFormat;
        private System.Windows.Forms.Label lblErrorMsg;
        private System.Windows.Forms.Label lblDefSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox capValueToSet;
        private System.Windows.Forms.Button setCapValue;
        private System.Windows.Forms.TextBox numberOfScansBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label pageSizeLabel;
        private System.Windows.Forms.ComboBox cbxPageSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnProtectPDF;
    }
}

