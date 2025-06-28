using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantBooleanValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantBooleanValueHandle _handle;

	private readonly bool _value;

	public ConstantBooleanValueHandle Handle => _handle;

	public bool Value => _value;

	internal ConstantBooleanValue(MetadataReader reader, ConstantBooleanValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
