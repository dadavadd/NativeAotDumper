using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct SZArraySignature
{
	private readonly MetadataReader _reader;

	private readonly SZArraySignatureHandle _handle;

	private readonly Handle _elementType;

	public SZArraySignatureHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle ElementType => _elementType;

	internal SZArraySignature(MetadataReader reader, SZArraySignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _elementType);
	}
}
