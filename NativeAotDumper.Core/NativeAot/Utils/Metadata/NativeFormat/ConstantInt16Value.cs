using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt16Value
{
	private readonly MetadataReader _reader;

	private readonly ConstantInt16ValueHandle _handle;

	private readonly short _value;

	public ConstantInt16ValueHandle Handle => _handle;

	public short Value => _value;

	internal ConstantInt16Value(MetadataReader reader, ConstantInt16ValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
