using NativeAotDumper.Core.Structs.NativeAot;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface IMetadataResolver
{
    RuntimeTypeInfo[] ParseMetadata(IReadOnlyList<ModuleInfoRow> rows);
}
