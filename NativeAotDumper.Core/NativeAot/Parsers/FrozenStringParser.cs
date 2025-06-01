using NativeAotDumper.Core.Enums.ReadyToRun;
using NativeAotDumper.Core.NativeAot.Extensions;
using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.Structs.NativeAot;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Parsers;

internal class FrozenStringParser : IFrozenStringParser
{
    private readonly IProcessMemoryReader _reader;
    public NativeAotRuntimeInfo AotRuntimeInfo { get; private set; }

    public FrozenStringParser(IProcessMemoryReader reader)
    {
        _reader = reader;
    }

    public IReadOnlyList<FrozenStringEntry> ParseStrings(IReadOnlyList<ModuleInfoRow> rows)
    {
        var frozenRegionRow = rows.First(r => r.SectionId == ReadyToRunSectionType.FrozenObjectRegion);

        IntPtr frozenStart = frozenRegionRow.Start;

        var stringVTableAddr = _reader.Read<IntPtr>(frozenStart + IntPtr.Size);
        var objectVTableAddr = _reader.Read<IntPtr>(stringVTableAddr + sizeof(uint) + sizeof(uint));

        AotRuntimeInfo = new NativeAotRuntimeInfo(
            stringVTableAddr,
            objectVTableAddr
        );

        var result = new List<FrozenStringEntry>();

        for (int i = 0; i <= frozenRegionRow.GetLength() - IntPtr.Size; i += IntPtr.Size)
        {
            var currentAddr = frozenStart + i;
            var potentialVTable = _reader.Read<IntPtr>(currentAddr);

            if (potentialVTable != stringVTableAddr)
                continue;

            var str = _reader.ReadStringFromMemory(currentAddr);
            result.Add(new(str, currentAddr));
        }

        return result;
    }

}
