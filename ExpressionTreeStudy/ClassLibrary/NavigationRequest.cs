using System.Threading.Tasks;

namespace ClassLibrary
{
    public class NavigationRequest : INavigationRequest
    {
        private INavigationRequestObserver _observer;
        
        public virtual void SetObserver(INavigationRequestObserver observer)
        {
            _observer = observer;
        }

        public virtual Task RaiseAsync(object parameter = null)
        {
            return _observer.Notify(parameter);
        }
    }

    public class NavigationRequest<T> : INavigationRequest<T>
    {
        private INavigationRequestObserver<T> _observer;
        
        public void SetObserver(INavigationRequestObserver observer)
        {
            _observer = observer as INavigationRequestObserver<T>;
        }

        public void SetObserver(INavigationRequestObserver<T> observer)
        {
            _observer = observer;
        }

        public Task RaiseAsync(object parameter = null)
        {
            if (parameter is T)
            {
                return RaiseAsync((T)parameter);
            }
            else
            {
                return _observer.Notify(null);
            }
        }

        public Task RaiseAsync(T parameter = default(T))
        {
            return _observer.Notify(parameter);
        }
    }
}