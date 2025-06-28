using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct CustomAttribute
{
	private readonly MetadataReader _reader;

	private readonly CustomAttributeHandle _handle;

	private readonly Handle _constructor;

	private readonly HandleCollection _fixedArguments;

	private readonly NamedArgumentHandleCollection _namedArguments;

	public CustomAttributeHandle Handle => _handle;

	/// One of: QualifiedMethod, MemberReference
	public Handle Constructor => _constructor;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ConstantBooleanArray, ConstantBooleanValue, ConstantByteArray, ConstantByteValue, ConstantCharArray, ConstantCharValue, ConstantDoubleArray, ConstantDoubleValue, ConstantEnumArray, ConstantEnumValue, ConstantHandleArray, ConstantInt16Array, ConstantInt16Value, ConstantInt32Array, ConstantInt32Value, ConstantInt64Array, ConstantInt64Value, ConstantReferenceValue, ConstantSByteArray, ConstantSByteValue, ConstantSingleArray, ConstantSingleValue, ConstantStringArray, ConstantStringValue, ConstantUInt16Array, ConstantUInt16Value, ConstantUInt32Array, ConstantUInt32Value, ConstantUInt64Array, ConstantUInt64Value
	public HandleCollection FixedArguments => _fixedArguments;

	public NamedArgumentHandleCollection NamedArguments => _namedArguments;

	internal CustomAttribute(MetadataReader reader, CustomAttributeHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _constructor);
		offset = streamReader.Read(offset, out _fixedArguments);
		offset = streamReader.Read(offset, out _namedArguments);
	}
}
