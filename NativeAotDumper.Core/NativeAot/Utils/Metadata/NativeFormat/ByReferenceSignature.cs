using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ByReferenceSignature
{
	private readonly MetadataReader _reader;

	private readonly ByReferenceSignatureHandle _handle;

	private readonly Handle _type;

	public ByReferenceSignatureHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle Type => _type;

	internal ByReferenceSignature(MetadataReader reader, ByReferenceSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _type);
	}
}
