using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt32Array
{
	private readonly MetadataReader _reader;

	private readonly ConstantInt32ArrayHandle _handle;

	private readonly Int32Collection _value;

	public ConstantInt32ArrayHandle Handle => _handle;

	public Int32Collection Value => _value;

	internal ConstantInt32Array(MetadataReader reader, ConstantInt32ArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
