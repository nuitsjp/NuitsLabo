using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.WindowsService
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new AdventureWorksService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
