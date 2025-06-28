using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ModifiedType
{
	private readonly MetadataReader _reader;

	private readonly ModifiedTypeHandle _handle;

	private readonly bool _isOptional;

	private readonly Handle _modifierType;

	private readonly Handle _type;

	public ModifiedTypeHandle Handle => _handle;

	public bool IsOptional => _isOptional;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public Handle ModifierType => _modifierType;

	/// One of: TypeDefinition, TypeReference, TypeSpecification, ModifiedType
	public Handle Type => _type;

	internal ModifiedType(MetadataReader reader, ModifiedTypeHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _isOptional);
		offset = streamReader.Read(offset, out _modifierType);
		offset = streamReader.Read(offset, out _type);
	}
}
