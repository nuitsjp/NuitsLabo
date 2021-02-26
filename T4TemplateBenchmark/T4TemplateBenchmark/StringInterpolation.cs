using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T4TemplateBenchmark
{
    public class StringInterpolation
    {
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Accessibility { get; set; }
        public List<string> Members { get; set; } = new();

        public string InvokeStringInterpolation()
        {
            return $@"using System;

namespace {Namespace}
{{
    {Accessibility} partial {Type} {Name} : IComparable, IComparable<{Name}>
    {{
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {{
            if (other is null) return 1;

            if (other is {Name} concreteObject)
            {{
                return CompareTo(concreteObject);
            }}

            throw new ArgumentException(""Object is not a {Namespace}.{Name}."");
        }}

{(Type == "class" ? "#nullable disable" : string.Empty)}
        public int CompareTo({Name} other)
{(Type == "class" ? "#nullable enable" : string.Empty)}
        {{
{(Type == "class" ? @"            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {{
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }}

" : string.Empty)}
{(1 < Members.Count ? @"            int compared;

" : string.Empty)}
{ToStringMembers()}
        }}
    }}
}}
";
        }

        private string ToStringMembers()
        {
            var builder = new StringBuilder();

            foreach (var member in Members)
            {
                builder.Append($@"
{(member == Members.Last() 
                    ? Type == "class"
                        ? $"return LocalCompareTo({member}, other.{member});"
                        : $"return {member}.CompareTo(other.{member});"
                    : $@"{(Type == "class"
                        ? $"compared = LocalCompareTo({member}, other.{member});"
                        : $"compared = {member}.CompareTo(other.{member});;")}
            if (compared != 0) return compared;

")}");
                
            }

            return builder.ToString();
        }
    }
}