using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct FunctionPointerSignature
{
	private readonly MetadataReader _reader;

	private readonly FunctionPointerSignatureHandle _handle;

	private readonly MethodSignatureHandle _signature;

	public FunctionPointerSignatureHandle Handle => _handle;

	public MethodSignatureHandle Signature => _signature;

	internal FunctionPointerSignature(MetadataReader reader, FunctionPointerSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _signature);
	}
}
