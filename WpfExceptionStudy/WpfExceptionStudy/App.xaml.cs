using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfExceptionStudy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += (o, args) =>
            {
                // ログ出力処理
                args.Handled = true; // 例外処理の中断
                if ( /** アプリケーションを継続可能か判定する **/ false)
                {
                    // アプリケーションが継続実行可能な場合
                    // ユーザーへの適切な通知処理など
                }
                else
                {
                    // アプリケーションが継続不可能な場合
                    // リソースを解法してエラーとしてアプリケーションを終了する
                    Environment.Exit(1);
                }

            };
            AppDomain.CurrentDomain.UnhandledException += (o, args) =>
            {
                // リソースの解放とログ出力の実装
                Environment.Exit(1);
            };

            TaskScheduler.UnobservedTaskException += (o, args) =>
            {
                // リソースの解放とログ出力の実装
                args.SetObserved();
            };
        }
    }
}
