using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt32Value
{
	private readonly MetadataReader _reader;

	private readonly ConstantUInt32ValueHandle _handle;

	private readonly uint _value;

	public ConstantUInt32ValueHandle Handle => _handle;

	public uint Value => _value;

	internal ConstantUInt32Value(MetadataReader reader, ConstantUInt32ValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
