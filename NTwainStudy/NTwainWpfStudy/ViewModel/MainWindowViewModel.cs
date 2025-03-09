using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NTwain;
using NTwain.Data;

namespace NTwainWpfStudy.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        DataSources = new ObservableCollection<DataSourceVM>();
        CapturedImages = new ObservableCollection<ImageSource>();

        //this.SynchronizationContext = SynchronizationContext.Current;
        var appId = TWIdentity.CreateFromAssembly(DataGroups.Image | DataGroups.Audio, Assembly.GetEntryAssembly());
        _session = new TwainSession(appId);
        _session.TransferError += _session_TransferError;
        _session.TransferReady += _session_TransferReady;
        _session.DataTransferred += _session_DataTransferred;
        _session.SourceDisabled += _session_SourceDisabled;
        _session.StateChanged += (s, e) =>
        {
            _dispatcher.BeginInvoke(() =>
            {
                ReloadSourcesCommand.NotifyCanExecuteChanged();
                CaptureCommand.NotifyCanExecuteChanged();
            });
        };
    }

    private readonly TwainSession _session;
    private IntPtr _windowHandle;
    private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    public void Initialize(IntPtr windowHandle, Dispatcher dispatcher)
    {
        _windowHandle = windowHandle;
        _dispatcher = dispatcher;

        if (_windowHandle == IntPtr.Zero)
        {

        }
        else
        {
            if (_session.Open() == ReturnCode.Success)
            {
                var ds = _session.FirstOrDefault();
                ReloadSources();
            }
        }

    }

    #region properties

    public string AppTitle
    {
        get
        {
            if (NTwain.PlatformInfo.Current.IsApp64Bit)
            {
                return "TWAIN Data Source Tester (64bit)";
            }
            else
            {
                return "TWAIN Data Source Tester (32bit)";
            }
        }
    }
    public ObservableCollection<DataSourceVM> DataSources { get; private set; }
    private DataSourceVM? _selectedSource;

    public DataSourceVM? SelectedSource
    {
        get => _selectedSource;
        set
        {
            if (_session.State == 4)
            {
                _session.CurrentSource.Close();
            }
            _selectedSource = value;
            OnPropertyChanged();
            if (_selectedSource != null)
            {
                _selectedSource.Open();
            }
        }
    }

    public int State => _session.State;

    public IntPtr WindowHandle
    {
        get => _windowHandle;
        set
        {
            _windowHandle = value;
            if (value == IntPtr.Zero)
            {

            }
            else
            {
                // use this for internal msg loop
                var rc = _session.Open();

                // use this to hook into current app loop
                //var rc = _session.Open(new WpfMessageLoopHook(value));

                if (rc == ReturnCode.Success)
                {
                    ReloadSourcesCommand.Execute(null);
                }
            }
        }
    }

    public bool ShowUI
    {
        get; set;
    }



    private ICommand _showDriverCommand;
    public ICommand ShowDriverCommand
    {
        get
        {
            return _showDriverCommand ?? (_showDriverCommand = new RelayCommand(() =>
            {
                if (_session.State == 4)
                {
                    var rc = _session.CurrentSource.Enable(SourceEnableMode.ShowUIOnly, false, WindowHandle);
                }
            }, () =>
            {
                return _session.State == 4 && _session.CurrentSource.Capabilities.CapEnableDSUIOnly.GetCurrent() == BoolType.True;
            }));
        }
    }

    public bool CanCapture()
    {
        return _session.State == 4;
    }

    [RelayCommand]
    private void Capture()
    {
        if (_session.State == 4)
        {
            //if (this.CurrentSource.ICapPixelType.Get().Contains(PixelType.BlackWhite))
            //{
            //    this.CurrentSource.ICapPixelType.Set(PixelType.BlackWhite);
            //}

            //if (this.CurrentSource.ICapXferMech.Get().Contains(XferMech.File))
            //{
            //    this.CurrentSource.ICapXferMech.Set(XferMech.File);
            //}

            var rc = _session.CurrentSource.Enable(SourceEnableMode.NoUI, false, WindowHandle);
        }
    }


    //private ICommand _saveCommand;
    //public ICommand SaveCommand
    //{
    //    get
    //    {
    //        return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
    //        {
    //            Messenger.Default.Send(new ChooseFileMessage(this, files =>
    //            {
    //                var tiffPath = files.FirstOrDefault();

    //                var srcFiles = CapturedImages.Select(ci=>ci.)
    //            })
    //            {
    //                Caption = "Save to File",
    //                Filters = "Tiff files|*.tif,*.tiff"
    //            });
    //        }, () =>
    //        {
    //            return CapturedImages.Count > 0;
    //        }));
    //    }
    //}

    private ICommand _clearCommand;
    public ICommand ClearCommand
    {
        get
        {
            return _clearCommand ?? (_clearCommand = new RelayCommand(() =>
            {
                CapturedImages.Clear();
            }, () =>
            {
                return CapturedImages.Count > 0;
            }));
        }
    }
    public bool CanReloadSources()
    {
        return _session.State > 2;
    }

    [RelayCommand(CanExecute = nameof(CanReloadSources))]
    private void ReloadSources()
    {
        DataSources.Clear();
        foreach (var s in _session.Select(s => new DataSourceVM { DS = s }))
        {
            DataSources.Add(s);
        }
        //SelectedSource = DataSources.FirstOrDefault();
    }

    /// <summary>
    /// Gets the captured images.
    /// </summary>
    /// <value>
    /// The captured images.
    /// </value>
    public ObservableCollection<ImageSource> CapturedImages { get; private set; }

    public double MinThumbnailSize => 50;
    public double MaxThumbnailSize => 300;

    private double _thumbSize = 150;
    public double ThumbnailSize
    {
        get => _thumbSize;
        set
        {
            if (value > MaxThumbnailSize) { value = MaxThumbnailSize; }
            else if (value < MinThumbnailSize) { value = MinThumbnailSize; }
            _thumbSize = value;
            OnPropertyChanged();
        }
    }


    #endregion

    void _session_SourceDisabled(object? sender, EventArgs e)
    {
        // notify source disabled
    }

    void _session_TransferError(object? sender, TransferErrorEventArgs e)
    {
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            // notify error
        }));
    }

    void _session_TransferReady(object? sender, TransferReadyEventArgs e)
    {
        var mech = _session.CurrentSource.Capabilities.ICapXferMech.GetCurrent();
        if (mech == XferMech.File)
        {
            var formats = _session.CurrentSource.Capabilities.ICapImageFileFormat.GetValues();
            var wantFormat = formats.Contains(FileFormat.Tiff) ? FileFormat.Tiff : FileFormat.Bmp;

            var fileSetup = new TWSetupFileXfer
            {
                Format = wantFormat,
                FileName = GetUniqueName(Path.GetTempPath(), "twain-test", "." + wantFormat)
            };
            var rc = _session.CurrentSource.DGControl.SetupFileXfer.Set(fileSetup);
        }
        else if (mech == XferMech.Memory)
        {
            // ?

        }
    }

    string GetUniqueName(string dir, string name, string ext)
    {
        var filePath = Path.Combine(dir, name + ext);
        int next = 1;
        while (File.Exists(filePath))
        {
            filePath = Path.Combine(dir, $"{name} ({next++}){ext}");
        }
        return filePath;
    }

    void _session_DataTransferred(object? sender, DataTransferredEventArgs e)
    {
        ImageSource img = GenerateThumbnail(e);
        if (img != null)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CapturedImages.Add(img);
            }));
        }
    }


    ImageSource GenerateThumbnail(DataTransferredEventArgs e)
    {
        BitmapSource img = null;

        switch (e.TransferType)
        {
            case XferMech.Native:
                using (var stream = e.GetNativeImageStream())
                {
                    if (stream != null)
                    {
                        img = stream.ConvertToWpfBitmap(300, 0);
                    }
                }
                break;
            case XferMech.File:
                img = new BitmapImage(new Uri(e.FileDataPath));
                if (img.CanFreeze)
                {
                    img.Freeze();
                }
                break;
            case XferMech.Memory:
                // TODO: build current image from multiple data-xferred event
                break;
        }

        //if (img != null)
        //{
        //    // from http://stackoverflow.com/questions/18189501/create-thumbnail-image-directly-from-header-less-image-byte-array
        //    var scale = MaxThumbnailSize / img.PixelWidth;
        //    var transform = new ScaleTransform(scale, scale);
        //    var thumbnail = new TransformedBitmap(img, transform);
        //    img = new WriteableBitmap(new TransformedBitmap(img, transform));
        //    img.Freeze();
        //}
        return img;
    }

    internal void CloseDown()
    {
        if (_session.State == 4)
        {
            _session.CurrentSource.Close();
        }
        _session.Close();
    }

}