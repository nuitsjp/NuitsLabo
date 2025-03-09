using AdvanceSample;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TwainScanning;
using TwainScanning.Capability;
using TwainScanning.Collectors;
using TwainScanning.ExtendedImageInfo;
using TwainScanning.NativeStructs;
using static AdvanceSample.AcquireProcess;
using TwainScanningTestCommon;

namespace TwainDllTest
{

    public partial class AdvanceSampleForm : Form
    {
        private ConfigFile configFile = new ConfigFile();
        private bool _updating = false;
        private AcquireProcess acquireProcess;
        private PdfProtection pdfProtection = null;

        public AdvanceSampleForm()
        {
            InitializeComponent();

            ConfigFile.SetConfigFromIni(configFile);
            bool isValidLicense = GlobalConfig.SetLicenseKey(configFile.Company, configFile.LicenseKey);
            GlobalConfig.Force64BitDriver = false; //False is default, set to true to use 64-bit device drivers in 64-bit applications
            //Subscribe on events
            acquireProcess = new AcquireProcess(this);
            acquireProcess.OnErrors += AcquireProcess_OnErrors;
            acquireProcess.OnImageAcquired += AcquireProcess_ImageAcquired;
            acquireProcess.OnProgressUpdate += AcquireProcess_UpdateProgress;
            acquireProcess.OnScanFinish += AcquireProcess_ScanFinish;
            acquireProcess.OnExtendedImageInformation += AcquireProcess_OnExtendedImageInformation;
            lbSources.SelectedIndex = -1;
            foreach (var id in acquireProcess.Devices)
                lbSources.Items.Add(id.ProductName);
            try
            {
                tbDefSource.Text = acquireProcess.DefaultDevice.ProductName;
            }
            catch (BadRcTwainException ex)
            {
                MessageBox.Show("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\twainImg\";
            System.IO.Directory.CreateDirectory(path);
            tbImgPath.Text = path + "img.bmp";
            tbFileTransfer.Text = path + "imgFileTransfer.bmp";
            tbMPPDF.Text = path + "multipagePDF.pdf";
            tbMPTiff.Text = path + "multipageTIFF.tiff";

            UpdateFormForCurrentDS();
        }

        private void AdvanceSampleForm_Load(object sender, EventArgs e)
        {
            Text += (GlobalConfig.Is64BitProcess ? " 64-bit" : " 32-bit") + " ver. " + Application.ProductVersion;
        }

        private void AcquireProcess_OnExtendedImageInformation(ExtendedImageInformations info)
        {
         
        }

        private void UpdateFormForCurrentDS()
        {
            bool enableForm = acquireProcess.IsDsOpened;
            foreach (Control control in this.Controls)
            {
                if (control == lbSources) continue;
                if (control == lblErrorMsg) continue;
                if (control == btnCancel) continue;
                control.Enabled = enableForm;
            }
        }

        private void lbSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSources.SelectedItem != null)
            {
                string deviceName = lbSources.SelectedItem.ToString();
                acquireProcess.OpenDevice(deviceName);
                UpdateFormForCurrentDS();
                FillScannerData();
            }
        }

        private void FillScannerData()
        {
            _updating = true;

            //Get all capability
            List<ICapabilityObjBase> allCaps = acquireProcess.AllCapabilities;
            Dictionary<Type, bool> tt = new Dictionary<Type, bool>();
            foreach (var c in allCaps)
                tt[c.UnderlyingType] = false;

            capability.Sorted = true;
            capability.DataSource = allCaps;
            capability.DisplayMember = "Cap";
            if (allCaps.Count < 1) { capValue.DataSource = null; }
            FillCapValueList();

            lbTransferMechanism.DataSource = acquireProcess.TransfersMechanisms;
            cbxPixelType.DataSource = acquireProcess.PixelTypes;
            cbxResolution.DataSource = acquireProcess.Resolutions;
            comboFileTransferFormat.DataSource = acquireProcess.FileTransferFormats;
            cbxPageSize.DataSource = acquireProcess.PageSizes;

            _updating = false;

            UpdateSelections();
        }

        void FillCapValueList()
        {
            ICapabilityObjBase cap = acquireProcess.CurrentCapability as ICapabilityObjBase;

            if (cap == null)
            {
                capValue.DataSource = new List<object>();
            }
            else
            {
                capValue.DataSource = new List<object>(cap.GetSupportedValuesObj());
            }
        }

        private void SetSelectionAndEnable<T>(T? prop, ComboBox c) where T : struct
        {
            c.Enabled = prop.HasValue;
            if (prop.HasValue)
                c.SelectedItem = prop.Value;
            else
                c.SelectedItem = null;
        }

        private void SetSelectionAndEnable<T>(T? prop, ListBox c) where T : struct
        {
            c.Enabled = prop.HasValue;
            if (prop.HasValue)
                c.SelectedItem = prop.Value;
            else
                c.SelectedItem = null;
        }

        private void SetCheckedAndEnable(bool? prop, CheckBox c)
        {
            c.Enabled = prop.HasValue;
            if (prop.HasValue)
                c.Checked = prop.Value;
            else
                c.Checked = false;
        }

