using System.Text;

namespace SqlBulkCopyStudy;

public class FixedLengthFileReader(
    Stream reader, 
    Encoding encoding, 
    string newLine) : IDisposable, IAsyncDisposable
{


    public bool Read()
    {
        throw new NotImplementedException();
    }

    public string GetField(int index, int bytes)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await reader.DisposeAsync();
    }
}