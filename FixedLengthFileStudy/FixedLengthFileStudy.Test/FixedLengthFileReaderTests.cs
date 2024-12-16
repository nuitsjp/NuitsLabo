using System.Text;
using FluentAssertions;

namespace FixedLengthFileStudy.Test;

public abstract class FixedLengthFileReaderTestsBase
{
    static FixedLengthFileReaderTestsBase()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    protected abstract IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096);

    [Theory]
    [InlineData("Shift_JIS", "\r\n", true)]
    [InlineData("UTF-8", "\r\n", true)]
    [InlineData("Shift_JIS", "\n", true)]
    [InlineData("UTF-8", "\n", true)]
    [InlineData("Shift_JIS", "\r\n", false)]
    [InlineData("UTF-8", "\r\n", false)]
    [InlineData("Shift_JIS", "\n", false)]
    [InlineData("UTF-8", "\n", false)]
    public void Read_SingleLine_ShouldReturnTrueAndReadCorrectData(string encodingName, string newLine, bool endWithNewLine)
    {
        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);

        // ReSharper disable StringLiteralTypo
        var content = "ABCDE12345FGHIJ" + (endWithNewLine ? newLine : string.Empty);
        // ReSharper restore StringLiteralTypo

        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = CreateReader(stream, encoding, newLine);

        // Act
        var result = reader.Read();

        // Assert
        result.Should().BeTrue();

        // ReSharper disable StringLiteralTypo
        reader.GetField(0, 5).Should().Be("ABCDE");
        reader.GetField(5, 5).Should().Be("12345");
        reader.GetField(10, 5).Should().Be("FGHIJ");
        // ReSharper restore StringLiteralTypo
    }

    [Theory]
    [InlineData("Shift_JIS", "\r\n", true)]
    [InlineData("UTF-8", "\r\n", true)]
    [InlineData("Shift_JIS", "\n", true)]
    [InlineData("UTF-8", "\n", true)]
    [InlineData("Shift_JIS", "\r\n", false)]
    [InlineData("UTF-8", "\r\n", false)]
    [InlineData("Shift_JIS", "\n", false)]
    [InlineData("UTF-8", "\n", false)]
    public void Read_WithLargeFile_ShouldHandleBufferBoundaries(string encodingName, string newLine, bool endWithNewLine)
    {
        // Arrange
        var line = new string('A', 8000) + newLine;  // バッファーサイズ（4096）より大きいライン
        var content =
            new string('A', 8000) + newLine
            + new string('A', 8000) + (endWithNewLine? newLine : string.Empty);  // 2行
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, newLine);

        // Act & Assert
        reader.Read().Should().BeTrue();
        reader.GetField(0, 8000).Should().Be(new string('A', 8000));
        reader.GetField(7900, 100).Should().Be(new string('A', 100));

        reader.Read().Should().BeTrue();
        reader.GetField(0, 100).Should().Be(new string('A', 100));
        reader.GetField(7900, 100).Should().Be(new string('A', 100));

        reader.Read().Should().BeFalse();
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
