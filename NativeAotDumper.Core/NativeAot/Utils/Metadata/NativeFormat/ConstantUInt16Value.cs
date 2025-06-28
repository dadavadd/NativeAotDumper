using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt16Value
{
	private readonly MetadataReader _reader;

	private readonly ConstantUInt16ValueHandle _handle;

	private readonly ushort _value;

	public ConstantUInt16ValueHandle Handle => _handle;

	public ushort Value => _value;

	internal ConstantUInt16Value(MetadataReader reader, ConstantUInt16ValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
