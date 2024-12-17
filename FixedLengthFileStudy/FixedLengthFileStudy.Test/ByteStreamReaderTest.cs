using System.Text;
using FluentAssertions;

namespace FixedLengthFileStudy.Test;

public class ByteStreamReaderTest
{
    static ByteStreamReaderTest()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    //[Theory]
    //[InlineData("Shift_JIS", "\r")]
    //[InlineData("UTF-8", "\r")]
    //[InlineData("Shift_JIS", "\n")]
    //[InlineData("UTF-8", "\n")]
    //[InlineData("Shift_JIS", "\r\n")]
    //[InlineData("UTF-8", "\r\n")]
    //public void ReadTest(string encodingName, string newline)
    //{
    //    // Arrange
    //    var encoding = Encoding.GetEncoding(encodingName);
    //    var content = "123" + newline + "456" + newline + "789";
    //    var stream = new MemoryStream(encoding.GetBytes(content));
    //    using var reader = new ByteStreamReader(stream, encoding);

    //    // Act
    //    reader.ReadLine().Should().Be("123");
    //    reader.ReadLine().Should().Be("456");
    //    reader.ReadLine().Should().Be("789");
    //    reader.ReadLine().Should().BeNull();
    //}

    [Theory]
    [InlineData("Shift_JIS", "\r")]
    [InlineData("UTF-8", "\r")]
    [InlineData("Shift_JIS", "\n")]
    [InlineData("UTF-8", "\n")]
    [InlineData("Shift_JIS", "\r\n")]
    [InlineData("UTF-8", "\r\n")]
    public void ReadByteArrayTest(string encodingName, string newline)
    {
        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);
        var first = new string('あ', 8000);
        var second = new string('A', 4096);
        var third = "123";
        var forth = new string('B', 100);
        var content = first + newline + second + newline + third + newline + forth;
        var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = new ByteStreamReader(stream, encoding);

        // Act
        reader.ReadByteLine().Should().BeEquivalentTo(encoding.GetBytes(first));
        reader.ReadByteLine().Should().BeEquivalentTo(encoding.GetBytes(second));
        reader.ReadByteLine().Should().BeEquivalentTo(encoding.GetBytes(third));
        reader.ReadByteLine().Should().BeEquivalentTo(encoding.GetBytes(forth));
        reader.ReadByteLine().Should().BeNull();
    }

    [Fact]
    public void Dispose_SecondTime()
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
        var reader = new ByteStreamReader(stream, Encoding.UTF8);

        reader.Dispose();
        reader.Dispose();
    }

    [Fact]
    public void Constructor_StreamCanNotRead()
    {
        var stream = new MemoryStream();
        stream.Close();
        stream.CanRead.Should().BeFalse();
        Action action = () => new ByteStreamReader(stream, Encoding.UTF8);
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_BufferNegativeOrZero(int bufferSize)
    {
        using var stream = new MemoryStream();
        // ReSharper disable once AccessToDisposedClosure
        Action action = () => new ByteStreamReader(stream, Encoding.UTF8, bufferSize);
        action.Should().Throw<ArgumentException>();
    }

}