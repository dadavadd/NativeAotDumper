using System;
using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct Method
{
	private readonly MetadataReader _reader;

	private readonly MethodHandle _handle;

	private readonly MethodAttributes _flags;

	private readonly MethodImplAttributes _implFlags;

	private readonly ConstantStringValueHandle _name;

	private readonly MethodSignatureHandle _signature;

	private readonly ParameterHandleCollection _parameters;

	private readonly GenericParameterHandleCollection _genericParameters;

	private readonly CustomAttributeHandleCollection _customAttributes;

	public MethodHandle Handle => _handle;

	public MethodAttributes Flags => _flags;

	public MethodImplAttributes ImplFlags => _implFlags;

	public ConstantStringValueHandle Name => _name;

	public MethodSignatureHandle Signature => _signature;

	public ParameterHandleCollection Parameters => _parameters;

	public GenericParameterHandleCollection GenericParameters => _genericParameters;

	public CustomAttributeHandleCollection CustomAttributes => _customAttributes;

	internal Method(MetadataReader reader, MethodHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _implFlags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _signature);
		offset = streamReader.Read(offset, out _parameters);
		offset = streamReader.Read(offset, out _genericParameters);
		offset = streamReader.Read(offset, out _customAttributes);
	}
}
