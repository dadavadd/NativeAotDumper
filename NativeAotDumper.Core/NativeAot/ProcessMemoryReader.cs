using NativeAotDumper.Core.NativeAot.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

using static Windows.Win32.PInvoke;
using static Windows.Win32.System.Threading.PROCESS_ACCESS_RIGHTS;

namespace NativeAotDumper.Core.NativeAot;

internal class ProcessMemoryReader : IProcessMemoryReader
{
    private readonly Process _process;

    public HANDLE ProcessHandle { get; }
    public (IntPtr Start, int Size) MainModuleInfo => (_process.MainModule!.BaseAddress, _process.MainModule.ModuleMemorySize);

    public ProcessMemoryReader(string processName)
    {
        var processes = Process.GetProcessesByName(processName.Replace(".exe", string.Empty)) ?? throw new ArgumentException($"Process '{processName}' not found.");
        _process = processes[0];
        ProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, false, (uint)_process.Id);
    }

    public unsafe T Read<T>(IntPtr address) where T : unmanaged
    {
        ReadOnlySpan<byte> bytes = ReadBytes(address, sizeof(T));
        return MemoryMarshal.Read<T>(bytes);
    }

    public unsafe byte[] ReadBytes(IntPtr address, int count)
    {
        byte[] buffer = new byte[count];

        fixed (void* bufPtr = buffer)
        {
            if (ReadProcessMemory(ProcessHandle, address.ToPointer(), bufPtr, (nuint)count, null))
                return buffer;
        }

        throw new Win32Exception(Marshal.GetLastWin32Error(), $"Failed to read memory at address {address.ToString("X")}.");
    }

    public unsafe void Write<T>(IntPtr address, T value) where T : unmanaged
    {
        Span<T> buffer = [value];
        var bytes = MemoryMarshal.AsBytes(buffer);
        WriteBytes(address, bytes.ToArray());
    }

    public unsafe void WriteBytes(IntPtr address, byte[] buffer)
    {
        fixed (void* bufPtr = buffer)
        {
            if (!WriteProcessMemory(ProcessHandle, address.ToPointer(), bufPtr, (nuint)buffer.Length, null))
                throw new Win32Exception(Marshal.GetLastWin32Error(), $"Failed to write memory at address {address.ToString("X")}.");
        }
    }

    public unsafe nint FindPatternInMemory(byte[] pattern)
    {
        var readBuffer = ReadBytes(MainModuleInfo.Start, MainModuleInfo.Size).AsSpan();

        for (int i = readBuffer.Length - pattern.Length; i >= 0; i--)
        {
            if (readBuffer.Slice(i, pattern.Length).SequenceEqual(pattern))
            {
                return MainModuleInfo.Start + i;
            }
        }

        return IntPtr.Zero;
    }

    public void Dispose()
    {
        _process.Dispose();
        CloseHandle(ProcessHandle);
    }
}
