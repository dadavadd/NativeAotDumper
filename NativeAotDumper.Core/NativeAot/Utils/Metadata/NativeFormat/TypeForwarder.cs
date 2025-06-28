using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeForwarder
{
	private readonly MetadataReader _reader;

	private readonly TypeForwarderHandle _handle;

	private readonly ScopeReferenceHandle _scope;

	private readonly ConstantStringValueHandle _name;

	private readonly TypeForwarderHandleCollection _nestedTypes;

	public TypeForwarderHandle Handle => _handle;

	public ScopeReferenceHandle Scope => _scope;

	public ConstantStringValueHandle Name => _name;

	public TypeForwarderHandleCollection NestedTypes => _nestedTypes;

	internal TypeForwarder(MetadataReader reader, TypeForwarderHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _scope);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _nestedTypes);
	}
}
