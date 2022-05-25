namespace TextTemplateBenchmarks;

public class Model
{
    public Model(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string? FirstName { get; }
    public string? LastName { get; }
}