using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector.Integration.Wcf;

namespace WfcStudy.ServiceHost
{
    public class WcfServiceFactory : SimpleInjectorServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType,
            Uri[] baseAddresses)
        {
            return new SimpleInjectorServiceHost(
                Bootstrapper.Container,
                serviceType,
                baseAddresses);
        }
    }
}
