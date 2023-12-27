using UnitGenerator;

namespace ImagingTest;

/// <summary>
/// しきい値
/// </summary>
[UnitOf(typeof(byte))]
public partial struct Threshold
{
    /// <summary>
    /// デフォルト値
    /// </summary>
    public static Threshold Default => new(75);
}