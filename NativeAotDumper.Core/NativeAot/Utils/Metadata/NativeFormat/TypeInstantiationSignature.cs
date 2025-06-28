using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeInstantiationSignature
{
	private readonly MetadataReader _reader;

	private readonly TypeInstantiationSignatureHandle _handle;

	private readonly Handle _genericType;

	private readonly HandleCollection _genericTypeArguments;

	public TypeInstantiationSignatureHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public Handle GenericType => _genericType;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public HandleCollection GenericTypeArguments => _genericTypeArguments;

	internal TypeInstantiationSignature(MetadataReader reader, TypeInstantiationSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _genericType);
		offset = streamReader.Read(offset, out _genericTypeArguments);
	}
}
