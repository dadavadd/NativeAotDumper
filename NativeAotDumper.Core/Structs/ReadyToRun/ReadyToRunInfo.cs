namespace NativeAotDumper.Core.Structs.ReadyToRun;

public readonly struct ReadyToRunInfo
{
    public ReadyToRunInfo(ReadyToRunHeader header, IntPtr moduleRowsStart)
    {
        Header = header;
        ModuleRowsStart = moduleRowsStart;
    }

    public ReadyToRunHeader Header { get; }
    public IntPtr ModuleRowsStart { get; }
}
