using NativeAotDumper.Core.Enums.ReadyToRun;
using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.NativeAot.Readers;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Utils;

internal class MetadataResolver : IMetadataResolver
{
    private readonly IProcessMemoryReader _reader;
    private byte[] _commonFixupsData = null!;
    private nint _commonFixupsBaseAddress;

    private bool UseRelativePointers = true;

    public MetadataResolver(IProcessMemoryReader reader)
    {
        _reader = reader;
    }

    public unsafe IReadOnlyList<nint> ParseMetadata(IReadOnlyList<ModuleInfoRow> rows)
    {
        EnsureCommonFixupsLoaded(rows);

        var typeMapRow = rows.First(r => r.SectionId == (ReadyToRunSectionType)301); // TypeMap
        var typeMapData = _reader.ReadBytes(typeMapRow.Start, typeMapRow.GetLength());

        fixed (byte* ptr = typeMapData)
        {
            var reader = new NativeReader(ptr, (uint)typeMapData.Length);
            var hashtable = new NativeHashtable(new NativeParser(reader, 0));
            var entries = hashtable.EnumerateAllEntries();

            var addresses = new HashSet<nint>();
            NativeParser parser;

            while (!(parser = entries.GetNext()).IsNull)
            {
                uint index = parser.GetUnsigned();
                var addr = GetAddressFromIndex(index);
                if (addr != nint.Zero)
                    addresses.Add(addr);
            }

            var result = addresses.OrderBy(a => a).ToList();
            return result;
        }
    }

    private void EnsureCommonFixupsLoaded(IReadOnlyList<ModuleInfoRow> rows)
    {
        if (_commonFixupsData != null)
            return;

        var row = rows.First(r => r.SectionId == (ReadyToRunSectionType)308); // CommonFixupsTable
        _commonFixupsData = _reader.ReadBytes(row.Start, row.GetLength());
        _commonFixupsBaseAddress = row.Start;
    }

    private unsafe nint GetAddressFromIndex(uint index)
    {
        fixed (byte* pBase = _commonFixupsData)
        {
            if (UseRelativePointers)
            {
                int offset = *(int*)(pBase + index * sizeof(int));
                return _commonFixupsBaseAddress + (int)(index * sizeof(int)) + offset;
            }
            else
            {
                return *(nint*)(pBase + index * nint.Size);
            }
        }
    }
}
