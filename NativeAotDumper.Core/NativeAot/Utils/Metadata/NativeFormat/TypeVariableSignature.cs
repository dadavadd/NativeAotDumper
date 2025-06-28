using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeVariableSignature
{
	private readonly MetadataReader _reader;

	private readonly TypeVariableSignatureHandle _handle;

	private readonly int _number;

	public TypeVariableSignatureHandle Handle => _handle;

	public int Number => _number;

	internal TypeVariableSignature(MetadataReader reader, TypeVariableSignatureHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		offset = reader._streamReader.Read(offset, out _number);
	}
}
