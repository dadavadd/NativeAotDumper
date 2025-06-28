using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct MethodSemantics
{
	private readonly MetadataReader _reader;

	private readonly MethodSemanticsHandle _handle;

	private readonly MethodSemanticsAttributes _attributes;

	private readonly MethodHandle _method;

	public MethodSemanticsHandle Handle => _handle;

	public MethodSemanticsAttributes Attributes => _attributes;

	public MethodHandle Method => _method;

	internal MethodSemantics(MetadataReader reader, MethodSemanticsHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _attributes);
		offset = streamReader.Read(offset, out _method);
	}
}
