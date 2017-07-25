using System;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface INavigationRequest
    {
        INavigationAction NavigationAction { set; }
    }
}