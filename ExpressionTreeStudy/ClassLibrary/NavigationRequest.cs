using System;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class NavigationRequest<T> : INavigationRequest<T>
    {
        private Func<T, Task> _observer;

        public Func<T, Task> Observer
        {
            set => _observer = value;
        }

        Func<object, Task> INavigationRequest.Observer
        {
            set => _observer = value as Func<T, Task>;
        }

        public Task RaiseAsync(T parameter = default(T))
        {
            return _observer(parameter);
        }

    }
    public class NavigationRequest : NavigationRequest<object>
    {
    }
}