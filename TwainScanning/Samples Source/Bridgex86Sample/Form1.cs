using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TwainScanning;
using TwainScanning.Bridgex86;
using TwainScanning.Collectors;
using TwainScanningTestCommon;

namespace Bridgex86Sample
{
    public partial class Form1 : Form
    {
        private ConfigFile configFile = new ConfigFile();
        private PdfProtection pdfProtection = null;

        public Form1()
        {
            InitializeComponent();
            ConfigFile.SetConfigFromIni(configFile);
            bool isValidLicense = GlobalConfig.SetLicenseKey(configFile.Company, configFile.LicenseKey);
            SetInitialAcquirePath();
            DetectDefaultScanner();
            chkChangeScanSettings_CheckedChanged(this, null);
        }


        #region Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Text += (GlobalConfig.Is64BitProcess ? " 64-bit" : " 32-bit") + " ver. " + Application.ProductVersion;
        }

        private void tbDefaultScanner_DoubleClick(object sender, EventArgs e)
        {
            lbScanners.SelectedItem = tbDefaultScanner.Text;
        }

        private void lbScanners_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbScanners.SelectedItem != null)
            {
                string selectedScannerName = lbScanners.SelectedItem.ToString();
                FillScannerData(selectedScannerName);
            }
        }

        private void chkChangeScanSettings_CheckedChanged(object sender, EventArgs e)
        {
            grpScanSettings.Enabled = chkChangeScanSettings.Checked;
        }

        private void tbOutputFileName_TextChanged(object sender, EventArgs e)
        {
            if (tbOutputFileName.Text.Trim() != "")
            {
                string dirPath = Path.GetDirectoryName(tbOutputFileName.Text);
                if (!Directory.Exists(dirPath))
                    MessageBox.Show("Output directory doesn't exist!");
            }
        }

        private void btnSetOutputFileName_Click(object sender, EventArgs e)
        {
            ChangeAcquirePath(tbOutputFileName);
        }

        private void btnAvailableScanners_Click(object sender, EventArgs e)
        {
            DetectAvailableScanners();
        }

        private void btnProtectPDF_Click(object sender, EventArgs e)
        {
            SetPdfProtection();
        }

        private void btnAcquire_Click(object sender, EventArgs e)
        {
            AcquireImages(tbOutputFileName.Text, chkChangeScanSettings.Checked);
        }

        #endregion


        #region Methods
        private void SetInitialAcquirePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\twainBridgex86\";
            Directory.CreateDirectory(path);
            tbOutputFileName.Text = path + "img.jpg";
        }

        private void ChangeAcquirePath(TextBox tb)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                RestoreDirectory = true,
                InitialDirectory = Path.GetDirectoryName(tb.Text),
                Filter = GetSupportedFileFormats(),
                FilterIndex = 2, //1-based index for default extension
                AddExtension = true,
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tb.Text = saveFileDialog.FileName;
            }
        }

        private string GetSupportedFileFormats()
        {
            var formats = new string[]
            {
                "Bitmap (*.bmp)|*.bmp",
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg",
                "TIFF (*.tiff;*.tif)|*.tiff;*.tif",
                "PNG (*.png)|*.png",
                "PDF (*.pdf)|*.pdf"
            };
            string filter = string.Join("|", formats);
            return filter;
        }

        private void DetectDefaultScanner()
        {
            this.Cursor = Cursors.WaitCursor;

            var defaultScanner = Bridgex86.GetDefaultDevice();
            if (defaultScanner.Status != StatusType.OK)
            {
                this.Cursor = Cursors.Default;
                Bridgex86Helper.ShowBridgex86Messages(defaultScanner);
                //If default scanner not found then no other scanners exist either
                btnAcquire.Enabled = false;
                chkChangeScanSettings.Enabled = false;
                return;
            }
            tbDefaultScanner.Text = defaultScanner.Value;

            this.Cursor = Cursors.Default;

            FillScannerData(defaultScanner.Value);
        }

        private void DetectAvailableScanners()
        {
            this.Cursor = Cursors.WaitCursor;

            lbScanners.Items.Clear();
            var scanners = Bridgex86.GetAllDevices();
            if (scanners.Status != StatusType.OK)
            {
                this.Cursor = Cursors.Default;
                Bridgex86Helper.ShowBridgex86Messages(scanners);
                return;
            }
            foreach (var scanner in scanners.Value)
            {
                lbScanners.Items.Add(scanner);
            }

            this.Cursor = Cursors.Default;
        }

        private string GetScannerToUse()
        {
            //Note: Default scanner is used when no scanner specified or when specified explicitly

            if (lbScanners.Items.Count == 0)
            {
                return "";
            }
            else if (lbScanners.SelectedIndex < 0)
            {
                return "";
            }
            else
            {
                return lbScanners.SelectedItem.ToString();
            }
        }

        private void FillScannerData(string scanner)
        {
            this.Cursor = Cursors.WaitCursor;

            var currentSettingsResult = Bridgex86.GetCurrentDeviceSettings(scanner);
            if (currentSettingsResult.Status == StatusType.Error)
            {
                this.Cursor = Cursors.Default;
                Bridgex86Helper.ShowBridgex86Messages(currentSettingsResult);
                return;
            }

            //Note: Capabilities can be checked if supported by a given scanner before further actions are performed
            ResultBool isSupported;

            isSupported = Bridgex86.GetIsSupportedCapability("TransferMechanism", scanner);
            var tm = Bridgex86.GetSupportedTransferMechanisms(scanner);
            Bridgex86Helper.FillComboBox(cbxTransferMechanism, isSupported.Value, tm, currentSettingsResult.Value.TransferMechanism);

            isSupported = Bridgex86.GetIsSupportedCapability("PageSize", scanner);
            var ps = Bridgex86.GetSupportedPageSizes(scanner);
            Bridgex86Helper.FillComboBox(cbxPageSize, isSupported.Value, ps, currentSettingsResult.Value.PageSize);

            isSupported = Bridgex86.GetIsSupportedCapability("DuplexEnabled", scanner);
            var de = Bridgex86.GetSupportedDuplexEnabled(scanner);
            Bridgex86Helper.FillComboBox(cbxDuplexEnabled, isSupported.Value, de, currentSettingsResult.Value.DuplexEnabled);

            isSupported = Bridgex86.GetIsSupportedCapability("Resolution", scanner);
            var res = Bridgex86.GetSupportedResolutions(scanner);
            Bridgex86Helper.FillComboBox(cbxResolution, isSupported.Value, res, currentSettingsResult.Value.Resolution);

            isSupported = Bridgex86.GetIsSupportedCapability("AutoFeed", scanner);
            var af = Bridgex86.GetSupportedAutoFeeds(scanner);
            Bridgex86Helper.FillComboBox(cbxAutoFeed, isSupported.Value, af, currentSettingsResult.Value.AutoFeed);

            isSupported = Bridgex86.GetIsSupportedCapability("ColorMode", scanner);
            var cm = Bridgex86.GetSupportedColorModes(scanner);
            Bridgex86Helper.FillComboBox(cbxColorMode, isSupported.Value, cm, currentSettingsResult.Value.ColorMode);

            //Note: ImageQuality is a feature of the library and therefore doesn't need to be checked if supported
            //Note: For ImageQuality, possible values are always between 10-100
            Bridgex86Helper.FillComboBox(cbxImageQuality, true, 10, 100, 80);

            //Note: TiffImageQuality is a feature of the library and therefore doesn't need to be checked if supported
            var tiq = Bridgex86Helper.GetNamesFromEnum<System.Drawing.Imaging.EncoderValue>();
            Bridgex86Helper.FillComboBox(cbxTiffImageQuality, isSupported.Value, tiq, System.Drawing.Imaging.EncoderValue.CompressionLZW.ToString());

            isSupported = Bridgex86.GetIsSupportedCapability("Rotation", scanner);
            var rot = Bridgex86.GetSupportedRotations(scanner);
            Bridgex86Helper.FillComboBox(cbxRotation, isSupported.Value, rot, currentSettingsResult.Value.Rotation);

            isSupported = Bridgex86.GetIsSupportedCapability("AutomaticBorderDetection", scanner);
            var abd = Bridgex86.GetSupportedAutomaticBorderDetections(scanner);
            Bridgex86Helper.FillComboBox(cbxAutomaticBorderDetection, isSupported.Value, abd, currentSettingsResult.Value.AutomaticBorderDetection);

            isSupported = Bridgex86.GetIsSupportedCapability("Threshold", scanner);
            var thr = Bridgex86.GetSupportedThresholds(scanner);
            Bridgex86Helper.FillComboBox(cbxThreshold, isSupported.Value, thr, currentSettingsResult.Value.Threshold);

            isSupported = Bridgex86.GetIsSupportedCapability("Contrast", scanner);
            var con = Bridgex86.GetSupportedContrasts(scanner);
            Bridgex86Helper.FillComboBox(cbxContrast, isSupported.Value, con, currentSettingsResult.Value.Contrast);

            isSupported = Bridgex86.GetIsSupportedCapability("Brightness", scanner);
            var bri = Bridgex86.GetSupportedBrightnesses(scanner);
            Bridgex86Helper.FillComboBox(cbxBrightness, isSupported.Value, bri, currentSettingsResult.Value.Brightness);

            //Note: ImageCount must be supported by all scanners, example is therefore redundant but still useful
            isSupported = Bridgex86.GetIsSupportedCapability("ImageCount", scanner);
            var ic = Bridgex86.GetSupportedImageCount(scanner);
            //Note: Allow -1 for minimum value because it indicates acquiring all available images, maximum can basically be any value
            Bridgex86Helper.FillComboBox(cbxImageCount, isSupported.Value, -1, 100, -1); //Always start by selecting to acquire all available images (-1)

            isSupported = Bridgex86.GetIsSupportedCapability("IgnoreBlankPages", scanner);
            var ibp = Bridgex86.GetSupportedIgnoreBlankPages(scanner);
            Bridgex86Helper.FillComboBox(cbxIgnoreBlankPages, isSupported.Value, ibp, currentSettingsResult.Value.IgnoreBlankPages);

            //Note: OutputFileNamesList is a feature of the library and therefore doesn't need to be checked if supported
            //Note: For OutputFileNamesList, possible values are strings with absolute file paths (added as list entries)
            //Note: This application showcases the usage by generating the specified number of file paths: 0 (none, don't use) or greater (use, how many)
            Bridgex86Helper.FillComboBox(cbxOutputFileNamesListCount, true, 0, 100, 0); //Always start by selecting to not use it (0)

            this.Cursor = Cursors.Default;
        }

        private ScanSettings GetUserScanSettings()
        {
            var settings = new ScanSettings();

            settings.Device = GetScannerToUse();
            settings.ShowUI = Bridgex86Helper.GetValueAsBool(chkShowUI);
            settings.CloseUIAfterAcquire = Bridgex86Helper.GetValueAsBool(chkCloseUIAfterAcquire);
            settings.MultiPageScan = Bridgex86Helper.GetValueAsBool(chkMultiPageScan);
            settings.TransferMechanism = Bridgex86Helper.GetValueAsEnum<TwainScanning.NativeStructs.TwSX>(cbxTransferMechanism, TwainScanning.NativeStructs.TwSX.Native);
            settings.PageSize = Bridgex86Helper.GetValueAsEnum<TwainScanning.NativeStructs.TwSS>(cbxPageSize);
            settings.DuplexEnabled = Bridgex86Helper.GetValueAsBool(cbxDuplexEnabled);
            settings.Resolution = Bridgex86Helper.GetValueAsScanResolution(cbxResolution); //Note: Resolution can also have independent values for X and Y
            settings.AutoFeed = Bridgex86Helper.GetValueAsBool(cbxAutoFeed);
            settings.ColorMode = Bridgex86Helper.GetValueAsEnum<TwainScanning.NativeStructs.TwPixelType>(cbxColorMode);
            settings.ImageQuality = Bridgex86Helper.GetValueAsInt(cbxImageQuality);
            settings.TiffImageQuality = Bridgex86Helper.GetValueAsEnum<System.Drawing.Imaging.EncoderValue>(cbxTiffImageQuality);
            settings.Rotation = Bridgex86Helper.GetValueAsFloat(cbxRotation);
            settings.AutomaticBorderDetection = Bridgex86Helper.GetValueAsBool(cbxAutomaticBorderDetection);
            settings.Threshold = Bridgex86Helper.GetValueAsFloat(cbxThreshold);
            settings.Contrast = Bridgex86Helper.GetValueAsFloat(cbxContrast);
            settings.Brightness = Bridgex86Helper.GetValueAsFloat(cbxBrightness);
            settings.ImageCount = Bridgex86Helper.GetValueAsShort(cbxImageCount);
            settings.IgnoreBlankPages = Bridgex86Helper.GetValueAsEnum<TwainScanning.NativeStructs.TwBP>(cbxIgnoreBlankPages);
            settings.PdfProtection = GetPdfProtectionSettingsToUse();
            settings.OutputFileNamesList = GetOutputFileNamesList(Bridgex86Helper.GetValueAsInt(cbxOutputFileNamesListCount), tbOutputFileName.Text);

            return settings;
        }

        private void SetPdfProtection()
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

        private PdfProtection GetPdfProtectionSettingsToUse()
        {
            return pdfProtection;
        }

        private List<string> GetOutputFileNamesList(int? count, string sourceFileName)
        {
            //Note: The file names can be specified to fit any required use case or pattern, but they must have an absolute path
            //Note: Always ensure at least as many file names as there are items to scan, having more than that is not a problem, but having less is a problem

            //Prepare file names based on the currently selected file name and extension, and add a zero-padded sequential number suffix

            if ((count.HasValue && count < 1) || string.IsNullOrEmpty(sourceFileName))
            {
                return new List<string>();
            }

            string path = Path.GetDirectoryName(sourceFileName);
            string filename = Path.GetFileNameWithoutExtension(sourceFileName);
            string extension = Path.GetExtension(sourceFileName);
            int digits = count.ToString().Length + 1; //Ensure a leading zero is always present
            
            var list = new List<string>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(
                    Path.Combine(path, filename + i.ToString("D" + digits) + extension)
                    );
            }
            return list;
        }

        private void AcquireImages(string outputFileName, bool changeScanSettings)
        {
            this.Cursor = Cursors.WaitCursor;

            ResultNoValue result;
            if (changeScanSettings)
            {
                var settings = GetUserScanSettings();
                result = Bridgex86.Acquire(outputFileName, settings);
            }
            else
            {
                result = Bridgex86.Acquire(outputFileName);
            }

            this.Cursor = Cursors.Default;

            Bridgex86Helper.ShowBridgex86Messages(result);
        }

        #endregion

    }
}
