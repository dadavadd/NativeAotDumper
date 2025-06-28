using System.Reflection;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

internal static class MdBinaryReader
{
	public static uint Read(this NativeReader reader, uint offset, out bool value)
	{
		value = reader.ReadUInt8(offset) != 0;
		return offset + 1;
	}

	public static uint Read(this NativeReader reader, uint offset, out string value)
	{
		return reader.DecodeString(offset, out value);
	}

	public static uint Read(this NativeReader reader, uint offset, out char value)
	{
		offset = reader.DecodeUnsigned(offset, out var val);
		value = (char)val;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out short value)
	{
		offset = reader.DecodeSigned(offset, out var val);
		value = (short)val;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out sbyte value)
	{
		value = (sbyte)reader.ReadUInt8(offset);
		return offset + 1;
	}

	public static uint Read(this NativeReader reader, uint offset, out ulong value)
	{
		return reader.DecodeUnsignedLong(offset, out value);
	}

	public static uint Read(this NativeReader reader, uint offset, out int value)
	{
		return reader.DecodeSigned(offset, out value);
	}

	public static uint Read(this NativeReader reader, uint offset, out uint value)
	{
		return reader.DecodeUnsigned(offset, out value);
	}

	public static uint Read(this NativeReader reader, uint offset, out byte value)
	{
		value = reader.ReadUInt8(offset);
		return offset + 1;
	}

	public static uint Read(this NativeReader reader, uint offset, out ushort value)
	{
		offset = reader.DecodeUnsigned(offset, out var val);
		value = (ushort)val;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out long value)
	{
		return reader.DecodeSignedLong(offset, out value);
	}

	public static uint Read(this NativeReader reader, uint offset, out Handle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var rawValue);
		handle = new Handle((HandleType)(rawValue & 0x7F), (int)(rawValue >> 7));
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out float value)
	{
		value = reader.ReadFloat(offset);
		return offset + 4;
	}

	public static uint Read(this NativeReader reader, uint offset, out double value)
	{
		value = reader.ReadDouble(offset);
		return offset + 8;
	}

	public static uint Read(this NativeReader reader, uint offset, out BooleanCollection values)
	{
		values = new BooleanCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out CharCollection values)
	{
		values = new CharCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 2);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ByteCollection values)
	{
		values = new ByteCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out SByteCollection values)
	{
		values = new SByteCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out Int16Collection values)
	{
		values = new Int16Collection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 2);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out UInt16Collection values)
	{
		values = new UInt16Collection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 2);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out Int32Collection values)
	{
		values = new Int32Collection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 4);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out UInt32Collection values)
	{
		values = new UInt32Collection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 4);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out Int64Collection values)
	{
		values = new Int64Collection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 8);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out UInt64Collection values)
	{
		values = new UInt64Collection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 8);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out SingleCollection values)
	{
		values = new SingleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 4);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out DoubleCollection values)
	{
		values = new DoubleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		offset = checked(offset + count * 8);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out AssemblyFlags value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (AssemblyFlags)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out AssemblyHashAlgorithm value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (AssemblyHashAlgorithm)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out CallingConventions value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (CallingConventions)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out SignatureCallingConvention value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (SignatureCallingConvention)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out EventAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (EventAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out FieldAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (FieldAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out GenericParameterAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (GenericParameterAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out GenericParameterKind value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (GenericParameterKind)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (MethodAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodImplAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (MethodImplAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodSemanticsAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (MethodSemanticsAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out NamedArgumentMemberKind value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (NamedArgumentMemberKind)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ParameterAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (ParameterAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out PInvokeAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (PInvokeAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out PropertyAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (PropertyAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeAttributes value)
	{
		offset = reader.DecodeUnsigned(offset, out var ivalue);
		value = (TypeAttributes)ivalue;
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out HandleCollection values)
	{
		values = new HandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ArraySignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ArraySignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ByReferenceSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ByReferenceSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantBooleanArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantBooleanArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantBooleanValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantBooleanValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantByteArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantByteArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantByteValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantByteValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantCharArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantCharArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantCharValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantCharValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantDoubleArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantDoubleArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantDoubleValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantDoubleValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantEnumArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantEnumArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantEnumValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantEnumValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantHandleArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantHandleArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantInt16ArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantInt16ArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantInt16ValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantInt16ValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantInt32ArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantInt32ArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantInt32ValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantInt32ValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantInt64ArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantInt64ArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantInt64ValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantInt64ValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantReferenceValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantReferenceValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantSByteArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantSByteArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantSByteValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantSByteValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantSingleArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantSingleArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantSingleValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantSingleValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantStringArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantStringArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantStringValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantStringValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantUInt16ArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantUInt16ArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantUInt16ValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantUInt16ValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantUInt32ArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantUInt32ArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantUInt32ValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantUInt32ValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantUInt64ArrayHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantUInt64ArrayHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ConstantUInt64ValueHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ConstantUInt64ValueHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out CustomAttributeHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new CustomAttributeHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out EventHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new EventHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out FieldHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new FieldHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out FieldSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new FieldSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out FunctionPointerSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new FunctionPointerSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out GenericParameterHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new GenericParameterHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MemberReferenceHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new MemberReferenceHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new MethodHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodInstantiationHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new MethodInstantiationHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodSemanticsHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new MethodSemanticsHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new MethodSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodTypeVariableSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new MethodTypeVariableSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ModifiedTypeHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ModifiedTypeHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out NamedArgumentHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new NamedArgumentHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out NamespaceDefinitionHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new NamespaceDefinitionHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out NamespaceReferenceHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new NamespaceReferenceHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ParameterHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ParameterHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out PointerSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new PointerSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out PropertyHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new PropertyHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out PropertySignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new PropertySignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out QualifiedFieldHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new QualifiedFieldHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out QualifiedMethodHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new QualifiedMethodHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out SZArraySignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new SZArraySignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ScopeDefinitionHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ScopeDefinitionHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ScopeReferenceHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new ScopeReferenceHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeDefinitionHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new TypeDefinitionHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeForwarderHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new TypeForwarderHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeInstantiationSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new TypeInstantiationSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeReferenceHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new TypeReferenceHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeSpecificationHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new TypeSpecificationHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeVariableSignatureHandle handle)
	{
		offset = reader.DecodeUnsigned(offset, out var value);
		handle = new TypeVariableSignatureHandle((int)value);
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out NamedArgumentHandleCollection values)
	{
		values = new NamedArgumentHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodSemanticsHandleCollection values)
	{
		values = new MethodSemanticsHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out CustomAttributeHandleCollection values)
	{
		values = new CustomAttributeHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ParameterHandleCollection values)
	{
		values = new ParameterHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out GenericParameterHandleCollection values)
	{
		values = new GenericParameterHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeDefinitionHandleCollection values)
	{
		values = new TypeDefinitionHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out TypeForwarderHandleCollection values)
	{
		values = new TypeForwarderHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out NamespaceDefinitionHandleCollection values)
	{
		values = new NamespaceDefinitionHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out MethodHandleCollection values)
	{
		values = new MethodHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out FieldHandleCollection values)
	{
		values = new FieldHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out PropertyHandleCollection values)
	{
		values = new PropertyHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out EventHandleCollection values)
	{
		values = new EventHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}

	public static uint Read(this NativeReader reader, uint offset, out ScopeDefinitionHandleCollection values)
	{
		values = new ScopeDefinitionHandleCollection(reader, offset);
		offset = reader.DecodeUnsigned(offset, out var count);
		for (uint i = 0u; i < count; i++)
		{
			offset = reader.SkipInteger(offset);
		}
		return offset;
	}
}
