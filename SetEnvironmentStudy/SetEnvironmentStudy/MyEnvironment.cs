using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace SetEnvironmentStudy;

public class MyEnvironment
{
    // Assume the following constants include the terminating '\0' - use <, not <=
    const int MaxEnvVariableValueLength = 32767;  // maximum length for environment variable name and value
    // System environment variables are stored in the registry, and have 
    // a size restriction that is separate from both normal environment 
    // variables and registry value name lengths, according to MSDN.
    // MSDN doesn't detail whether the name is limited to 1024, or whether
    // that includes the contents of the environment variable.
    const int MaxSystemEnvVariableLength = 1024;
    const int MaxUserEnvVariableLength = 255;
    const uint SMTO_ABORTIFHUNG = 0x0002;    // ハングしたウィンドウをスキップ
    const uint SMTO_NOTIMEOUTIFNOTHUNG = 0x0008;  // ハングしていなければタイムアウトしない
    [DllImport("user32.dll", SetLastError = true, BestFitMapping = false)]
    internal static extern IntPtr SendMessageTimeout(
        IntPtr hWnd,
        int Msg,
        IntPtr wParam,
        string lParam,
        uint fuFlags,
        uint uTimeout,
        IntPtr lpdwResult);

    public static void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target, bool permissionDemand, bool notify, bool skipHung = false, bool notSkipHang = false)
    {
        CheckEnvironmentVariableName(variable);

        // System-wide environment variables stored in the registry are
        // limited to 1024 chars for the environment variable name.
        if (variable.Length >= MaxSystemEnvVariableLength)
        {
            throw new ArgumentException();
        }

        if (permissionDemand)
        {
            new EnvironmentPermission(PermissionState.Unrestricted).Demand();
        }
        // explicitly null out value if is the empty string. 
        if (String.IsNullOrEmpty(value) || value[0] == '\0')
        {
            value = null;
        }

        if (target == EnvironmentVariableTarget.Machine)
        {
            using (RegistryKey environmentKey =
                   Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\Environment", true))
            {

                Contract.Assert(environmentKey != null, @"HKLM\System\CurrentControlSet\Control\Session Manager\Environment is missing!");
                if (environmentKey != null)
                {
                    if (value == null)
                        environmentKey.DeleteValue(variable, false);
                    else
                        environmentKey.SetValue(variable, value);
                }
            }
        }
        else if (target == EnvironmentVariableTarget.User)
        {
            // User-wide environment variables stored in the registry are
            // limited to 255 chars for the environment variable name.
            if (variable.Length >= MaxUserEnvVariableLength)
            {
                throw new ArgumentException();
            }
            using (RegistryKey environmentKey =
                   Registry.CurrentUser.OpenSubKey("Environment", true))
            {
                Contract.Assert(environmentKey != null, @"HKCU\Environment is missing!");
                if (environmentKey != null)
                {
                    if (value == null)
                        environmentKey.DeleteValue(variable, false);
                    else
                        environmentKey.SetValue(variable, value);
                }
            }
        }
        else
        {
            throw new ArgumentException();
        }
        // 環境変数の変更をシステムに通知
        if (notify)
        {
            if (skipHung)
            {
                if (notSkipHang)
                {
                    int num = SendMessageTimeout(new IntPtr((int)ushort.MaxValue), 26, IntPtr.Zero, nameof(Environment), SMTO_ABORTIFHUNG | SMTO_NOTIMEOUTIFNOTHUNG, 1000U, IntPtr.Zero) == IntPtr.Zero ? 1 : 0;
                }
                else
                {
                    int num = SendMessageTimeout(new IntPtr((int)ushort.MaxValue), 26, IntPtr.Zero, nameof(Environment), SMTO_ABORTIFHUNG, 1000U, IntPtr.Zero) == IntPtr.Zero ? 1 : 0;
                }
            }
            else
            {
                int num = SendMessageTimeout(new IntPtr((int)ushort.MaxValue), 26, IntPtr.Zero, nameof(Environment), 0U, 1000U, IntPtr.Zero) == IntPtr.Zero ? 1 : 0;
            }
        }
    }

    private static void CheckEnvironmentVariableName(string variable)
    {
        if (variable == null)
        {
            throw new ArgumentNullException("variable");
        }

        if (variable.Length == 0)
        {
            throw new ArgumentException();
        }

        if (variable[0] == '\0')
        {
            throw new ArgumentException();
        }

        // Make sure the environment variable name isn't longer than the 
        // max limit on environment variable values.  (MSDN is ambiguous 
        // on whether this check is necessary.)
        if (variable.Length >= MaxEnvVariableValueLength)
        {
            throw new ArgumentException();
        }

        if (variable.IndexOf('=') != -1)
        {
            throw new ArgumentException();
        }
        Contract.EndContractBlock();
    }
}