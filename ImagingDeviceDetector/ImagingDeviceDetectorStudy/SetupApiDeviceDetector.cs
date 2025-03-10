using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ImagingDeviceDetectorStudy;

public class SetupApiDeviceDetector : IDisposable
{
    // Windows API 定数
    private const int DIGCF_PRESENT = 0x00000002;
    private const int DIGCF_DEVICEINTERFACE = 0x00000010;
    private const int DBT_DEVICEARRIVAL = 0x8000;
    private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
    private const int WM_DEVICECHANGE = 0x0219;
    private const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;

    // イメージングデバイスクラスのGUID
    private static Guid GUID_DEVCLASS_IMAGE = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");

    // イベント
    public event EventHandler<ImageDeviceEventArgs> DeviceConnected;
    public event EventHandler<ImageDeviceEventArgs> DeviceDisconnected;

    private HwndSource _hwndSource;
    private Window _window;
    private IntPtr _notificationHandle;

    public SetupApiDeviceDetector(Window window)
    {
        _window = window;

        if (_window.IsLoaded)
        {
            RegisterForDeviceNotifications();
        }
        else
        {
            _window.Loaded += (s, e) => RegisterForDeviceNotifications();
        }

        _window.Closed += (s, e) => UnregisterDeviceNotifications();
    }

    private void RegisterForDeviceNotifications()
    {
        try
        {
            // HwndSourceの取得
            WindowInteropHelper helper = new WindowInteropHelper(_window);
            _hwndSource = HwndSource.FromHwnd(helper.Handle);
            _hwndSource.AddHook(WndProc);

            // デバイス通知の登録
            DEV_BROADCAST_DEVICEINTERFACE_1 dbi = new DEV_BROADCAST_DEVICEINTERFACE_1
            {
                dbcc_size = Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACE_1)),
                dbcc_devicetype = 5, // DBT_DEVTYP_DEVICEINTERFACE
                dbcc_classguid = GUID_DEVCLASS_IMAGE
            };

            IntPtr buffer = Marshal.AllocHGlobal(dbi.dbcc_size);
            Marshal.StructureToPtr(dbi, buffer, true);

            _notificationHandle = RegisterDeviceNotification(
                helper.Handle,
                buffer,
                DEVICE_NOTIFY_WINDOW_HANDLE);

            Marshal.FreeHGlobal(buffer);

