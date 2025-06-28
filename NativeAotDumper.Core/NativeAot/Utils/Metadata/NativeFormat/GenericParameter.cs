using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct GenericParameter
{
	private readonly MetadataReader _reader;

	private readonly GenericParameterHandle _handle;

	private readonly ushort _number;

	private readonly GenericParameterAttributes _flags;

	private readonly GenericParameterKind _kind;

	private readonly ConstantStringValueHandle _name;

	private readonly HandleCollection _constraints;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public GenericParameterHandle Handle => _handle;

	public ushort Number => _number;

	public GenericParameterAttributes Flags => _flags;

	public GenericParameterKind Kind => _kind;

	public ConstantStringValueHandle Name => _name;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public HandleCollection Constraints => _constraints;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal GenericParameter(MetadataReader reader, GenericParameterHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _number);
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _kind);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _constraints);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
