using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt32Value
{
	private readonly MetadataReader _reader;

	private readonly ConstantInt32ValueHandle _handle;

	private readonly int _value;

	public ConstantInt32ValueHandle Handle => _handle;

	public int Value => _value;

	internal ConstantInt32Value(MetadataReader reader, ConstantInt32ValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
