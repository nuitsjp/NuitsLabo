namespace T4vsRazor;

public partial class SearchEmployee
{
    public SearchEmployee(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }
}