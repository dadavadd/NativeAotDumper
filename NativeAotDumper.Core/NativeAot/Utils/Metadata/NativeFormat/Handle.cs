using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct Handle
{
	internal readonly int _value;

	public HandleType HandleType => (HandleType)((uint)_value >> 25);

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	public override bool Equals(object obj)
	{
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(Handle handle)
	{
		return _value == handle._value;
	}

	public override int GetHashCode()
	{
		return _value;
	}

	internal Handle(int value)
	{
		_value = value;
	}

	internal void Validate(params HandleType[] permittedTypes)
	{
		HandleType myHandleType = (HandleType)((uint)_value >> 25);
		foreach (HandleType hType in permittedTypes)
		{
			if (myHandleType == hType)
			{
				return;
			}
		}
		if (myHandleType == HandleType.Null)
		{
			return;
		}
		throw new ArgumentException("Invalid handle type");
	}

	public Handle(HandleType type, int offset)
	{
		_value = (int)((uint)type << 25) | offset;
	}

	public int ToIntToken()
	{
		return _value;
	}

	public static Handle FromIntToken(int value)
	{
		return new Handle(value);
	}

	public ArraySignatureHandle ToArraySignatureHandle(MetadataReader reader)
	{
		return new ArraySignatureHandle(this);
	}

	public ByReferenceSignatureHandle ToByReferenceSignatureHandle(MetadataReader reader)
	{
		return new ByReferenceSignatureHandle(this);
	}

	public ConstantBooleanArrayHandle ToConstantBooleanArrayHandle(MetadataReader reader)
	{
		return new ConstantBooleanArrayHandle(this);
	}

	public ConstantBooleanValueHandle ToConstantBooleanValueHandle(MetadataReader reader)
	{
		return new ConstantBooleanValueHandle(this);
	}

	public ConstantByteArrayHandle ToConstantByteArrayHandle(MetadataReader reader)
	{
		return new ConstantByteArrayHandle(this);
	}

	public ConstantByteValueHandle ToConstantByteValueHandle(MetadataReader reader)
	{
		return new ConstantByteValueHandle(this);
	}

	public ConstantCharArrayHandle ToConstantCharArrayHandle(MetadataReader reader)
	{
		return new ConstantCharArrayHandle(this);
	}

	public ConstantCharValueHandle ToConstantCharValueHandle(MetadataReader reader)
	{
		return new ConstantCharValueHandle(this);
	}

	public ConstantDoubleArrayHandle ToConstantDoubleArrayHandle(MetadataReader reader)
	{
		return new ConstantDoubleArrayHandle(this);
	}

	public ConstantDoubleValueHandle ToConstantDoubleValueHandle(MetadataReader reader)
	{
		return new ConstantDoubleValueHandle(this);
	}

	public ConstantEnumArrayHandle ToConstantEnumArrayHandle(MetadataReader reader)
	{
		return new ConstantEnumArrayHandle(this);
	}

	public ConstantEnumValueHandle ToConstantEnumValueHandle(MetadataReader reader)
	{
		return new ConstantEnumValueHandle(this);
	}

	public ConstantHandleArrayHandle ToConstantHandleArrayHandle(MetadataReader reader)
	{
		return new ConstantHandleArrayHandle(this);
	}

	public ConstantInt16ArrayHandle ToConstantInt16ArrayHandle(MetadataReader reader)
	{
		return new ConstantInt16ArrayHandle(this);
	}

	public ConstantInt16ValueHandle ToConstantInt16ValueHandle(MetadataReader reader)
	{
		return new ConstantInt16ValueHandle(this);
	}

	public ConstantInt32ArrayHandle ToConstantInt32ArrayHandle(MetadataReader reader)
	{
		return new ConstantInt32ArrayHandle(this);
	}

	public ConstantInt32ValueHandle ToConstantInt32ValueHandle(MetadataReader reader)
	{
		return new ConstantInt32ValueHandle(this);
	}

	public ConstantInt64ArrayHandle ToConstantInt64ArrayHandle(MetadataReader reader)
	{
		return new ConstantInt64ArrayHandle(this);
	}

	public ConstantInt64ValueHandle ToConstantInt64ValueHandle(MetadataReader reader)
	{
		return new ConstantInt64ValueHandle(this);
	}

	public ConstantReferenceValueHandle ToConstantReferenceValueHandle(MetadataReader reader)
	{
		return new ConstantReferenceValueHandle(this);
	}

	public ConstantSByteArrayHandle ToConstantSByteArrayHandle(MetadataReader reader)
	{
		return new ConstantSByteArrayHandle(this);
	}

	public ConstantSByteValueHandle ToConstantSByteValueHandle(MetadataReader reader)
	{
		return new ConstantSByteValueHandle(this);
	}

	public ConstantSingleArrayHandle ToConstantSingleArrayHandle(MetadataReader reader)
	{
		return new ConstantSingleArrayHandle(this);
	}

	public ConstantSingleValueHandle ToConstantSingleValueHandle(MetadataReader reader)
	{
		return new ConstantSingleValueHandle(this);
	}

	public ConstantStringArrayHandle ToConstantStringArrayHandle(MetadataReader reader)
	{
		return new ConstantStringArrayHandle(this);
	}

	public ConstantStringValueHandle ToConstantStringValueHandle(MetadataReader reader)
	{
		return new ConstantStringValueHandle(this);
	}

	public ConstantUInt16ArrayHandle ToConstantUInt16ArrayHandle(MetadataReader reader)
	{
		return new ConstantUInt16ArrayHandle(this);
	}

	public ConstantUInt16ValueHandle ToConstantUInt16ValueHandle(MetadataReader reader)
	{
		return new ConstantUInt16ValueHandle(this);
	}

	public ConstantUInt32ArrayHandle ToConstantUInt32ArrayHandle(MetadataReader reader)
	{
		return new ConstantUInt32ArrayHandle(this);
	}

	public ConstantUInt32ValueHandle ToConstantUInt32ValueHandle(MetadataReader reader)
	{
		return new ConstantUInt32ValueHandle(this);
	}

	public ConstantUInt64ArrayHandle ToConstantUInt64ArrayHandle(MetadataReader reader)
	{
		return new ConstantUInt64ArrayHandle(this);
	}

	public ConstantUInt64ValueHandle ToConstantUInt64ValueHandle(MetadataReader reader)
	{
		return new ConstantUInt64ValueHandle(this);
	}

	public CustomAttributeHandle ToCustomAttributeHandle(MetadataReader reader)
	{
		return new CustomAttributeHandle(this);
	}

	public EventHandle ToEventHandle(MetadataReader reader)
	{
		return new EventHandle(this);
	}

	public FieldHandle ToFieldHandle(MetadataReader reader)
	{
		return new FieldHandle(this);
	}

	public FieldSignatureHandle ToFieldSignatureHandle(MetadataReader reader)
	{
		return new FieldSignatureHandle(this);
	}

	public FunctionPointerSignatureHandle ToFunctionPointerSignatureHandle(MetadataReader reader)
	{
		return new FunctionPointerSignatureHandle(this);
	}

	public GenericParameterHandle ToGenericParameterHandle(MetadataReader reader)
	{
		return new GenericParameterHandle(this);
	}

	public MemberReferenceHandle ToMemberReferenceHandle(MetadataReader reader)
	{
		return new MemberReferenceHandle(this);
	}

	public MethodHandle ToMethodHandle(MetadataReader reader)
	{
		return new MethodHandle(this);
	}

	public MethodInstantiationHandle ToMethodInstantiationHandle(MetadataReader reader)
	{
		return new MethodInstantiationHandle(this);
	}

	public MethodSemanticsHandle ToMethodSemanticsHandle(MetadataReader reader)
	{
		return new MethodSemanticsHandle(this);
	}

	public MethodSignatureHandle ToMethodSignatureHandle(MetadataReader reader)
	{
		return new MethodSignatureHandle(this);
	}

	public MethodTypeVariableSignatureHandle ToMethodTypeVariableSignatureHandle(MetadataReader reader)
	{
		return new MethodTypeVariableSignatureHandle(this);
	}

	public ModifiedTypeHandle ToModifiedTypeHandle(MetadataReader reader)
	{
		return new ModifiedTypeHandle(this);
	}

	public NamedArgumentHandle ToNamedArgumentHandle(MetadataReader reader)
	{
		return new NamedArgumentHandle(this);
	}

	public NamespaceDefinitionHandle ToNamespaceDefinitionHandle(MetadataReader reader)
	{
		return new NamespaceDefinitionHandle(this);
	}

	public NamespaceReferenceHandle ToNamespaceReferenceHandle(MetadataReader reader)
	{
		return new NamespaceReferenceHandle(this);
	}

	public ParameterHandle ToParameterHandle(MetadataReader reader)
	{
		return new ParameterHandle(this);
	}

	public PointerSignatureHandle ToPointerSignatureHandle(MetadataReader reader)
	{
		return new PointerSignatureHandle(this);
	}

	public PropertyHandle ToPropertyHandle(MetadataReader reader)
	{
		return new PropertyHandle(this);
	}

	public PropertySignatureHandle ToPropertySignatureHandle(MetadataReader reader)
	{
		return new PropertySignatureHandle(this);
	}

	public QualifiedFieldHandle ToQualifiedFieldHandle(MetadataReader reader)
	{
		return new QualifiedFieldHandle(this);
	}

	public QualifiedMethodHandle ToQualifiedMethodHandle(MetadataReader reader)
	{
		return new QualifiedMethodHandle(this);
	}

	public SZArraySignatureHandle ToSZArraySignatureHandle(MetadataReader reader)
	{
		return new SZArraySignatureHandle(this);
	}

	public ScopeDefinitionHandle ToScopeDefinitionHandle(MetadataReader reader)
	{
		return new ScopeDefinitionHandle(this);
	}

	public ScopeReferenceHandle ToScopeReferenceHandle(MetadataReader reader)
	{
		return new ScopeReferenceHandle(this);
	}

	public TypeDefinitionHandle ToTypeDefinitionHandle(MetadataReader reader)
	{
		return new TypeDefinitionHandle(this);
	}

	public TypeForwarderHandle ToTypeForwarderHandle(MetadataReader reader)
	{
		return new TypeForwarderHandle(this);
	}

	public TypeInstantiationSignatureHandle ToTypeInstantiationSignatureHandle(MetadataReader reader)
	{
		return new TypeInstantiationSignatureHandle(this);
	}

	public TypeReferenceHandle ToTypeReferenceHandle(MetadataReader reader)
	{
		return new TypeReferenceHandle(this);
	}

	public TypeSpecificationHandle ToTypeSpecificationHandle(MetadataReader reader)
	{
		return new TypeSpecificationHandle(this);
	}

	public TypeVariableSignatureHandle ToTypeVariableSignatureHandle(MetadataReader reader)
	{
		return new TypeVariableSignatureHandle(this);
	}
}
