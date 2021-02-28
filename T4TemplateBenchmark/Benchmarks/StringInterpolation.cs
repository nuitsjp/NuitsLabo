namespace Benchmarks
{
    public class StringInterpolation
    {
        public string Namespace { get; set; }
        public string Name { get; set; }

        public string TransformText()
        {
            return $@"namespace {Namespace}
{{
    public class {Name}
    {{
        
    }}
}}";
        }
    }
}