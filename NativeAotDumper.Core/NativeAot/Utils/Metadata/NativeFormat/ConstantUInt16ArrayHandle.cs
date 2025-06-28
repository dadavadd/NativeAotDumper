using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt16ArrayHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantUInt16ArrayHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantUInt16ArrayHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x36000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantUInt16ArrayHandle)
		{
			return _value == ((ConstantUInt16ArrayHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantUInt16ArrayHandle handle)
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

	public static implicit operator Handle(ConstantUInt16ArrayHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantUInt16Array GetConstantUInt16Array(MetadataReader reader)
	{
		return new ConstantUInt16Array(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 27)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
