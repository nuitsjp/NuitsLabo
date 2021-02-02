using System.Threading.Tasks;
using Xunit;
using VerifyCS = MakeConst.Test.CSharpAnalyzerVerifier<
    MakeConst.MakeConstAnalyzer>;

namespace MakeConst.Test
{
    public class MakeConstUnitTest
    {
        //No diagnostics expected to show up
        [Theory]
        [InlineData("")]
        public async Task WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [Theory]
        [InlineData(LocalIntCouldBeConstant, LocalIntCouldBeConstantFixed, 10, 13)]
        public async Task WhenDiagnosticIsRaisedFixUpdatesCode(
            string test,
            string fixTest,
            int line,
            int column)
        {
            var expected = VerifyCS.Diagnostic(MakeConstAnalyzer.Rule).WithLocation(line, column);
            await VerifyCS.VerifyAnalyzerAsync(test, expected);

            //await VerifyCS.VerifyCodeFixAsync(test, fixTest);
        }

        private const string LocalIntCouldBeConstant = @"
using System;

namespace MakeConstTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            Console.WriteLine(i);
        }
    }
}";

        private const string LocalIntCouldBeConstantFixed = @"
using System;

namespace MakeConstTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int i = 0;
            Console.WriteLine(i);
        }
    }
}";
    }
}
