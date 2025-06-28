using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt16Array
{
	private readonly MetadataReader _reader;

	private readonly ConstantInt16ArrayHandle _handle;

	private readonly Int16Collection _value;

	public ConstantInt16ArrayHandle Handle => _handle;

	public Int16Collection Value => _value;

	internal ConstantInt16Array(MetadataReader reader, ConstantInt16ArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
