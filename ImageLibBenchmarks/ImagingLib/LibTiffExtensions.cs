using BitMiracle.LibTiff.Classic;
using System.IO;
using System.Runtime.InteropServices;

namespace ImagingLib;

public static class LibTiffExtensions
{
    public static unsafe Binary ToBinary(byte[] tiffData)
    {
        using var ms = new MemoryStream(tiffData);
        var memStream = new MemoryStreamTiffStream(ms);
        using var tiff = Tiff.ClientOpen("in-memory", "r", ms, memStream);
        if (tiff == null)
            throw new Exception("TIFFイメージの読み込みに失敗しました。");

        // 必要なタグを取得
        var value = tiff.GetField(TiffTag.IMAGEWIDTH);
        var width = value[0].ToInt();

        value = tiff.GetField(TiffTag.IMAGELENGTH);
        var height = value[0].ToInt();

        value = tiff.GetField(TiffTag.BITSPERSAMPLE);
        var bitsPerSample = value[0].ToInt();
        if (bitsPerSample != 1)
            throw new Exception("二値TIFF（1bpp）のみ対応しています。");

        // 入力側の1行あたりのサイズ（TIFF内部のサイズ、通常は (width+7)/8）
        var inputScanlineSize = tiff.ScanlineSize();
        // 出力バッファの1行あたりサイズ（4バイト境界に揃える）
        var outputStride = ((width + 7) / 8 + 3) & ~3;
        var outBufferSize = outputStride * height;
        var outBuffer = Marshal.AllocHGlobal(outBufferSize);

        // 出力バッファをゼロクリア
        for (var i = 0; i < outBufferSize; i++)
        {
            Marshal.WriteByte(outBuffer, i, 0);
        }

        var scanline = new byte[inputScanlineSize];
        fixed (byte* _ = scanline)
        {
            for (var row = 0; row < height; row++)
            {
                if (!tiff.ReadScanline(scanline, row))
                    throw new Exception($"Scanline {row} の読み込みに失敗しました。");

                // 出力バッファの該当行先頭アドレスを取得
                var destPtr = (byte*)outBuffer + row * outputStride;
                // scanlineの内容をコピー
                for (var i = 0; i < inputScanlineSize; i++)
                {
                    destPtr[i] = scanline[i];
                }
                // 残りのバイトはすでにゼロクリア済み
            }
        }

        return new Binary(outBuffer, width, height, outputStride);
    }
}

public class MemoryStreamTiffStream(MemoryStream ms) : TiffStream
{
    public override int Read(object clientData, byte[] buffer, int offset, int count)
    {
        return ms.Read(buffer, offset, count);
    }

    public override void Write(object clientData, byte[] buffer, int offset, int count)
    {
        ms.Write(buffer, offset, count);
    }

    public override long Seek(object clientData, long offset, SeekOrigin origin)
    {
        return ms.Seek(offset, origin);
    }

    public override void Close(object clientData)
    {
        ms.Close();
    }

    public override long Size(object clientData)
    {
        return ms.Length;
    }
}