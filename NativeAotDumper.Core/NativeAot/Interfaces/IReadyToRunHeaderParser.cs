using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface IReadyToRunHeaderParser
{
    ReadyToRunInfo Find();
}
