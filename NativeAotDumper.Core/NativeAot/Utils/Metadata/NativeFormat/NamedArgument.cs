using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct NamedArgument
{
	private readonly MetadataReader _reader;

	private readonly NamedArgumentHandle _handle;

	private readonly NamedArgumentMemberKind _flags;

	private readonly ConstantStringValueHandle _name;

	private readonly Handle _type;

	private readonly Handle _value;

	public NamedArgumentHandle Handle => _handle;

	public NamedArgumentMemberKind Flags => _flags;

	public ConstantStringValueHandle Name => _name;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public Handle Type => _type;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ConstantBooleanArray, ConstantBooleanValue, ConstantByteArray, ConstantByteValue, ConstantCharArray, ConstantCharValue, ConstantDoubleArray, ConstantDoubleValue, ConstantEnumArray, ConstantEnumValue, ConstantHandleArray, ConstantInt16Array, ConstantInt16Value, ConstantInt32Array, ConstantInt32Value, ConstantInt64Array, ConstantInt64Value, ConstantReferenceValue, ConstantSByteArray, ConstantSByteValue, ConstantSingleArray, ConstantSingleValue, ConstantStringArray, ConstantStringValue, ConstantUInt16Array, ConstantUInt16Value, ConstantUInt32Array, ConstantUInt32Value, ConstantUInt64Array, ConstantUInt64Value
	public Handle Value => _value;

	internal NamedArgument(MetadataReader reader, NamedArgumentHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _type);
		offset = streamReader.Read(offset, out _value);
	}
}
