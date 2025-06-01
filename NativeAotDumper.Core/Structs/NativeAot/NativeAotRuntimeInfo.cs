namespace NativeAotDumper.Core.Structs.NativeAot;

public struct NativeAotRuntimeInfo
{
    public IntPtr FrozenStringVTable { get; }
    public IntPtr ObjectVTable { get; }

    public NativeAotRuntimeInfo(IntPtr frozenStringVTable, IntPtr objectVTable)
    {
        FrozenStringVTable = frozenStringVTable;
        ObjectVTable = objectVTable;
    }
}
