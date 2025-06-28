using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public sealed class MetadataReader
{
	private MetadataHeader _header;

	internal NativeReader _streamReader;

	/// <summary>
	/// Used as the root entrypoint for metadata, this is where all top-down
	/// structural walks of metadata must start.
	/// </summary>
	public ScopeDefinitionHandleCollection ScopeDefinitions => _header.ScopeDefinitions;

	/// <summary>
	/// Returns a Handle value representing the null value. Can be used
	/// to test handle values of all types for null.
	/// </summary>
	public Handle NullHandle => new Handle(0);

	public unsafe MetadataReader(nint pBuffer, int cbBuffer)
	{
		_streamReader = new NativeReader((byte*)pBuffer, (uint)cbBuffer);
		_header = new MetadataHeader();
		_header.Decode(_streamReader);
	}

	/// <summary>
	/// Returns true if handle is null.
	/// </summary>
	public bool IsNull(Handle handle)
	{
		return handle._value == NullHandle._value;
	}

	/// <summary>
	/// Idempotent - simply returns the provided handle value. Exists for
	/// consistency so that generated code does not need to handle this
	/// as a special case.
	/// </summary>
	public Handle ToHandle(Handle handle)
	{
		return handle;
	}

	internal bool StringEquals(ConstantStringValueHandle handle, string value)
	{
		return _streamReader.StringEquals((uint)handle.Offset, value);
	}

	public ArraySignature GetArraySignature(ArraySignatureHandle handle)
	{
		return new ArraySignature(this, handle);
	}

	public ByReferenceSignature GetByReferenceSignature(ByReferenceSignatureHandle handle)
	{
		return new ByReferenceSignature(this, handle);
	}

	public ConstantBooleanArray GetConstantBooleanArray(ConstantBooleanArrayHandle handle)
	{
		return new ConstantBooleanArray(this, handle);
	}

	public ConstantBooleanValue GetConstantBooleanValue(ConstantBooleanValueHandle handle)
	{
		return new ConstantBooleanValue(this, handle);
	}

	public ConstantByteArray GetConstantByteArray(ConstantByteArrayHandle handle)
	{
		return new ConstantByteArray(this, handle);
	}

	public ConstantByteValue GetConstantByteValue(ConstantByteValueHandle handle)
	{
		return new ConstantByteValue(this, handle);
	}

	public ConstantCharArray GetConstantCharArray(ConstantCharArrayHandle handle)
	{
		return new ConstantCharArray(this, handle);
	}

	public ConstantCharValue GetConstantCharValue(ConstantCharValueHandle handle)
	{
		return new ConstantCharValue(this, handle);
	}

	public ConstantDoubleArray GetConstantDoubleArray(ConstantDoubleArrayHandle handle)
	{
		return new ConstantDoubleArray(this, handle);
	}

	public ConstantDoubleValue GetConstantDoubleValue(ConstantDoubleValueHandle handle)
	{
		return new ConstantDoubleValue(this, handle);
	}

	public ConstantEnumArray GetConstantEnumArray(ConstantEnumArrayHandle handle)
	{
		return new ConstantEnumArray(this, handle);
	}

	public ConstantEnumValue GetConstantEnumValue(ConstantEnumValueHandle handle)
	{
		return new ConstantEnumValue(this, handle);
	}

	public ConstantHandleArray GetConstantHandleArray(ConstantHandleArrayHandle handle)
	{
		return new ConstantHandleArray(this, handle);
	}

	public ConstantInt16Array GetConstantInt16Array(ConstantInt16ArrayHandle handle)
	{
		return new ConstantInt16Array(this, handle);
	}

	public ConstantInt16Value GetConstantInt16Value(ConstantInt16ValueHandle handle)
	{
		return new ConstantInt16Value(this, handle);
	}

	public ConstantInt32Array GetConstantInt32Array(ConstantInt32ArrayHandle handle)
	{
		return new ConstantInt32Array(this, handle);
	}

	public ConstantInt32Value GetConstantInt32Value(ConstantInt32ValueHandle handle)
	{
		return new ConstantInt32Value(this, handle);
	}

	public ConstantInt64Array GetConstantInt64Array(ConstantInt64ArrayHandle handle)
	{
		return new ConstantInt64Array(this, handle);
	}

	public ConstantInt64Value GetConstantInt64Value(ConstantInt64ValueHandle handle)
	{
		return new ConstantInt64Value(this, handle);
	}

	public ConstantReferenceValue GetConstantReferenceValue(ConstantReferenceValueHandle handle)
	{
		return new ConstantReferenceValue(this, handle);
	}

	public ConstantSByteArray GetConstantSByteArray(ConstantSByteArrayHandle handle)
	{
		return new ConstantSByteArray(this, handle);
	}

	public ConstantSByteValue GetConstantSByteValue(ConstantSByteValueHandle handle)
	{
		return new ConstantSByteValue(this, handle);
	}

	public ConstantSingleArray GetConstantSingleArray(ConstantSingleArrayHandle handle)
	{
		return new ConstantSingleArray(this, handle);
	}

	public ConstantSingleValue GetConstantSingleValue(ConstantSingleValueHandle handle)
	{
		return new ConstantSingleValue(this, handle);
	}

	public ConstantStringArray GetConstantStringArray(ConstantStringArrayHandle handle)
	{
		return new ConstantStringArray(this, handle);
	}

	public ConstantStringValue GetConstantStringValue(ConstantStringValueHandle handle)
	{
		return new ConstantStringValue(this, handle);
	}

	public ConstantUInt16Array GetConstantUInt16Array(ConstantUInt16ArrayHandle handle)
	{
		return new ConstantUInt16Array(this, handle);
	}

	public ConstantUInt16Value GetConstantUInt16Value(ConstantUInt16ValueHandle handle)
	{
		return new ConstantUInt16Value(this, handle);
	}

	public ConstantUInt32Array GetConstantUInt32Array(ConstantUInt32ArrayHandle handle)
	{
		return new ConstantUInt32Array(this, handle);
	}

	public ConstantUInt32Value GetConstantUInt32Value(ConstantUInt32ValueHandle handle)
	{
		return new ConstantUInt32Value(this, handle);
	}

	public ConstantUInt64Array GetConstantUInt64Array(ConstantUInt64ArrayHandle handle)
	{
		return new ConstantUInt64Array(this, handle);
	}

	public ConstantUInt64Value GetConstantUInt64Value(ConstantUInt64ValueHandle handle)
	{
		return new ConstantUInt64Value(this, handle);
	}

	public CustomAttribute GetCustomAttribute(CustomAttributeHandle handle)
	{
		return new CustomAttribute(this, handle);
	}

	public Event GetEvent(EventHandle handle)
	{
		return new Event(this, handle);
	}

	public Field GetField(FieldHandle handle)
	{
		return new Field(this, handle);
	}

	public FieldSignature GetFieldSignature(FieldSignatureHandle handle)
	{
		return new FieldSignature(this, handle);
	}

	public FunctionPointerSignature GetFunctionPointerSignature(FunctionPointerSignatureHandle handle)
	{
		return new FunctionPointerSignature(this, handle);
	}

	public GenericParameter GetGenericParameter(GenericParameterHandle handle)
	{
		return new GenericParameter(this, handle);
	}

	public MemberReference GetMemberReference(MemberReferenceHandle handle)
	{
		return new MemberReference(this, handle);
	}

	public Method GetMethod(MethodHandle handle)
	{
		return new Method(this, handle);
	}

	public MethodInstantiation GetMethodInstantiation(MethodInstantiationHandle handle)
	{
		return new MethodInstantiation(this, handle);
	}

	public MethodSemantics GetMethodSemantics(MethodSemanticsHandle handle)
	{
		return new MethodSemantics(this, handle);
	}

	public MethodSignature GetMethodSignature(MethodSignatureHandle handle)
	{
		return new MethodSignature(this, handle);
	}

	public MethodTypeVariableSignature GetMethodTypeVariableSignature(MethodTypeVariableSignatureHandle handle)
	{
		return new MethodTypeVariableSignature(this, handle);
	}

	public ModifiedType GetModifiedType(ModifiedTypeHandle handle)
	{
		return new ModifiedType(this, handle);
	}

	public NamedArgument GetNamedArgument(NamedArgumentHandle handle)
	{
		return new NamedArgument(this, handle);
	}

	public NamespaceDefinition GetNamespaceDefinition(NamespaceDefinitionHandle handle)
	{
		return new NamespaceDefinition(this, handle);
	}

	public NamespaceReference GetNamespaceReference(NamespaceReferenceHandle handle)
	{
		return new NamespaceReference(this, handle);
	}

	public Parameter GetParameter(ParameterHandle handle)
	{
		return new Parameter(this, handle);
	}

	public PointerSignature GetPointerSignature(PointerSignatureHandle handle)
	{
		return new PointerSignature(this, handle);
	}

	public Property GetProperty(PropertyHandle handle)
	{
		return new Property(this, handle);
	}

	public PropertySignature GetPropertySignature(PropertySignatureHandle handle)
	{
		return new PropertySignature(this, handle);
	}

	public QualifiedField GetQualifiedField(QualifiedFieldHandle handle)
	{
		return new QualifiedField(this, handle);
	}

	public QualifiedMethod GetQualifiedMethod(QualifiedMethodHandle handle)
	{
		return new QualifiedMethod(this, handle);
	}

	public SZArraySignature GetSZArraySignature(SZArraySignatureHandle handle)
	{
		return new SZArraySignature(this, handle);
	}

	public ScopeDefinition GetScopeDefinition(ScopeDefinitionHandle handle)
	{
		return new ScopeDefinition(this, handle);
	}

	public ScopeReference GetScopeReference(ScopeReferenceHandle handle)
	{
		return new ScopeReference(this, handle);
	}

	public TypeDefinition GetTypeDefinition(TypeDefinitionHandle handle)
	{
		return new TypeDefinition(this, handle);
	}

	public TypeForwarder GetTypeForwarder(TypeForwarderHandle handle)
	{
		return new TypeForwarder(this, handle);
	}

	public TypeInstantiationSignature GetTypeInstantiationSignature(TypeInstantiationSignatureHandle handle)
	{
		return new TypeInstantiationSignature(this, handle);
	}

	public TypeReference GetTypeReference(TypeReferenceHandle handle)
	{
		return new TypeReference(this, handle);
	}

	public TypeSpecification GetTypeSpecification(TypeSpecificationHandle handle)
	{
		return new TypeSpecification(this, handle);
	}

	public TypeVariableSignature GetTypeVariableSignature(TypeVariableSignatureHandle handle)
	{
		return new TypeVariableSignature(this, handle);
	}
}
