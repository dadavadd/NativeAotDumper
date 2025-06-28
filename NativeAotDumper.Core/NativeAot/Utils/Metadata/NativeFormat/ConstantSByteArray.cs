using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantSByteArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantSByteArrayHandle _handle;

	private readonly SByteCollection _value;

	public ConstantSByteArrayHandle Handle => _handle;

	public SByteCollection Value => _value;

	internal ConstantSByteArray(MetadataReader reader, ConstantSByteArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
