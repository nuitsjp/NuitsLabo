using HandlebarsDotNet;

namespace TextTemplateBenchmarks;

public class HandlebarsBenchmarks
{
    private const string Template = @"
select
	*
from
	Employee
where
	1 = 1
{{ #if FirstName }}
	and FirstName = @FirstName
{{ /if }}
{{ #if LastName }}
	and LastName = @LastName
{{ /if }}
";

    private readonly HandlebarsTemplate<object, object> _handlebars = Handlebars.Compile(Template);

    public string Invoke() => _handlebars(new Model("FirstName", "LastName"));
    public string InvokeWithCompile() => Handlebars.Compile(Template)(new Model("FirstName", "LastName"));

}