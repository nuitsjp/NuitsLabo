using RazorEngine;
using RazorEngine.Templating;

namespace TextTemplateBenchmarks;

public class RazorBenchmarks
{
    private const string Template = @"
select
	*
from
	Employee
where
	1 = 1
@if (@Model.FirstName != null) {
	@Model.FirstName
}
@if (@Model.LastName != null) {
	@Model.LastName
}";

    public RazorBenchmarks()
    {
        Engine.Razor.Compile(Template, "SqlTemplate", typeof(Model));
    }

    public string Run()
    {
        return Engine
            .Razor
            .Run("SqlTemplate", typeof(Model), new Model("FirstName", "LastName"));
    }

    public string RunWithCompile()
    {
        return Engine
            .Razor
            .RunCompile(Template, "SqlTemplate", typeof(Model), new Model("FirstName", "LastName"));

    }
}