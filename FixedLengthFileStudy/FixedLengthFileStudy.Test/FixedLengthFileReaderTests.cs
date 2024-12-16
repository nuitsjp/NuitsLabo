using System.Text;
using FluentAssertions;

namespace FixedLengthFileStudy.Test;

public abstract class FixedLengthFileReaderTestsBase
{
    private const string NewLine = "\r\n";

    protected abstract IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096);

    [Fact]
    public void Read_SingleLine_ShouldReturnTrueAndReadCorrectData()
    {
        // Arrange
        var content = "ABCDE12345FGHIJ" + NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, NewLine);

        // Act
        var result = reader.Read();

        // Assert
        result.Should().BeTrue();
        reader.GetField(0, 5).Should().Be("ABCDE");
        reader.GetField(5, 5).Should().Be("12345");
        reader.GetField(10, 5).Should().Be("FGHIJ");
    }

}

public class ChatGptFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.ChatGPT.FixedLengthFileReader(reader, encoding, newLine, bufferSize);
    }
}

public class ClaudeFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.Claude.FixedLengthFileReader(reader, encoding, newLine, bufferSize);
    }
}


public class GeminiFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.Gemini.FixedLengthFileReader(reader, encoding, newLine, bufferSize);
    }
}
