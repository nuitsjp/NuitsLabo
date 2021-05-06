using System.Text;

namespace Benchmarks
{
    public class StringBuilderBench
    {
        public string Namespace { get; set; }
        public string Name { get; set; }

        public string TransformText()
        {
            var builder = new StringBuilder(256);
            builder.Append("namespace ");
            builder.Append(Namespace);
            builder.Append("\r\n{\r\n    public class ");
            builder.Append(Name);
            builder.Append("\r\n    {\r\n        \r\n    }\r\n}");
            return builder.ToString();
        }

    }
}