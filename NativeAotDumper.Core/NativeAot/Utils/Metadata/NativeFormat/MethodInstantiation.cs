using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct MethodInstantiation
{
	private readonly MetadataReader _reader;

	private readonly MethodInstantiationHandle _handle;

	private readonly Handle _method;

	private readonly HandleCollection _genericTypeArguments;

	public MethodInstantiationHandle Handle => _handle;

	/// One of: QualifiedMethod, MemberReference
	public Handle Method => _method;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public HandleCollection GenericTypeArguments => _genericTypeArguments;

	internal MethodInstantiation(MetadataReader reader, MethodInstantiationHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _method);
		offset = streamReader.Read(offset, out _genericTypeArguments);
	}
}
