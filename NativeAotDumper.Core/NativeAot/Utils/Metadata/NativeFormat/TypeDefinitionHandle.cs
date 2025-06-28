using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeDefinitionHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal TypeDefinitionHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal TypeDefinitionHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x74000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is TypeDefinitionHandle)
		{
			return _value == ((TypeDefinitionHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(TypeDefinitionHandle handle)
	{
		return _value == handle._value;
	}

	public bool Equals(Handle handle)
	{
		return _value == handle._value;
	}

	public override int GetHashCode()
	{
		return _value;
	}

	public static implicit operator Handle(TypeDefinitionHandle handle)
	{
		return new Handle(handle._value);
	}

	public TypeDefinition GetTypeDefinition(MetadataReader reader)
	{
		return new TypeDefinition(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 58)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
