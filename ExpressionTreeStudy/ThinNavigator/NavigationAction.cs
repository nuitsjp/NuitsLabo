using System.Diagnostics;
using System.Threading.Tasks;

namespace ThinNavigator
{
    public class NavigationAction
    {

        public INavigationRequest Request { get; set; }

        public static void PropertyChanged(object bindable, object newValue)
        {
            var navigationBase = (NavigationAction) bindable;
            ((INavigationRequest)newValue).Observer = navigationBase.Navigate;
        }

        protected Task Navigate<T>(T parameter = default(T))
        {
            Debug.WriteLine($"parameter:{parameter}");
            return Task.FromResult(true);
        }
    }
}
