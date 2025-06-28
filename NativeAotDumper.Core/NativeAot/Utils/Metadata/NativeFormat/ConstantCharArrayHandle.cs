using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantCharArrayHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantCharArrayHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantCharArrayHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0xE000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantCharArrayHandle)
		{
			return _value == ((ConstantCharArrayHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantCharArrayHandle handle)
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

	public static implicit operator Handle(ConstantCharArrayHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantCharArray GetConstantCharArray(MetadataReader reader)
	{
		return new ConstantCharArray(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 7)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
