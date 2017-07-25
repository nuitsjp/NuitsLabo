using System;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class NavigationRequest<T> : INavigationRequest
    {
        private INavigationAction _navigationAction;

        public INavigationAction NavigationAction
        {
            set => _navigationAction = value;
        }

        public Task RaiseAsync(T parameter = default(T))
        {
            return _navigationAction.Navigate(parameter);
        }

    }
    public class NavigationRequest : NavigationRequest<object>
    {
    }
}