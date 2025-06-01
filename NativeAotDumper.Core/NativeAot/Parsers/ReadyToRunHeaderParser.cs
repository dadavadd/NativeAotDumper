using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Parsers;

internal class ReadyToRunHeaderParser : IReadyToRunHeaderParser
{
    private readonly IProcessMemoryReader _reader;

    private static byte[] RTRDotnet10 = [(byte)'R', (byte)'T', (byte)'R', 0x00, 0x0D];

    public ReadyToRunHeaderParser(IProcessMemoryReader reader)
    {
        _reader = reader;
    }

    public unsafe ReadyToRunInfo Find()
    {
        nint headerAddress = nint.Zero;
        nint moduleInfoRowsStart = nint.Zero;

        ReadyToRunHeader readyToRunHeader;

        if ((headerAddress = _reader.FindPatternInMemory(RTRDotnet10)) != nint.Zero)
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
