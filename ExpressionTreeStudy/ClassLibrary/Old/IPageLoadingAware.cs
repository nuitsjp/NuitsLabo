namespace ClassLibrary.Old
{
    public interface IPageLoadingAware<in T>
    {
        void OnLoading(T parameter);
    }
}