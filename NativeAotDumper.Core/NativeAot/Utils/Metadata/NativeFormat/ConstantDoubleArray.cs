using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantDoubleArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantDoubleArrayHandle _handle;

	private readonly DoubleCollection _value;

	public ConstantDoubleArrayHandle Handle => _handle;

	public DoubleCollection Value => _value;

	internal ConstantDoubleArray(MetadataReader reader, ConstantDoubleArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
