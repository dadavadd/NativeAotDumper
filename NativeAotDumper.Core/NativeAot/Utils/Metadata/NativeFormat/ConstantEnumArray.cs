using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantEnumArray
{
	private readonly MetadataReader _reader;

	private readonly ConstantEnumArrayHandle _handle;

	private readonly Handle _elementType;

	private readonly Handle _value;

	public ConstantEnumArrayHandle Handle => _handle;

	public Handle ElementType => _elementType;

	public Handle Value => _value;

	internal ConstantEnumArray(MetadataReader reader, ConstantEnumArrayHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _elementType);
		offset = streamReader.Read(offset, out _value);
	}
}
