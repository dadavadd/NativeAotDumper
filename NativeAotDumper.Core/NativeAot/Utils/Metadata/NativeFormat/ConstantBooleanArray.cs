using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantBooleanArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantBooleanArrayHandle _handle;

	private readonly BooleanCollection _value;

	public ConstantBooleanArrayHandle Handle => _handle;

	public BooleanCollection Value => _value;

	internal ConstantBooleanArray(MetadataReader reader, ConstantBooleanArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
