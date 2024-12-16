using System.Text;
using FixedLengthFileStudy.Gemini;
using FluentAssertions;

namespace FixedLengthFileStudy.Test.Gemini;

public class FixedLengthFileReaderTests
{
    [Fact]
    public void Read_ValidData_ReturnsTrue()
    {
        // Arrange
        string testData = "0123456789ABCDEFGHIJ\r\nKLMNOPQRSTUVWXYZ012345\r\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");

        // Act
        bool result = reader.Read();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Read_EmptyStream_ReturnsFalse()
    {
        // Arrange
        using MemoryStream stream = new MemoryStream(Array.Empty<byte>());
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");

        // Act
        bool result = reader.Read();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Read_NoNewLine_ReturnsFalse()
    {
        // Arrange
        string testData = "0123456789ABCDEFGHIJ";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");

        // Act
        bool result = reader.Read();

        // Assert
        result.Should().BeFalse();
    }



    [Fact]
    public void GetField_ValidInput_ReturnsCorrectValue()
    {
        // Arrange
        string testData = "0123456789ABCDEFGHIJ\r\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");
        reader.Read(); // Read the line

        // Act
        ReadOnlySpan<byte> field = reader.GetField(5, 5);
        string fieldString = Encoding.UTF8.GetString(field);

        // Assert
        fieldString.Should().Be("56789");
    }


    [Fact]
    public void GetFieldString_ValidInput_ReturnsCorrectString()
    {
        // Arrange
        string testData = "あいうえおかきくけこ\r\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");
        reader.Read();

        // Act
        string field = reader.GetFieldString(3, 9);

        // Assert
        field.Should().Be("えおかきく");
    }

    [Fact]
    public void GetField_InvalidIndex_ThrowsException()
    {
        // Arrange
        string testData = "0123456789\r\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");
        reader.Read();

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => reader.GetField(15, 5));
    }

    [Fact]
    public void MultipleReads_ValidData_ReturnsCorrectValues()
    {
        // Arrange
        string testData = "0123456789\r\nABCDEFGHIJ\r\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");

        // Act & Assert
        reader.Read().Should().BeTrue();
        Encoding.UTF8.GetString(reader.GetField(0, 5)).Should().Be("01234");

        reader.Read().Should().BeTrue();
        Encoding.UTF8.GetString(reader.GetField(0, 5)).Should().Be("ABCDE");
    }

    [Fact]
    public void DifferentNewLine_ValidData_ReturnsCorrectValues()
    {
        // Arrange
        string testData = "0123456789\nABCDEFGHIJ\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\n");

        // Act & Assert
        reader.Read().Should().BeTrue();
        Encoding.UTF8.GetString(reader.GetField(0, 5)).Should().Be("01234");

        reader.Read().Should().BeTrue();
        Encoding.UTF8.GetString(reader.GetField(0, 5)).Should().Be("ABCDE");
    }

    [Theory]
    [InlineData("あいうえおかきくけこ", 0, 3, "あい")] // 先頭から3文字
    [InlineData("あいうえおかきくけこ", 3, 6, "うえおか")] // 中間から6文字
    [InlineData("あいうえおかきくけこ", 6, 9, "きくけこ")] // 末尾まで
    [InlineData("abcあいうdef", 3, 9, "あいう")] // 英数字と日本語の混在
    [InlineData("漢字テスト文字列", 0, 6, "漢字テ")] // 漢字を含む
    [InlineData("ビール乾杯", 0, 9, "ビール")] // 絵文字を含む
    public void GetFieldString_VariousInput_ReturnsCorrectString(string testData, int index, int bytes, string expected)
    {
        // Arrange
        testData += "\r\n"; // 改行を追加
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");
        reader.Read();

        // Act
        string field = reader.GetFieldString(index, bytes);

        // Assert
        field.Should().Be(expected);
    }

    [Fact]
    public void GetFieldString_EmptyField_ReturnsEmptyString()
    {
        // Arrange
        string testData = "0123456789\r\n";
        using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));
        using FixedLengthFileReader reader = new FixedLengthFileReader(stream, Encoding.UTF8, "\r\n");
        reader.Read();

        //Act
        string field = reader.GetFieldString(5, 0);
        //Assert
        field.Should().BeEmpty();

    }
}