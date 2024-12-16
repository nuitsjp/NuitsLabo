namespace FixedLengthFileStudy;

public interface IFixedLengthFileReader : IDisposable, IAsyncDisposable
{
    bool Read();
    string GetField(int index, int bytes);
}