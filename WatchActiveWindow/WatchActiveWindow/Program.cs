using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace WatchActiveWindow;

/// <summary>
/// アクティブウィンドウを監視し、システムトレイに常駐するアプリケーションのメインクラス
/// </summary>
class Program : Form
{
    // ReSharper disable IdentifierTypo
    // ReSharper disable ArrangeTypeMemberModifiers
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// イベントフックがアプリケーションのコンテキスト外で実行されることを示すフラグ
    /// </summary>
    const uint WINEVENT_OUTOFCONTEXT = 0;

    /// <summary>
    /// フォアグラウンドウィンドウが変更されたことを示すイベント
    /// </summary>
    const uint EVENT_SYSTEM_FOREGROUND = 3;
    // ReSharper restore InconsistentNaming
    // ReSharper restore ArrangeTypeMemberModifiers

    /// <summary>
    /// ウィンドウイベントのコールバック関数の定義
    /// Win32 APIのイベントハンドラーとして使用される
    /// </summary>
    delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    /// <summary>
    /// ウィンドウイベントをフックするためのWin32 API関数
    /// </summary>
    [DllImport("user32.dll")]
    static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    /// <summary>
    /// ウィンドウイベントのフックを解除するためのWin32 API関数
    /// </summary>
    [DllImport("user32.dll")]
    static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    /// <summary>
    /// ウィンドウのタイトルテキストを取得するためのWin32 API関数
    /// </summary>
    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    /// <summary>
    /// ウィンドウに関連付けられたプロセスIDを取得するためのWin32 API関数
    /// </summary>
    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    // ReSharper restore IdentifierTypo

    /// <summary>
    /// イベントフックのハンドル
    /// </summary>
    static IntPtr _hWinEventHook;

    /// <summary>
    /// システムトレイのアイコン
    /// </summary>
    private NotifyIcon? _trayIcon;

    /// <summary>
    /// システムトレイのコンテキストメニュー
    /// </summary>
    private ContextMenuStrip? _trayMenu;

    /// <summary>
    /// アプリケーションのエントリーポイント
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Program());
    }

    /// <summary>
    /// Programクラスのコンストラクタ
    /// フォームの初期化とイベントフックの設定を行う
    /// </summary>
    public Program()
    {
        InitializeComponent();
        // フォアグラウンドウィンドウの変更イベントをフックする
        _hWinEventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, WinEventProc, 0, 0, WINEVENT_OUTOFCONTEXT);
    }

    /// <summary>
    /// コンポーネントの初期化
    /// システムトレイアイコンとメニューの設定、フォームの非表示化を行う
    /// </summary>
    private void InitializeComponent()
    {
        _trayMenu = new ContextMenuStrip();
        _trayMenu.Items.Add("終了", null, OnExit);
        _trayIcon = new NotifyIcon();
        _trayIcon.Text = "ウィンドウ監視アプリ";
        _trayIcon.Icon = SystemIcons.Application;
        _trayIcon.ContextMenuStrip = _trayMenu;
        _trayIcon.Visible = true;
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        Opacity = 0;
    }

    /// <summary>
    /// フォームのロード時の処理
    /// フォームを非表示にする
    /// </summary>
    /// <param name="e">イベント引数</param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Hide();
    }

    /// <summary>
    /// アプリケーション終了時の処理
    /// イベントフックの解除、システムトレイアイコンの非表示、アプリケーションの終了を行う
    /// </summary>
    /// <param name="sender">イベントの送信元</param>
    /// <param name="e">イベント引数</param>
    private void OnExit(object? sender, EventArgs e)
    {
        UnhookWinEvent(_hWinEventHook);
        _trayIcon!.Visible = false;
        Application.Exit();
    }

    /// <summary>
    /// ウィンドウイベントのコールバック関数
    /// フォアグラウンドウィンドウが変更されたときに呼び出される
    /// 新しいアクティブウィンドウの情報をコンソールに出力する
    /// </summary>
    // ReSharper disable IdentifierTypo
    static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    // ReSharper restore IdentifierTypo
    {
        if (eventType != EVENT_SYSTEM_FOREGROUND) return;
        var title = new StringBuilder(256);
        GetWindowText(hwnd, title, 256);
        var windowTitle = title.ToString();
        // タイトルが空または「タスクの切り替え」の場合はスキップ
        if (string.IsNullOrWhiteSpace(windowTitle) || windowTitle == "タスクの切り替え")
        {
            return;
        }
        GetWindowThreadProcessId(hwnd, out var processId);
        var processName = "Unknown";
        try
        {
            using var process = Process.GetProcessById((int)processId);
            processName = process.ProcessName;
        }
        catch (ArgumentException)
        {
            // プロセスが見つからない場合は "Unknown" のままにする
        }
        Console.WriteLine("アクティブウィンドウが変更されました:");
        Console.WriteLine($"ウィンドウタイトル: {windowTitle}");
        Console.WriteLine($"プロセス名: {processName}");
        Console.WriteLine($"プロセスID: {processId}");
        Console.WriteLine();
    }

    /// <summary>
    /// リソースの解放
    /// イベントフックの解除とシステムトレイアイコンの破棄を行う
    /// </summary>
    /// <param name="disposing">マネージリソースを解放するかどうか</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            UnhookWinEvent(_hWinEventHook);
            _trayIcon!.Dispose();
        }
        base.Dispose(disposing);
    }
}