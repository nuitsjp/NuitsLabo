using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T4TemplateBenchmark
{
    public class GenerateByStringBuilder
    {
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Accessibility { get; set; }
        public List<string> Members { get; set; } = new();

        private StringBuilder GenerationEnvironment { get; } = new();

        /// <summary>
        /// Create the template output
        /// </summary>
        public string TransformText()
        {
            GenerationEnvironment.Append("using System;\r\n\r\nnamespace ");
            GenerationEnvironment.Append((Namespace));
            GenerationEnvironment.Append("\r\n{\r\n    ");
            GenerationEnvironment.Append((Accessibility));
            GenerationEnvironment.Append(" partial ");
            GenerationEnvironment.Append((Type));
            GenerationEnvironment.Append(" ");
            GenerationEnvironment.Append((Name));
            GenerationEnvironment.Append(" : IComparable, IComparable<");
            GenerationEnvironment.Append((Name));
            GenerationEnvironment.Append(">\r\n    {\r\n#nullable disable\r\n        public int CompareTo(object other)\r\n#nullable enable\r\n        {\r\n            if (other is null) return 1;\r\n\r\n            if (other is ");
            GenerationEnvironment.Append((Name));
            GenerationEnvironment.Append(" concreteObject)\r\n            {\r\n                return CompareTo(concreteObject);\r\n            }\r\n\r\n            throw new ArgumentException(\"Object is not a ");
            GenerationEnvironment.Append((Namespace));
            GenerationEnvironment.Append(".");
            GenerationEnvironment.Append((Name));
            GenerationEnvironment.Append(".\");\r\n        }\r\n\r\n");

            if (Type == "class")
            {

                GenerationEnvironment.Append("#nullable disable\r\n");

            }

            GenerationEnvironment.Append("        public int CompareTo(");
            GenerationEnvironment.Append((Name));
            GenerationEnvironment.Append(" other)\r\n");

            if (Type == "class")
            {

                GenerationEnvironment.Append("#nullable enable\r\n");

            }

            GenerationEnvironment.Append("        {\r\n");

            if (Type == "class")
            {

                GenerationEnvironment.Append(@"            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

");

            }
            if (1 < Members.Count)
            {

                GenerationEnvironment.Append("            int compared;\r\n\r\n");

            }
            foreach (var member in Members)
            {


                if (member == Members.Last())
                {
                    if (Type == "class")
                    {

                        GenerationEnvironment.Append("            return LocalCompareTo(");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(", other.");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(");\r\n");

                    }
                    else
                    {

                        GenerationEnvironment.Append("            return ");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(".CompareTo(other.");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(");\r\n");

                    }
                }
                else
                {
                    if (Type == "class")
                    {

                        GenerationEnvironment.Append("            compared = LocalCompareTo(");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(", other.");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(");\r\n");

                    }
                    else
                    {

                        GenerationEnvironment.Append("            compared = ");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(".CompareTo(other.");
                        GenerationEnvironment.Append((member));
                        GenerationEnvironment.Append(");\r\n");

                    }

                    GenerationEnvironment.Append("            if (compared != 0) return compared;\r\n\r\n");

                }
            }

            GenerationEnvironment.Append("        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }

    }
}