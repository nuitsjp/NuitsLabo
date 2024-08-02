namespace AudioCalibrationStudy;

/// <summary>
/// キャリブレーションポイントを表すレコード
/// </summary>
/// <param name="Decibels">デシベル値</param>
/// <param name="Amplitude">振幅</param>
public record CalibrationPoint(double Decibels, double Amplitude);

/// <summary>
/// スプライン補間を使用してオーディオのキャリブレーションを行うクラス
/// </summary>
public class AudioSplineCalibrator
{
    private readonly double[] _decibels;
    private readonly double[] _amplitudes;
    // スプライン補間の係数
    private readonly double[] _a; // 定数項
    private readonly double[] _b; // 1次の項
    private readonly double[] _c; // 2次の項
    private readonly double[] _d; // 3次の項

    /// <summary>
    /// AudioSplineCalibratorのコンストラクタ
    /// </summary>
    /// <param name="calibrationPoints">キャリブレーションポイントのリスト</param>
    public AudioSplineCalibrator(List<CalibrationPoint> calibrationPoints)
    {
        var sortedPoints = calibrationPoints.OrderBy(p => p.Decibels).ToList();

        _decibels = sortedPoints.Select(p => p.Decibels).ToArray();
        _amplitudes = sortedPoints.Select(p => p.Amplitude).ToArray();

        var numberOfPoints = _decibels.Length;
        _a = new double[numberOfPoints - 1];
        _b = new double[numberOfPoints - 1];
        _c = new double[numberOfPoints - 1];
        _d = new double[numberOfPoints - 1];

        CalculateSplineCoefficients();
    }

    /// <summary>
    /// スプライン補間の係数を計算するプライベートメソッド
    /// </summary>
    private void CalculateSplineCoefficients()
    {
        var numberOfPoints = _decibels.Length;

        // 隣接するデシベル値の差分
        var decibelDifferences = new double[numberOfPoints - 1];
        for (var i = 0; i < numberOfPoints - 1; i++)
        {
            decibelDifferences[i] = _decibels[i + 1] - _decibels[i];
        }

        // スプライン方程式の右辺
        var equationRightSide = new double[numberOfPoints - 1];
        for (var i = 1; i < numberOfPoints - 1; i++)
        {
            equationRightSide[i] = 3 * ((_amplitudes[i + 1] - _amplitudes[i]) / decibelDifferences[i] -
                                        (_amplitudes[i] - _amplitudes[i - 1]) / decibelDifferences[i - 1]);
        }

        // トーマスアルゴリズムの係数
        var lowerDiagonal = new double[numberOfPoints];
        var diagonal = new double[numberOfPoints];
        var upperDiagonal = new double[numberOfPoints];
        var result = new double[numberOfPoints];

        lowerDiagonal[0] = 0;
        diagonal[0] = 1;
        result[0] = 0;

        for (var i = 1; i < numberOfPoints - 1; i++)
        {
            lowerDiagonal[i] = decibelDifferences[i - 1];
            diagonal[i] = 2 * (_decibels[i + 1] - _decibels[i - 1]);
            upperDiagonal[i] = decibelDifferences[i];
            result[i] = equationRightSide[i];
        }

        lowerDiagonal[numberOfPoints - 1] = 0;
        diagonal[numberOfPoints - 1] = 1;
        result[numberOfPoints - 1] = 0;

        // トーマスアルゴリズムで方程式を解く
        for (var i = 1; i < numberOfPoints; i++)
        {
            var m = lowerDiagonal[i] / diagonal[i - 1];
            diagonal[i] -= m * upperDiagonal[i - 1];
            result[i] -= m * result[i - 1];
        }

        var secondDerivatives = new double[numberOfPoints];
        secondDerivatives[numberOfPoints - 1] = result[numberOfPoints - 1] / diagonal[numberOfPoints - 1];
        for (var i = numberOfPoints - 2; i >= 0; i--)
        {
            secondDerivatives[i] = (result[i] - upperDiagonal[i] * secondDerivatives[i + 1]) / diagonal[i];
        }

        // スプライン係数の計算
        for (var i = 0; i < numberOfPoints - 1; i++)
        {
            _a[i] = _amplitudes[i];
            _b[i] = (_amplitudes[i + 1] - _amplitudes[i]) / decibelDifferences[i] -
                    decibelDifferences[i] * (secondDerivatives[i + 1] + 2 * secondDerivatives[i]) / 3;
            _c[i] = secondDerivatives[i];
            _d[i] = (secondDerivatives[i + 1] - secondDerivatives[i]) / (3 * decibelDifferences[i]);
        }
    }

    /// <summary>
    /// 指定されたデシベル値に対応する振幅を推定します
    /// </summary>
    /// <param name="targetDecibels">目標のデシベル値</param>
    /// <returns>推定された振幅</returns>
    public double EstimateAmplitude(double targetDecibels)
    {
        // 適切なスプライン区間を見つける
        var intervalIndex = Array.BinarySearch(_decibels, targetDecibels);
        if (intervalIndex < 0)
        {
            intervalIndex = ~intervalIndex - 1;
        }
        intervalIndex = Math.Clamp(intervalIndex, 0, _decibels.Length - 2);

        // スプライン補間を使用して振幅を計算
        var decibelDifference = targetDecibels - _decibels[intervalIndex];
        return _a[intervalIndex] +
               _b[intervalIndex] * decibelDifference +
               _c[intervalIndex] * decibelDifference * decibelDifference +
               _d[intervalIndex] * decibelDifference * decibelDifference * decibelDifference;
    }
}