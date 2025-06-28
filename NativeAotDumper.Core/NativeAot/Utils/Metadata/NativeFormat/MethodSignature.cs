using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct MethodSignature
{
	private readonly MetadataReader _reader;

	private readonly MethodSignatureHandle _handle;

	private readonly SignatureCallingConvention _callingConvention;

	private readonly int _genericParameterCount;

	private readonly Handle _returnType;

	private readonly HandleCollection _parameters;

	private readonly HandleCollection _varArgParameters;

	public MethodSignatureHandle Handle => _handle;

	public SignatureCallingConvention CallingConvention => _callingConvention;

	public int GenericParameterCount => _genericParameterCount;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle ReturnType => _returnType;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public HandleCollection Parameters => _parameters;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public HandleCollection VarArgParameters => _varArgParameters;

	internal MethodSignature(MetadataReader reader, MethodSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _callingConvention);
		offset = streamReader.Read(offset, out _genericParameterCount);
		offset = streamReader.Read(offset, out _returnType);
		offset = streamReader.Read(offset, out _parameters);
		offset = streamReader.Read(offset, out _varArgParameters);
	}
}
