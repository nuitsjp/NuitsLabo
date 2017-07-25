using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class NavigationAction
    {

        public INavigationRequest Request { get; set; }

        public static void PropertyChanged(object bindable, object newValue)
        {
            var navigationBase = (NavigationAction) bindable;
            var interfaceType = GetImplementedInterface(newValue, typeof(INavigationRequest<>));
            var propertyInfo = interfaceType.GetRuntimeProperties().FirstOrDefault(x => x.Name == "Observer");
            Func<object, Task> func = navigationBase.Navigate;
            propertyInfo.SetValue(newValue, func);
            //var genericMethodInfo = methodInfo.MakeGenericMethod(interfaceType);
            //((INavigationRequest)newValue).Observer = navigationBase.Navigate;
        }

        private static Type GetImplementedInterface(object target, Type interfacType)
        {
            foreach (var implementedInterface in target.GetType().GetTypeInfo().ImplementedInterfaces)
            {
                if (interfacType.Name == implementedInterface.Name)
                    return implementedInterface;
            }
            return null;
        }

        protected Task Navigate<T>(T parameter = default(T))
        {
            Debug.WriteLine($"parameter:{parameter}");
            return Task.FromResult(true);
        }
    }
}
