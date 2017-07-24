using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
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
