using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class NavigationAction : INavigationAction
    {

        public INavigationRequest Request { get; set; }

        public static void PropertyChanged(object bindable, object newValue)
        {
            var navigationAction = (NavigationAction) bindable;
            ((INavigationRequest)newValue).NavigationAction = navigationAction;
        }

        public Task Navigate<T>(T parameter = default(T))
        {
            Debug.WriteLine($"parameter:{parameter}");
            return Task.FromResult(true);
        }
    }
}
