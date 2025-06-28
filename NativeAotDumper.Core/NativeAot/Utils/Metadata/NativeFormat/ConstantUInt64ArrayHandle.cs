using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantUInt64ArrayHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantUInt64ArrayHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantUInt64ArrayHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x3E000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantUInt64ArrayHandle)
		{
			return _value == ((ConstantUInt64ArrayHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantUInt64ArrayHandle handle)
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

	public static implicit operator Handle(ConstantUInt64ArrayHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantUInt64Array GetConstantUInt64Array(MetadataReader reader)
	{
		return new ConstantUInt64Array(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 31)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
