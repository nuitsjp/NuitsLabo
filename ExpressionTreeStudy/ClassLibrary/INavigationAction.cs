using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface INavigationAction
    {
        Task Navigate<T>(T parameter = default(T));
    }
}