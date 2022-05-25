using System.Text;

namespace T4vsRazor;

public abstract class TemplateBase
{
    protected StringBuilder GenerationEnvironment { get; } = new();

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

    public ToStringInstanceHelper ToStringHelper { get; } = new();

    public abstract string TransformText();
}