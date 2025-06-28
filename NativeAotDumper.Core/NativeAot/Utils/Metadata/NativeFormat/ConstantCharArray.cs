using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantCharArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantCharArrayHandle _handle;

	private readonly CharCollection _value;

	public ConstantCharArrayHandle Handle => _handle;

	public CharCollection Value => _value;

	internal ConstantCharArray(MetadataReader reader, ConstantCharArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
