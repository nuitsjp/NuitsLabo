using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface INavigationRequestObserver
    {
        Task Notify(object parameter);
    }
    
    public interface INavigationRequestObserver<in T> : INavigationRequestObserver
    {
        Task Notify(T parameter);
    }

}