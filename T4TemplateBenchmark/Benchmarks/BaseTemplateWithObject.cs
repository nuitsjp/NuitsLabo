using System.Collections.Generic;
using System.Text;

namespace Benchmarks
{
    public abstract class BaseTemplateWithObject
    {
        public static int Size { get; set; } = 256;
        public string Namespace { get; set; }
        public string Name { get; set; }

        protected StringBuilder GenerationEnvironment { get; } = new StringBuilder(Size);

        public abstract string TransformText();

        public void Write(string text)
        {
            GenerationEnvironment.Append(text);
        }

        public class ToStringInstanceHelper
        {
            public string ToStringWithCulture(object objectToConvert)
            {
                return (string)objectToConvert;
            }
        }

        public ToStringInstanceHelper ToStringHelper { get; } = new ToStringInstanceHelper();

    }
}