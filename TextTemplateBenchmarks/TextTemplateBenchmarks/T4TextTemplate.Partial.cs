namespace TextTemplateBenchmarks;

public partial class T4TextTemplate
{
    public T4TextTemplate(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    string? FirstName { get; }
    string? LastName { get; }
}