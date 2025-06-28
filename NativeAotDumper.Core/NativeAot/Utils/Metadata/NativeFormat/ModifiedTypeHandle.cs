using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ModifiedTypeHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ModifiedTypeHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ModifiedTypeHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x5A000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ModifiedTypeHandle)
		{
			return _value == ((ModifiedTypeHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ModifiedTypeHandle handle)
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

	public static implicit operator Handle(ModifiedTypeHandle handle)
	{
		return new Handle(handle._value);
	}

	public ModifiedType GetModifiedType(MetadataReader reader)
	{
		return new ModifiedType(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 45)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
