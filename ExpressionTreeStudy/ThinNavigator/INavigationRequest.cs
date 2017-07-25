using System;
using System.Threading.Tasks;

namespace ThinNavigator
{
    public interface INavigationRequest
    {
        Func<object, Task> Observer { set; }

        Task RaiseAsync(object parameter = null);
    }

    public interface INavigationRequest<T> : INavigationRequest
    {
        new Func<T, Task> Observer { set; }

        Task RaiseAsync(T parameter = default(T));
    }
}