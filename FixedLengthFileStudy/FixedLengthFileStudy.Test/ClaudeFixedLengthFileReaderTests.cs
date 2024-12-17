using System.Text;

namespace FixedLengthFileStudy.Test;

public class ClaudeFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.Claude.FixedLengthFileReader(reader, encoding, newLine, trim, bufferSize);
    }
}