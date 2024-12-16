using System.Text;
using FixedLengthFileStudy.Claude;
using FluentAssertions;

namespace FixedLengthFileStudy.Test.Claude;

public class FixedLengthFileReaderTests
{
    static FixedLengthFileReaderTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

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
        using var stream = new MemoryStream([]);
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

    [Theory]
    [InlineData("utf-8")]
    [InlineData("shift-jis")]
    public void GetField_WithVariousJapaneseCharacters_ShouldReadCorrectly(string encodingName)
    {
        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);
        var content = "漢字かなカナ１２３" + NewLine +  // 漢字、ひらがな、カタカナ、全角数字
                     "まつもと太郎    " + NewLine +     // 氏名（パディング付き）
                     "ﾊﾝｶｸｶﾀｶﾅ123" + NewLine +         // 半角カタカナと半角数字
                     "住所　東京都　" + NewLine +       // 全角スペース含む
                     "㈱企業♪☆○" + NewLine +           // 機種依存文字
                     "ｱｲｳｴｵあいうえお" + NewLine;      // 半角・全角カナ混在

        using var stream = new MemoryStream(encoding.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, encoding, NewLine);

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
        reader.GetField(0, 7).Should().Be("ﾊﾝｶｸｶﾀｶﾅ");  // 半角カタカナは1バイト
        reader.GetField(7, 3).Should().Be("123");      // 半角数字は1バイト

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
        var mixedKana = reader.GetField(0, 5 + (bytesPerChar * 5));  // 半角5文字 + 全角5文字
        mixedKana.Should().Be("ｱｲｳｴｵあいうえお");
        mixedKana.Length.Should().Be(10);  // 文字数を確認
    }

    [Fact]
    public void GetField_WithSurrogatePairs_ShouldReadCorrectly()
    {
        // Arrange
        var content = "🎌日本🗾観光" + NewLine +  // サロゲートペア文字（絵文字、記号）
                     "👨👩👧👦家族" + NewLine;     // 連続するサロゲートペア

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var reader = new FixedLengthFileReader(stream, Encoding.UTF8, NewLine);

        // Act & Assert
        // 1行目
        reader.Read().Should().BeTrue();
        reader.GetField(0, 8).Should().Be("🎌");     // サロゲートペアは4バイト
        reader.GetField(8, 6).Should().Be("日本");   // 通常の漢字は3バイト
        reader.GetField(14, 4).Should().Be("🗾");    // サロゲートペア
        reader.GetField(18, 6).Should().Be("観光");

        // 2行目
        reader.Read().Should().BeTrue();
        var family = reader.GetField(0, 16);  // 4つの絵文字（4バイト×4）
        family.Should().Be("👨👩👧👦");
        family.Length.Should().Be(4);         // サロゲートペアを正しくカウント
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
        using var reader = new FixedLengthFileReader(stream, encoding, NewLine);

        // Act & Assert
        reader.Read().Should().BeTrue();
        var companyName = reader.GetField(0, encoding.GetBytes(expected).Length);
        companyName.Should().Be(expected);
    }
}