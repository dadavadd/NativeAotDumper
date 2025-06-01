using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface IModuleInfoParser
{
    IReadOnlyList<ModuleInfoRow> Parse(ReadyToRunInfo rtrInfo);
}
