using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers;

namespace T4TemplateBenchmark
{
    public class GenerateSource
    {
        [Benchmark]
        public void T4Template()
        {
            var source = new CodeTemplate
            {
                Namespace = "MyNamespace",
                Name = "MyClass",
                Type = "class",
                Accessibility = "public",
                Members = new List<string>
                {
                    "Value1", "Value2", "Value3"
                }
            }.TransformText();
        }

        [Benchmark]
        public void CustomT4Template()
        {
            var source = new CodeTemplateWithMyBaseTemplate()
            {
                Namespace = "MyNamespace",
                Name = "MyClass",
                Type = "class",
                Accessibility = "public",
                Members = new List<string>
                {
                    "Value1", "Value2", "Value3"
                }
            }.TransformText();
        }

        [Benchmark]
        public void StringInterpolation()
        {
            var source = new StringInterpolation
            {
                Namespace = "MyNamespace",
                Name = "MyClass",
                Type = "class",
                Accessibility = "public",
                Members = new List<string>
                {
                    "Value1", "Value2", "Value3"
                }
            }.InvokeStringInterpolation();
        }

        [Benchmark]
        public void StringBuilder()
        {
            var source = new GenerateByStringBuilder()
            {
                Namespace = "MyNamespace",
                Name = "MyClass",
                Type = "class",
                Accessibility = "public",
                Members = new List<string>
                {
                    "Value1", "Value2", "Value3"
                }
            }.TransformText();
        }
    }
}