using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantByteValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantByteValueHandle _handle;

	private readonly byte _value;

	public ConstantByteValueHandle Handle => _handle;

	public byte Value => _value;

	internal ConstantByteValue(MetadataReader reader, ConstantByteValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
