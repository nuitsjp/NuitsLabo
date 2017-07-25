using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface INavigationRequest
    {
        void SetObserver(INavigationRequestObserver observer);

        Task RaiseAsync(object parameter = null);
    }

    public interface INavigationRequest<T> : INavigationRequest
    {
        void SetObserver(INavigationRequestObserver<T> observer);

        Task RaiseAsync(T parameter = default(T));
    }
}