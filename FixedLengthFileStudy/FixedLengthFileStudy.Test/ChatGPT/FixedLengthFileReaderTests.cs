using System.Text;
using FixedLengthFileStudy.ChatGPT;
using FluentAssertions;

namespace FixedLengthFileStudy.Test.ChatGPT;

public class FixedLengthFileReaderTests
{
    private const string NewLine = "\r\n";

    [Fact]
    public void Read_SingleLine_ShouldReturnTrueAndReadCorrectData()
    {
        // Arrange
        var content = "ABCDE12345FGHIJ" + NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act
        var result = reader.Read();

        // Assert
        result.Should().BeTrue();
        reader.GetField(0, 5).Should().Be("ABCDE");
        reader.GetField(5, 5).Should().Be("12345");
        reader.GetField(10, 5).Should().Be("FGHIJ");
    }

    [Fact]
    public void Read_MultipleLines_ShouldReadAllLines()
    {
        // Arrange
        var content = "ABCDE12345" + NewLine + "FGHIJ67890" + NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act & Assert
        reader.Read().Should().BeTrue();
        reader.GetField(0, 5).Should().Be("ABCDE");
        reader.GetField(5, 5).Should().Be("12345");

        reader.Read().Should().BeTrue();
        reader.GetField(0, 5).Should().Be("FGHIJ");
        reader.GetField(5, 5).Should().Be("67890");

        reader.Read().Should().BeFalse();
    }

    [Fact]
    public void GetField_WithJapaneseCharacters_ShouldReadCorrectly()
    {
        // Arrange
        var content = "あいうえお12345" + NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act
        reader.Read();

        // Assert
        // UTF-8では日本語1文字が3バイトなので、5文字で15バイト
        reader.GetField(0, 15).Should().Be("あいうえお");
        reader.GetField(15, 5).Should().Be("12345");
    }

    [Fact]
    public void GetField_WithShiftJIS_ShouldReadCorrectly()
    {
        // Arrange
        var encoding = Encoding.GetEncoding("shift-jis");
        var content = "あいうえお12345" + NewLine;
        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, encoding, NewLine);

        // Act
        reader.Read();

        // Assert
        // Shift-JISでは日本語1文字が2バイトなので、5文字で10バイト
        reader.GetField(0, 10).Should().Be("あいうえお");
        reader.GetField(10, 5).Should().Be("12345");
    }

    [Fact]
    public void GetField_WithInvalidIndex_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var content = "ABCDE" + NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act
        reader.Read();

        // Assert
        var action = () => reader.GetField(10, 1);
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetField_BeforeRead_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var content = "ABCDE" + NewLine;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act & Assert
        var action = () => reader.GetField(0, 1);
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Read_WithLargeFile_ShouldHandleBufferBoundaries()
    {
        // Arrange
        var line = new string('A', 8000) + NewLine;  // バッファーサイズ（4096）より大きいライン
        var content = line + line;  // 2行
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

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
        using var stream = new MemoryStream(Array.Empty<byte>());
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act & Assert
        reader.Read().Should().BeFalse();
    }

    [Fact]
    public void GetField_WithPaddedData_ShouldTrimEndCorrectly()
    {
        // Arrange
        var content = "ABC  12   " + NewLine;  // スペースでパディングされたデータ
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act
        reader.Read();

        // Assert
        reader.GetField(0, 5).Should().Be("ABC");  // 末尾のスペースが除去される
        reader.GetField(5, 5).Should().Be("12");   // 末尾のスペースが除去される
    }
}