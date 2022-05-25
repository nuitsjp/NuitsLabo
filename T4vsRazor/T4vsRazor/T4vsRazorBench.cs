using System.Security.Policy;
using BenchmarkDotNet.Attributes;
using DotLiquid;
using HandlebarsDotNet;
using RazorEngine;
using RazorEngine.Templating;
using Hash = DotLiquid.Hash;

namespace T4vsRazor;

public class T4VsRazorBench
{
    private readonly Template _template;
    private readonly Scriban.Template _scriban;

    public T4VsRazorBench()
    {
        var template = @"
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
}
";
        Engine.Razor.Compile(template, "SqlTemplate", typeof(RazorModel));

        _template = Template.Parse(@"
select
	*
from
	Employee
where
	1 = 1
{% if FirstName %}
	and FirstName = {{FirstName}}
{% endif %}
{% if LastName %}
	and LastName = {{LastName}}
{% endif %}
");  // Parses and compiles the template

        _scriban = Scriban.Template.Parse(@"
select
	*
from
	Employee
where
	1 = 1
{{ if firstname }}
	and FirstName = {{firstname}}
{{ end }}
{{ if lastname }}
	and LastName = {{lastname}}
{{ end }}
");
        _handlebars = Handlebars.Compile(@"
select
	*
from
	Employee
where
	1 = 1
{{ #if FirstName }}
	and FirstName = {{FirstName}}
{{ /if }}
{{ #if LastName }}
	and LastName = {{LastName}}
{{ /if }}
");
    }

    [Benchmark]
    public string T4()
    {
        SearchEmployee searchEmployee = new("FistName", "LastName");
        return searchEmployee.TransformText();
    }

    [Benchmark]
    public string Razor()
    {
        return Engine
            .Razor
            .Run("SqlTemplate", typeof(RazorModel), new RazorModel("FirstName", "LastName"));
    }

    [Benchmark]
    public string RazorWithCompile()
    {
        var template = @"
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
}
";
        return Engine
            .Razor
            .RunCompile(template, "SqlTemplate", typeof(RazorModel), new RazorModel("FirstName", "LastName"));
    }

    [Benchmark(Description = "Scriban")]
    public string ScribanRender()
    {
        return _scriban.Render(
            new
            {
                Firstname = "FirstName",
                Lastname = "LastName"
            });
    }

    public string ScribanWithParse()
    {
        var scriban = Scriban.Template.Parse(@"
select
	*
from
	Employee
where
	1 = 1
{{ if firstname }}
	and FirstName = {{firstname}}
{{ end }}
{{ if lastname }}
	and LastName = {{lastname}}
{{ end }}
");
        return scriban.Render(
            new
            {
                Firstname = "FirstName",
                Lastname = "LastName"
            });
    }


    [Benchmark]
    public string DotLiquid()
    {
        return _template.Render(
            Hash.FromAnonymousObject(
                new RazorModel("FirstName", "LastName"))); // Renders the output => "hi tobi"
    }

    [Benchmark]
    public string DotLiquidWithParse()
    {
        Template template = Template.Parse(@"
select
	*
from
	Employee
where
	1 = 1
{% if FirstName %}
	and FirstName = {{FirstName}}
{% endif %}
{% if LastName %}
	and LastName = {{LastName}}
{% endif %}
");  // Parses and compiles the template
        return template.Render(
            Hash.FromAnonymousObject(
                new RazorModel("FirstName", "LastName"))); // Renders the output => "hi tobi"
    }


    private readonly HandlebarsTemplate<object, object> _handlebars;

    [Benchmark(Description = "Handlebars")]
    public string HandlebarsInvoke()
    {
        return _handlebars(new
        {
            FirstName = "FirstName",
            LastName = "LastName"
        });
    }

    [Benchmark]
    public string HandlebarsWithParse()
    {
        return Handlebars.Compile(@"
select
	*
from
	Employee
where
	1 = 1
{{ #if FirstName }}
	and FirstName = {{FirstName}}
{{ /if }}
{{ #if LastName }}
	and LastName = {{LastName}}
{{ /if }}
")(new
        {
            FirstName = "FirstName",
            LastName = "LastName"
        });
    }
}

public class RazorModel
{
    public RazorModel(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }
}