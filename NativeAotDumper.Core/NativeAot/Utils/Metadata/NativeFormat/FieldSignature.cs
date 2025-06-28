using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct FieldSignature
{
	private readonly MetadataReader _reader;

	private readonly FieldSignatureHandle _handle;

	private readonly Handle _type;

	public FieldSignatureHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle Type => _type;

	internal FieldSignature(MetadataReader reader, FieldSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _type);
	}
}
