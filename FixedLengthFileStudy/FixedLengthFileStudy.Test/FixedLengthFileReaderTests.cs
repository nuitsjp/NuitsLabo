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
    public async Task DisposeAsync()
    {
        // Arrange
        using var stream = new MemoryStream("1234"u8.ToArray());
        await using var reader = CreateReader(stream, Encoding.UTF8, "\r\n");

        // Act & Assert
        reader.Read().Should().BeTrue();
    }

    [Fact]
    public async Task DisposeAsync_WhenNotRead()
    {
        // Arrange
        using var stream = new MemoryStream([]);
        await using var reader = CreateReader(stream, Encoding.UTF8, "\r\n");
    }


    [Fact]
    public void GetField_WithPaddedData_ShouldTrimNoneCorrectly()
    {
        // Arrange
        var content = " ABC  12  ";  // スペースでパディングされたデータ
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, "\r\n", trim: Trim.None);

        // Act
        reader.Read();
        // Assert
        reader.GetField(0, 5).Should().Be(" ABC ");  // 末尾のスペースが除去される
        reader.GetField(5, 5).Should().Be(" 12  ");   // 末尾のスペースが除去される
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

    [Fact]
    public void GetField_WithPaddedData_ShouldTrimStartCorrectly()
    {
        // Arrange
        var content = " ABC  12  ";  // スペースでパディングされたデータ
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, "\r\n", trim: Trim.Start);

        // Act
        reader.Read();
        // Assert
        reader.GetField(0, 5).Should().Be("ABC ");  // 末尾のスペースが除去される
        reader.GetField(5, 5).Should().Be("12  ");   // 末尾のスペースが除去される
    }

    [Fact]
    public void GetField_WithPaddedData_ShouldTrimEndCorrectly()
    {
        // Arrange
        var content = " ABC  12  ";  // スペースでパディングされたデータ
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, "\r\n", trim: Trim.End);

        // Act
        reader.Read();
        // Assert
        reader.GetField(0, 5).Should().Be(" ABC");  // 末尾のスペースが除去される
        reader.GetField(5, 5).Should().Be(" 12");   // 末尾のスペースが除去される
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

    [Theory]
    [InlineData("utf-8")]
    [InlineData("shift-jis")]
    public void GetField_WithVariousJapaneseCharacters_ShouldReadCorrectly(string encodingName)
    {
        const string newLine = "\r\n";

        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);
        var content = "漢字かなカナ１２３" + newLine +  // 漢字、ひらがな、カタカナ、全角数字
                     "まつもと太郎    " + newLine +     // 氏名（パディング付き）
                     "ﾊﾝｶｸｶﾀｶ123" + newLine +         // 半角カタカナと半角数字
                     "住所　東京都　" + newLine +       // 全角スペース含む
                     "㈱企業♪☆○" + newLine +           // 機種依存文字
                     "ｱｲｳｴｵあいうえお" + newLine;      // 半角・全角カナ混在

        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = CreateReader(stream, encoding, newLine);

        // Act & Assert
        // 1行目: 漢字、ひらがな、カタカナ、全角数字
        reader.Read().Should().BeTrue();
        var bytesPerChar = encodingName == "utf-8" ? 3 : 2;  // UTF-8は3バイト、Shift-JISは2バイト
        reader.GetField(0, bytesPerChar * 2).Should().Be("漢字");
        reader.GetField(bytesPerChar * 2, bytesPerChar * 2).Should().Be("かな");
        reader.GetField(bytesPerChar * 4, bytesPerChar * 2).Should().Be("カナ");
        reader.GetField(bytesPerChar * 6, bytesPerChar * 3).Should().Be("１２３");

        // 2行目: 氏名（パディング付き）
        reader.Read().Should().BeTrue();
        reader.GetField(0, bytesPerChar * 7).Should().Be("まつもと太郎");  // パディングは自動で除去される

        // 3行目: 半角カタカナと半角数字
        reader.Read().Should().BeTrue();
        reader.GetField(0, encoding.GetBytes("ﾊﾝｶｸｶﾀｶ").Length).Should().Be("ﾊﾝｶｸｶﾀｶ");  // 半角カタカナは1バイト
        reader.GetField(encoding.GetBytes("ﾊﾝｶｸｶﾀｶ").Length, 3).Should().Be("123");      // 半角数字は1バイト

        // 4行目: 全角スペース含む
        reader.Read().Should().BeTrue();
        var address = reader.GetField(0, bytesPerChar * 6);
        address.Should().Be("住所　東京都");  // 全角スペースが保持される
        address.Length.Should().Be(6);        // 文字数を確認

        // 5行目: 機種依存文字
        reader.Read().Should().BeTrue();
        reader.GetField(0, bytesPerChar * 5).Should().Be("㈱企業♪☆");

        // 6行目: 半角・全角カナ混在
        reader.Read().Should().BeTrue();
        var mixedKana = reader.GetField(0, encoding.GetBytes("ｱｲｳｴｵあいうえお").Length);  // 半角5文字 + 全角5文字
        mixedKana.Should().Be("ｱｲｳｴｵあいうえお");
        mixedKana.Length.Should().Be(10);  // 文字数を確認
    }

    [Fact]
    public void GetField_WithSurrogatePairs_ShouldReadCorrectly()
    {
        const string newLine = "\r\n";
        // Arrange
        var content = "🎌日本🗾観光" + newLine +  // サロゲートペア文字（絵文字、記号）
                      "👨👩👧👦家族" + newLine;     // 連続するサロゲートペア

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, newLine);

        // Act & Assert
        // 1行目
        reader.Read().Should().BeTrue();
        reader.GetField(0, "🎌"u8.ToArray().Length).Should().Be("🎌");
        reader.GetField("🎌"u8.ToArray().Length, "日本"u8.ToArray().Length).Should().Be("日本");
        reader.GetField("🎌日本"u8.ToArray().Length, "🗾"u8.ToArray().Length).Should().Be("🗾");
        reader.GetField("🎌日本🗾"u8.ToArray().Length, "観光"u8.ToArray().Length).Should().Be("観光");

        // 2行目
        reader.Read().Should().BeTrue();
        reader.GetField(0, "👨👩👧👦"u8.ToArray().Length).Should().Be("👨👩👧👦");
    }

    [Theory]
    [InlineData("utf-8", "株式会社ABC Company", "株式会社ABC Company")]
    [InlineData("shift-jis", "株式会社ABC Company", "株式会社ABC Company")]
    [InlineData("utf-8", "ABC漢字123かなカナ", "ABC漢字")]
    [InlineData("shift-jis", "ABC漢字123かなカナ", "ABC漢字")]
    [InlineData("utf-8", "1234５６７８9012", "1234５６７８9012")]
    [InlineData("shift-jis", "1234５６７８9012", "1234５６７８9012")]
    [InlineData("utf-8", "ﾃｽﾄテストTest", "ﾃｽﾄテストTest")]
    [InlineData("shift-jis", "ﾃｽﾄテストTest", "ﾃｽﾄテストTest")]
    public void GetField_WithMixedWidthCharacters_ShouldHandleCorrectly(string encodingName, string content, string expected)
    {
        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);

        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = CreateReader(stream, encoding, Environment.NewLine);

        // Act & Assert
        reader.Read().Should().BeTrue();
        var companyName = reader.GetField(0, encoding.GetBytes(expected).Length);
        companyName.Should().Be(expected);
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
    public void GetField_WithMinusIndex_ShouldThrowException()
    {
        // Arrange
        var content = "ABCDE";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, Environment.NewLine);

        // Act
        reader.Read();

        // Assert
        // ReSharper disable once AccessToDisposedClosure
        var action = () => reader.GetField(-1, 1);
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetField_WithMinusBytes_ShouldThrowException()
    {
        // Arrange
        var content = "ABCDE";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, Environment.NewLine);

        // Act
        reader.Read();

        // Assert
        // ReSharper disable once AccessToDisposedClosure
        var action = () => reader.GetField(1, -1);
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetField_WithInvalidTrim_ShouldThrowException()
    {
        // Arrange
        var content = "ABCDE";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = CreateReader(stream, Encoding.UTF8, Environment.NewLine, trim: (Trim)int.MaxValue);

        // Act
        reader.Read();

        // Assert
        // ReSharper disable once AccessToDisposedClosure
        var action = () => reader.GetField(0, 1);
        action.Should().Throw<InvalidOperationException>();
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