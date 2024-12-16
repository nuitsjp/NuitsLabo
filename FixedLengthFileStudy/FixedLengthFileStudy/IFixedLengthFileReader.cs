namespace FixedLengthFileStudy;

public interface IFixedLengthFileReader : IDisposable, IAsyncDisposable
{
    Trim Trim { get; }
    bool Read();
    string GetField(int index, int bytes);
}