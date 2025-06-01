using NativeAotDumper.Core.NativeAot.Interfaces;
using System.Runtime.CompilerServices;

namespace NativeAotDumper.Core.NativeAot.Extensions;

public static class ProcessMemoryReaderExtensions
{
    private static int StringLengthOffset => IntPtr.Size;
    private static int StringFirstCharOffset => IntPtr.Size + sizeof(int);

    public static string ReadStringFromMemory(this IProcessMemoryReader reader, IntPtr address)
    {
        int length = reader.Read<int>(address + StringLengthOffset);
        var raw = reader.ReadBytes(address + StringFirstCharOffset, length * 2);
        return System.Text.Encoding.Unicode.GetString(raw);
    }

    public unsafe static byte[] StructToBytes<T>(this IProcessMemoryReader reader, T value) where T : unmanaged
    {
        var size = Unsafe.SizeOf<T>();
        byte[] buffer = new byte[size];

        fixed (byte* ptr = buffer)
        {
            Unsafe.Write(ptr, value);
        }

        return buffer;
    }
}
