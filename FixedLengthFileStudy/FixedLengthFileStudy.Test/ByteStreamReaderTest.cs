﻿using System.Text;
using FluentAssertions;

namespace FixedLengthFileStudy.Test;

public class ByteStreamReaderTest
{
    static ByteStreamReaderTest()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [Theory]
    [InlineData("Shift_JIS", "\r")]
    [InlineData("UTF-8", "\r")]
    [InlineData("Shift_JIS", "\n")]
    [InlineData("UTF-8", "\n")]
    [InlineData("Shift_JIS", "\r\n")]
    [InlineData("UTF-8", "\r\n")]
    public void ReadTest(string encodingName, string newline)
    {
        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);
        var content = "123" + newline + "456" + newline + "789";
        var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = new ByteStreamReader(stream, encoding);

        // Act
        reader.ReadLine().Should().Be("123");
        reader.ReadLine().Should().Be("456");
        reader.ReadLine().Should().Be("789");
        reader.ReadLine().Should().BeNull();
    }
}