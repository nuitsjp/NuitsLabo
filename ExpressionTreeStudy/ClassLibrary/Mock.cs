using System.Diagnostics;

namespace ClassLibrary
{
    public class Mock : Page, IPageLoadingAware<string>
    {
        public void OnLoading(string parameter)
        {
            Debug.WriteLine(parameter);
        }
    }

    public class Mock<T>
    {
    }
}
