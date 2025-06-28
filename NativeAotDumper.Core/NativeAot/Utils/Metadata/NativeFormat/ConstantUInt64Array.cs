using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt64Array
{
	private readonly MetadataReader _reader;

	private readonly ConstantUInt64ArrayHandle _handle;

	private readonly UInt64Collection _value;

	public ConstantUInt64ArrayHandle Handle => _handle;

	public UInt64Collection Value => _value;

	internal ConstantUInt64Array(MetadataReader reader, ConstantUInt64ArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
