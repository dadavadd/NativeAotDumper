using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantEnumValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantEnumValueHandle _handle;

	private readonly Handle _value;

	private readonly Handle _type;

	public ConstantEnumValueHandle Handle => _handle;

	public Handle Value => _value;

	public Handle Type => _type;

	internal ConstantEnumValue(MetadataReader reader, ConstantEnumValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _value);
		offset = streamReader.Read(offset, out _type);
	}
}
