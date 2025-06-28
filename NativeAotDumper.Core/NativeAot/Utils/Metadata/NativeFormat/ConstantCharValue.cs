using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantCharValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantCharValueHandle _handle;

	private readonly char _value;

	public ConstantCharValueHandle Handle => _handle;

	public char Value => _value;

	internal ConstantCharValue(MetadataReader reader, ConstantCharValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
