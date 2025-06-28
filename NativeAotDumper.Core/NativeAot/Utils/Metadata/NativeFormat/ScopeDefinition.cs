using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ScopeDefinition
{
	private readonly MetadataReader _reader;

	private readonly ScopeDefinitionHandle _handle;

	private readonly AssemblyFlags _flags;

	private readonly ConstantStringValueHandle _name;

	private readonly AssemblyHashAlgorithm _hashAlgorithm;

	private readonly ushort _majorVersion;

	private readonly ushort _minorVersion;

	private readonly ushort _buildNumber;

	private readonly ushort _revisionNumber;

	private readonly ByteCollection _publicKey;

	private readonly ConstantStringValueHandle _culture;

	private readonly NamespaceDefinitionHandle _rootNamespaceDefinition;

	private readonly QualifiedMethodHandle _entryPoint;

	private readonly TypeDefinitionHandle _globalModuleType;

	private readonly CustomAttributeHandleCollection _customAttributes;

	private readonly ConstantStringValueHandle _moduleName;

	private readonly ByteCollection _mvid;

	private readonly CustomAttributeHandleCollection _moduleCustomAttributes;

	public ScopeDefinitionHandle Handle => _handle;

	public AssemblyFlags Flags => _flags;

	public ConstantStringValueHandle Name => _name;

	public AssemblyHashAlgorithm HashAlgorithm => _hashAlgorithm;

	public ushort MajorVersion => _majorVersion;

	public ushort MinorVersion => _minorVersion;

	public ushort BuildNumber => _buildNumber;

	public ushort RevisionNumber => _revisionNumber;

	public ByteCollection PublicKey => _publicKey;

	public ConstantStringValueHandle Culture => _culture;

	public NamespaceDefinitionHandle RootNamespaceDefinition => _rootNamespaceDefinition;

	public QualifiedMethodHandle EntryPoint => _entryPoint;

	public TypeDefinitionHandle GlobalModuleType => _globalModuleType;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	public ConstantStringValueHandle ModuleName => _moduleName;

	public ByteCollection Mvid => _mvid;

	public CustomAttributeHandleCollection ModuleCustomAttributes => _moduleCustomAttributes;

	internal ScopeDefinition(MetadataReader reader, ScopeDefinitionHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _hashAlgorithm);
		offset = streamReader.Read(offset, out _majorVersion);
		offset = streamReader.Read(offset, out _minorVersion);
		offset = streamReader.Read(offset, out _buildNumber);
		offset = streamReader.Read(offset, out _revisionNumber);
		offset = streamReader.Read(offset, out _publicKey);
		offset = streamReader.Read(offset, out _culture);
		offset = streamReader.Read(offset, out _rootNamespaceDefinition);
		offset = streamReader.Read(offset, out _entryPoint);
		offset = streamReader.Read(offset, out _globalModuleType);
		offset = streamReader.Read(offset, out _customAttributes);
		offset = streamReader.Read(offset, out _moduleName);
		offset = streamReader.Read(offset, out _mvid);
		offset = streamReader.Read(offset, out _moduleCustomAttributes);
	}
}
