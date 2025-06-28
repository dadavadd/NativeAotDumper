using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantByteArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantByteArrayHandle _handle;

	private readonly ByteCollection _value;

	public ConstantByteArrayHandle Handle => _handle;

	public ByteCollection Value => _value;

	internal ConstantByteArray(MetadataReader reader, ConstantByteArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
