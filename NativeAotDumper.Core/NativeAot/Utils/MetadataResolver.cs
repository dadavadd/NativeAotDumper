using Internal.Metadata.NativeFormat;
using Internal.NativeFormat;
using NativeAotDumper.Core.Enums.ReadyToRun;
using NativeAotDumper.Core.NativeAot.Interfaces;
using NativeAotDumper.Core.Structs.NativeAot;
using NativeAotDumper.Core.Structs.ReadyToRun;

namespace NativeAotDumper.Core.NativeAot.Utils;

internal class MetadataResolver : IMetadataResolver
{
    private readonly IProcessMemoryReader _reader;
    private byte[] _commonFixupsData = null!;
    private nint _commonFixupsBaseAddress;
    private bool _useRelativePointers = true;

    public MetadataResolver(IProcessMemoryReader reader)
    {
        _reader = reader;
    }

    public unsafe RuntimeTypeInfo[] ParseMetadata(IReadOnlyList<ModuleInfoRow> rows)
    {
        EnsureCommonFixupsLoaded(rows);

        var typeMapRow = rows.First(r => r.SectionId == (ReadyToRunSectionType)301); // TypeMap
        var typeMapData = _reader.ReadBytes(typeMapRow.Start, typeMapRow.GetLength());

        var metadataBlobRow = rows.First(r => r.SectionId == (ReadyToRunSectionType)313); // MetadataBlob
        var metadataBlobData = _reader.ReadBytes(metadataBlobRow.Start, metadataBlobRow.GetLength());

        var runtimeTypes = new List<RuntimeTypeInfo>();

        fixed (byte* pTypeMapData = typeMapData)
        fixed (byte* pMetadataBlob = metadataBlobData)
        {
            var nativeHashtableReader = new NativeReader(pTypeMapData, (uint)typeMapData.Length);
            var hashtable = new NativeHashtable(new NativeParser(nativeHashtableReader, 0));
            var entries = hashtable.EnumerateAllEntries();

            var mdReader = new MetadataReader((nint)pMetadataBlob, metadataBlobData.Length);

            NativeParser parser;
            while (!(parser = entries.GetNext()).IsNull)
            {
                uint index = parser.GetUnsigned();
                uint rawHandleValue = parser.GetUnsigned();

                var methodTableAddress = GetAddressFromIndex(index);
                if (methodTableAddress == IntPtr.Zero)
                    continue;

                var mdHandle = Handle.FromIntToken((int)rawHandleValue);
                if (mdHandle.HandleType != HandleType.TypeDefinition)
                    continue;

                var typeDefHandle = new TypeDefinitionHandle(mdHandle);
                var typeDef = mdReader.GetTypeDefinition(typeDefHandle);
                string typeName = mdReader.GetString(typeDef.Name);
                string namespaceName = GetNamespaceFullName(mdReader, typeDef.NamespaceDefinition);
                string fullName = string.IsNullOrEmpty(namespaceName)
                    ? typeName
                    : $"{namespaceName}.{typeName}";

                var runtimeType = new RuntimeTypeInfo
                {
                    MethodTableAddress = methodTableAddress,
                    FullName = fullName,
                    MetadataToken = ((Handle)typeDefHandle).ToIntToken(),
                    Methods = EnumerateAllMethods(mdReader, typeDef.Methods),
                    Fields = EnumerateAllFields(mdReader, typeDef.Fields),
                    Properties = EnumerateAllProperties(mdReader, typeDef.Properties)
                };

                runtimeTypes.Add(runtimeType);
            }
        }

        return runtimeTypes
            .OrderBy(t => t.MethodTableAddress)
            .ToArray();
    }

    private string GetNamespaceFullName(MetadataReader mdReader, NamespaceDefinitionHandle nsDefHandle)
    {
        if (nsDefHandle.IsNil)
            return string.Empty;

        var parts = new List<string>();
        var currentNsDefHandle = nsDefHandle;

        while (!currentNsDefHandle.IsNil)
        {
            var nsDef = mdReader.GetNamespaceDefinition(currentNsDefHandle);
            string name = mdReader.GetString(nsDef.Name);

            if (!string.IsNullOrEmpty(name))
                parts.Add(name);

            if (nsDef.ParentScopeOrNamespace.HandleType == HandleType.NamespaceDefinition)
                currentNsDefHandle = new NamespaceDefinitionHandle(nsDef.ParentScopeOrNamespace);
            else
                break;
        }

        parts.Reverse();
        return string.Join(".", parts);
    }

    private List<RuntimeMethodInfo> EnumerateAllMethods(MetadataReader mdReader, MethodHandleCollection methods)
    {
        var list = new List<RuntimeMethodInfo>();
        foreach (var methodHandle in methods)
        {
            if (methodHandle.IsNil)
                continue;

            var method = mdReader.GetMethod(methodHandle);
            string name = mdReader.GetString(method.Name);
            int token = ((Handle)methodHandle).ToIntToken();

            list.Add(new RuntimeMethodInfo
            {
                Name = name,
                MetadataToken = token
            });
        }
        return list;
    }

    private List<RuntimeFieldInfo> EnumerateAllFields(MetadataReader mdReader, FieldHandleCollection fields)
    {
        var list = new List<RuntimeFieldInfo>();
        foreach (var fieldHandle in fields)
        {
            if (fieldHandle.IsNil)
                continue;

            var field = mdReader.GetField(fieldHandle);
            string name = mdReader.GetString(field.Name);
            int token = ((Handle)fieldHandle).ToIntToken();

            list.Add(new RuntimeFieldInfo
            {
                Name = name,
                MetadataToken = token,
                Offset = field.Offset
            });
        }
        return list;
    }

    private List<RuntimePropertyInfo> EnumerateAllProperties(MetadataReader mdReader, PropertyHandleCollection properties)
    {
        var list = new List<RuntimePropertyInfo>();
        foreach (var propertyHandle in properties)
        {
            if (propertyHandle.IsNil)
                continue;

            var property = mdReader.GetProperty(propertyHandle);
            string name = mdReader.GetString(property.Name);
            int token = ((Handle)propertyHandle).ToIntToken();

            list.Add(new RuntimePropertyInfo
            {
                Name = name,
                MetadataToken = token
            });
        }
        return list;
    }

    private void EnsureCommonFixupsLoaded(IReadOnlyList<ModuleInfoRow> rows)
    {
        if (_commonFixupsData != null)
            return;

        var row = rows.First(r => r.SectionId == (ReadyToRunSectionType)308); // CommonFixupsTable
        _commonFixupsData = _reader.ReadBytes(row.Start, row.GetLength());
        _commonFixupsBaseAddress = row.Start;
    }

    private unsafe nint GetAddressFromIndex(uint index)
    {
        fixed (byte* pBase = _commonFixupsData)
        {
            if (_useRelativePointers)
            {
                int offset = *(int*)(pBase + index * sizeof(int));
                return _commonFixupsBaseAddress + (int)(index * sizeof(int)) + offset;
            }
            else
            {
                return *(nint*)(pBase + index * IntPtr.Size);
            }
        }
    }
}
