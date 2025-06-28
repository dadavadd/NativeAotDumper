using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantStringArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantStringArrayHandle _handle;

	private readonly HandleCollection _value;

	public ConstantStringArrayHandle Handle => _handle;

	/// One of: ConstantStringValue, ConstantReferenceValue
	public HandleCollection Value => _value;

	internal ConstantStringArray(MetadataReader reader, ConstantStringArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
