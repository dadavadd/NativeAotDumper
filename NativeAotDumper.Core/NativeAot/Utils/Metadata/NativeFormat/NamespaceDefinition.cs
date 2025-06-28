using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct NamespaceDefinition
{
	private readonly MetadataReader _reader;

	private readonly NamespaceDefinitionHandle _handle;

	private readonly Handle _parentScopeOrNamespace;

	private readonly ConstantStringValueHandle _name;

	private readonly TypeDefinitionHandleCollection _typeDefinitions;

	private readonly TypeForwarderHandleCollection _typeForwarders;

	private readonly NamespaceDefinitionHandleCollection _namespaceDefinitions;

	public NamespaceDefinitionHandle Handle => _handle;

	/// One of: NamespaceDefinition, ScopeDefinition
	public Handle ParentScopeOrNamespace => _parentScopeOrNamespace;

	public ConstantStringValueHandle Name => _name;

	public TypeDefinitionHandleCollection TypeDefinitions => _typeDefinitions;

	public TypeForwarderHandleCollection TypeForwarders => _typeForwarders;

	public NamespaceDefinitionHandleCollection NamespaceDefinitions => _namespaceDefinitions;

	internal NamespaceDefinition(MetadataReader reader, NamespaceDefinitionHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _parentScopeOrNamespace);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _typeDefinitions);
		offset = streamReader.Read(offset, out _typeForwarders);
		offset = streamReader.Read(offset, out _namespaceDefinitions);
	}
}
