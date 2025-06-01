
namespace NativeAotDumper.Core.Structs.NativeAot;

public readonly struct FrozenStringEntry
{
    public FrozenStringEntry(string data, IntPtr startAddress)
    {
        Data = data;
        StartAddress = startAddress;
    }

    public string Data { get; }
    public IntPtr StartAddress { get; }
}
