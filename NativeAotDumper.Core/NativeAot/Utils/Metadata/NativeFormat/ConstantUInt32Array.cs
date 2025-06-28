using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt32Array
{
	private readonly MetadataReader _reader;

	private readonly ConstantUInt32ArrayHandle _handle;

	private readonly UInt32Collection _value;

	public ConstantUInt32ArrayHandle Handle => _handle;

	public UInt32Collection Value => _value;

	internal ConstantUInt32Array(MetadataReader reader, ConstantUInt32ArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
