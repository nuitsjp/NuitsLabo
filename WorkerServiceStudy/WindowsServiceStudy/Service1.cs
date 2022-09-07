using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServiceStudy
{
    public partial class Service1 : ServiceBase
    {
        private bool _isRunning = true;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    File.AppendAllText("log.txt", $@"Worker running at: {DateTimeOffset.Now}{Environment.NewLine}");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            });
        }

        protected override void OnStop()
        {
            _isRunning = false;
        }
    }
}