            if (_notificationHandle == IntPtr.Zero)
            {
                throw new ApplicationException("デバイス通知の登録に失敗しました。");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"デバイス通知の初期化エラー: {ex.Message}");
        }
    }

    private void UnregisterDeviceNotifications()
    {
        if (_notificationHandle != IntPtr.Zero)
        {
            UnregisterDeviceNotification(_notificationHandle);
            _notificationHandle = IntPtr.Zero;
        }

        if (_hwndSource != null)
        {
            _hwndSource.RemoveHook(WndProc);
            _hwndSource = null;
        }
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_DEVICECHANGE)
        {
            switch ((int)wParam)
            {
                case DBT_DEVICEARRIVAL:
                    ProcessDeviceChange(lParam, true);
                    break;
                case DBT_DEVICEREMOVECOMPLETE:
                    ProcessDeviceChange(lParam, false);
                    break;
            }
        }
        return IntPtr.Zero;
    }

    private void ProcessDeviceChange(IntPtr lParam, bool isConnected)
    {
        try
        {
            DEV_BROADCAST_HDR header = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_HDR));

            // デバイスインターフェースのイベントの場合
            if (header.dbch_devicetype == 5) // DBT_DEVTYP_DEVICEINTERFACE
            {
                DEV_BROADCAST_DEVICEINTERFACE_1 devInterface = (DEV_BROADCAST_DEVICEINTERFACE_1)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_DEVICEINTERFACE_1));

                // GUIDの比較
                if (devInterface.dbcc_classguid == GUID_DEVCLASS_IMAGE)
                {
                    // デバイスパスの取得（デバイス名を取得するため）
                    string devicePath = GetDevicePath(lParam);
                    string deviceName = GetDeviceNameFromPath(devicePath);
                    bool isScanner = IsDeviceScanner(devicePath);

                    var args = new ImageDeviceEventArgs(devicePath, deviceName, isScanner);

                    if (isConnected)
                        DeviceConnected?.Invoke(this, args);
                    else
                        DeviceDisconnected?.Invoke(this, args);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"デバイス変更処理エラー: {ex.Message}");
        }
    }

    private string GetDevicePath(IntPtr pDevBroadcastDeviceInterface)
    {
        DEV_BROADCAST_DEVICEINTERFACE_1 devInterface = (DEV_BROADCAST_DEVICEINTERFACE_1)Marshal.PtrToStructure(pDevBroadcastDeviceInterface, typeof(DEV_BROADCAST_DEVICEINTERFACE_1));

        // デバイスパス文字列の取得
        IntPtr pDeviceName = new IntPtr((long)pDevBroadcastDeviceInterface + Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACE_1)));
        return Marshal.PtrToStringAuto(pDeviceName);
    }

    private string GetDeviceNameFromPath(string devicePath)
    {
        try
        {
            // SetupAPIを使用してデバイス名を取得
            IntPtr hDevInfo = SetupDiGetClassDevs(
                ref GUID_DEVCLASS_IMAGE,
                IntPtr.Zero,
                IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

            if (hDevInfo == (IntPtr)(-1))
                return "不明なデバイス";

            try
            {
                SP_DEVINFO_DATA devInfoData = new SP_DEVINFO_DATA();
                devInfoData.cbSize = Marshal.SizeOf(devInfoData);

                for (int i = 0; SetupDiEnumDeviceInfo(hDevInfo, i, ref devInfoData); i++)
                {
                    // デバイス説明を取得
                    StringBuilder descBuffer = new StringBuilder(256);
                    if (SetupDiGetDeviceRegistryProperty(
                            hDevInfo,
                            ref devInfoData,
                            7, // SPDRP_FRIENDLYNAME
                            IntPtr.Zero,
                            descBuffer,
                            256,
                            IntPtr.Zero))
                    {
                        return descBuffer.ToString();
                    }

                    // フレンドリー名が取得できない場合は説明を取得
                    if (SetupDiGetDeviceRegistryProperty(
                            hDevInfo,
                            ref devInfoData,
                            0, // SPDRP_DEVICEDESC
                            IntPtr.Zero,
                            descBuffer,
                            256,
                            IntPtr.Zero))
                    {
                        return descBuffer.ToString();
                    }
                }
            }
            finally
            {
                SetupDiDestroyDeviceInfoList(hDevInfo);
            }

            // パスからデバイス名を抽出（フォールバック）
            int lastIndex = devicePath.LastIndexOf('\\');
            if (lastIndex != -1 && lastIndex < devicePath.Length - 1)
            {
                return devicePath.Substring(lastIndex + 1);
            }

            return "不明なイメージングデバイス";
        }
        catch
        {
            return "不明なイメージングデバイス";
        }
    }

    private bool IsDeviceScanner(string devicePath)
    {
        // "Scanner"または"スキャナ"の文字列を含む場合は、スキャナーと判断
        return devicePath.IndexOf("scanner", StringComparison.OrdinalIgnoreCase) >= 0 ||
               devicePath.IndexOf("スキャナ", StringComparison.OrdinalIgnoreCase) >= 0 ||
               // WIA_デバイスIDがスキャナーを示す
               devicePath.Contains("\\wia\\");
    }

    public void Dispose()
    {
        UnregisterDeviceNotifications();
    }

    // P/Invoke 定義
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, int Flags);

    [DllImport("user32.dll")]
    private static extern bool UnregisterDeviceNotification(IntPtr Handle);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint Property, IntPtr PropertyRegDataType, StringBuilder PropertyBuffer, int PropertyBufferSize, IntPtr RequiredSize);

    [DllImport("setupapi.dll")]
    private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

    // Windows API 構造体
    [StructLayout(LayoutKind.Sequential)]
    private struct DEV_BROADCAST_HDR
    {
        public int dbch_size;
        public int dbch_devicetype;
        public int dbch_reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DEV_BROADCAST_DEVICEINTERFACE_1
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;
        public Guid dbcc_classguid;
        // 実際にはこの後に文字列が続く
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SP_DEVINFO_DATA
    {
        public int cbSize;
        public Guid ClassGuid;
        public uint DevInst;
        public IntPtr Reserved;
    }

    public class ImageDeviceEventArgs : EventArgs
    {
        public string DevicePath { get; }
        public string DeviceName { get; }
        public bool IsScanner { get; }

        public ImageDeviceEventArgs(string devicePath, string deviceName, bool isScanner)
        {
            DevicePath = devicePath;
            DeviceName = deviceName;
            IsScanner = isScanner;
        }
    }
}