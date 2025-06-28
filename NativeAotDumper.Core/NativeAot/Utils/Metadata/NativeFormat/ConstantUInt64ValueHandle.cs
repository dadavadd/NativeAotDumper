using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt64ValueHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantUInt64ValueHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantUInt64ValueHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x40000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantUInt64ValueHandle)
		{
			return _value == ((ConstantUInt64ValueHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantUInt64ValueHandle handle)
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

	public static implicit operator Handle(ConstantUInt64ValueHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantUInt64Value GetConstantUInt64Value(MetadataReader reader)
	{
		return new ConstantUInt64Value(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 32)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
