namespace Extensions;

public static class EnumerableExtensions
{
    public static bool Empty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Any() is false;
    }
}
