using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotLiquid;
using RazorEngine;
using RazorEngine.Templating;

namespace T4vsRazor;

public class T4vsRazorBench
{
    private Template _template;
    public T4vsRazorBench()
    {
        var template = @"
select
	*
from
	Employy
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
	Employy
where
	1 = 1
{% if FirstName %}
	and FirstName = {{FirstName}}
{% endif %}
{% if LastName %}
	and LastName = {{LastName}}
{% endif %}
");  // Parses and compiles the template

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
	Employy
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