using DotLiquid;

namespace TextTemplateBenchmarks;

public class DotLiquidBenchmarks
{
    private const string Template = @"
select
	*
from
	Employee
where
	1 = 1
{% if FirstName %}
	and FirstName = @FirstName
{% endif %}
{% if LastName %}
	and LastName = @LastName
{% endif %}
";

    private readonly Template _template = DotLiquid.Template.Parse(Template);

    public string Render() => _template.Render(
        Hash.FromAnonymousObject(
            new Model("FirstName", "LastName")));

    public string RenderWithParse() => DotLiquid.Template
        .Parse(Template)
        .Render(
            Hash.FromAnonymousObject(
                new Model("FirstName", "LastName")));
}