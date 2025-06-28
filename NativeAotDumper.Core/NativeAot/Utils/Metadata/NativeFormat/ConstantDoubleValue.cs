using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantDoubleValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantDoubleValueHandle _handle;

	private readonly double _value;

	public ConstantDoubleValueHandle Handle => _handle;

	public double Value => _value;

	internal ConstantDoubleValue(MetadataReader reader, ConstantDoubleValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
