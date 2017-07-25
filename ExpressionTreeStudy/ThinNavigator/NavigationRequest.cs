using System;
using System.Threading.Tasks;

namespace ThinNavigator
{
    public class NavigationRequest : INavigationRequest
    {
        private Func<object, Task> _observer;
        
        public virtual void SetObserver(Func<object, Task> observer)
        {
            _observer = observer;
        }

        public Func<object, Task> Observer
        {
            set => _observer = value;
        }

        public virtual Task RaiseAsync(object parameter = null)
        {
            return _observer(parameter);
        }
    }

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

        public Task RaiseAsync(object parameter = null)
        {
            if (parameter is T)
            {
                return RaiseAsync((T)parameter);
            }
            else
            {
                throw new InvalidOperationException($"parameter is not {typeof(T)}.");
            }
        }

        public Task RaiseAsync(T parameter = default(T))
        {
            return _observer(parameter);
        }
    }
}