using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct QualifiedField
{
	private readonly MetadataReader _reader;

	private readonly QualifiedFieldHandle _handle;

	private readonly FieldHandle _field;

	private readonly TypeDefinitionHandle _enclosingType;

	public QualifiedFieldHandle Handle => _handle;

	public FieldHandle Field => _field;

	public TypeDefinitionHandle EnclosingType => _enclosingType;

	internal QualifiedField(MetadataReader reader, QualifiedFieldHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _field);
		offset = streamReader.Read(offset, out _enclosingType);
	}
}
