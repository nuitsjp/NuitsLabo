using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NTwain;
using NTwain.Data;

namespace NTwainV4x64WpfStudy;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly TwainAppSession _twainSession = new();
    private SynchronizationContext CurrentContext { get; } = SynchronizationContext.Current;
    public async Task InitializeAsync()
    {
        _twainSession.StateChanged += Twain_StateChanged;
        _twainSession.DefaultSourceChanged += Twain_DefaultSourceChanged;
        _twainSession.CurrentSourceChanged += Twain_CurrentSourceChanged;
        _twainSession.SourceDisabled += Twain_SourceDisabled;
        _twainSession.TransferReady += Twain_TransferReady;
        _twainSession.Transferred += Twain_Transferred;
        _twainSession.TransferError += Twain_TransferError;
        _twainSession.DeviceEvent += Twain_DeviceEvent;

        var status = await _twainSession.OpenDSMAsync();
        Debug.WriteLine($"OpenDSMAsync: {status}");
    }

    [ObservableProperty] private STATE? _sessionState;
    [ObservableProperty] private string _productName = string.Empty;
    [ObservableProperty] private string _currentSource = string.Empty;

    [RelayCommand]
    private void OpenDefaultSource()
    {
        _twainSession.TryStepdown(STATE.S3);
        _twainSession.OpenSource(_twainSession.DefaultSource);
    }

    [RelayCommand]
    private void ShowSettingOnly()
    {
        var status = _twainSession.EnableSource(true, true);
        Debug.WriteLine($"ShowSettingOnly: {status}");
    }

    public bool CanScan() => SessionState == STATE.S4;

    [RelayCommand(CanExecute = nameof(CanScan))]
    private void Scan()
    {
        var status = _twainSession.EnableSource(false, false);
        Debug.WriteLine($"Scan: {status}");
    }

    private void Twain_StateChanged(TwainAppSession sender, STATE e)
    {
        Debug.WriteLine($"Twain_StateChanged: {e}");
        SessionState = e;
        CurrentContext.Post(_ => ScanCommand.NotifyCanExecuteChanged(), null);
    }

    private void Twain_DeviceEvent(TwainAppSession sender, TW_DEVICEEVENT e)
    {
        Debug.WriteLine($"Twain_DeviceEvent: {e}");
        // TODO: 実装を追加
    }

    private void Twain_TransferError(TwainAppSession sender, TransferErrorEventArgs e)
    {
        Debug.WriteLine($"Twain_TransferError: {e.Exception?.Message ?? "Unknown error"}");
        // TODO: 実装を追加
    }

    private void Twain_Transferred(TwainAppSession sender, TransferredEventArgs e)
    {
        Debug.WriteLine($"Twain_Transferred: ImageInfo={e.ImageInfo}");
        // TODO: 実装を追加
    }

    private void Twain_TransferReady(TwainAppSession sender, TransferReadyEventArgs e)
    {
        Debug.WriteLine($"Twain_TransferReady: PendingCount={e.PendingCount}");
        // TODO: 実装を追加
    }

    private void Twain_SourceDisabled(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
        Debug.WriteLine($"Twain_SourceDisabled: Id={e.Id}, ProductName={e.ProductName}");
        // TODO: 実装を追加
    }

    private void Twain_CurrentSourceChanged(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
        Debug.WriteLine($"Twain_CurrentSourceChanged: Id={e.Id}, ProductName={e.ProductName}");
        CurrentSource = e.ProductName;
    }

    private void Twain_DefaultSourceChanged(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
        Debug.WriteLine($"Twain_DefaultSourceChanged: Id={e.Id}, ProductName={e.ProductName}");
        _twainSession.TryStepdown(STATE.S3);
        _twainSession.OpenSource(e);
        ProductName = e.ProductName;
    }
}
