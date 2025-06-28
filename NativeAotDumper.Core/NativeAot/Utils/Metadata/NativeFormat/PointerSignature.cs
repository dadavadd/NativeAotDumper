using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct PointerSignature
{
	private readonly MetadataReader _reader;

	private readonly PointerSignatureHandle _handle;

	private readonly Handle _type;

	public PointerSignatureHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle Type => _type;

	internal PointerSignature(MetadataReader reader, PointerSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _type);
	}
}
