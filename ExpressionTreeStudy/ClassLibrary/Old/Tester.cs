namespace ClassLibrary.Old
{
    public class Tester
    {
        public static void Test()
        {
            Page mock = new Mock();
            mock.OnLoading("Hello, World.");
        }
    }
}
