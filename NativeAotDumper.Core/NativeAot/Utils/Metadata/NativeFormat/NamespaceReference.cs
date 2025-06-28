using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct NamespaceReference
{
	private readonly MetadataReader _reader;

	private readonly NamespaceReferenceHandle _handle;

	private readonly Handle _parentScopeOrNamespace;

	private readonly ConstantStringValueHandle _name;

	public NamespaceReferenceHandle Handle => _handle;

	/// One of: NamespaceReference, ScopeReference
	public Handle ParentScopeOrNamespace => _parentScopeOrNamespace;

	public ConstantStringValueHandle Name => _name;

	internal NamespaceReference(MetadataReader reader, NamespaceReferenceHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _parentScopeOrNamespace);
		offset = streamReader.Read(offset, out _name);
	}
}
