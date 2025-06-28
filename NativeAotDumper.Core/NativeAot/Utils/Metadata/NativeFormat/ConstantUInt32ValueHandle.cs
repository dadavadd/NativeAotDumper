using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt32ValueHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantUInt32ValueHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantUInt32ValueHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x3C000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantUInt32ValueHandle)
		{
			return _value == ((ConstantUInt32ValueHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantUInt32ValueHandle handle)
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

	public static implicit operator Handle(ConstantUInt32ValueHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantUInt32Value GetConstantUInt32Value(MetadataReader reader)
	{
		return new ConstantUInt32Value(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 30)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
