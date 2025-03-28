using BitMiracle.LibTiff.Classic;

namespace LibTiffStudy;

public class WriteMultiPage
{
    public static void Invoke()
    {
        var inputFile = "Sample.tif";   // 読み込み元ファイル
        var outputFile = "output.tif"; // 書き出し先ファイル

        // 入力TIFFファイルをオープン
        using (var input = Tiff.Open(inputFile, "r"))
        {
            if (input == null)
            {
                Console.WriteLine("入力ファイルをオープンできませんでした。");
                return;
            }

            // 基本情報の取得
            var width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            var height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            var bitsPerSample = input.GetField(TiffTag.BITSPERSAMPLE)[0].ToShort();
            var samplesPerPixel = input.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToShort();

            // Photometric タグ取得
            var photoField = input.GetField(TiffTag.PHOTOMETRIC);
            var photometric = (photoField != null) ? photoField[0].ToInt() : (int)Photometric.MINISBLACK;

            // DPI情報の取得（存在する場合）
            float xResolution = 0, yResolution = 0;
            short resolutionUnit = 0;
            var xResField = input.GetField(TiffTag.XRESOLUTION);
            var yResField = input.GetField(TiffTag.YRESOLUTION);
            var resUnitField = input.GetField(TiffTag.RESOLUTIONUNIT);

            if (xResField != null)
                xResolution = xResField[0].ToFloat();
            if (yResField != null)
                yResolution = yResField[0].ToFloat();
            if (resUnitField != null)
                resolutionUnit = resUnitField[0].ToShort();

            // 画像データの読み込み（シングルページなので全スキャンラインをバッファに格納）
            var scanlineSize = input.ScanlineSize();
            byte[][] imageData = new byte[height][];
            for (var row = 0; row < height; row++)
            {
                imageData[row] = new byte[scanlineSize];
                input.ReadScanline(imageData[row], row);
            }

            // 出力用のマルチページTIFFを作成
            using (var output = Tiff.Open(outputFile, "w"))
            {
                if (output == null)
                {
                    Console.WriteLine("出力ファイルをオープンできませんでした。");
                    return;
                }

                // 3ページ分ループ（各ページは同じ画像データ）
                for (var page = 0; page < 3; page++)
                {
                    // 必要なTIFFタグを設定
                    output.SetField(TiffTag.IMAGEWIDTH, width);
                    output.SetField(TiffTag.IMAGELENGTH, height);
                    output.SetField(TiffTag.BITSPERSAMPLE, bitsPerSample);
                    output.SetField(TiffTag.SAMPLESPERPIXEL, samplesPerPixel);

                    // 圧縮処理：LZW圧縮を利用
                    output.SetField(TiffTag.COMPRESSION, Compression.LZW);

                    output.SetField(TiffTag.PHOTOMETRIC, photometric);
                    output.SetField(TiffTag.ROWSPERSTRIP, height);
                    output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);

                    // DPI情報の設定（入力ファイルからコピー）
                    if (xResField != null)
                        output.SetField(TiffTag.XRESOLUTION, xResolution);
                    if (yResField != null)
                        output.SetField(TiffTag.YRESOLUTION, yResolution);
                    if (resUnitField != null)
                        output.SetField(TiffTag.RESOLUTIONUNIT, resolutionUnit);

                    // 各スキャンラインの画像データを書き込み
                    for (var row = 0; row < height; row++)
                    {
                        output.WriteScanline(imageData[row], row);
                    }

                    // 現在のページのディレクトリを書き出し、新たなページを開始
                    output.WriteDirectory();
                }
            }
        }
    }
}