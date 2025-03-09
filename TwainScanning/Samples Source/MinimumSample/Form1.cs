using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TwainScanning;
using TwainScanning.Collectors;
using TwainScanning.NativeStructs;

namespace MinimumSample
{
    public partial class Simple : Form
    {
        private ConfigFile _configFile = new ConfigFile();
        private TwainScanning.DataSourceManager _asyncDSM = null;
        private TwainScanning.DataSource _asyncDS = null;
        public Simple()
        {
            InitializeComponent();

            tbPath.Text= SetPathAndName().ToString();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            ConfigFile.SetConfigFromIni(_configFile);
            bool isValid = TwainScanning.GlobalConfig.SetLicenseKey(_configFile.Company, _configFile.LicenseKey);
            TwainScanning.GlobalConfig.Force64BitDriver = false; //False is default, set to true to use 64-bit device drivers in 64-bit applications

            OpenDSM();//Open data source manager method.

        }
        private FileInfo SetPathAndName()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return MakeUnique(path + "\\scan");
        }
        public FileInfo MakeUnique(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileExt = Path.GetExtension(path);

            for (int i = 1; ; ++i)
            {
                if (!File.Exists(path))
                    return new FileInfo(path);

                path = Path.Combine(dir, fileName + " " + i + fileExt);
            }
        }
        private void OpenDSM()
        {
            TwainScanning.AppInfo info = new TwainScanning.AppInfo();//Create instance of AppInfo
            info.name = "Terminal";//Set app name
            info.manufacturer = "terminalworks";//Set manufacture name
            _asyncDSM = new TwainScanning.DataSourceManager(this.Handle, info); //Open DataSourceManager
        }
        private void Simple_Load(object sender, EventArgs e)
        {
            Text += (GlobalConfig.Is64BitProcess ? " 64-bit" : " 32-bit") + " ver. " + Application.ProductVersion;
        }
        private void btnOpenSource_Click(object sender, EventArgs e)
        {
            try
            {
                CloseDs();
                TwIdentity scanner =  _asyncDSM.SelectDefaultSourceDlg();//Call twain Source dialog.
                if (scanner.Id == 0)
                {
                    MessageBox.Show("Please select scanner");
                }
                _asyncDS = _asyncDSM.OpenSource(scanner);
            }
            catch (BadRcTwainException ex)
            {
                MessageBox.Show("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
            }
        }

        private void btnAcquire_Click(object sender, EventArgs e)
        {
            try
            {
                if (_asyncDS == null)
                {
                    MessageBox.Show("DS closed, please choose Data source");
                    return;
                }

                ImageCollector col = new ImageCollector();
                btnAcquire.Enabled = false;
                bool showUI = false;

                if (string.IsNullOrEmpty(tbPath.Text))//If device name is null or empty return.
                {
                    btnAcquire.Enabled = true;
                    MessageBox.Show("Add save path");
                    return;
                }
 
                _asyncDS.OnSingleImageAcquired += _asyncDS_ImageAcquired;//Event image acquired.
                _asyncDS.AcquireAsync(asyncDS_OnScanFinished, showUI, false, TwainScanning.NativeStructs.TwSX.Native, -1);//Acquire image async.       
 
            }
            catch (TwainScanning.BadRcTwainException ex)
            {
                btnAcquire.Enabled = true;
                MessageBox.Show("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
            }

            
            btnAcquire.Enabled = true;
        }
     
        private void CloseDs()
        {
            if (_asyncDS != null)
            {
                _asyncDS.Dispose();
                _asyncDS = null;
            }
        }

        void asyncDS_OnScanFinished(ImageCollector collector)
        {
            btnAcquire.Enabled = true;
            if (string.IsNullOrEmpty(tbPath.Text))
                return;//If device name is null or empty return.
            if (collector != null)
            {
                string pathPDF = "";
                string pathTIFF = "";
                int i = 0;
                do
                {
                    string dir = Path.GetDirectoryName(tbPath.Text);
                    string noExtension = Path.GetFileNameWithoutExtension(tbPath.Text);
                    pathPDF = dir + "\\" + noExtension + i + ".pdf";
                    pathTIFF = dir + "\\" + noExtension + i + ".tiff";
                    i++;
                }
                while (File.Exists(pathPDF));
                collector.SaveAllToMultipagePdf(pathPDF);
                collector.SaveAllToMultipageTiff(pathTIFF);
                collector.Dispose();
            }
            
        }

        private void _asyncDS_ImageAcquired(object sender, DataSource.SingleImageAcquiredEventArgs e)
        {
            var imgOld = pictureBox1.Image;
            if (e.Image == null)//Check if image is null
                return;
            Bitmap img = new Bitmap((Image)e.Image);//scat image to bitmap type
            pictureBox1.Image = img;//Set image to picturebox
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;//Set image zoom 
            pictureBox1.Refresh();
            if (imgOld != null)
                imgOld.Dispose();
        }

        private void btnImgSavePath_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = saveFileDialog1.FileName;
            }
        }


    }
}
