using System;
using System.Text;

namespace T4TemplateBenchmark
{
    public abstract class MyBaseTemplate
    {
        protected StringBuilder GenerationEnvironment { get; } = new ();

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