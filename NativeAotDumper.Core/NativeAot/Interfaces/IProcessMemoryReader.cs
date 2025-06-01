using Windows.Win32.Foundation;

namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface IProcessMemoryReader : IDisposable
{
    HANDLE ProcessHandle { get; }
    (IntPtr Start, int Size) MainModuleInfo { get; }

    T Read<T>(IntPtr address) where T : unmanaged;
    byte[] ReadBytes(IntPtr address, int count);

    void Write<T>(IntPtr address, T value) where T : unmanaged;
    void WriteBytes(IntPtr address, byte[] buffer);

    IntPtr FindPatternInMemory(byte[] pattern);
}
