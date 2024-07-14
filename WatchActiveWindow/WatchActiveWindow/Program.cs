using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WatchActiveWindow;

class Program : Form
{
    // ReSharper disable IdentifierTypo
    // ReSharper disable ArrangeTypeMemberModifiers
    // ReSharper disable InconsistentNaming
    const uint WINEVENT_OUTOFCONTEXT = 0;
    const uint EVENT_SYSTEM_FOREGROUND = 3;
    // ReSharper restore InconsistentNaming
    // ReSharper restore ArrangeTypeMemberModifiers

    delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [DllImport("user32.dll")]
    static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


    // ReSharper restore IdentifierTypo

    static IntPtr _hWinEventHook;
    private NotifyIcon? _trayIcon;
    private ContextMenuStrip? _trayMenu;

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Program());
    }

    public Program()
    {
        InitializeComponent();
        _hWinEventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, WinEventProc, 0, 0, WINEVENT_OUTOFCONTEXT);
    }

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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Hide();
    }

    private void OnExit(object? sender, EventArgs e)
    {
        UnhookWinEvent(_hWinEventHook);
        _trayIcon!.Visible = false;
        Application.Exit();
    }

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