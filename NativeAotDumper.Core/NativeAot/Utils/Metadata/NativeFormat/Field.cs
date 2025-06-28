using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct Field
{
	private readonly MetadataReader _reader;

	private readonly FieldHandle _handle;

	private readonly FieldAttributes _flags;

	private readonly ConstantStringValueHandle _name;

	private readonly FieldSignatureHandle _signature;

	private readonly Handle _defaultValue;

	private readonly uint _offset;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public FieldHandle Handle => _handle;

	public FieldAttributes Flags => _flags;

	public ConstantStringValueHandle Name => _name;

	public FieldSignatureHandle Signature => _signature;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ConstantBooleanArray, ConstantBooleanValue, ConstantByteArray, ConstantByteValue, ConstantCharArray, ConstantCharValue, ConstantDoubleArray, ConstantDoubleValue, ConstantEnumArray, ConstantEnumValue, ConstantHandleArray, ConstantInt16Array, ConstantInt16Value, ConstantInt32Array, ConstantInt32Value, ConstantInt64Array, ConstantInt64Value, ConstantReferenceValue, ConstantSByteArray, ConstantSByteValue, ConstantSingleArray, ConstantSingleValue, ConstantStringArray, ConstantStringValue, ConstantUInt16Array, ConstantUInt16Value, ConstantUInt32Array, ConstantUInt32Value, ConstantUInt64Array, ConstantUInt64Value
	public Handle DefaultValue => _defaultValue;

	public uint Offset => _offset;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal Field(MetadataReader reader, FieldHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _signature);
		offset = streamReader.Read(offset, out _defaultValue);
		offset = streamReader.Read(offset, out _offset);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
