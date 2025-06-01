namespace NativeAotDumper.Core.NativeAot.Interfaces;

public interface ITypeNameResolver : IDisposable
{
    public string ResolveNameFromVTable(IntPtr vTableAddress);
    Dictionary<IntPtr, string> ResolveNamesFromVTables(IReadOnlyList<IntPtr> vTableAddresses);
}
