using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace PdfStudy.WindowsDataPdf;

/// <summary>
/// PDFファイルをイメージへレンダリングするクラス。
/// </summary>
public class PdfDocument
{
    /// <summary>
    /// Windows.Data.Pdf.PdfDocument
    /// </summary>
    private readonly Windows.Data.Pdf.PdfDocument _pdfDocument;

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="pdfDocument"></param>
    public PdfDocument(Windows.Data.Pdf.PdfDocument pdfDocument)
    {
        _pdfDocument = pdfDocument;
    }

    /// <summary>
    /// ページ数を取得する。
    /// </summary>
    public int PageCount => (int)_pdfDocument.PageCount;

    /// <summary>
    /// PdfDocumentをロードする。
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static async Task<PdfDocument> LoadAsync(MemoryStream stream)
    {
        var randomAccessStream = stream.AsRandomAccessStream();
        // PdfDocumentをロード
        var document = await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(randomAccessStream);
        return new PdfDocument(document);
    }

    /// <summary>
    /// 指定のページをJPEGに変換する。
    /// </summary>
    /// <param name="pageNo"></param>
    /// <param name="dpi"></param>
    /// <returns></returns>
    public async Task<byte[]> ToJpeg(int pageNo, float dpi)
    {
        // PDFの1ページ目を取得
        using var page = _pdfDocument.GetPage((uint)pageNo);
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


}
