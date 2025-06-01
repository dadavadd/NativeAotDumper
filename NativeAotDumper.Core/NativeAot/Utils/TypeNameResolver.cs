using Iced.Intel;
using NativeAotDumper.Core.NativeAot.Extensions;
using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.Structs.NativeAot;
using System.Runtime.InteropServices;

using static Iced.Intel.AssemblerRegisters;
using static Windows.Win32.Foundation.WAIT_EVENT;
using static Windows.Win32.PInvoke;
using static Windows.Win32.System.Memory.PAGE_PROTECTION_FLAGS;
using static Windows.Win32.System.Memory.VIRTUAL_ALLOCATION_TYPE;
using static Windows.Win32.System.Memory.VIRTUAL_FREE_TYPE;

namespace NativeAotDumper.Core.NativeAot.Utils;

internal class TypeNameResolver : ITypeNameResolver
{
    private static int RuntimeTypeStructSize => IntPtr.Size * 2;

    private const int MaxShellSize = 300;
    private const int ResultMaxSize = 255;

    private readonly IProcessMemoryReader _reader;
    private readonly Dictionary<IntPtr, string> _typeNamesCache = new();

    private readonly IntPtr _findGetRuntimeTypeInfoMethod;
    private readonly IntPtr _codePlaceAlloc;
    private readonly IntPtr _codeResultAlloc;

    private readonly NativeAotRuntimeInfo _aotRuntimeInfo;

    public TypeNameResolver(IProcessMemoryReader reader, NativeAotRuntimeInfo aotRuntimeInfo)
    {
        _reader = reader;
        _aotRuntimeInfo = aotRuntimeInfo;

        _findGetRuntimeTypeInfoMethod = FindGetRuntimeTypeInfoMethod();

        _codePlaceAlloc = AllocateExecReadWriteMemory(MaxShellSize + RuntimeTypeStructSize);
        _codeResultAlloc = AllocateExecReadWriteMemory(ResultMaxSize);

        if (_codePlaceAlloc == IntPtr.Zero)
            throw new Exception($"VirtualAllocEx (code) failed: {Marshal.GetLastWin32Error()}");

        if (_codeResultAlloc == IntPtr.Zero)
            throw new Exception($"VirtualAllocEx (result) failed: {Marshal.GetLastWin32Error()}");
    }

    public unsafe Dictionary<IntPtr, string> ResolveNamesFromVTables(IReadOnlyList<IntPtr> vTableAddresses)
    {
        if (vTableAddresses == null || !vTableAddresses.Any())
            return [];

        var uncachedVTables = vTableAddresses.Where(addr => !_typeNamesCache.ContainsKey(addr)).ToList();

        if (uncachedVTables.Count == 0)
            return _typeNamesCache;
        

        int vtableCount = uncachedVTables.Count;
        int vtableArraySize = vtableCount * IntPtr.Size;
        int resultArraySize = vtableCount * IntPtr.Size;

        var vtableArrayAlloc = AllocateExecReadWriteMemory(vtableArraySize);
        var resultArrayAlloc = AllocateExecReadWriteMemory(resultArraySize);

        if (vtableArrayAlloc == IntPtr.Zero || resultArrayAlloc == IntPtr.Zero)
            throw new Exception("Failed to allocate memory for batch processing");

        try
        {
            for (int i = 0; i < vtableCount; i++)
            {
                _reader.Write(vtableArrayAlloc + i * IntPtr.Size, uncachedVTables[i]);
            }

            var shell = CreateBatchShellCode(vtableArrayAlloc, resultArrayAlloc, vtableCount);
            _reader.WriteBytes(_codePlaceAlloc, shell);

            var hThread = CreateRemoteThread(_reader.ProcessHandle,
                                       null,
                                       0,
                                       (delegate* unmanaged[Stdcall]<void*, uint>)_codePlaceAlloc,
                                       null,
                                       0,
                                       null);

            try
            {
                var waitResult = WaitForSingleObject(hThread, 10000);

                if (waitResult != WAIT_OBJECT_0)
                {
                    CloseHandle(hThread);
                    throw new TimeoutException("Remote thread execution timed out during batch processing");
                }

                for (int i = 0; i < vtableCount; i++)
                {
                    var nameAddress = _reader.Read<IntPtr>(resultArrayAlloc + i * IntPtr.Size);
                    string typeName = nameAddress != IntPtr.Zero
                        ? _reader.ReadStringFromMemory(nameAddress)
                        : "Unknown type";

                    _typeNamesCache[uncachedVTables[i]] = typeName;
                }
            }
            finally
            {
                CloseHandle(hThread);
            }
        }
        finally
        {
            VirtualFreeEx(_reader.ProcessHandle, (void*)vtableArrayAlloc, 0, MEM_RELEASE);
            VirtualFreeEx(_reader.ProcessHandle, (void*)resultArrayAlloc, 0, MEM_RELEASE);
        }

        return _typeNamesCache;
    }

