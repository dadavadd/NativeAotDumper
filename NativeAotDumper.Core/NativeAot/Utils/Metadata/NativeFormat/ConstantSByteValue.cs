using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantSByteValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantSByteValueHandle _handle;

	private readonly sbyte _value;

	public ConstantSByteValueHandle Handle => _handle;

	public sbyte Value => _value;

	internal ConstantSByteValue(MetadataReader reader, ConstantSByteValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
