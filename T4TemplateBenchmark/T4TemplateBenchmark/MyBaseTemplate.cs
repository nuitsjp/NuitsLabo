using System;
using System.Collections.Generic;
using System.Text;

namespace T4TemplateBenchmark
{
    public abstract class MyBaseTemplate
    {
        public static int Size { get; set; }
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Accessibility { get; set; }
        public List<string> Members { get; set; } = new();

        protected StringBuilder GenerationEnvironment { get; } = new (Size);

        public abstract string TransformText();

        public void Write(string text)
        {
            GenerationEnvironment.Append(text);
        }

        public class ToStringInstanceHelper
        {
            public string ToStringWithCulture(object objectToConvert)
            {
                return (string) objectToConvert;
            }
        }

        public ToStringInstanceHelper ToStringHelper { get; } = new ();
    }
}