using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantSingleArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantSingleArrayHandle _handle;

	private readonly SingleCollection _value;

	public ConstantSingleArrayHandle Handle => _handle;

	public SingleCollection Value => _value;

	internal ConstantSingleArray(MetadataReader reader, ConstantSingleArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
