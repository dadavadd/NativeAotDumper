using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.NativeAot.Parsers;
using NativeAotDumper.Core.NativeAot.Utils;
using NativeAotDumper.Core.Structs.NativeAot;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot;

public class Dumper : IDisposable
{
    private readonly IProcessMemoryReader _reader;
    private readonly IReadyToRunHeaderParser _rtrParser;
    private readonly IModuleInfoParser _moduleInfoParser;
    private readonly IFrozenStringParser _frozenStringParser;
    private readonly IMetadataResolver _metadataResolver;

    public IReadOnlyList<ModuleInfoRow> ModuleInfoRows { get; private set; } = null!;
    public IReadOnlyList<FrozenStringEntry> FrozenStrings { get; private set; } = null!;
    public IReadOnlyList<RuntimeTypeInfo> RuntimeTypeInfos { get; private set; } = null!;


    public Dumper(string processName)
    {
        _reader = new ProcessMemoryReader(processName);
        _rtrParser = new ReadyToRunHeaderParser(_reader);
        _moduleInfoParser = new ModuleInfoParser(_reader);
        _frozenStringParser = new FrozenStringParser(_reader);
        _metadataResolver = new MetadataResolver(_reader);

        Run();
    }

    private void Run()
    {
        var rtrInfo = _rtrParser.Find();

        ModuleInfoRows = _moduleInfoParser.Parse(rtrInfo) ?? throw new NullReferenceException($"{nameof(ModuleInfoRows)} is null");
        FrozenStrings = _frozenStringParser.ParseStrings(ModuleInfoRows) ?? throw new NullReferenceException($"{nameof(FrozenStrings)} is null");

        RuntimeTypeInfos = _metadataResolver.ParseMetadata(ModuleInfoRows);
    }

    public void Dispose()
    {
        _reader.Dispose();
    }
}
