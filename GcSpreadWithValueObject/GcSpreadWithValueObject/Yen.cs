namespace GcSpreadWithValueObject;

/// <summary>
/// 通貨「円」を表す構造体
/// </summary>
public readonly struct Yen
{
    private readonly int _value;

    public Yen(int value)
    {
        _value = value;
    }

    public int AsPrimitive() => _value;

    public override string ToString() => _value.ToString("###,###,###,###");
}