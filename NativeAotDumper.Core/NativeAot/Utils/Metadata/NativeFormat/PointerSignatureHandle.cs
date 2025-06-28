using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct PointerSignatureHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal PointerSignatureHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal PointerSignatureHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x64000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is PointerSignatureHandle)
		{
			return _value == ((PointerSignatureHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(PointerSignatureHandle handle)
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

	public static implicit operator Handle(PointerSignatureHandle handle)
	{
		return new Handle(handle._value);
	}

	public PointerSignature GetPointerSignature(MetadataReader reader)
	{
		return new PointerSignature(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 50)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
