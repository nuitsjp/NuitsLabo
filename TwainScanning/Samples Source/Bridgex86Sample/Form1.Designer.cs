
namespace Bridgex86Sample
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
            this.cbxTiffImageQuality = new System.Windows.Forms.ComboBox();
            this.chkMultiPageScan = new System.Windows.Forms.CheckBox();
            this.cbxIgnoreBlankPages = new System.Windows.Forms.ComboBox();
            this.cbxBrightness = new System.Windows.Forms.ComboBox();
            this.cbxContrast = new System.Windows.Forms.ComboBox();
            this.cbxThreshold = new System.Windows.Forms.ComboBox();
            this.cbxAutomaticBorderDetection = new System.Windows.Forms.ComboBox();
            this.cbxRotation = new System.Windows.Forms.ComboBox();
            this.cbxColorMode = new System.Windows.Forms.ComboBox();
            this.cbxAutoFeed = new System.Windows.Forms.ComboBox();
            this.cbxResolution = new System.Windows.Forms.ComboBox();
            this.cbxDuplexEnabled = new System.Windows.Forms.ComboBox();
            this.cbxPageSize = new System.Windows.Forms.ComboBox();
            this.cbxTransferMechanism = new System.Windows.Forms.ComboBox();
            this.lblIgnoreBlankPages = new System.Windows.Forms.Label();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.lblContrast = new System.Windows.Forms.Label();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.lblAutomaticBorderDetection = new System.Windows.Forms.Label();
            this.lblRotation = new System.Windows.Forms.Label();
            this.lblColorMode = new System.Windows.Forms.Label();
            this.lblAutoFeed = new System.Windows.Forms.Label();
            this.lblResolutionX = new System.Windows.Forms.Label();
            this.lblDuplexEnabled = new System.Windows.Forms.Label();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.lblTransferMechanism = new System.Windows.Forms.Label();
            this.lbScanners = new System.Windows.Forms.ListBox();
            this.btnSetOutputFileName = new System.Windows.Forms.Button();
            this.tbOutputFileName = new System.Windows.Forms.TextBox();
            this.lblOutputFileName = new System.Windows.Forms.Label();
            this.btnAcquire = new System.Windows.Forms.Button();
            this.chkShowUI = new System.Windows.Forms.CheckBox();
            this.chkCloseUIAfterAcquire = new System.Windows.Forms.CheckBox();
            this.lblSelectScanner = new System.Windows.Forms.Label();
            this.chkChangeScanSettings = new System.Windows.Forms.CheckBox();
            this.grpScanSettings = new System.Windows.Forms.GroupBox();
            this.lblTiffImageQuality = new System.Windows.Forms.Label();
            this.tbDefaultScanner = new System.Windows.Forms.TextBox();
            this.lblDefaultScanner = new System.Windows.Forms.Label();
            this.btnAvailableScanners = new System.Windows.Forms.Button();
            this.lblImageQuality = new System.Windows.Forms.Label();
            this.lblImageCount = new System.Windows.Forms.Label();
            this.cbxImageCount = new System.Windows.Forms.ComboBox();
            this.cbxImageQuality = new System.Windows.Forms.ComboBox();
            this.lblOutputFileNamesListCount = new System.Windows.Forms.Label();
            this.cbxOutputFileNamesListCount = new System.Windows.Forms.ComboBox();
            this.lblPdfProtection = new System.Windows.Forms.Label();
            this.btnProtectPDF = new System.Windows.Forms.Button();
            this.grpScanSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxTiffImageQuality
            // 
            this.cbxTiffImageQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTiffImageQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTiffImageQuality.FormattingEnabled = true;
            this.cbxTiffImageQuality.Location = new System.Drawing.Point(445, 209);
            this.cbxTiffImageQuality.Name = "cbxTiffImageQuality";
            this.cbxTiffImageQuality.Size = new System.Drawing.Size(150, 21);
            this.cbxTiffImageQuality.TabIndex = 74;
            // 
            // chkMultiPageScan
            // 
            this.chkMultiPageScan.AutoSize = true;
            this.chkMultiPageScan.Location = new System.Drawing.Point(296, 159);
            this.chkMultiPageScan.Name = "chkMultiPageScan";
            this.chkMultiPageScan.Size = new System.Drawing.Size(100, 17);
            this.chkMultiPageScan.TabIndex = 85;
            this.chkMultiPageScan.Text = "Multipage Scan";
            this.chkMultiPageScan.UseVisualStyleBackColor = true;
            // 
            // cbxIgnoreBlankPages
            // 
            this.cbxIgnoreBlankPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxIgnoreBlankPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxIgnoreBlankPages.FormattingEnabled = true;
            this.cbxIgnoreBlankPages.Location = new System.Drawing.Point(445, 236);
            this.cbxIgnoreBlankPages.Name = "cbxIgnoreBlankPages";
            this.cbxIgnoreBlankPages.Size = new System.Drawing.Size(150, 21);
            this.cbxIgnoreBlankPages.TabIndex = 124;
            // 
            // cbxBrightness
            // 
            this.cbxBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxBrightness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBrightness.FormattingEnabled = true;
            this.cbxBrightness.Location = new System.Drawing.Point(445, 317);
            this.cbxBrightness.Name = "cbxBrightness";
            this.cbxBrightness.Size = new System.Drawing.Size(150, 21);
            this.cbxBrightness.TabIndex = 122;
            // 
            // cbxContrast
            // 
            this.cbxContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxContrast.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxContrast.FormattingEnabled = true;
            this.cbxContrast.Location = new System.Drawing.Point(445, 290);
            this.cbxContrast.Name = "cbxContrast";
            this.cbxContrast.Size = new System.Drawing.Size(150, 21);
            this.cbxContrast.TabIndex = 121;
            // 
            // cbxThreshold
            // 
            this.cbxThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxThreshold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxThreshold.FormattingEnabled = true;
            this.cbxThreshold.Location = new System.Drawing.Point(445, 263);
            this.cbxThreshold.Name = "cbxThreshold";
            this.cbxThreshold.Size = new System.Drawing.Size(150, 21);
            this.cbxThreshold.TabIndex = 120;
            // 
            // cbxAutomaticBorderDetection
            // 
            this.cbxAutomaticBorderDetection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAutomaticBorderDetection.FormattingEnabled = true;
            this.cbxAutomaticBorderDetection.Location = new System.Drawing.Point(151, 371);
            this.cbxAutomaticBorderDetection.Name = "cbxAutomaticBorderDetection";
            this.cbxAutomaticBorderDetection.Size = new System.Drawing.Size(150, 21);
            this.cbxAutomaticBorderDetection.TabIndex = 119;
            // 
            // cbxRotation
            // 
            this.cbxRotation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRotation.FormattingEnabled = true;
            this.cbxRotation.Location = new System.Drawing.Point(151, 398);
            this.cbxRotation.Name = "cbxRotation";
            this.cbxRotation.Size = new System.Drawing.Size(150, 21);
            this.cbxRotation.TabIndex = 118;
            // 
            // cbxColorMode
            // 
            this.cbxColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxColorMode.FormattingEnabled = true;
            this.cbxColorMode.Location = new System.Drawing.Point(151, 209);
            this.cbxColorMode.Name = "cbxColorMode";
            this.cbxColorMode.Size = new System.Drawing.Size(150, 21);
            this.cbxColorMode.TabIndex = 117;
            // 
            // cbxAutoFeed
            // 
            this.cbxAutoFeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAutoFeed.FormattingEnabled = true;
            this.cbxAutoFeed.Location = new System.Drawing.Point(151, 344);
            this.cbxAutoFeed.Name = "cbxAutoFeed";
            this.cbxAutoFeed.Size = new System.Drawing.Size(150, 21);
            this.cbxAutoFeed.TabIndex = 116;
            // 
            // cbxResolution
            // 
            this.cbxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxResolution.FormattingEnabled = true;
            this.cbxResolution.Location = new System.Drawing.Point(151, 317);
            this.cbxResolution.Name = "cbxResolution";
            this.cbxResolution.Size = new System.Drawing.Size(150, 21);
            this.cbxResolution.TabIndex = 114;
            // 
            // cbxDuplexEnabled
            // 
            this.cbxDuplexEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDuplexEnabled.FormattingEnabled = true;
            this.cbxDuplexEnabled.Location = new System.Drawing.Point(151, 290);
            this.cbxDuplexEnabled.Name = "cbxDuplexEnabled";
            this.cbxDuplexEnabled.Size = new System.Drawing.Size(150, 21);
            this.cbxDuplexEnabled.TabIndex = 113;
            // 
            // cbxPageSize
            // 
            this.cbxPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPageSize.FormattingEnabled = true;
            this.cbxPageSize.Location = new System.Drawing.Point(151, 236);
            this.cbxPageSize.Name = "cbxPageSize";
            this.cbxPageSize.Size = new System.Drawing.Size(150, 21);
            this.cbxPageSize.TabIndex = 112;
            // 
            // cbxTransferMechanism
            // 
            this.cbxTransferMechanism.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTransferMechanism.FormattingEnabled = true;
            this.cbxTransferMechanism.Location = new System.Drawing.Point(151, 182);
            this.cbxTransferMechanism.Name = "cbxTransferMechanism";
            this.cbxTransferMechanism.Size = new System.Drawing.Size(150, 21);
            this.cbxTransferMechanism.TabIndex = 111;
            // 
            // lblIgnoreBlankPages
            // 
            this.lblIgnoreBlankPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIgnoreBlankPages.AutoSize = true;
            this.lblIgnoreBlankPages.Location = new System.Drawing.Point(325, 239);
            this.lblIgnoreBlankPages.Name = "lblIgnoreBlankPages";
            this.lblIgnoreBlankPages.Size = new System.Drawing.Size(103, 13);
            this.lblIgnoreBlankPages.TabIndex = 110;
            this.lblIgnoreBlankPages.Text = "Ignore Blank Pages:";
            // 
            // lblBrightness
            // 
            this.lblBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.Location = new System.Drawing.Point(325, 320);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(59, 13);
            this.lblBrightness.TabIndex = 108;
            this.lblBrightness.Text = "Brightness:";
            // 
            // lblContrast
            // 
            this.lblContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContrast.AutoSize = true;
            this.lblContrast.Location = new System.Drawing.Point(325, 293);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(49, 13);
            this.lblContrast.TabIndex = 107;
            this.lblContrast.Text = "Contrast:";
            // 
            // lblThreshold
            // 
            this.lblThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(325, 266);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(57, 13);
            this.lblThreshold.TabIndex = 106;
            this.lblThreshold.Text = "Threshold:";
            // 
            // lblAutomaticBorderDetection
            // 
            this.lblAutomaticBorderDetection.AutoSize = true;
            this.lblAutomaticBorderDetection.Location = new System.Drawing.Point(5, 374);
            this.lblAutomaticBorderDetection.Name = "lblAutomaticBorderDetection";
            this.lblAutomaticBorderDetection.Size = new System.Drawing.Size(140, 13);
            this.lblAutomaticBorderDetection.TabIndex = 105;
            this.lblAutomaticBorderDetection.Text = "Automatic Border Detection:";
            // 
            // lblRotation
            // 
            this.lblRotation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(5, 401);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(50, 13);
            this.lblRotation.TabIndex = 104;
            this.lblRotation.Text = "Rotation:";
            // 
            // lblColorMode
            // 
            this.lblColorMode.AutoSize = true;
            this.lblColorMode.Location = new System.Drawing.Point(5, 212);
            this.lblColorMode.Name = "lblColorMode";
            this.lblColorMode.Size = new System.Drawing.Size(64, 13);
            this.lblColorMode.TabIndex = 103;
            this.lblColorMode.Text = "Color Mode:";
            // 
            // lblAutoFeed
            // 
            this.lblAutoFeed.AutoSize = true;
            this.lblAutoFeed.Location = new System.Drawing.Point(5, 347);
            this.lblAutoFeed.Name = "lblAutoFeed";
            this.lblAutoFeed.Size = new System.Drawing.Size(59, 13);
            this.lblAutoFeed.TabIndex = 102;
            this.lblAutoFeed.Text = "Auto Feed:";
            // 
            // lblResolutionX
            // 
            this.lblResolutionX.AutoSize = true;
            this.lblResolutionX.Location = new System.Drawing.Point(5, 320);
            this.lblResolutionX.Name = "lblResolutionX";
            this.lblResolutionX.Size = new System.Drawing.Size(60, 13);
            this.lblResolutionX.TabIndex = 100;
            this.lblResolutionX.Text = "Resolution:";
            // 
            // lblDuplexEnabled
            // 
            this.lblDuplexEnabled.AutoSize = true;
            this.lblDuplexEnabled.Location = new System.Drawing.Point(5, 293);
            this.lblDuplexEnabled.Name = "lblDuplexEnabled";
            this.lblDuplexEnabled.Size = new System.Drawing.Size(85, 13);
            this.lblDuplexEnabled.TabIndex = 99;
            this.lblDuplexEnabled.Text = "Duplex Enabled:";
            // 
            // lblPageSize
            // 
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(5, 239);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(58, 13);
            this.lblPageSize.TabIndex = 98;
            this.lblPageSize.Text = "Page Size:";
            // 
            // lblTransferMechanism
            // 
            this.lblTransferMechanism.AutoSize = true;
            this.lblTransferMechanism.Location = new System.Drawing.Point(5, 185);
            this.lblTransferMechanism.Name = "lblTransferMechanism";
            this.lblTransferMechanism.Size = new System.Drawing.Size(106, 13);
            this.lblTransferMechanism.TabIndex = 97;
            this.lblTransferMechanism.Text = "Transfer Mechanism:";
            // 
            // lbScanners
            // 
            this.lbScanners.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbScanners.FormattingEnabled = true;
            this.lbScanners.Location = new System.Drawing.Point(89, 19);
            this.lbScanners.Name = "lbScanners";
            this.lbScanners.Size = new System.Drawing.Size(506, 134);
            this.lbScanners.TabIndex = 74;
            this.lbScanners.SelectedIndexChanged += new System.EventHandler(this.lbScanners_SelectedIndexChanged);
            // 
            // btnSetOutputFileName
            // 
            this.btnSetOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetOutputFileName.Location = new System.Drawing.Point(498, 65);
            this.btnSetOutputFileName.Name = "btnSetOutputFileName";
            this.btnSetOutputFileName.Size = new System.Drawing.Size(51, 24);
            this.btnSetOutputFileName.TabIndex = 70;
            this.btnSetOutputFileName.Text = "...";
            this.btnSetOutputFileName.UseVisualStyleBackColor = true;
            this.btnSetOutputFileName.Click += new System.EventHandler(this.btnSetOutputFileName_Click);
            // 
            // tbOutputFileName
            // 
            this.tbOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFileName.Location = new System.Drawing.Point(12, 65);
            this.tbOutputFileName.Name = "tbOutputFileName";
            this.tbOutputFileName.Size = new System.Drawing.Size(480, 20);
            this.tbOutputFileName.TabIndex = 69;
            this.tbOutputFileName.TextChanged += new System.EventHandler(this.tbOutputFileName_TextChanged);
            // 
            // lblOutputFileName
            // 
            this.lblOutputFileName.AutoSize = true;
            this.lblOutputFileName.Location = new System.Drawing.Point(9, 48);
            this.lblOutputFileName.Name = "lblOutputFileName";
            this.lblOutputFileName.Size = new System.Drawing.Size(87, 13);
            this.lblOutputFileName.TabIndex = 68;
            this.lblOutputFileName.Text = "Output file name:";
            // 
            // btnAcquire
            // 
            this.btnAcquire.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcquire.Location = new System.Drawing.Point(555, 65);
            this.btnAcquire.Name = "btnAcquire";
            this.btnAcquire.Size = new System.Drawing.Size(62, 24);
            this.btnAcquire.TabIndex = 67;
            this.btnAcquire.Text = "Acquire";
            this.btnAcquire.UseVisualStyleBackColor = true;
            this.btnAcquire.Click += new System.EventHandler(this.btnAcquire_Click);
            // 
            // chkShowUI
            // 
            this.chkShowUI.AutoSize = true;
            this.chkShowUI.Checked = true;
            this.chkShowUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUI.Location = new System.Drawing.Point(89, 159);
            this.chkShowUI.Name = "chkShowUI";
            this.chkShowUI.Size = new System.Drawing.Size(67, 17);
            this.chkShowUI.TabIndex = 77;
            this.chkShowUI.Text = "Show UI";
            this.chkShowUI.UseVisualStyleBackColor = true;
            // 
            // chkCloseUIAfterAcquire
            // 
            this.chkCloseUIAfterAcquire.AutoSize = true;
            this.chkCloseUIAfterAcquire.Checked = true;
            this.chkCloseUIAfterAcquire.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloseUIAfterAcquire.Location = new System.Drawing.Point(162, 159);
            this.chkCloseUIAfterAcquire.Name = "chkCloseUIAfterAcquire";
            this.chkCloseUIAfterAcquire.Size = new System.Drawing.Size(128, 17);
            this.chkCloseUIAfterAcquire.TabIndex = 76;
            this.chkCloseUIAfterAcquire.Text = "Close UI after acquire";
            this.chkCloseUIAfterAcquire.UseVisualStyleBackColor = true;
            // 
            // lblSelectScanner
            // 
            this.lblSelectScanner.AutoSize = true;
            this.lblSelectScanner.Location = new System.Drawing.Point(6, 54);
            this.lblSelectScanner.Name = "lblSelectScanner";
            this.lblSelectScanner.Size = new System.Drawing.Size(83, 13);
            this.lblSelectScanner.TabIndex = 97;
            this.lblSelectScanner.Text = "Select Scanner:";
            // 
            // chkChangeScanSettings
            // 
            this.chkChangeScanSettings.AutoSize = true;
            this.chkChangeScanSettings.Location = new System.Drawing.Point(12, 91);
            this.chkChangeScanSettings.Name = "chkChangeScanSettings";
            this.chkChangeScanSettings.Size = new System.Drawing.Size(128, 17);
            this.chkChangeScanSettings.TabIndex = 98;
            this.chkChangeScanSettings.Text = "Change scan settings";
            this.chkChangeScanSettings.UseVisualStyleBackColor = true;
            this.chkChangeScanSettings.CheckedChanged += new System.EventHandler(this.chkChangeScanSettings_CheckedChanged);
            // 
            // grpScanSettings
            // 
            this.grpScanSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpScanSettings.Controls.Add(this.btnProtectPDF);
            this.grpScanSettings.Controls.Add(this.lblPdfProtection);
            this.grpScanSettings.Controls.Add(this.cbxOutputFileNamesListCount);
            this.grpScanSettings.Controls.Add(this.lblOutputFileNamesListCount);
            this.grpScanSettings.Controls.Add(this.cbxImageQuality);
            this.grpScanSettings.Controls.Add(this.cbxImageCount);
            this.grpScanSettings.Controls.Add(this.lblImageCount);
            this.grpScanSettings.Controls.Add(this.lblImageQuality);
            this.grpScanSettings.Controls.Add(this.btnAvailableScanners);
            this.grpScanSettings.Controls.Add(this.lblTiffImageQuality);
            this.grpScanSettings.Controls.Add(this.cbxIgnoreBlankPages);
            this.grpScanSettings.Controls.Add(this.lblSelectScanner);
            this.grpScanSettings.Controls.Add(this.lblIgnoreBlankPages);
            this.grpScanSettings.Controls.Add(this.lbScanners);
            this.grpScanSettings.Controls.Add(this.chkMultiPageScan);
            this.grpScanSettings.Controls.Add(this.cbxTransferMechanism);
            this.grpScanSettings.Controls.Add(this.lblTransferMechanism);
            this.grpScanSettings.Controls.Add(this.cbxBrightness);
            this.grpScanSettings.Controls.Add(this.lblPageSize);
            this.grpScanSettings.Controls.Add(this.chkShowUI);
            this.grpScanSettings.Controls.Add(this.cbxContrast);
            this.grpScanSettings.Controls.Add(this.cbxPageSize);
            this.grpScanSettings.Controls.Add(this.chkCloseUIAfterAcquire);
            this.grpScanSettings.Controls.Add(this.cbxThreshold);
            this.grpScanSettings.Controls.Add(this.lblBrightness);
            this.grpScanSettings.Controls.Add(this.cbxDuplexEnabled);
            this.grpScanSettings.Controls.Add(this.lblContrast);
            this.grpScanSettings.Controls.Add(this.cbxAutomaticBorderDetection);
            this.grpScanSettings.Controls.Add(this.lblDuplexEnabled);
            this.grpScanSettings.Controls.Add(this.cbxRotation);
            this.grpScanSettings.Controls.Add(this.lblResolutionX);
            this.grpScanSettings.Controls.Add(this.cbxColorMode);
            this.grpScanSettings.Controls.Add(this.cbxResolution);
            this.grpScanSettings.Controls.Add(this.cbxAutoFeed);
            this.grpScanSettings.Controls.Add(this.lblThreshold);
            this.grpScanSettings.Controls.Add(this.lblAutomaticBorderDetection);
            this.grpScanSettings.Controls.Add(this.lblRotation);
            this.grpScanSettings.Controls.Add(this.lblAutoFeed);
            this.grpScanSettings.Controls.Add(this.lblColorMode);
            this.grpScanSettings.Controls.Add(this.cbxTiffImageQuality);
            this.grpScanSettings.Location = new System.Drawing.Point(12, 114);
            this.grpScanSettings.Name = "grpScanSettings";
            this.grpScanSettings.Size = new System.Drawing.Size(605, 427);
            this.grpScanSettings.TabIndex = 99;
            this.grpScanSettings.TabStop = false;
            this.grpScanSettings.Text = "Scan settings";
            // 
            // lblTiffImageQuality
            // 
            this.lblTiffImageQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTiffImageQuality.AutoSize = true;
            this.lblTiffImageQuality.Location = new System.Drawing.Point(325, 212);
            this.lblTiffImageQuality.Name = "lblTiffImageQuality";
            this.lblTiffImageQuality.Size = new System.Drawing.Size(99, 13);
            this.lblTiffImageQuality.TabIndex = 125;
            this.lblTiffImageQuality.Text = "TIFF Image Quality:";
            // 
            // tbDefaultScanner
            // 
            this.tbDefaultScanner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDefaultScanner.Location = new System.Drawing.Point(12, 25);
            this.tbDefaultScanner.Name = "tbDefaultScanner";
            this.tbDefaultScanner.ReadOnly = true;
            this.tbDefaultScanner.Size = new System.Drawing.Size(605, 20);
            this.tbDefaultScanner.TabIndex = 101;
            this.tbDefaultScanner.TabStop = false;
            this.tbDefaultScanner.DoubleClick += new System.EventHandler(this.tbDefaultScanner_DoubleClick);
            // 
            // lblDefaultScanner
            // 
            this.lblDefaultScanner.AutoSize = true;
            this.lblDefaultScanner.Location = new System.Drawing.Point(9, 9);
            this.lblDefaultScanner.Name = "lblDefaultScanner";
            this.lblDefaultScanner.Size = new System.Drawing.Size(87, 13);
            this.lblDefaultScanner.TabIndex = 100;
            this.lblDefaultScanner.Text = "Default Scanner:";
            // 
            // btnAvailableScanners
            // 
            this.btnAvailableScanners.Location = new System.Drawing.Point(6, 70);
            this.btnAvailableScanners.Name = "btnAvailableScanners";
            this.btnAvailableScanners.Size = new System.Drawing.Size(77, 48);
            this.btnAvailableScanners.TabIndex = 102;
            this.btnAvailableScanners.Text = "Available Scanners";
            this.btnAvailableScanners.UseVisualStyleBackColor = true;
            this.btnAvailableScanners.Click += new System.EventHandler(this.btnAvailableScanners_Click);
            // 
            // lblImageQuality
            // 
            this.lblImageQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImageQuality.AutoSize = true;
            this.lblImageQuality.Location = new System.Drawing.Point(325, 185);
            this.lblImageQuality.Name = "lblImageQuality";
            this.lblImageQuality.Size = new System.Drawing.Size(74, 13);
            this.lblImageQuality.TabIndex = 128;
            this.lblImageQuality.Text = "Image Quality:";
            // 
            // lblImageCount
            // 
            this.lblImageCount.AutoSize = true;
            this.lblImageCount.Location = new System.Drawing.Point(6, 266);
            this.lblImageCount.Name = "lblImageCount";
            this.lblImageCount.Size = new System.Drawing.Size(70, 13);
            this.lblImageCount.TabIndex = 129;
            this.lblImageCount.Text = "Image Count:";
            // 
            // cbxImageCount
            // 
            this.cbxImageCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxImageCount.FormattingEnabled = true;
            this.cbxImageCount.Location = new System.Drawing.Point(151, 263);
            this.cbxImageCount.Name = "cbxImageCount";
            this.cbxImageCount.Size = new System.Drawing.Size(150, 21);
            this.cbxImageCount.TabIndex = 130;
            // 
            // cbxImageQuality
            // 
            this.cbxImageQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxImageQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxImageQuality.FormattingEnabled = true;
            this.cbxImageQuality.Location = new System.Drawing.Point(445, 182);
            this.cbxImageQuality.Name = "cbxImageQuality";
            this.cbxImageQuality.Size = new System.Drawing.Size(150, 21);
            this.cbxImageQuality.TabIndex = 131;
            // 
            // lblPdfProtection
            // 
            this.lblPdfProtection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPdfProtection.AutoSize = true;
            this.lblPdfProtection.Location = new System.Drawing.Point(325, 374);
            this.lblPdfProtection.Name = "lblPdfProtection";
            this.lblPdfProtection.Size = new System.Drawing.Size(82, 13);
            this.lblPdfProtection.TabIndex = 134;
            this.lblPdfProtection.Text = "PDF Protection:";
            // 
            // btnProtectPDF
            // 
            this.btnProtectPDF.Location = new System.Drawing.Point(444, 371);
            this.btnProtectPDF.Name = "btnProtectPDF";
            this.btnProtectPDF.Size = new System.Drawing.Size(75, 23);
            this.btnProtectPDF.TabIndex = 133;
            this.btnProtectPDF.Text = "Protect PDF";
            this.btnProtectPDF.UseVisualStyleBackColor = true;
            this.btnProtectPDF.Click += new System.EventHandler(this.btnProtectPDF_Click);
            // 
            // lblOutputFileNamesListCount
            // 
            this.lblOutputFileNamesListCount.AutoSize = true;
            this.lblOutputFileNamesListCount.Location = new System.Drawing.Point(325, 347);
            this.lblOutputFileNamesListCount.Name = "lblOutputFileNamesListCount";
            this.lblOutputFileNamesListCount.Size = new System.Drawing.Size(109, 13);
            this.lblOutputFileNamesListCount.TabIndex = 135;
            this.lblOutputFileNamesListCount.Text = "Output Names Count:";
            // 
            // cbxOutputFileNamesListCount
            // 
            this.cbxOutputFileNamesListCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxOutputFileNamesListCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOutputFileNamesListCount.FormattingEnabled = true;
            this.cbxOutputFileNamesListCount.Location = new System.Drawing.Point(445, 344);
            this.cbxOutputFileNamesListCount.Name = "cbxOutputFileNamesListCount";
            this.cbxOutputFileNamesListCount.Size = new System.Drawing.Size(150, 21);
            this.cbxOutputFileNamesListCount.TabIndex = 137;
            // 
            // Form1
            // 
