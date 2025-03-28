using BitMiracle.LibTiff.Classic;
using ZXing.Net.Bindings.LibTiff;

var imageBytes = File.ReadAllBytes("MultiPage.tif");
using var stream = new MemoryStream(imageBytes);
using var tiff = Tiff.ClientOpen("in-memory", "r", stream, new TiffStream());

var reader = new BarcodeReader();
int page = 1;
do
{
    Console.WriteLine($"Page {page++}");
    var barcodes = reader.DecodeMultiple(tiff);
    foreach (var barcode in barcodes)
    {
        Console.WriteLine($"Format: {barcode.BarcodeFormat}, Text: {barcode.Text}");
    }

} while (tiff.ReadDirectory());

