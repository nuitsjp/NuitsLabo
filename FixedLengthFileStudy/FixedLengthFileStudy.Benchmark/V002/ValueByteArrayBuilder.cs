// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FixedLengthFileStudy.Benchmark.V002;

internal ref struct ValueByteArrayBuilder(Span<byte> initialBuffer)
{
    private byte[]? _arrayToReturnToPool = null;
    private Span<byte> _bytes = initialBuffer;
    private int _pos = 0;

    public bool IsEmpty => _pos == 0;

    public void Clear() => _pos = 0;

    public ReadOnlySpan<byte> AsSpan() => _bytes.Slice(0, _pos);

    public void Append(scoped ReadOnlySpan<byte> value)
    {
        if (_pos > _bytes.Length - value.Length)
        {
            Grow(value.Length);
        }

        value.CopyTo(_bytes.Slice(_pos));
        _pos += value.Length;
    }

    /// <summary>
    /// Resize the internal buffer either by doubling current buffer size or
    /// by adding <paramref name="additionalCapacityBeyondPos"/> to
    /// <see cref="_pos"/> whichever is greater.
    /// </summary>
    /// <param name="additionalCapacityBeyondPos">
    /// Number of chars requested beyond current position.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int additionalCapacityBeyondPos)
    {
        Debug.Assert(additionalCapacityBeyondPos > 0);
        Debug.Assert(_pos > _bytes.Length - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

        const uint arrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

        // Increase to at least the required size (_pos + additionalCapacityBeyondPos), but try
        // to double the size if possible, bounding the doubling to not go beyond the max array length.
        var newCapacity = (int)Math.Max(
            (uint)(_pos + additionalCapacityBeyondPos),
            Math.Min((uint)_bytes.Length * 2, arrayMaxLength));

        // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative.
        // This could also go negative if the actual required length wraps around.
        var poolArray = ArrayPool<byte>.Shared.Rent(newCapacity);

        _bytes.Slice(0, _pos).CopyTo(poolArray);

        var toReturn = _arrayToReturnToPool;
        _bytes = _arrayToReturnToPool = poolArray;
        if (toReturn != null)
        {
            ArrayPool<byte>.Shared.Return(toReturn);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        var toReturn = _arrayToReturnToPool;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
        if (toReturn != null)
        {
            ArrayPool<byte>.Shared.Return(toReturn);
        }
    }
}