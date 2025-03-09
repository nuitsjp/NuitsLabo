
namespace TwainScanningTestCommon
{
    partial class FormProtectPDF
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
            this.lblOwnerPassword = new System.Windows.Forms.Label();
            this.tbUserPassword = new System.Windows.Forms.TextBox();
            this.tbOwnerPassword = new System.Windows.Forms.TextBox();
            this.lblUserPassword = new System.Windows.Forms.Label();
            this.gbUserPermissions = new System.Windows.Forms.GroupBox();
            this.btnUserPermissionsToggleAll = new System.Windows.Forms.Button();
            this.cbPermitPrint = new System.Windows.Forms.CheckBox();
            this.cbPermitAnnotations = new System.Windows.Forms.CheckBox();
            this.cbPermitExtractContent = new System.Windows.Forms.CheckBox();
            this.cbPermitModifyDocument = new System.Windows.Forms.CheckBox();
            this.lblEncryptionAlgorithm = new System.Windows.Forms.Label();
            this.cmbxEncryptionAlgorithm = new System.Windows.Forms.ComboBox();
            this.cbProtectPDF = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbProtectionSettings = new System.Windows.Forms.GroupBox();
            this.cbPermitFullQualityPrint = new System.Windows.Forms.CheckBox();
            this.cbPermitAssembleDocument = new System.Windows.Forms.CheckBox();
            this.cbPermitAccessibilityExtractContent = new System.Windows.Forms.CheckBox();
            this.cbPermitFormsFill = new System.Windows.Forms.CheckBox();
            this.gbUserPermissions128bitEncryption = new System.Windows.Forms.GroupBox();
            this.gbUserPermissions.SuspendLayout();
            this.gbProtectionSettings.SuspendLayout();
            this.gbUserPermissions128bitEncryption.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOwnerPassword
            // 
            this.lblOwnerPassword.AutoSize = true;
            this.lblOwnerPassword.Location = new System.Drawing.Point(5, 54);
            this.lblOwnerPassword.Name = "lblOwnerPassword";
            this.lblOwnerPassword.Size = new System.Drawing.Size(89, 13);
            this.lblOwnerPassword.TabIndex = 28;
            this.lblOwnerPassword.Text = "Owner password:";
            // 
            // tbUserPassword
            // 
            this.tbUserPassword.Location = new System.Drawing.Point(117, 77);
            this.tbUserPassword.Name = "tbUserPassword";
            this.tbUserPassword.Size = new System.Drawing.Size(355, 20);
            this.tbUserPassword.TabIndex = 27;
            // 
            // tbOwnerPassword
            // 
            this.tbOwnerPassword.Location = new System.Drawing.Point(117, 51);
            this.tbOwnerPassword.Name = "tbOwnerPassword";
            this.tbOwnerPassword.Size = new System.Drawing.Size(355, 20);
            this.tbOwnerPassword.TabIndex = 25;
            // 
            // lblUserPassword
            // 
            this.lblUserPassword.AutoSize = true;
            this.lblUserPassword.Location = new System.Drawing.Point(5, 80);
            this.lblUserPassword.Name = "lblUserPassword";
            this.lblUserPassword.Size = new System.Drawing.Size(80, 13);
            this.lblUserPassword.TabIndex = 29;
            this.lblUserPassword.Text = "User password:";
            // 
            // gbUserPermissions
            // 
            this.gbUserPermissions.Controls.Add(this.gbUserPermissions128bitEncryption);
            this.gbUserPermissions.Controls.Add(this.btnUserPermissionsToggleAll);
            this.gbUserPermissions.Controls.Add(this.cbPermitPrint);
            this.gbUserPermissions.Controls.Add(this.cbPermitAnnotations);
            this.gbUserPermissions.Controls.Add(this.cbPermitExtractContent);
            this.gbUserPermissions.Controls.Add(this.cbPermitModifyDocument);
            this.gbUserPermissions.Location = new System.Drawing.Point(9, 103);
            this.gbUserPermissions.Name = "gbUserPermissions";
            this.gbUserPermissions.Size = new System.Drawing.Size(463, 144);
            this.gbUserPermissions.TabIndex = 24;
            this.gbUserPermissions.TabStop = false;
            this.gbUserPermissions.Text = "User access permission (owner has all permissions)";
            // 
            // btnUserPermissionsToggleAll
            // 
            this.btnUserPermissionsToggleAll.Location = new System.Drawing.Point(6, 34);
            this.btnUserPermissionsToggleAll.Name = "btnUserPermissionsToggleAll";
            this.btnUserPermissionsToggleAll.Size = new System.Drawing.Size(99, 23);
            this.btnUserPermissionsToggleAll.TabIndex = 31;
            this.btnUserPermissionsToggleAll.Text = "Permit";
            this.btnUserPermissionsToggleAll.UseVisualStyleBackColor = true;
            this.btnUserPermissionsToggleAll.Click += new System.EventHandler(this.btnUserPermissionsToggleAll_Click);
            // 
            // cbPermitPrint
            // 
            this.cbPermitPrint.AutoSize = true;
            this.cbPermitPrint.Location = new System.Drawing.Point(111, 38);
            this.cbPermitPrint.Name = "cbPermitPrint";
            this.cbPermitPrint.Size = new System.Drawing.Size(78, 17);
            this.cbPermitPrint.TabIndex = 2;
            this.cbPermitPrint.Text = "Permit print";
            this.cbPermitPrint.UseVisualStyleBackColor = true;
            // 
            // cbPermitAnnotations
            // 
            this.cbPermitAnnotations.AutoSize = true;
            this.cbPermitAnnotations.Location = new System.Drawing.Point(111, 107);
            this.cbPermitAnnotations.Name = "cbPermitAnnotations";
            this.cbPermitAnnotations.Size = new System.Drawing.Size(113, 17);
            this.cbPermitAnnotations.TabIndex = 5;
            this.cbPermitAnnotations.Text = "Permit annotations";
            this.cbPermitAnnotations.UseVisualStyleBackColor = true;
            // 
            // cbPermitExtractContent
            // 
            this.cbPermitExtractContent.AutoSize = true;
            this.cbPermitExtractContent.Location = new System.Drawing.Point(111, 84);
            this.cbPermitExtractContent.Name = "cbPermitExtractContent";
            this.cbPermitExtractContent.Size = new System.Drawing.Size(129, 17);
            this.cbPermitExtractContent.TabIndex = 4;
            this.cbPermitExtractContent.Text = "Permit extract content";
            this.cbPermitExtractContent.UseVisualStyleBackColor = true;
            // 
            // cbPermitModifyDocument
            // 
            this.cbPermitModifyDocument.AutoSize = true;
            this.cbPermitModifyDocument.Location = new System.Drawing.Point(111, 61);
            this.cbPermitModifyDocument.Name = "cbPermitModifyDocument";
            this.cbPermitModifyDocument.Size = new System.Drawing.Size(138, 17);
            this.cbPermitModifyDocument.TabIndex = 3;
            this.cbPermitModifyDocument.Text = "Permit modify document";
            this.cbPermitModifyDocument.UseVisualStyleBackColor = true;
            // 
            // lblEncryptionAlgorithm
            // 
            this.lblEncryptionAlgorithm.AutoSize = true;
            this.lblEncryptionAlgorithm.Location = new System.Drawing.Point(6, 27);
            this.lblEncryptionAlgorithm.Name = "lblEncryptionAlgorithm";
            this.lblEncryptionAlgorithm.Size = new System.Drawing.Size(105, 13);
            this.lblEncryptionAlgorithm.TabIndex = 26;
            this.lblEncryptionAlgorithm.Text = "Encryption algorithm:";
            // 
            // cmbxEncryptionAlgorithm
            // 
            this.cmbxEncryptionAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxEncryptionAlgorithm.FormattingEnabled = true;
            this.cmbxEncryptionAlgorithm.Items.AddRange(new object[] {
            "None",
            "RC4 40 bit",
            "RC4 Custom bit",
            "RC4 128 bit",
            "AES 128 bit",
            "AES weak 256 bit",
            "AES 256 bit"});
            this.cmbxEncryptionAlgorithm.Location = new System.Drawing.Point(117, 24);
            this.cmbxEncryptionAlgorithm.Name = "cmbxEncryptionAlgorithm";
            this.cmbxEncryptionAlgorithm.Size = new System.Drawing.Size(355, 21);
            this.cmbxEncryptionAlgorithm.TabIndex = 23;
            // 
            // cbProtectPDF
            // 
            this.cbProtectPDF.AutoSize = true;
            this.cbProtectPDF.Location = new System.Drawing.Point(12, 12);
            this.cbProtectPDF.Name = "cbProtectPDF";
            this.cbProtectPDF.Size = new System.Drawing.Size(90, 17);
            this.cbProtectPDF.TabIndex = 10;
            this.cbProtectPDF.Text = "Protect PDF?";
            this.cbProtectPDF.UseVisualStyleBackColor = true;
            this.cbProtectPDF.CheckedChanged += new System.EventHandler(this.cbProtectPDF_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(222, 300);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 30;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbProtectionSettings
            // 
            this.gbProtectionSettings.Controls.Add(this.lblEncryptionAlgorithm);
            this.gbProtectionSettings.Controls.Add(this.cmbxEncryptionAlgorithm);
            this.gbProtectionSettings.Controls.Add(this.gbUserPermissions);
            this.gbProtectionSettings.Controls.Add(this.lblOwnerPassword);
            this.gbProtectionSettings.Controls.Add(this.lblUserPassword);
            this.gbProtectionSettings.Controls.Add(this.tbUserPassword);
            this.gbProtectionSettings.Controls.Add(this.tbOwnerPassword);
            this.gbProtectionSettings.Location = new System.Drawing.Point(12, 35);
            this.gbProtectionSettings.Name = "gbProtectionSettings";
            this.gbProtectionSettings.Size = new System.Drawing.Size(482, 259);
            this.gbProtectionSettings.TabIndex = 31;
            this.gbProtectionSettings.TabStop = false;
            this.gbProtectionSettings.Text = "Protection settings";
            // 
            // cbPermitFullQualityPrint
            // 
            this.cbPermitFullQualityPrint.AutoSize = true;
            this.cbPermitFullQualityPrint.Location = new System.Drawing.Point(6, 19);
            this.cbPermitFullQualityPrint.Name = "cbPermitFullQualityPrint";
            this.cbPermitFullQualityPrint.Size = new System.Drawing.Size(127, 17);
            this.cbPermitFullQualityPrint.TabIndex = 35;
            this.cbPermitFullQualityPrint.Text = "Permit full quality print";
            this.cbPermitFullQualityPrint.UseVisualStyleBackColor = true;
            // 
            // cbPermitAssembleDocument
            // 
            this.cbPermitAssembleDocument.AutoSize = true;
            this.cbPermitAssembleDocument.Location = new System.Drawing.Point(6, 42);
            this.cbPermitAssembleDocument.Name = "cbPermitAssembleDocument";
            this.cbPermitAssembleDocument.Size = new System.Drawing.Size(152, 17);
            this.cbPermitAssembleDocument.TabIndex = 34;
            this.cbPermitAssembleDocument.Text = "Permit assemble document";
            this.cbPermitAssembleDocument.UseVisualStyleBackColor = true;
            // 
            // cbPermitAccessibilityExtractContent
            // 
            this.cbPermitAccessibilityExtractContent.AutoSize = true;
            this.cbPermitAccessibilityExtractContent.Location = new System.Drawing.Point(6, 65);
            this.cbPermitAccessibilityExtractContent.Name = "cbPermitAccessibilityExtractContent";
            this.cbPermitAccessibilityExtractContent.Size = new System.Drawing.Size(188, 17);
            this.cbPermitAccessibilityExtractContent.TabIndex = 33;
            this.cbPermitAccessibilityExtractContent.Text = "Permit accessibility extract content";
            this.cbPermitAccessibilityExtractContent.UseVisualStyleBackColor = true;
            // 
            // cbPermitFormsFill
            // 
            this.cbPermitFormsFill.AutoSize = true;
            this.cbPermitFormsFill.Location = new System.Drawing.Point(6, 88);
            this.cbPermitFormsFill.Name = "cbPermitFormsFill";
            this.cbPermitFormsFill.Size = new System.Drawing.Size(95, 17);
            this.cbPermitFormsFill.TabIndex = 32;
            this.cbPermitFormsFill.Text = "Permit forms fill";
            this.cbPermitFormsFill.UseVisualStyleBackColor = true;
            // 
            // gbUserPermissions128bitEncryption
            // 
            this.gbUserPermissions128bitEncryption.Controls.Add(this.cbPermitFormsFill);
            this.gbUserPermissions128bitEncryption.Controls.Add(this.cbPermitFullQualityPrint);
            this.gbUserPermissions128bitEncryption.Controls.Add(this.cbPermitAssembleDocument);
            this.gbUserPermissions128bitEncryption.Controls.Add(this.cbPermitAccessibilityExtractContent);
            this.gbUserPermissions128bitEncryption.Location = new System.Drawing.Point(255, 19);
            this.gbUserPermissions128bitEncryption.Name = "gbUserPermissions128bitEncryption";
            this.gbUserPermissions128bitEncryption.Size = new System.Drawing.Size(198, 113);
            this.gbUserPermissions128bitEncryption.TabIndex = 36;
            this.gbUserPermissions128bitEncryption.TabStop = false;
            this.gbUserPermissions128bitEncryption.Text = "For 128-bit encryption only";
            // 
            // FormProtectPDF
            // 
            this.AcceptButton = this.btnOK;
#if !TWCore
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
#endif
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 331);
            this.Controls.Add(this.gbProtectionSettings);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbProtectPDF);
            this.MaximizeBox = false;
            this.Name = "FormProtectPDF";
            this.Text = "PDF Protection Settings";
            this.gbUserPermissions.ResumeLayout(false);
            this.gbUserPermissions.PerformLayout();
            this.gbProtectionSettings.ResumeLayout(false);
            this.gbProtectionSettings.PerformLayout();
            this.gbUserPermissions128bitEncryption.ResumeLayout(false);
            this.gbUserPermissions128bitEncryption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOwnerPassword;
        private System.Windows.Forms.TextBox tbUserPassword;
        private System.Windows.Forms.TextBox tbOwnerPassword;
        private System.Windows.Forms.Label lblUserPassword;
        private System.Windows.Forms.GroupBox gbUserPermissions;
        private System.Windows.Forms.CheckBox cbPermitPrint;
        private System.Windows.Forms.CheckBox cbPermitAnnotations;
        private System.Windows.Forms.CheckBox cbPermitExtractContent;
        private System.Windows.Forms.CheckBox cbPermitModifyDocument;
        private System.Windows.Forms.Label lblEncryptionAlgorithm;
        private System.Windows.Forms.ComboBox cmbxEncryptionAlgorithm;
        private System.Windows.Forms.CheckBox cbProtectPDF;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnUserPermissionsToggleAll;
        private System.Windows.Forms.GroupBox gbProtectionSettings;
        private System.Windows.Forms.CheckBox cbPermitFormsFill;
        private System.Windows.Forms.CheckBox cbPermitAssembleDocument;
        private System.Windows.Forms.CheckBox cbPermitFullQualityPrint;
        private System.Windows.Forms.CheckBox cbPermitAccessibilityExtractContent;
        private System.Windows.Forms.GroupBox gbUserPermissions128bitEncryption;
    }
}