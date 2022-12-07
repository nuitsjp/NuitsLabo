using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAsyncStudyApp
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
            };
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {

            };
            Application.ThreadException += (sender, args) =>
            {

            };
            Application.Run(new Form1());
        }
    }
}
