using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct MemberReference
{
	private readonly MetadataReader _reader;

	private readonly MemberReferenceHandle _handle;

	private readonly Handle _parent;

	private readonly ConstantStringValueHandle _name;

	private readonly Handle _signature;

	public MemberReferenceHandle Handle => _handle;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public Handle Parent => _parent;

	public ConstantStringValueHandle Name => _name;

	/// One of: MethodSignature, FieldSignature
	public Handle Signature => _signature;

	internal MemberReference(MetadataReader reader, MemberReferenceHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _parent);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _signature);
	}
}
