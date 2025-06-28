using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct Parameter
{
	private readonly MetadataReader _reader;

	private readonly ParameterHandle _handle;

	private readonly ParameterAttributes _flags;

	private readonly ushort _sequence;

	private readonly ConstantStringValueHandle _name;

	private readonly Handle _defaultValue;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public ParameterHandle Handle => _handle;

	public ParameterAttributes Flags => _flags;

	public ushort Sequence => _sequence;

	public ConstantStringValueHandle Name => _name;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ConstantBooleanArray, ConstantBooleanValue, ConstantByteArray, ConstantByteValue, ConstantCharArray, ConstantCharValue, ConstantDoubleArray, ConstantDoubleValue, ConstantEnumArray, ConstantEnumValue, ConstantHandleArray, ConstantInt16Array, ConstantInt16Value, ConstantInt32Array, ConstantInt32Value, ConstantInt64Array, ConstantInt64Value, ConstantReferenceValue, ConstantSByteArray, ConstantSByteValue, ConstantSingleArray, ConstantSingleValue, ConstantStringArray, ConstantStringValue, ConstantUInt16Array, ConstantUInt16Value, ConstantUInt32Array, ConstantUInt32Value, ConstantUInt64Array, ConstantUInt64Value
	public Handle DefaultValue => _defaultValue;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal Parameter(MetadataReader reader, ParameterHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _sequence);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _defaultValue);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
