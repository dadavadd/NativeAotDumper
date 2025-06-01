using System.Runtime.InteropServices;

namespace NativeAotDumper.Core.Structs.NativeAot;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MethodTable
{
    public uint Flags;
    public uint BaseSize;
    public IntPtr RelatedType;
    public ushort NumVtableSlots;
    public ushort NumInterfaces;
    public uint HashCode;

    private const uint MTFlag_Interface = 0x00000020;
    private const uint MTFlag_Abstract = 0x00000004;
    private const uint MTFlag_Sealed = 0x00000008;
    private const uint MTFlag_ValueType = 0x00000001;
    private const uint MTFlag_ContainsPointers = 0x00000002;
    private const uint MTFlag_HasFinalizer = 0x00000010;
    private const uint MTFlag_HasTypeEquivalence = 0x00000200;

    public bool IsInterface => (Flags & MTFlag_Interface) != 0;
    public bool IsAbstract => (Flags & MTFlag_Abstract) != 0;
    public bool IsSealed => (Flags & MTFlag_Sealed) != 0;
    public bool IsValueType => (Flags & MTFlag_ValueType) != 0;
    public bool ContainsPointers => (Flags & MTFlag_ContainsPointers) != 0;
    public bool HasFinalizer => (Flags & MTFlag_HasFinalizer) != 0;
    public bool HasTypeEquivalence => (Flags & MTFlag_HasTypeEquivalence) != 0;
}
