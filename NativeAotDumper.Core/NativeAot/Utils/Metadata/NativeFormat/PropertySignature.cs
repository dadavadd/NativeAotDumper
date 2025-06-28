using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct PropertySignature
{
	private readonly MetadataReader _reader;

	private readonly PropertySignatureHandle _handle;

	private readonly CallingConventions _callingConvention;

	private readonly Handle _type;

	private readonly HandleCollection _parameters;

	public PropertySignatureHandle Handle => _handle;

	public CallingConventions CallingConvention => _callingConvention;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle Type => _type;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public HandleCollection Parameters => _parameters;

	internal PropertySignature(MetadataReader reader, PropertySignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _callingConvention);
		offset = streamReader.Read(offset, out _type);
		offset = streamReader.Read(offset, out _parameters);
	}
}
