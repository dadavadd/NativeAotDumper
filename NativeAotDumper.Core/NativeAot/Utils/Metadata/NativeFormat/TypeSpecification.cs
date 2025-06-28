using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeSpecification
{
	private readonly MetadataReader _reader;

	private readonly TypeSpecificationHandle _handle;

	private readonly Handle _signature;

	public TypeSpecificationHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeInstantiationSignature, SZArraySignature, ArraySignature, PointerSignature, FunctionPointerSignature, ByReferenceSignature, TypeVariableSignature, MethodTypeVariableSignature
	public Handle Signature => _signature;

	internal TypeSpecification(MetadataReader reader, TypeSpecificationHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _signature);
	}
}
