using System.Collections.Generic;

namespace T4TemplateBenchmark
{
    public partial class CodeTemplateWithMyBaseTemplate
    {
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Accessibility { get; set; }
        public List<string> Members { get; set; } = new();

    }
}