namespace ClassLibrary
{
    public interface IPageLoadingAware<in T>
    {
        void OnLoading(T parameter);
    }
}