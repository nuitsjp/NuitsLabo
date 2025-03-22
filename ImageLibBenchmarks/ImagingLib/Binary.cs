using System.Runtime.InteropServices;

namespace ImagingLib;

public class Binary(
    IntPtr data,
    int width,
    int height,
    int stride) : IDisposable
{
    public void Dispose()
    {
        if (Data == IntPtr.Zero) return;

        Marshal.FreeHGlobal(Data);
        Data = IntPtr.Zero;
    }

    public IntPtr Data { get; private set; } = data;
    public int Width { get; private set; } = width;
    public int Height { get; private set; } = height;
    public int Stride { get; private set; } = stride;

    public byte[] ToBytes()
    {
        var buffer = new byte[Stride * Height];
        Marshal.Copy(Data, buffer, 0, buffer.Length);
        return buffer;
    }
}