        private void UpdateSelections()
        {

            if (!acquireProcess.IsDsOpened)
                return;

            _updating = true;

            lblErrorMsg.Text = "";

            SetSelectionAndEnable(acquireProcess.TransferMechanism, lbTransferMechanism);
            SetSelectionAndEnable(acquireProcess.PixelType, cbxPixelType);
            SetSelectionAndEnable(acquireProcess.Resolution, cbxResolution);
            SetSelectionAndEnable(acquireProcess.FileTransferFormat, comboFileTransferFormat);
            SetSelectionAndEnable(acquireProcess.PageSize, cbxPageSize);

            SetCheckedAndEnable(acquireProcess.Duplex, cbDuplex);
            SetCheckedAndEnable(acquireProcess.Feeder, cbADF);

            SetEnabledByTransferMethod();
            SetSelectionOnCapValueList();

            _updating = false;
        }

        private void SetEnabledByTransferMethod()
        {
            bool stateFile = ((TwSX)lbTransferMechanism.SelectedItem == TwSX.File) || ((TwSX)lbTransferMechanism.SelectedItem == TwSX.MemFile);
            bool stateMemory = ((TwSX)lbTransferMechanism.SelectedItem == TwSX.Memory) || ((TwSX)lbTransferMechanism.SelectedItem == TwSX.Native);

            cbMultiPagePDF.Enabled = stateMemory;
            cbMultiPageTiff.Enabled = stateMemory;
            tbMPPDF.Enabled = stateMemory;
            tbMPTiff.Enabled = stateMemory;
            tbImgPath.Enabled = stateMemory;
            lblImgSavePath.Enabled = stateMemory;
            btnPDFPath.Enabled = stateMemory;
            btnTiff.Enabled = stateMemory;
            btnImgSavePath.Enabled = stateMemory;
            btnProtectPDF.Enabled = stateMemory;

            tbFileTransfer.Enabled = stateFile;
            comboFileTransferFormat.Enabled = stateFile;
            lblFileTrans.Enabled = stateFile;
            lblFileFormat.Enabled = stateFile;
            btnFilePath.Enabled = stateFile;
        }

