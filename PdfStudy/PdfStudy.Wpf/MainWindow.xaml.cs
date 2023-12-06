using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using System.Windows;
using Windows.Foundation;
using System.Runtime.InteropServices.ComTypes;
// ReSharper disable MethodHasAsyncOverload


namespace PdfStudy.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            // バイト配列をメモリストリームに変換
            var pdfData = File.ReadAllBytes(@"Assets\MultiPage.pdf");
            using var stream = new MemoryStream(pdfData);
            // メモリストリームをIRandomAccessStreamに変換
            //var randomAccessStream = stream.AsRandomAccessStream();

            //// PdfDocumentをロード
            //var pdfDocument = await PdfDocument.LoadFromStreamAsync(randomAccessStream);
            var pdfDocument = await PdfDocument.LoadAsync(stream);
            var jpegBytes = await pdfDocument.ToJpeg(0, 300);
            File.WriteAllBytes("Pdf.jpg", jpegBytes);
        }

        public async Task<Windows.Data.Pdf.PdfDocument> LoadDocumentAsync(MemoryStream stream)
        {
            //var pdfData = File.ReadAllBytes(@"Assets\MultiPage.pdf");
            //using var stream = new MemoryStream(pdfData);
            var randomAccessStream = stream.AsRandomAccessStream();
            // PdfDocumentをロード
            return await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(randomAccessStream);
        }

        public async Task<byte[]> RenderPdfPageToJpegAsync(Windows.Data.Pdf.PdfDocument pdfDocument, float dpi)
        {
            // PDFの1ページ目を取得
            using var page = pdfDocument.GetPage(0);
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
    public class PdfDocument
    {
        private readonly Windows.Data.Pdf.PdfDocument _pdfDocument;

        public PdfDocument(Windows.Data.Pdf.PdfDocument pdfDocument)
        {
            _pdfDocument = pdfDocument;
        }

        public static async Task<PdfDocument> LoadAsync(MemoryStream stream)
        {
            //var pdfData = File.ReadAllBytes(@"Assets\MultiPage.pdf");
            //using var stream = new MemoryStream(pdfData);
            var randomAccessStream = stream.AsRandomAccessStream();
            // PdfDocumentをロード
            var document = await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(randomAccessStream);
            return new PdfDocument(document);
        }

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
}

