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

    private ITypeNameResolver _typeNameResolver = null!;

    public IReadOnlyList<ModuleInfoRow> ModuleInfoRows { get; private set; } = null!;
    public IReadOnlyList<FrozenStringEntry> FrozenStrings { get; private set; } = null!;
    public IReadOnlyDictionary<IntPtr, string> TypeNames { get; private set; } = null!;


    public Dumper(string processName)
    {
        _reader = new ProcessMemoryReader(processName);
        _rtrParser = new ReadyToRunHeaderParser(_reader);
        _moduleInfoParser = new ModuleInfoParser(_reader);
        _frozenStringParser = new FrozenStringParser(_reader);
        _metadataResolver = new MetadataResolver(_reader);
    }

    public void Run()
    {
        var rtrInfo = _rtrParser.Find();

        ModuleInfoRows = _moduleInfoParser.Parse(rtrInfo) ?? throw new NullReferenceException($"{nameof(ModuleInfoRows)} is null");
        FrozenStrings = _frozenStringParser.ParseStrings(ModuleInfoRows) ?? throw new NullReferenceException($"{nameof(FrozenStrings)} is null");
        _typeNameResolver = new TypeNameResolver(_reader, _frozenStringParser.AotRuntimeInfo) ?? throw new NullReferenceException($"{nameof(FrozenStrings)} is null");

        var allMethodTables = _metadataResolver.ParseMetadata(ModuleInfoRows);

        TypeNames = _typeNameResolver.ResolveNamesFromVTables(allMethodTables);
    }

    public string ResolveTypeNameFromVTable(IntPtr vTableAddress)
    {
        if (_typeNameResolver == null)
            throw new InvalidOperationException($"Dumper has not been run yet. Call {nameof(Run)}() before resolving type names.");

        return _typeNameResolver.ResolveNameFromVTable(vTableAddress);
    }

    public void Dispose()
    {
        _reader.Dispose();
        _typeNameResolver.Dispose();
    }
}
