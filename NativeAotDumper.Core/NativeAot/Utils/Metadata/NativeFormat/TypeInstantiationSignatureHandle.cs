using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeInstantiationSignatureHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal TypeInstantiationSignatureHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal TypeInstantiationSignatureHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x78000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is TypeInstantiationSignatureHandle)
		{
			return _value == ((TypeInstantiationSignatureHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(TypeInstantiationSignatureHandle handle)
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

	public static implicit operator Handle(TypeInstantiationSignatureHandle handle)
	{
		return new Handle(handle._value);
	}

	public TypeInstantiationSignature GetTypeInstantiationSignature(MetadataReader reader)
	{
		return new TypeInstantiationSignature(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 60)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
