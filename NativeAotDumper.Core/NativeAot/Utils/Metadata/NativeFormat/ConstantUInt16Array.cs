using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt16Array
{
	private readonly MetadataReader _reader;

	private readonly ConstantUInt16ArrayHandle _handle;

	private readonly UInt16Collection _value;

	public ConstantUInt16ArrayHandle Handle => _handle;

	public UInt16Collection Value => _value;

	internal ConstantUInt16Array(MetadataReader reader, ConstantUInt16ArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