        private void SavePath(TextBox tb)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tb.Text = saveFileDialog1.FileName;
            }
        }

        private void btnPDFPath_Click(object sender, EventArgs e)
        {
            SavePath(tbMPPDF);
        }

        private void lbTransferMechanism_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.TransferMechanism = (TwSX)lbTransferMechanism.SelectedItem;
            UpdateSelections();
        }

        private void cbxPixelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.PixelType = (TwPixelType)cbxPixelType.SelectedItem;
            UpdateSelections();
        }

        private void cbxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.Resolution = (float)cbxResolution.SelectedItem;
            UpdateSelections();
        }

        private void comboFileTransferFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.FileTransferFormat = (TwFF)comboFileTransferFormat.SelectedItem;
            UpdateSelections();
        }

        private void cbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.PageSize = (TwSS)cbxPageSize.SelectedItem;
            UpdateSelections();
        }

        private void cbADF_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.Feeder = cbADF.Checked;
            UpdateSelections();
        }

        private void cbDuplex_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.Duplex = cbDuplex.Checked;
            UpdateSelections();
        }

        private void btnTiff_Click(object sender, EventArgs e)
        {
            SavePath(tbMPTiff);
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            SavePath(tbFileTransfer);

        }

        private void btnImgSavePath_Click(object sender, EventArgs e)
        {
            SavePath(tbImgPath);
        }

        private void capability_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            acquireProcess.CurrentCapability = capability.SelectedItem as ICapabilityObjBase;

            _updating = true;
            FillCapValueList();
            SetSelectionOnCapValueList();
            _updating = false;
        }

        void SetSelectionOnCapValueList()
        {
            ICapabilityObjBase cap = acquireProcess.CurrentCapability as ICapabilityObjBase;

            //   capValue.SelectionMode = SelectionMode.None;
            //   capValue.SelectedItems.Clear();

            if (cap == null || cap.IsReadOnly)
                return;

            capValue.SelectionMode = cap.IsMultiVal ? SelectionMode.MultiExtended : SelectionMode.One;

            foreach (object o in cap.GetCurrentValuesObj())
                capValue.SelectedItems.Add(o);

        }

        private void capValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            ICapabilityObjBase selCap = acquireProcess.CurrentCapability as ICapabilityObjBase;

            if (selCap == null)
                return;

            var x = new List<object>();
            foreach (object o in capValue.SelectedItems)
                x.Add(o);

            selCap.SetCurrentValuesObj(x);
            UpdateSelections();
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            if (!acquireProcess.IsDsOpened)
            {
                MessageBox.Show("DataSource is not opened!");
                return;
            }

            int numberOfScans = -1;
            if(!int.TryParse(numberOfScansBox.Text, out numberOfScans))
            {
                numberOfScans = -1;
            }
            
            acquireProcess.CancelScanning = false;
            btnAsync.Enabled = false;
            btnCancel.Enabled = !btnAsync.Enabled;
            acquireProcess.Acquire(cbMultiPageTiff.Checked ? tbMPTiff.Text : null,
                cbMultiPagePDF.Checked ? tbMPPDF.Text : null,
                pdfProtection,
                tbFileTransfer.Text,
                cbADF.Checked,
                cbDuplex.Checked,
                (TwSX)lbTransferMechanism.SelectedItem,
                cbCloseDlgAfterScan.Checked,
                cbUI.Checked,
                numberOfScans);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if ((TwSX)lbTransferMechanism.SelectedItem == TwSX.Native || (TwSX)lbTransferMechanism.SelectedItem == TwSX.Memory)
            {
                acquireProcess.CancelScanning = true;
                btnAsync.Enabled = true;
                btnCancel.Enabled = !btnAsync.Enabled;
            }
            else
            {
                MessageBox.Show("Scan cancel only supported for Native and Memory transfer mechanisms!\nCurrent transfer mechanism: " + lbTransferMechanism.SelectedItem);
            }
        }

        private void AcquireProcess_ScanFinish(CustomMultiCollector c)
        {
            MessageBox.Show("Scanning finished");
            btnAsync.Enabled = true;
            btnCancel.Enabled = !btnAsync.Enabled;
            if (c != null)
                c.Dispose();

            GC.Collect();
        }

        private void AcquireProcess_ImageAcquired(Bitmap image)
        {
            Image oldImg = pictureBox.Image;
            pictureBox.Image = null;
            if (oldImg != null)
                oldImg.Dispose();

            float scale = Math.Min( (float)pictureBox.Width / image.Width, (float)pictureBox.Height / image.Height);

            Bitmap img = new Bitmap(image, new Size((int)(image.Width * scale), (int)(image.Height * scale) ) );// image.Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
            pictureBox.Image = img;
            pictureBox.Refresh();
            image.Save(tbImgPath.Text.ToString());
        }

        //Event will be triggered only on memory transfer.
        private void AcquireProcess_UpdateProgress(int progress)
        {
            progressBar1.Enabled = true;
            progressBar1.Value = progress;
        }

        private void AcquireProcess_OnErrors(string err)
        {
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Text = err.ToString();
            if (err.ToString().Equals("It worked!"))
            {
                btnAsync.Enabled = true;
                btnCancel.Enabled = !btnAsync.Enabled;
            }
        }

        //Set value to supported capability.
        private void setCapValue_Click(object sender, EventArgs e)
        {
            var cap = acquireProcess.CurrentCapability as ICapabilityObjBase;
            if (cap == null)
                return;
            if (capValueToSet.Text == "")
                return;
            cap.SetCurrentValuesStr(new string[] { capValueToSet.Text });
            capValueToSet.Text = "";
            UpdateSelections();
            _updating = true;
            FillCapValueList();
            SetSelectionOnCapValueList();
            _updating = false;
        }

        private void tbMPTiff_TextChanged(object sender, EventArgs e)
        {
            if (tbMPTiff.Text.Trim() != "")
            {
                string dirPath = Path.GetDirectoryName(tbMPTiff.Text);
                if (!Directory.Exists(dirPath))
                    WrongDirPath("Directory doesn't exist (*Textbox Tiff*)!!!");
                else
                    WrongDirPath("");
            }
        }

        private void tbImgPath_TextChanged(object sender, EventArgs e)
        {
            if (tbImgPath.Text.Trim() != "")
            {
                string dirPath = Path.GetDirectoryName(tbImgPath.Text);
                if (!Directory.Exists(dirPath))
                    WrongDirPath("Directory doesn't exist(*TextBox ImgPath*)!!!");
                else
                    WrongDirPath("");
            }
        }

        private void tbMPPDF_TextChanged(object sender, EventArgs e)
        {
            if (tbMPPDF.Text.Trim() != "")
            {
                string dirPath = Path.GetDirectoryName(tbMPPDF.Text);
                if (!Directory.Exists(dirPath))
                    WrongDirPath("Directory doesn't exist ( * TextBox PDF* ) !!!");
                else
                    WrongDirPath("");
            }
        }

        private void tbFileTransfer_TextChanged(object sender, EventArgs e)
        {

            if (tbFileTransfer.Text.Trim() != "")
            {
                string dirPath = Path.GetDirectoryName(tbFileTransfer.Text);
                if (!Directory.Exists(dirPath))
                    WrongDirPath("Directory doesn't exist ( *TextBox FileTransfer* )!!!");
                else
                    WrongDirPath("");
            }
        }

        private void WrongDirPath(string msg)
        {
            lblErrorMsg.ForeColor = Color.Red;
            lblErrorMsg.Text = msg;
        }

        private void AdvanceSampleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            acquireProcess.Dispose();
        }

        private void btnProtectPDF_Click(object sender, EventArgs e)
        {
            var frmProtection = new FormProtectPDF(pdfProtection);
            frmProtection.StartPosition = FormStartPosition.CenterParent;
            var result = frmProtection.ShowDialog();
            if (result == DialogResult.OK)
            {
                pdfProtection = frmProtection.GetProtectionSettings();
            }
            frmProtection.Dispose();
        }

    }
}



