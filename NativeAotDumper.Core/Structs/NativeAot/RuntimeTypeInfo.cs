namespace NativeAotDumper.Core.Structs.NativeAot;

public struct RuntimeMethodInfo
{
    public string Name { get; set; }
    public int MetadataToken { get; set; }
}

public struct RuntimeFieldInfo
{
    public string Name { get; set; }
    public int MetadataToken { get; set; }
    public uint Offset { get; set; }
}

public struct RuntimePropertyInfo
{
    public string Name { get; set; }
    public int MetadataToken { get; set; }
}

public struct RuntimeTypeInfo
{
    public IntPtr MethodTableAddress { get; set; }
    public string FullName { get; set; }
    public int MetadataToken { get; set; }

    public List<RuntimeMethodInfo> Methods { get; set; }
    public List<RuntimeFieldInfo> Fields { get; set; }
    public List<RuntimePropertyInfo> Properties { get; set; }
}