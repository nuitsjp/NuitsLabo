using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Diagnostics.Tracing.Parsers;

namespace T4TemplateBenchmark
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class SourceGeneratorBenchmarks
    {
        [Params(2048)] 
        public int Capacity;

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
        public void CustomT4Template01()
        {
            MyBaseTemplate01.Size = Capacity;
            var source = new CustomT4Template01()
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
        public void CustomT4Template02()
        {
            MyBaseTemplate01.Size = Capacity;
            var source = new CustomT4Template02()
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
            GenerateByStringBuilder.Size = Capacity;
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

        //[Benchmark]
        //public void NoGenerate()
        //{
        //    var source = new CodeTemplate()
        //    {
        //        Namespace = "MyNamespace",
        //        Name = "MyClass",
        //        Type = "class",
        //        Accessibility = "public",
        //        Members = new List<string>
        //        {
        //            "Value1", "Value2", "Value3"
        //        }
        //    };
        //}
//        private string Namespace = "MyNamespace";
//        private string Name = "MyClass";

//        [Benchmark]
//        public void StringInterpolation()
//        {
//            var source = $@"
//namespace {Namespace}
//{{
//    public partial class {Name}
//    {{
//        public string? Namespace {{ get; set; }}
//        public string? Name {{ get; set; }}
//        public string? Type {{ get; set; }}
//        public string? Accessibility {{ get; set; }}
//        public List<string> Members {{ get; set; }} = new();
//    }}
//}}";
//        }

//        [Benchmark]
//        public void StringBuilder()
//        {
//            var builder = new StringBuilder(@"namespace ", 321);
//            builder.Append(Namespace);
//            builder.Append(@"
//{
//    public partial class ");
//            builder.Append(Name);
//            builder.Append(@"
//    {
//        public string? Namespace { get; set; }
//        public string? Name { get; set; }
//        public string? Type { get; set; }
//        public string? Accessibility { get; set; }
//        public List<string> Members { get; set; } = new();
//    }
//}");

//            var source = builder.ToString();
//        }

    }
}