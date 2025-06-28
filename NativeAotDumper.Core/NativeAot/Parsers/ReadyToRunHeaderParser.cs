using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Parsers;

internal class ReadyToRunHeaderParser : IReadyToRunHeaderParser
{
    private readonly IProcessMemoryReader _reader;

    private static byte[] RTRSignature = [(byte)'R', (byte)'T', (byte)'R', 0x00];

    public ReadyToRunHeaderParser(IProcessMemoryReader reader)
    {
        _reader = reader;
    }

    public unsafe ReadyToRunInfo Find()
    {
        IntPtr headerAddress = IntPtr.Zero;
        IntPtr moduleInfoRowsStart = IntPtr.Zero;

        ReadyToRunHeader readyToRunHeader;

        if ((headerAddress = _reader.FindPatternInMemory(RTRSignature)) != IntPtr.Zero)
        {
            readyToRunHeader = _reader.Read<ReadyToRunHeader>(headerAddress);
        }
        else
        {
            throw new InvalidOperationException("ReadyToRun header not found.");
        }

        moduleInfoRowsStart = headerAddress + sizeof(ReadyToRunHeader);

        return new(readyToRunHeader, moduleInfoRowsStart);
    }
}
