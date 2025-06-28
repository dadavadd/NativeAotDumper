using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt64Value
{
	private readonly MetadataReader _reader;

	private readonly ConstantInt64ValueHandle _handle;

	private readonly long _value;

	public ConstantInt64ValueHandle Handle => _handle;

	public long Value => _value;

	internal ConstantInt64Value(MetadataReader reader, ConstantInt64ValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
