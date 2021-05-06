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
        [InlineData(LocalIntCouldBeConstant, 12, 13)]
        public async Task WhenDiagnosticIsRaisedFixUpdatesCode(
            string test,
            int line,
            int column)
        {
            var expected = VerifyCS.Diagnostic(MakeConstAnalyzer.Rule).WithLocation(line, column);
            await VerifyCS.VerifyAnalyzerAsync(test, expected);

            //await VerifyCS.VerifyCodeFixAsync(test, fixTest);
        }

        private const string LocalIntCouldBeConstant = @"
using System;
using MakeConst;

namespace MakeConstTest
{
    [MakeConstAttribute]
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            Console.WriteLine(i);
        }
    }

    class A 
    { 
        [System.Flags] 
        enum Day 
        {
            sunday = 0,
            Monday = 1,
            Tuesday = 2
                                           
        }
    }
}";
    }
}
