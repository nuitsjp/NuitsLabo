using SimpleInjector;
using SimpleInjector.Integration.Wcf;

namespace AdventureWorks.ServiceHost
{
    public static class Bootstrapper
    {
        public static readonly Container Container;

        static Bootstrapper()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WcfOperationLifestyle();

            // register all your components with the container here:
            // container.Register<IService1, Service1>()
            // container.Register<IDataContext, DataContext>(Lifestyle.Scoped);

            container.Verify();

            Container = container;
        }
    }
}
