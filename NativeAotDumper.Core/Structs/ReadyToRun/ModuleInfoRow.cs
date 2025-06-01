using System.Runtime.InteropServices;
using NativeAotDumper.Core.Enums.ReadyToRun;

namespace NativeAotDumper.Core.Structs.ReadyToRun;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ModuleInfoRow
{
    public ReadyToRunSectionType SectionId;
    public int Flags;
    public IntPtr Start;
    public IntPtr End;

    public int GetLength() => (int)(End - Start);
};
