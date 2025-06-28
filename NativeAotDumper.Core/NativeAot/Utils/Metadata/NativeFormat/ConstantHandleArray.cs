using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantHandleArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantHandleArrayHandle _handle;

	private readonly HandleCollection _value;

	public ConstantHandleArrayHandle Handle => _handle;

	public HandleCollection Value => _value;

	internal ConstantHandleArray(MetadataReader reader, ConstantHandleArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
