using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantStringValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantStringValueHandle _handle;

	private readonly string _value;

	public ConstantStringValueHandle Handle => _handle;

	public string Value => _value;

	internal ConstantStringValue(MetadataReader reader, ConstantStringValueHandle handle)
	{
		_reader = null;
		_handle = default(ConstantStringValueHandle);
		_value = null;
		if (!handle.IsNil)
		{
			_reader = reader;
			_handle = handle;
			uint offset = (uint)handle.Offset;
			offset = reader._streamReader.Read(offset, out _value);
		}
	}
}
