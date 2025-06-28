using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ArraySignature
{
	private readonly MetadataReader _reader;

	private readonly ArraySignatureHandle _handle;

	private readonly Handle _elementType;

	private readonly int _rank;

	private readonly Int32Collection _sizes;

	private readonly Int32Collection _lowerBounds;

	public ArraySignatureHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle ElementType => _elementType;

	public int Rank => _rank;

	public Int32Collection Sizes => _sizes;

	public Int32Collection LowerBounds => _lowerBounds;

	internal ArraySignature(MetadataReader reader, ArraySignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _elementType);
		offset = streamReader.Read(offset, out _rank);
		offset = streamReader.Read(offset, out _sizes);
		offset = streamReader.Read(offset, out _lowerBounds);
	}
}
