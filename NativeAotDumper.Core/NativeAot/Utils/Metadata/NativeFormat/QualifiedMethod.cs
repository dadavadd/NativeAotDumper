using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct QualifiedMethod
{
	private readonly MetadataReader _reader;

	private readonly QualifiedMethodHandle _handle;

	private readonly MethodHandle _method;

	private readonly TypeDefinitionHandle _enclosingType;

	public QualifiedMethodHandle Handle => _handle;

	public MethodHandle Method => _method;

	public TypeDefinitionHandle EnclosingType => _enclosingType;

	internal QualifiedMethod(MetadataReader reader, QualifiedMethodHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _method);
		offset = streamReader.Read(offset, out _enclosingType);
	}
}
