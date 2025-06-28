using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct SZArraySignatureHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal SZArraySignatureHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal SZArraySignatureHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x6E000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is SZArraySignatureHandle)
		{
			return _value == ((SZArraySignatureHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(SZArraySignatureHandle handle)
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

	public static implicit operator Handle(SZArraySignatureHandle handle)
	{
		return new Handle(handle._value);
	}

	public SZArraySignature GetSZArraySignature(MetadataReader reader)
	{
		return new SZArraySignature(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 55)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
