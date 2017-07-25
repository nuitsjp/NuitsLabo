namespace ClassLibrary
{
    public static class TestClient
    {
        public static void Invoke()
        {
            var action = new NavigationAction();
            var request = new NavigationRequest<string>();
            action.Request = request;
            NavigationAction.PropertyChanged(action, request);
            request.RaiseAsync("Hello, World");
        }
    }
}
