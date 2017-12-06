using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealProxyConsole
{
    [WeaveAspect]
    public class Usecase : ContextBoundObject
    {
        public string Message { get; } = "Hello, World.";

        public int Add(int left, int right)
        {
            return left + right;
        }

        public void AddToOutParameter(int left, int right, out int result)
        {
            result = left + right;
        }

        public void Bar(out int first, int second, out int third, int fourth)
        {
            first = second;
            third = fourth;
        }
    }
}
