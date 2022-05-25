using System.Text;

namespace T4vsRazor;

public abstract class TemplateBase
{
    protected StringBuilder GenerationEnvironment { get; } = new();

    public void Write(string text)
    {
        GenerationEnvironment.Append(text);
    }

    public abstract string TransformText();
}