using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt64Array
{
	private readonly MetadataReader _reader;

	private readonly ConstantInt64ArrayHandle _handle;

	private readonly Int64Collection _value;

	public ConstantInt64ArrayHandle Handle => _handle;

	public Int64Collection Value => _value;

	internal ConstantInt64Array(MetadataReader reader, ConstantInt64ArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
