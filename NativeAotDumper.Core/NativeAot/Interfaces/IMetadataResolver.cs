using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface IMetadataResolver
{
    public IReadOnlyList<IntPtr> ParseMetadata(IReadOnlyList<ModuleInfoRow> rows);
}
