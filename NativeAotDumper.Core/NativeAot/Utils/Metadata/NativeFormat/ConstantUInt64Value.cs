using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt64Value
{
	private readonly MetadataReader _reader;

	private readonly ConstantUInt64ValueHandle _handle;

	private readonly ulong _value;

	public ConstantUInt64ValueHandle Handle => _handle;

	public ulong Value => _value;

	internal ConstantUInt64Value(MetadataReader reader, ConstantUInt64ValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
