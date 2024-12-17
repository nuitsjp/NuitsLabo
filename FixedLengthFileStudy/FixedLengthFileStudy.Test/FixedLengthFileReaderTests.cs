using System.Text;
using FluentAssertions;

namespace FixedLengthFileStudy.Test;

public abstract class FixedLengthFileReaderTestsBase
{
    static FixedLengthFileReaderTestsBase()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    protected abstract IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096);

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
    public void Read_MultipleLines_ShouldReadAllLines(string encodingName, string newLine, bool endWithNewLine)
    {
        // Arrange
        // ReSharper disable begin StringLiteralTypo
        var content = "ABCDE12345" + newLine + "FGHIJ67890" + (endWithNewLine ? newLine : string.Empty);
        // ReSharper disable end StringLiteralTypo
        using var stream = new MemoryStream(Encoding.GetEncoding(encodingName).GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, newLine);

        // Act & Assert
        reader.Read().Should().BeTrue();
        // ReSharper disable once StringLiteralTypo
        reader.GetField(0, 5).Should().Be("ABCDE");
        reader.GetField(5, 5).Should().Be("12345");

        reader.Read().Should().BeTrue();
        // ReSharper disable once StringLiteralTypo
        reader.GetField(0, 5).Should().Be("FGHIJ");
        reader.GetField(5, 5).Should().Be("67890");

        reader.Read().Should().BeFalse();
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
        var encoding = Encoding.GetEncoding(encodingName);
        var content =
            new string('A', 8000) + newLine
            + new string('A', 8000) + (endWithNewLine? newLine : string.Empty);  // 2行
        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = CreateReader(stream, encoding, newLine);

        // Act & Assert
        reader.Read().Should().BeTrue();
        reader.GetField(0, 100).Should().Be(new string('A', 100));
        reader.GetField(7900, 100).Should().Be(new string('A', 100));

        reader.Read().Should().BeTrue();
        reader.GetField(0, 100).Should().Be(new string('A', 100));
        reader.GetField(7900, 100).Should().Be(new string('A', 100));

        reader.Read().Should().BeFalse();
    }

    [Fact]
    public void Read_WithEmptyFile_ShouldReturnFalse()
    {
        // Arrange
        using var stream = new MemoryStream([]);
        using var reader = CreateReader(stream, Encoding.UTF8, "\r\n");

        // Act & Assert
        reader.Read().Should().BeFalse();
    }

    [Fact]
    public void GetField_WithPaddedData_ShouldTrimStartAndEndCorrectly()
    {
        // Arrange
        var content = " ABC  12  ";  // スペースでパディングされたデータ
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, "\r\n");

        // Act
        reader.Read();
        // Assert
        reader.GetField(0, 5).Should().Be("ABC");  // 末尾のスペースが除去される
        reader.GetField(5, 5).Should().Be("12");   // 末尾のスペースが除去される
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
    public void GetField_WithJapaneseCharacters_ShouldReadCorrectly(string encodingName, string newLine, bool endWithNewLine)
    {
        // Arrange

        var encoding = Encoding.GetEncoding(encodingName);
        var content = "あいうえお12345" + (endWithNewLine ? newLine : string.Empty);
        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = CreateReader(stream, encoding, newLine);

        // Act
        reader.Read();

        // Assert
        // Shift-JISでは日本語1文字が2バイトなので、5文字で10バイト
        var length = encoding.GetBytes("あいうえお").Length;
        reader.GetField(0, length).Should().Be("あいうえお");
        reader.GetField(length, 5).Should().Be("12345");
    }

    [Fact]
    public void GetField_WithInvalidIndex_ShouldThrowException()
    {
        // Arrange
        var content = "ABCDE";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, Environment.NewLine);

        // Act
        reader.Read();

        // Assert
        // ReSharper disable once AccessToDisposedClosure
        var action = () => reader.GetField(10, 1);
        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void GetField_WithInvalidLength_ShouldThrowException()
    {
        // Arrange
        var content = "ABCDE";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, Environment.NewLine);

        // Act
        reader.Read();

        // Assert
        // ReSharper disable once AccessToDisposedClosure
        var action = () => reader.GetField(0, 10);
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetField_BeforeRead_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var content = "ABCDE";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, Environment.NewLine);

        // Act & Assert
        // ReSharper disable once AccessToDisposedClosure
        var action = () => reader.GetField(0, 1);
        action.Should().Throw<InvalidOperationException>();
    }
}

public class ChatGptFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.ChatGPT.FixedLengthFileReader(reader, encoding, newLine, trim, bufferSize);
    }
}

public class ClaudeFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.Claude.FixedLengthFileReader(reader, encoding, newLine, trim, bufferSize);
    }
}


public class GeminiFixedLengthFileReaderTests : FixedLengthFileReaderTestsBase
{
    protected override IFixedLengthFileReader CreateReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096)
    {
        return new FixedLengthFileStudy.Gemini.FixedLengthFileReader(reader, encoding, newLine, trim, bufferSize);
    }
}
