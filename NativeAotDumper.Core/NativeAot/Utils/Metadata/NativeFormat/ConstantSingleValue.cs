using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantSingleValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantSingleValueHandle _handle;

	private readonly float _value;

	public ConstantSingleValueHandle Handle => _handle;

	public float Value => _value;

	internal ConstantSingleValue(MetadataReader reader, ConstantSingleValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _value);
	}
}
