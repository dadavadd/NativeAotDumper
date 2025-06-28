using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct Event
{
	private readonly MetadataReader _reader;

	private readonly EventHandle _handle;

	private readonly EventAttributes _flags;

	private readonly ConstantStringValueHandle _name;

	private readonly Handle _type;

	private readonly MethodSemanticsHandleCollection _methodSemantics;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public EventHandle Handle => _handle;

	public EventAttributes Flags => _flags;

	public ConstantStringValueHandle Name => _name;

	/// One of: TypeDefinition, TypeReference, TypeSpecification
	public Handle Type => _type;

	public MethodSemanticsHandleCollection MethodSemantics => _methodSemantics;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal Event(MetadataReader reader, EventHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _type);
		offset = streamReader.Read(offset, out _methodSemantics);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
