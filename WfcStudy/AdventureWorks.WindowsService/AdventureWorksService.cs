using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.ServiceHost;
using AdventureWorks.Services.Impl;
using SimpleInjector.Integration.Wcf;

namespace AdventureWorks.WindowsService
{
    public partial class AdventureWorksService : ServiceBase
    {
        private System.ServiceModel.ServiceHost ServiceHost { get; set; }
        public AdventureWorksService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceHost = new SimpleInjectorServiceHost(Bootstrapper.Container, typeof(EmployeeService));
            ServiceHost.Open();
        }

        protected override void OnStop()
        {
            ServiceHost.Close();
            ServiceHost = null;
        }
    }
}
