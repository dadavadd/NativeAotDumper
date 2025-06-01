using NativeAotDumper.Core.Structs.NativeAot;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface IFrozenStringParser
{
    NativeAotRuntimeInfo AotRuntimeInfo { get; }
    IReadOnlyList<FrozenStringEntry> ParseStrings(IReadOnlyList<ModuleInfoRow> rows);
}
