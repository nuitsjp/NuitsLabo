using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TwainScanning.Collectors;
using TwainScanning.Pdf.Security;

namespace TwainScanningTestCommon
{
    public partial class FormProtectPDF : Form
    {
        private bool permitAll = true; //Initial value controls default behavior

        public FormProtectPDF(PdfProtection protection)
        {
            InitializeComponent();
            cmbxEncryptionAlgorithm.DataSource = GetEncryptionAlgorithms();
            LoadProtectionSettings(protection);
            UpdateFormControls();
        }

        private void cbProtectPDF_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFormControls();
        }

        private void btnUserPermissionsToggleAll_Click(object sender, EventArgs e)
        {
            SetCheckBoxesChecked(gbUserPermissions, permitAll);
            SetCheckBoxesChecked(gbUserPermissions128bitEncryption, permitAll);
            ToggleUserPermissionsState();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool continueClosing = CheckProtectionConditions();
            if (!continueClosing)
                return;
            this.DialogResult = DialogResult.OK; //Sets result and closes dialog
        }

        private List<string> GetEncryptionAlgorithms()
        {
            var list = new List<string>();
            foreach (var name in Enum.GetNames(typeof(PdfDocumentSecurityLevel)))
            {
                list.Add(name);
            }
            return list;
        }

        private void SetCheckBoxesChecked(GroupBox groupBox, bool checkd)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is CheckBox)
                {
                    (control as CheckBox).Checked = checkd;
                }
            }
        }

        private void ToggleUserPermissionsState()
        {
            if (permitAll)
                btnUserPermissionsToggleAll.Text = "Permit NONE";
            else
                btnUserPermissionsToggleAll.Text = "Permit ALL";

            permitAll = !permitAll;
        }

        private void UpdateFormControls()
        {
            //Enable/disable controls as needed
            gbProtectionSettings.Enabled = cbProtectPDF.Checked;
        }

        private bool IsAnyPermissionChecked()
        {
            if (IsAnyPermissionChecked(gbUserPermissions))
                return true;
            
            if (IsAnyPermissionChecked(gbUserPermissions128bitEncryption))
                return true;
            else
                return false;
        }

        private bool IsAnyPermissionChecked(GroupBox groupBox)
        {
            foreach (var ctrl in groupBox.Controls)
            {
                if (!(ctrl is CheckBox))
                    continue;
                if ((ctrl as CheckBox).Checked)
                    return true;
            }

            return false;
        }

        private string SetSecurityLevel(PdfDocumentSecurityLevel securityLevel)
        {
            return securityLevel.ToString();
        }

        private void LoadProtectionSettings(PdfProtection protection)
        {
            cbProtectPDF.Checked = protection != null;

            if (protection == null)
            {
                //Set to default permissions
                btnUserPermissionsToggleAll_Click(null, null);
                return;
            }

            tbOwnerPassword.Text = protection.OwnerPassword;
            tbUserPassword.Text = protection.UserPassword;
            cbPermitAnnotations.Checked = protection.PermitAnnotations;
            cbPermitExtractContent.Checked = protection.PermitExtractContent;
            cbPermitModifyDocument.Checked = protection.PermitModifyDocument;
            cbPermitPrint.Checked = protection.PermitPrint;
            cmbxEncryptionAlgorithm.Text = SetSecurityLevel(protection.SecurityLevel);
            //Permissions below will only be applied if 128-bit encryption is used
            cbPermitFormsFill.Checked = protection.PermitFormsFill;
            cbPermitAccessibilityExtractContent.Checked = protection.PermitAccessibilityExtractContent;
            cbPermitAssembleDocument.Checked = protection.PermitAssembleDocument;
            cbPermitFullQualityPrint.Checked = protection.PermitFullQualityPrint;

            permitAll = IsAnyPermissionChecked();
            ToggleUserPermissionsState();
        }

        private PdfDocumentSecurityLevel GetSecurityLevel()
        {
            return (PdfDocumentSecurityLevel)Enum.Parse(typeof(PdfDocumentSecurityLevel), cmbxEncryptionAlgorithm.SelectedItem.ToString());
        }

        public PdfProtection GetProtectionSettings()
        {
            if (!cbProtectPDF.Checked)
                return null;

            var protection = new PdfProtection();

            protection.OwnerPassword = tbOwnerPassword.Text;
            protection.UserPassword = tbUserPassword.Text;
            protection.PermitAnnotations = cbPermitAnnotations.Checked;
            protection.PermitExtractContent = cbPermitExtractContent.Checked;
            protection.PermitModifyDocument = cbPermitModifyDocument.Checked;
            protection.PermitPrint = cbPermitPrint.Checked;
            protection.SecurityLevel = GetSecurityLevel();
            //Permissions below will only be applied if 128-bit encryption is used
            protection.PermitFormsFill = cbPermitFormsFill.Checked;
            protection.PermitAccessibilityExtractContent = cbPermitAccessibilityExtractContent.Checked;
            protection.PermitAssembleDocument = cbPermitAssembleDocument.Checked;
            protection.PermitFullQualityPrint = cbPermitFullQualityPrint.Checked;

            return protection;
        }

        private bool CheckProtectionConditions()
        {
            if (!cbProtectPDF.Checked)
                return true;

            var securityLevel = GetSecurityLevel();
            DialogResult choice;

            if (securityLevel != PdfDocumentSecurityLevel.None && string.IsNullOrEmpty(tbOwnerPassword.Text) && string.IsNullOrEmpty(tbUserPassword.Text))
            {
                MessageBox.Show("To protect a PDF, you need to set the owner or the user password.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (securityLevel != PdfDocumentSecurityLevel.None && string.IsNullOrEmpty(tbOwnerPassword.Text))
            {
                choice = MessageBox.Show("Owner password not set, will be set to same value as user password.", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (choice == DialogResult.Cancel)
                    return false;

                tbOwnerPassword.Text = tbUserPassword.Text;
            }

            if (securityLevel == PdfDocumentSecurityLevel.None && (!string.IsNullOrEmpty(tbOwnerPassword.Text) || !string.IsNullOrEmpty(tbUserPassword.Text)))
            {
                choice = MessageBox.Show("No encryption used, owner/user passwords will be ignored. PDF will not be protected.", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (choice == DialogResult.Cancel)
                    return false;
            }

            if (securityLevel == PdfDocumentSecurityLevel.None && string.IsNullOrEmpty(tbOwnerPassword.Text) && string.IsNullOrEmpty(tbUserPassword.Text))
            {
                choice = MessageBox.Show("No encryption nor password(s) used. PDF will not be protected.", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (choice == DialogResult.Cancel)
                    return false;
            }

            if (securityLevel != PdfDocumentSecurityLevel.Encrypted128Bit && IsAnyPermissionChecked(gbUserPermissions128bitEncryption))
            {
                choice = MessageBox.Show("Some permissions used require 128-bit encryption level. Those permissions will be ignored.", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (choice == DialogResult.Cancel)
                    return false;
            }

            return true;
        }

    }
}
