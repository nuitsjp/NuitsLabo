using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImagingDeviceDetectorStudy;

public partial class MainWindow : Window
{
    private ImagingDeviceDetector _wmiDetector;
    private SetupApiDeviceDetector _setupApiDetector;

    public MainWindow()
    {
        InitializeComponent();

        // WMI方式の検出器を初期化
        _wmiDetector = new ImagingDeviceDetector();
        _wmiDetector.DeviceConnected += (s, e) =>
        {
            if (e.IsScanner)
            {
                Dispatcher.Invoke(() =>
                {
                    LogMessage($"WMI: スキャナー接続 - {e.Description} (メーカー: {e.Manufacturer})");
                    // ここでスキャナー接続時の処理
                    statusLabel.Content = "スキャナーが接続されました";
                    scanButton.IsEnabled = true;
                });
            }
            else
            {
                // イメージングデバイスだがスキャナーではない（カメラなど）
                LogMessage($"WMI: イメージングデバイス接続 - {e.Description}");
            }
        };

        _wmiDetector.DeviceDisconnected += (s, e) =>
        {
            if (e.IsScanner)
            {
                Dispatcher.Invoke(() =>
                {
                    LogMessage($"WMI: スキャナー切断 - {e.Description}");
                    // ここでスキャナー切断時の処理
                    statusLabel.Content = "スキャナーが切断されました";
                    scanButton.IsEnabled = false;
                });
            }
            else
            {
                LogMessage($"WMI: イメージングデバイス切断 - {e.Description}");
            }
        };

        // SetupAPI方式の検出器も併用（より確実に）
        _setupApiDetector = new SetupApiDeviceDetector(this);
        _setupApiDetector.DeviceConnected += (s, e) =>
        {
            if (e.IsScanner)
            {
                Dispatcher.Invoke(() =>
                {
                    LogMessage($"SetupAPI: スキャナー接続 - {e.DeviceName}");
                    scanButton.IsEnabled = true;
                });
            }
        };

        _setupApiDetector.DeviceDisconnected += (s, e) =>
        {
            if (e.IsScanner)
            {
                Dispatcher.Invoke(() =>
                {
                    LogMessage($"SetupAPI: スキャナー切断 - {e.DeviceName}");
                    scanButton.IsEnabled = false;
                });
            }
        };

        // 検出開始
        _wmiDetector.Start();

        // 初期状態でスキャナーが接続されているか確認
        CheckInitialScannerConnection();
    }

    private void LogMessage(string message)
    {
        logTextBlock.Text += $"{DateTime.Now:HH:mm:ss} - {message}\n";
        logScrollViewer.ScrollToEnd();
    }

    private void CheckInitialScannerConnection()
    {
        // 現在接続されているスキャナーを検出
        // （この実装は別途必要）
    }

    protected override void OnClosed(EventArgs e)
    {
        _wmiDetector.Dispose();
        _setupApiDetector.Dispose();
        base.OnClosed(e);
    }
}