using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using System.Text;
using System.Windows.Forms;
using TwainScanning;
using TwainScanning.Capability;
using TwainScanning.Collectors;
using TwainScanning.ExtendedImageInfo;
using TwainScanning.NativeStructs;

namespace AdvanceSample
{
    public class CustomMultiCollector : ImageMultiCollector
    {
        Dictionary<string, IImageCollector> _map = new Dictionary<string, IImageCollector>();
        public void AddNamed(string name, IImageCollector col)
        {
            _map[name] = col;
            AddCollector(col);
        }
        public IImageCollector GetNamed(string name)
        {
            try
            {
                return _map[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }


    public class AcquireProcess
    {

        DataSourceManager _asyncDSM = null;
        DataSource _asyncDS = null;


        public delegate void ScanFinished(CustomMultiCollector collector);
        public event ScanFinished OnScanFinish;

        public delegate void ProgressUpdate(int percentage);
        public event ProgressUpdate OnProgressUpdate;

        public delegate void ImageAcquired(Bitmap Image);
        public event ImageAcquired OnImageAcquired;

        public delegate void ErrorHandler(string error);
        public event ErrorHandler OnErrors;

        public delegate void ExtendedImageInformationsAcquired (ExtendedImageInformations info);
        public event ExtendedImageInformationsAcquired OnExtendedImageInformation;

        public bool CancelScanning { get; set; }

        public AcquireProcess(Form frm)
        {
            AppInfo info = new AppInfo();
            info.name = "Terminal";//Set app name
            info.manufacturer = "terminalworks";//Set manufacturer name

            //Open DataSourceManager 
            _asyncDSM = new DataSourceManager(frm, info);

            CancelScanning = false;
        }
        //Open twain device
        public void OpenDevice(string deviceName)
        {
            if (_asyncDS != null)
            {
                _asyncDS.Dispose();//Dispose _asyncDS 
                _asyncDS = null;//Set _asyncDS to null value
            }

            try
            {
                //Open Device
                _asyncDS = _asyncDSM.OpenSource(deviceName);

                //Subscribe to events
                _asyncDS.OnScanningFinished += _asyncDS_ScanningFinished;
                _asyncDS.OnSingleImageAcquired += AsyncDS_ImageAcquired;
                _asyncDS.OnBatchFinished += AsyncDS_AllImagesAcquired;
                _asyncDS.OnMemoryTransferProgressUpdate += AsyncDS_MemoryProgressUpdate;
                _asyncDS.OnErrorEvent += _asyncDS_ErrorEvent;
               
              
                _asyncDS.ExtendedImageInformation += _asyncDS_ExtendedImageInformation;
            }
            catch (TwainException ex)
            {
                if (OnErrors != null)
                    OnErrors(ex.Message);
            }

        }

        private void _asyncDS_ExtendedImageInformation(object sender, DataSource.ExtImgInfoEventArgs e)
        {
            
            OnExtendedImageInformation(e.Info);
         
        }
 

        public bool IsDsOpened { get { return _asyncDS != null; } }
        public void Acquire(string tiffPath, 
            string pdfPath, 
            PdfProtection pdfProtection, 
            string fileTransferPath,
            bool ADF, 
            bool duplex, 
            TwSX transferMechanism, 
            bool closeUIAfterAcquire,
            bool ShowUI, 
            int? count)
        {

            if (_asyncDS == null)
                return;
 
            CustomMultiCollector multiCollector = null;

            switch (transferMechanism)
            {
                case TwSX.Memory:
                case TwSX.Native:
                    {
                        multiCollector = new CustomMultiCollector();
                        if (tiffPath != null)
                            multiCollector.AddNamed("tiffCollector", new ImageCollectorTiffMultipage(tiffPath));

                        if (pdfPath != null)
                            multiCollector.AddNamed("pdfCollector", new ImageCollectorPdf(filePath: pdfPath, protection: pdfProtection));
                    }
                    break;
                case TwSX.File:
                case TwSX.MemFile:
                    {
                        TwFF ff = _asyncDS.Settings.Transfer.ImageFileFormat.Value;
                        _asyncDS.SetupFileTransferParams( ff, fileTransferPath, ff==TwFF.TiffMulti);
                    }
                    break;
            }
            _asyncDS.Settings.Feeder.Enabled.Value = ADF;
            _asyncDS.Settings.Feeder.Autofeed.Value = ADF;
            _asyncDS.Settings.Duplex.Enabled.Value = duplex;
            try
            {
                 _asyncDS.AcquireAsync(multiCollector, 
                     ShowUI, 
                     closeUIAfterAcquire, 
                     transferMechanism, 
                     count);
            }
            catch (BadRcTwainException ex)
            {
                MessageBox.Show("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
            }
        }

        public bool? ClearPage
        {
            get { return GetValueFromProxy<bool>(_asyncDS.ClearPage); }
            set { SetValueToProxy<bool>(_asyncDS.ClearPage, value); }
        }

        #region DSEvent's
        // Event will be triggered only on memory transfer.
        private void AsyncDS_MemoryProgressUpdate(object sender, DataSource.MemoryTransferProgressUpdateEventArgs e)
        {
            if (OnProgressUpdate != null)
                OnProgressUpdate(e.Progress);
        }

        private void AsyncDS_AllImagesAcquired(object sender, DataSource.BatchFinishedEventArgs e)
        {

        }
        private void AsyncDS_ImageAcquired(object sender, DataSource.SingleImageAcquiredEventArgs e)
        {
            if (OnImageAcquired != null)
                OnImageAcquired(e.Image);
            
            e.CancelScanning = this.CancelScanning;
        }
        private void _asyncDS_ScanningFinished(object sender, DataSource.ScanningFinishedEventArgs e)
        {
            if (OnScanFinish != null)
                OnScanFinish(e.Collector as CustomMultiCollector);
        }

        private void _asyncDS_ErrorEvent(object sender, DataSource.ErrorEventArgs e)
        {
            if (OnErrors != null)
                OnErrors(e.Info.Message);
        }

        #endregion
        //Get all available sources. 
        public List<TwIdentity> Devices
        {
            get { return _asyncDSM.AvailableSources(); }
        }
        //Get default twain device
        public TwIdentity DefaultDevice
        {
            get
            {

                return _asyncDSM.DefaultSource();
            }
        }

        //Return list of all suported capabilities for current device.
        public List<ICapabilityObjBase> AllCapabilities
        {
            get
            {
                return (_asyncDS != null) ?
                    new List<ICapabilityObjBase>(_asyncDS.Settings.GetSupportedCapabilities()) :
                    new List<ICapabilityObjBase>();
            }
        }

        public ICapabilityObjBase CurrentCapability { get; set; }

        //Get value from supported capability
        private Nullable<T> GetValueFromProxy<T>(CapabilityProxy<T> capProxy) where T : struct
        {
            if (!capProxy.IsSupportedOnThisDevice())
                return null;
            return new Nullable<T>(capProxy.Value);
        }
        private void SetValueToProxy<T>(CapabilityProxy<T> capProxy, T? value) where T : struct
        {
            if (!capProxy.IsSupportedOnThisDevice())
                return;
            if (value.HasValue)
                capProxy.Value = value.Value;
        }
        /// <summary>
        /// Property TransferMechanism Get{}, Set{}.
        /// </summary>
        public TwSX? TransferMechanism
        {
            get { return GetValueFromProxy<TwSX>(_asyncDS.TransferMechanism); }
            set { SetValueToProxy<TwSX>(_asyncDS.TransferMechanism, value); }
        }
        /// <summary>
        /// Property PixelType  Get{}, Set{}.
        /// </summary>
        public TwPixelType? PixelType
        {
            get { return GetValueFromProxy<TwPixelType>(_asyncDS.ColorMode); }
            set { SetValueToProxy<TwPixelType>(_asyncDS.ColorMode, value); }
        }
        /// <summary>
        /// Property Duplex Get{}, Set{}.
        /// </summary>
        public bool? Duplex
        {
            get { return GetValueFromProxy<bool>(_asyncDS.UseDuplex); }
            set { SetValueToProxy<bool>(_asyncDS.UseDuplex, value); }
        }
        /// <summary>
        /// Property Feeder Get{}, Set{}.
        /// </summary>
        public bool? Feeder
        {
            get { return _asyncDS.HasFeeder ? new Nullable<bool>(_asyncDS.UseFeeder.Value) : new Nullable<bool>(); }
            set { SetValueToProxy<bool>(_asyncDS.UseFeeder, value); }
        }
        /// <summary>
        ///  Property Resolution Get{}, Set{}.
        /// </summary>
        public float? Resolution
        {
            get { return GetValueFromProxy<float>(_asyncDS.Resolution); }
            set { SetValueToProxy<float>(_asyncDS.Resolution, value); }
        }
        /// <summary>
        ///  Property FileTransferFormat Get{}, Set{}.
        /// </summary>
        public TwFF? FileTransferFormat
        {
            get
            {
                var ff = _asyncDS.Settings.Transfer.ImageFileFormat;
                if (ff.IsSupportedOnThisDevice())
                    return new Nullable<TwFF>(ff.Value);
                else
                    return new Nullable<TwFF>();
            }
            set
            {
                if (value.HasValue)
                    _asyncDS.Settings.Transfer.ImageFileFormat.Value = value.Value;
            }
        }
        /// <summary>
        ///  Property PageSize Get{}, Set{}.
        /// </summary>
        public TwSS? PageSize
        {
            get { return GetValueFromProxy<TwSS>(_asyncDS.PageSize); }
            set { SetValueToProxy<TwSS>(_asyncDS.PageSize, value); }
        }
        /// <summary>
        ///  Property TransfersMechanisms Get{}.
        /// </summary>
        public List<TwSX> TransfersMechanisms
        {
            get
            {
                return (_asyncDS != null) ?
                    new List<TwSX>(_asyncDS.TransferMechanism.AvailableValues) :
                    new List<TwSX>();
            }
        }
        /// <summary>
        ///  Property PixelTypes Get{}.
        /// </summary>
        public List<TwPixelType> PixelTypes
        {
            get
            {
                return (_asyncDS != null) ?
                    new List<TwPixelType>(_asyncDS.ColorMode.AvailableValues) :
                    new List<TwPixelType>();
            }
        }
        /// <summary>
        ///  Property Resolutions Get{}.
        /// </summary>
        public List<float> Resolutions
        {
            get
            {
                return (_asyncDS != null) ?
                    new List<float>(_asyncDS.Resolution.AvailableValues) :
                    new List<float>();
            }

        }
        /// <summary>
        ///  Property FileTransferFormats Get{}.
        /// </summary>
        public List<TwFF> FileTransferFormats
        {
            get
            {
                return (_asyncDS != null) ?
                    new List<TwFF>(_asyncDS.Settings.Transfer.ImageFileFormat.SupportedValues) :
                    new List<TwFF>();
            }
        }
        /// <summary>
        ///  Property PageSizes Get{}.
        /// </summary>
        public List<TwSS> PageSizes
        {
            get
            {
                return (_asyncDS != null) ?
                    new List<TwSS>(_asyncDS.PageSize.AvailableValues) :
                    new List<TwSS>();
            }
        }

        public void Dispose()
        {
            if (_asyncDS != null)
                _asyncDS.Dispose();
            if (_asyncDSM != null)
                _asyncDSM.Dispose();
        }

    }
}
