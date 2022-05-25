using RazorEngine;
using RazorEngine.Templating;

namespace RazorSqlTemplate
{
    public class QueryTemplate
    {
        public string Query()
        {
            return Engine
                .Razor
                .RunCompile(File.ReadAllText("BlazorPage1.sql"),
                    "templateKey",
                    null,
                    new
                    {
                        Foo = (string?)null,
                        Name = "World"
                    });
        }
    }
}