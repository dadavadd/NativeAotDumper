using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct Property
{
	private readonly MetadataReader _reader;

	private readonly PropertyHandle _handle;

	private readonly PropertyAttributes _flags;

	private readonly ConstantStringValueHandle _name;

	private readonly PropertySignatureHandle _signature;

	private readonly MethodSemanticsHandleCollection _methodSemantics;

	private readonly Handle _defaultValue;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public PropertyHandle Handle => _handle;

	public PropertyAttributes Flags => _flags;

	public ConstantStringValueHandle Name => _name;

	public PropertySignatureHandle Signature => _signature;

	public MethodSemanticsHandleCollection MethodSemantics => _methodSemantics;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ConstantBooleanArray, ConstantBooleanValue, ConstantByteArray, ConstantByteValue, ConstantCharArray, ConstantCharValue, ConstantDoubleArray, ConstantDoubleValue, ConstantEnumArray, ConstantEnumValue, ConstantHandleArray, ConstantInt16Array, ConstantInt16Value, ConstantInt32Array, ConstantInt32Value, ConstantInt64Array, ConstantInt64Value, ConstantReferenceValue, ConstantSByteArray, ConstantSByteValue, ConstantSingleArray, ConstantSingleValue, ConstantStringArray, ConstantStringValue, ConstantUInt16Array, ConstantUInt16Value, ConstantUInt32Array, ConstantUInt32Value, ConstantUInt64Array, ConstantUInt64Value
	public Handle DefaultValue => _defaultValue;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal Property(MetadataReader reader, PropertyHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _signature);
		offset = streamReader.Read(offset, out _methodSemantics);
		offset = streamReader.Read(offset, out _defaultValue);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