    private unsafe IntPtr AllocateExecReadWriteMemory(int size) 
        => (IntPtr)VirtualAllocEx(_reader.ProcessHandle,
                                  null,
                                  (nuint)size,
                                  MEM_COMMIT | MEM_RESERVE,
                                  PAGE_EXECUTE_READWRITE);

    private byte[] CreateBatchShellCode(IntPtr vtableArrayAddr, IntPtr resultArrayAddr, int vtableCount)
    {
        var asm = new Assembler(Environment.Is64BitProcess ? 64 : 32);

        if (Environment.Is64BitProcess)
        {
            asm.sub(rsp, 0x30);

            asm.push(rbx);
            asm.push(rsi);
            asm.push(rdi);

            // rbx = vtableArrayAddr, rsi = resultArrayAddr, rdi = counter
            asm.mov(rbx, (ulong)vtableArrayAddr);
            asm.mov(rsi, (ulong)resultArrayAddr);
            asm.xor(rdi, rdi); // counter = 0

            var loopLabel = asm.CreateLabel();
            var endLabel = asm.CreateLabel();

            asm.Label(ref loopLabel);

            asm.cmp(rdi, vtableCount);
            asm.jge(endLabel);

            asm.mov(rcx, __qword_ptr[rbx + rdi * 8]);

            // GetRuntimeTypeInfo
            asm.call((ulong)_findGetRuntimeTypeInfoMethod);

            asm.mov(rcx, rax);
            asm.mov(rax, __qword_ptr[rax]);
            asm.call(__qword_ptr[rax + 0x58]);

            asm.mov(__qword_ptr[rsi + rdi * 8], rax);

            asm.inc(rdi);
            asm.jmp(loopLabel);

            asm.Label(ref endLabel);

            asm.pop(rdi);
            asm.pop(rsi);
            asm.pop(rbx);

            asm.add(rsp, 0x30);
            asm.ret();
        }
        else
        {
            asm.pushad();

            // ebx = vtableArrayAddr, esi = resultArrayAddr, edi = counter
            asm.mov(ebx, (uint)vtableArrayAddr);
            asm.mov(esi, (uint)resultArrayAddr);
            asm.xor(edi, edi); // counter = 0

            var loopLabel = asm.CreateLabel();
            var endLabel = asm.CreateLabel();

            asm.Label(ref loopLabel);

            asm.cmp(edi, vtableCount);
            asm.jge(endLabel);

            asm.mov(ecx, __dword_ptr[ebx + edi * 4]);

            // GetRuntimeTypeInfo
            asm.call((uint)_findGetRuntimeTypeInfoMethod);

            asm.mov(ecx, eax);
            asm.mov(eax, __dword_ptr[eax]);
            asm.call(__dword_ptr[eax + 0x14]);

            asm.mov(__dword_ptr[esi + edi * 4], eax);

            asm.inc(edi);
            asm.jmp(loopLabel);

            asm.Label(ref endLabel);

            asm.popad();
            asm.ret();
        }

        using var stream = new MemoryStream();
        asm.Assemble(new StreamCodeWriter(stream), (ulong)_codePlaceAlloc);
        return stream.ToArray();
    }

    [Obsolete]
    public unsafe string ResolveNameFromVTable(IntPtr vTableAddress)
    {
        if (_typeNamesCache.TryGetValue(vTableAddress, out var cachedName))
            return cachedName;

        var vtablePublishAddr = _codePlaceAlloc + MaxShellSize;

        var shell = CreateShellCode(vTableAddress);
        _reader.WriteBytes(_codePlaceAlloc, shell);

        var hThread = CreateRemoteThread(_reader.ProcessHandle,
                                   null,
                                   0,
                                   (delegate* unmanaged[Stdcall]<void*, uint>)_codePlaceAlloc,
                                   null,
                                   0,
                                   null);

        try
        {
            var waitResult = WaitForSingleObject(hThread, 5000);

            if (waitResult != WAIT_OBJECT_0)
            {
                Console.Read();
                CloseHandle(hThread);
                throw new TimeoutException("Remote thread execution timed out");
            }

            var fullNameAddress = _reader.Read<IntPtr>(_codeResultAlloc);

            if (fullNameAddress == IntPtr.Zero)
                return "Unknown type";

            string parsedName = _reader.ReadStringFromMemory(fullNameAddress);

            _typeNamesCache[vTableAddress] = parsedName;

            return parsedName;
        }
        finally
        {
            CloseHandle(hThread);
        }
    }

