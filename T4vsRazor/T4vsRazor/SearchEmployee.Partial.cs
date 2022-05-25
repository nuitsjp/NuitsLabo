namespace T4vsRazor;

public partial class SearchEmployee
{
    public SearchEmployee(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    string? FirstName { get; }
    string? LastName { get; }
}