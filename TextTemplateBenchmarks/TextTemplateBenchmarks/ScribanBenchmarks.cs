using Scriban;

namespace TextTemplateBenchmarks;

public class ScribanBenchmarks
{
    private const string Template = @"
select
	*
from
	Employee
where
	1 = 1
{{ if first_name }}
	and FirstName = @FirstName
{{ end }}
{{ if last_name }}
	and LastName = @LastName
{{ end }}
";

    private readonly Template _template;

    public ScribanBenchmarks()
    {
        _template= Scriban.Template.Parse(Template);
	}

    public string Render() => _template.Render(new Model("FirstName", "LastName"));

    public string RenderWithParse() => Scriban.Template
        .Parse(Template)
        .Render(new Model("FirstName", "LastName"));

}