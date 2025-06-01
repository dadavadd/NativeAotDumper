using System.Runtime.InteropServices;

namespace NativeAotDumper.Core.Structs.ReadyToRun;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct ReadyToRunHeader
{
    [FieldOffset(0x00)] public uint Signature;
    [FieldOffset(0x04)] public ushort MajorVersion;
    [FieldOffset(0x06)] public ushort MinorVersion;
    [FieldOffset(0x08)] public uint Flags;
    [FieldOffset(0x0C)] public ushort NumberOfSections;
    [FieldOffset(0x0E)] public byte EntrySize;
    [FieldOffset(0x0F)] public byte EntryType;
}

