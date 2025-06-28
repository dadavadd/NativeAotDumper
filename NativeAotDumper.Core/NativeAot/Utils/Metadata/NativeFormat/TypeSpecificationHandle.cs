using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeSpecificationHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal TypeSpecificationHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal TypeSpecificationHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x7C000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is TypeSpecificationHandle)
		{
			return _value == ((TypeSpecificationHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(TypeSpecificationHandle handle)
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

	public static implicit operator Handle(TypeSpecificationHandle handle)
	{
		return new Handle(handle._value);
	}

	public TypeSpecification GetTypeSpecification(MetadataReader reader)
	{
		return new TypeSpecification(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 62)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
