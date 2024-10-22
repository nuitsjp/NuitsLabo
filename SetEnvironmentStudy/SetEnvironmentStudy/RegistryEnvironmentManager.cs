using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace SetEnvironmentStudy;

public class RegistryEnvironmentManager
{
    private const int HWND_BROADCAST = 0xffff;
    private const uint WM_SETTINGCHANGE = 0x1a;
    private const uint SMTO_ABORTIFHUNG = 0x0002;
    private const int TIMEOUT_MS = 5000;

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessageTimeout(
        IntPtr hWnd,
        uint Msg,
        UIntPtr wParam,
        string lParam,
        uint fuFlags,
        uint uTimeout,
        out UIntPtr lpdwResult
    );

    /// <summary>
    /// ユーザー環境変数をレジストリに直接設定します
    /// </summary>
    /// <param name="variableName">環境変数名</param>
    /// <param name="value">設定する値</param>
    /// <param name="notifyWindows">設定変更後にシステムに通知を送信するかどうか</param>
    /// <returns>設定が成功したかどうか</returns>
    public static bool SetUserEnvironmentVariable(string variableName, string value, bool notifyWindows = true, bool demandPermission = true)
    {
        try
        {
            if(demandPermission)
            {
                new EnvironmentPermission(PermissionState.Unrestricted).Demand();
            }

            using var key = Registry.CurrentUser.OpenSubKey("Environment", true);
            if (key == null)
            {
                throw new InvalidOperationException("Cannot open Environment registry key");
            }

            // レジストリに値を設定
            key.SetValue(variableName, value, RegistryValueKind.String);

            if (notifyWindows)
            {
                // 環境変数の変更をシステムに通知
                return NotifyEnvironmentChange();
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting environment variable: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 複数のユーザー環境変数を一括で設定します
    /// </summary>
    /// <param name="variables">キーが変数名、値が設定値の Dictionary</param>
    /// <param name="notifyWindows">設定変更後にシステムに通知を送信するかどうか</param>
    /// <returns>設定が成功したかどうか</returns>
    public bool SetMultipleUserEnvironmentVariables(Dictionary<string, string> variables, bool notifyWindows = true)
    {
        try
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Environment", true))
            {
                if (key == null)
                {
                    throw new InvalidOperationException("Cannot open Environment registry key");
                }

                // 一括で設定
                foreach (var variable in variables)
                {
                    key.SetValue(variable.Key, variable.Value, RegistryValueKind.String);
                }

                if (notifyWindows)
                {
                    // 最後に一回だけ通知
                    return NotifyEnvironmentChange();
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting environment variables: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 環境変数の変更をシステムに通知します
    /// </summary>
    /// <returns>通知が成功したかどうか</returns>
    private static bool NotifyEnvironmentChange()
    {
        try
        {
            UIntPtr result;
            IntPtr success = SendMessageTimeout(
                (IntPtr)HWND_BROADCAST,
                WM_SETTINGCHANGE,
                UIntPtr.Zero,
                "Environment",
                SMTO_ABORTIFHUNG,
                TIMEOUT_MS,
                out result
            );

            return success != IntPtr.Zero;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error notifying environment change: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// ユーザー環境変数の値を取得します
    /// </summary>
    /// <param name="variableName">環境変数名</param>
    /// <returns>環境変数の値。存在しない場合はnull</returns>
    public string GetUserEnvironmentVariable(string variableName)
    {
        try
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Environment"))
            {
                if (key == null)
                {
                    return null;
                }

                return key.GetValue(variableName) as string;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting environment variable: {ex.Message}");
            return null;
        }
    }
}