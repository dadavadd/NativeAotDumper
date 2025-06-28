using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeDefinition
{
	private readonly MetadataReader _reader;

	private readonly TypeDefinitionHandle _handle;

	private readonly TypeAttributes _flags;

	private readonly Handle _baseType;

	private readonly NamespaceDefinitionHandle _namespaceDefinition;

	private readonly ConstantStringValueHandle _name;

	private readonly uint _size;

	private readonly ushort _packingSize;

	private readonly TypeDefinitionHandle _enclosingType;

	private readonly TypeDefinitionHandleCollection _nestedTypes;

	private readonly MethodHandleCollection _methods;

	private readonly FieldHandleCollection _fields;

	private readonly PropertyHandleCollection _properties;

	private readonly EventHandleCollection _events;

	private readonly GenericParameterHandleCollection _genericParameters;

	private readonly HandleCollection _interfaces;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public TypeDefinitionHandle Handle => _handle;

	public TypeAttributes Flags => _flags;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public Handle BaseType => _baseType;

	public NamespaceDefinitionHandle NamespaceDefinition => _namespaceDefinition;

	public ConstantStringValueHandle Name => _name;

	public uint Size => _size;

	public ushort PackingSize => _packingSize;

	public TypeDefinitionHandle EnclosingType => _enclosingType;

	public TypeDefinitionHandleCollection NestedTypes => _nestedTypes;

	public MethodHandleCollection Methods => _methods;

	public FieldHandleCollection Fields => _fields;

	public PropertyHandleCollection Properties => _properties;

	public EventHandleCollection Events => _events;

	public GenericParameterHandleCollection GenericParameters => _genericParameters;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public HandleCollection Interfaces => _interfaces;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal TypeDefinition(MetadataReader reader, TypeDefinitionHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _baseType);
		offset = streamReader.Read(offset, out _namespaceDefinition);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _size);
		offset = streamReader.Read(offset, out _packingSize);
		offset = streamReader.Read(offset, out _enclosingType);
		offset = streamReader.Read(offset, out _nestedTypes);
		offset = streamReader.Read(offset, out _methods);
		offset = streamReader.Read(offset, out _fields);
		offset = streamReader.Read(offset, out _properties);
		offset = streamReader.Read(offset, out _events);
		offset = streamReader.Read(offset, out _genericParameters);
		offset = streamReader.Read(offset, out _interfaces);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
