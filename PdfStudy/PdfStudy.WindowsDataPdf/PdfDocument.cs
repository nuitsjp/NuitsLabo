using Microsoft.VisualBasic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace PdfStudy.WindowsDataPdf;

public class PdfDocument
{
    private readonly Windows.Data.Pdf.PdfDocument _document;

    public PdfDocument(Windows.Data.Pdf.PdfDocument document)
    {
        _document = document;
    }
    public uint PageCount => _document.PageCount;

    public async Task<byte[]> ToJpeg(uint pageNo, int dpi)
    {
        // PDFの1ページ目を取得
        using var page = _document.GetPage(0);
        // ページをレンダリングするためのストリームを作成
        using var stream = new InMemoryRandomAccessStream();

        // DPIに基づいてレンダリングサイズを計算
        double scale = dpi / 96.0; // 基本DPIは96
        uint width = (uint)(page.Size.Width * scale);
        uint height = (uint)(page.Size.Height * scale);


        // レンダリングオプションの設定
        var renderOptions = new PdfPageRenderOptions
        {
            DestinationWidth = width,
            DestinationHeight = height
        };

        // ページをストリームにレンダリング
        await page.RenderToStreamAsync(stream, renderOptions);

        // BitmapDecoderを使用してストリームからビットマップを作成
        var decoder = await BitmapDecoder.CreateAsync(stream);
        // JPEGにエンコード
        using var jpegStream = new InMemoryRandomAccessStream();
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, jpegStream);

        // DPI情報を設定したソフトウェアビットマップを用意
        using var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
        softwareBitmap.DpiX = dpi;
        softwareBitmap.DpiY = dpi;

        // ソフトウェアビットマップをエンコーダに設定
        encoder.SetSoftwareBitmap(softwareBitmap);
        await encoder.FlushAsync();

        // JPEGストリームをバイト配列に変換
        var jpegBytes = new byte[jpegStream.Size];
        await jpegStream.ReadAsync(jpegBytes.AsBuffer(), (uint)jpegStream.Size, InputStreamOptions.None);
        return jpegBytes;
    }

    public static async Task<PdfDocument> LoadAsync(string path)
    {
        // バイト配列をメモリストリームに変換
        var pdfData = File.ReadAllBytes(@"Assets\MultiPage.pdf");
        using var stream = new MemoryStream(pdfData);
        // メモリストリームをIRandomAccessStreamに変換
        var randomAccessStream = stream.AsRandomAccessStream();

        // PdfDocumentをロード
        var pdfDocument = await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(randomAccessStream);
        return new PdfDocument(pdfDocument);
    }
}