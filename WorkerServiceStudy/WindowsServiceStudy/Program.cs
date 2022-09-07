using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceStudy
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main()
        {
            using (var processModule = System.Diagnostics.Process.GetCurrentProcess().MainModule)
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(processModule.FileName));
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
