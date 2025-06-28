using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeReference
{
	private readonly MetadataReader _reader;

	private readonly TypeReferenceHandle _handle;

	private readonly Handle _parentNamespaceOrType;

	private readonly ConstantStringValueHandle _typeName;

	public TypeReferenceHandle Handle => _handle;

	/// One of: NamespaceReference, TypeReference
	public Handle ParentNamespaceOrType => _parentNamespaceOrType;

	public ConstantStringValueHandle TypeName => _typeName;

	internal TypeReference(MetadataReader reader, TypeReferenceHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _parentNamespaceOrType);
		offset = streamReader.Read(offset, out _typeName);
	}
}
