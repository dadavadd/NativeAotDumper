using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct MethodTypeVariableSignature
{
	private readonly MetadataReader _reader;

	private readonly MethodTypeVariableSignatureHandle _handle;

	private readonly int _number;

	public MethodTypeVariableSignatureHandle Handle => _handle;

	public int Number => _number;

	internal MethodTypeVariableSignature(MetadataReader reader, MethodTypeVariableSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _number);
	}
}