    [Obsolete]
    private byte[] CreateShellCode(IntPtr vtableAddr)
    {
        var asm = new Assembler(Environment.Is64BitProcess ? 64 : 32);

        if (Environment.Is64BitProcess)
        {
            asm.sub(rsp, 0x20);

            asm.mov(rcx, (ulong)vtableAddr);
            asm.call((ulong)_findGetRuntimeTypeInfoMethod);
            asm.mov(rcx, rax);
            asm.mov(rax, __qword_ptr[rax]);
            asm.call(__qword_ptr[rax + 0x68]);
            asm.mov(rcx, (ulong)_codeResultAlloc);
            asm.mov(__qword_ptr[rcx], rax);

            asm.add(rsp, 0x20);
            asm.ret();
        }
        else
        {
            asm.pushad();
            asm.mov(ecx, (uint)vtableAddr);
            asm.call((uint)_findGetRuntimeTypeInfoMethod);
            asm.mov(ecx, eax);
            asm.mov(eax, __dword_ptr[eax]);
            asm.call(__dword_ptr[eax + 0x14]);
            asm.mov(ecx, eax);
            asm.mov(edx, (uint)_codeResultAlloc);
            asm.mov(__dword_ptr[edx], eax);
            asm.popad();
            asm.ret();
        }

        using var stream = new MemoryStream();
        asm.Assemble(new StreamCodeWriter(stream), (ulong)_codePlaceAlloc);
        return stream.ToArray();
    }

    private IntPtr FindGetRuntimeTypeInfoMethod()
    {
        var toStringPtr = _reader.Read<IntPtr>(
            _aotRuntimeInfo.ObjectVTable + (Environment.Is64BitProcess ? 0x18 : 0x14)); // offset Object__ToString

        var toStringMethodBody = _reader.ReadBytes(toStringPtr, 500);
        int bitness = Environment.Is64BitProcess ? 64 : 32;

        var toStrMethodDec = Decoder.Create(bitness, new ByteArrayCodeReader(toStringMethodBody));
        toStrMethodDec.IP = (ulong)toStringPtr;
        var toStrEndRip = toStrMethodDec.IP + (uint)toStringMethodBody.Length;

        IntPtr findGetRuntimeTypeInfoMethodAddr = IntPtr.Zero;
        int callIndex = 0;
        while (toStrMethodDec.IP < toStrEndRip)
        {
            toStrMethodDec.Decode(out var instruction);
            if (instruction.FlowControl == FlowControl.Call && ++callIndex == 2)
            {
                findGetRuntimeTypeInfoMethodAddr = (IntPtr)instruction.NearBranchTarget;
                break;
            }
        }

        if (findGetRuntimeTypeInfoMethodAddr == IntPtr.Zero)
            return IntPtr.Zero;

        var initializeMethodBody = _reader.ReadBytes(findGetRuntimeTypeInfoMethodAddr, 500);
        var getTypeMethodDec = Decoder.Create(bitness, new ByteArrayCodeReader(initializeMethodBody));
        getTypeMethodDec.IP = (ulong)findGetRuntimeTypeInfoMethodAddr;
        var initEndRip = getTypeMethodDec.IP + (uint)initializeMethodBody.Length;

        while (getTypeMethodDec.IP < initEndRip)
        {
            getTypeMethodDec.Decode(out var instruction);
            if (instruction.FlowControl == FlowControl.Call)
                return (IntPtr)instruction.NearBranchTarget;
        }

        return IntPtr.Zero;
    }

    public unsafe void Dispose()
    {
        VirtualFreeEx(_reader.ProcessHandle, (void*)_codePlaceAlloc, 0, MEM_RELEASE);
        VirtualFreeEx(_reader.ProcessHandle, (void*)_codeResultAlloc, 0, MEM_RELEASE);
    }
}
