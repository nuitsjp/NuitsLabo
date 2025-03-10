using System;
using System.Management;
using System.Windows;
using System.Windows.Threading;

public class ImagingDeviceDetector : IDisposable
{
    private ManagementEventWatcher? _insertWatcher;
    private ManagementEventWatcher? _removeWatcher;
    private readonly Dispatcher _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

    // イメージングデバイスクラスGUID（スキャナーやカメラなどが含まれる）
    private static readonly Guid IMAGING_DEVICES_CLASS = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");

    // スキャナー専用（サブタイプで識別する場合）
    private const string SCANNER_DEVICE_TYPE = "Scanner";

    // デバイス接続/切断イベント
    public event EventHandler<ImagingDeviceEventArgs>? DeviceConnected;
    public event EventHandler<ImagingDeviceEventArgs>? DeviceDisconnected;

    public void Start()
    {
        try
        {
            // WMIイベントクエリの設定
            var insertQuery = new WqlEventQuery(
                "SELECT * FROM __InstanceCreationEvent WITHIN 2 " +
                "WHERE TargetInstance ISA 'Win32_PnPEntity'"
            );

            var removeQuery = new WqlEventQuery(
                "SELECT * FROM __InstanceDeletionEvent WITHIN 2 " +
                "WHERE TargetInstance ISA 'Win32_PnPEntity'"
            );

            _insertWatcher = new ManagementEventWatcher(insertQuery);
            _removeWatcher = new ManagementEventWatcher(removeQuery);

            _insertWatcher.EventArrived += DeviceInsertedEvent;
            _removeWatcher.EventArrived += DeviceRemovedEvent;

            _insertWatcher.Start();
            _removeWatcher.Start();
        }
        catch (Exception ex)
        {
            _dispatcher.Invoke(() =>
            {
                MessageBox.Show($"イメージングデバイス検出の初期化エラー: {ex.Message}");
            });
        }
    }

    public void Stop()
    {
        try
        {
            if (_insertWatcher != null)
            {
                _insertWatcher.Stop();
                _insertWatcher.Dispose();
                _insertWatcher = null;
            }

            if (_removeWatcher != null)
            {
                _removeWatcher.Stop();
                _removeWatcher.Dispose();
                _removeWatcher = null;
            }
        }
        catch (Exception ex)
        {
            _dispatcher.Invoke(() =>
            {
                MessageBox.Show($"イメージングデバイス検出の停止エラー: {ex.Message}");
            });
        }
    }

    private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
    {
        ProcessDeviceEvent(e, true);
    }

    private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
    {
        ProcessDeviceEvent(e, false);
    }

    private void ProcessDeviceEvent(EventArrivedEventArgs e, bool isConnected)
    {
        try
        {
            var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            // デバイスクラスGUIDの取得（ClassGuid）
            var classGuidString = instance.Properties["ClassGuid"]?.Value?.ToString();
            if (string.IsNullOrEmpty(classGuidString))
                return;

            // GUIDの比較（大文字小文字を無視）
            if (Guid.TryParse(classGuidString, out var classGuid) &&
                classGuid.Equals(IMAGING_DEVICES_CLASS))
            {
                // イメージングデバイスと判定された場合
                var deviceId = instance.Properties["DeviceID"].Value.ToString();
                var description = instance.Properties["Caption"].Value?.ToString() ??
                                  instance.Properties["Description"].Value?.ToString() ??
                                  "不明なイメージングデバイス";
                var manufacturer = instance.Properties["Manufacturer"].Value?.ToString() ?? "不明";

                // スキャナーかどうかの追加判定（必要に応じて）
                var isScanner = IsScanner(instance);

                // イベント発火（UIスレッドで）
                _dispatcher.Invoke(() =>
                {
                    var args = new ImagingDeviceEventArgs(deviceId, description, manufacturer, isScanner);
                    if (isConnected)
                        DeviceConnected?.Invoke(this, args);
                    else
                        DeviceDisconnected?.Invoke(this, args);
                });
            }
        }
        catch (Exception ex)
        {
            _dispatcher.Invoke(() =>
            {
                MessageBox.Show($"デバイスイベント処理エラー: {ex.Message}");
            });
        }
    }

    private bool IsScanner(ManagementBaseObject deviceInstance)
    {
        try
        {
            // 方法1: デバイス種別で判断（より正確）
            var pnpDeviceID = deviceInstance.Properties["PNPDeviceID"]?.Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(pnpDeviceID))
            {
                // WIA (Windows Image Acquisition) スキャナーの識別
                if (pnpDeviceID.StartsWith("WIA\\", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // 方法2: WMIでスキャナーの詳細情報を取得
            var deviceID = deviceInstance.Properties["DeviceID"]?.Value?.ToString();
            if (!string.IsNullOrEmpty(deviceID))
            {
                using (var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_SCSIController WHERE DeviceID = '{deviceID}'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        // SCSIコントローラーのタイプがスキャナーの場合
                        if (obj["DeviceType"]?.ToString() == SCANNER_DEVICE_TYPE)
                        {
                            return true;
                        }
                    }
                }
            }

            // 方法3: デバイス説明で判断（言語に依存するため最終手段）
            var description = deviceInstance.Properties["Description"]?.Value?.ToString() ?? string.Empty;
            var caption = deviceInstance.Properties["Caption"]?.Value?.ToString() ?? string.Empty;

            // 言語に依存しない判定方法（英語だけでなく様々な言語に対応）
            return description.IndexOf("scan", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   caption.IndexOf("scan", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   description.IndexOf("スキャナ", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   caption.IndexOf("スキャナ", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        Stop();
    }

    public class ImagingDeviceEventArgs : EventArgs
    {
        public string DeviceId { get; }
        public string Description { get; }
        public string Manufacturer { get; }
        public bool IsScanner { get; } // スキャナーかどうか

        public ImagingDeviceEventArgs(string deviceId, string description, string manufacturer, bool isScanner)
        {
            DeviceId = deviceId;
            Description = description;
            Manufacturer = manufacturer;
            IsScanner = isScanner;
        }
    }
}