#if !TWCore
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
#endif
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 550);
            this.Controls.Add(this.tbDefaultScanner);
            this.Controls.Add(this.lblDefaultScanner);
            this.Controls.Add(this.grpScanSettings);
            this.Controls.Add(this.chkChangeScanSettings);
            this.Controls.Add(this.btnSetOutputFileName);
            this.Controls.Add(this.tbOutputFileName);
            this.Controls.Add(this.lblOutputFileName);
            this.Controls.Add(this.btnAcquire);
            this.Name = "Form1";
#if !TWCore
            this.Text = "Terminalworks - TwainScanning - Bridgex86 Sample - ";
#else
            this.Text = "Terminalworks - TwainScanningCore - Bridgex86 Sample - "; 
#endif
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpScanSettings.ResumeLayout(false);
            this.grpScanSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbxTiffImageQuality;
        private System.Windows.Forms.CheckBox chkMultiPageScan;
        private System.Windows.Forms.ListBox lbScanners;
        private System.Windows.Forms.Button btnSetOutputFileName;
        private System.Windows.Forms.TextBox tbOutputFileName;
        private System.Windows.Forms.Label lblOutputFileName;
        private System.Windows.Forms.Button btnAcquire;
        private System.Windows.Forms.CheckBox chkShowUI;
        private System.Windows.Forms.CheckBox chkCloseUIAfterAcquire;
        private System.Windows.Forms.Label lblIgnoreBlankPages;
        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.Label lblContrast;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblAutomaticBorderDetection;
        private System.Windows.Forms.Label lblRotation;
        private System.Windows.Forms.Label lblColorMode;
        private System.Windows.Forms.Label lblAutoFeed;
        private System.Windows.Forms.Label lblResolutionX;
        private System.Windows.Forms.Label lblDuplexEnabled;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.Label lblTransferMechanism;
        private System.Windows.Forms.Label lblSelectScanner;
        private System.Windows.Forms.ComboBox cbxIgnoreBlankPages;
        private System.Windows.Forms.ComboBox cbxBrightness;
        private System.Windows.Forms.ComboBox cbxContrast;
        private System.Windows.Forms.ComboBox cbxThreshold;
        private System.Windows.Forms.ComboBox cbxAutomaticBorderDetection;
        private System.Windows.Forms.ComboBox cbxRotation;
        private System.Windows.Forms.ComboBox cbxColorMode;
        private System.Windows.Forms.ComboBox cbxAutoFeed;
        private System.Windows.Forms.ComboBox cbxResolution;
        private System.Windows.Forms.ComboBox cbxDuplexEnabled;
        private System.Windows.Forms.ComboBox cbxPageSize;
        private System.Windows.Forms.ComboBox cbxTransferMechanism;
        private System.Windows.Forms.CheckBox chkChangeScanSettings;
        private System.Windows.Forms.GroupBox grpScanSettings;
        private System.Windows.Forms.Label lblTiffImageQuality;
        private System.Windows.Forms.TextBox tbDefaultScanner;
        private System.Windows.Forms.Label lblDefaultScanner;
        private System.Windows.Forms.Button btnAvailableScanners;
        private System.Windows.Forms.ComboBox cbxImageQuality;
        private System.Windows.Forms.ComboBox cbxImageCount;
        private System.Windows.Forms.Label lblImageCount;
        private System.Windows.Forms.Label lblImageQuality;
        private System.Windows.Forms.Label lblPdfProtection;
        private System.Windows.Forms.Button btnProtectPDF;
        private System.Windows.Forms.Label lblOutputFileNamesListCount;
        private System.Windows.Forms.ComboBox cbxOutputFileNamesListCount;
    }
}

