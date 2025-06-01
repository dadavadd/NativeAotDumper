using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Parsers;

internal class ModuleInfoParser : IModuleInfoParser
{
    private readonly IProcessMemoryReader _reader;

    public ModuleInfoParser(IProcessMemoryReader reader)
    {
        _reader = reader;
    }

    public unsafe IReadOnlyList<ModuleInfoRow> Parse(ReadyToRunInfo rtrInfo)
    {
        ModuleInfoRow[] rows = new ModuleInfoRow[rtrInfo.Header.NumberOfSections];

        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = _reader.Read<ModuleInfoRow>(rtrInfo.ModuleRowsStart + i * sizeof(ModuleInfoRow));
        }

        return rows;
    }
}
