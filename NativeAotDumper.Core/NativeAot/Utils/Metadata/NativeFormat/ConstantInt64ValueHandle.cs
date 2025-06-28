using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantInt64ValueHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantInt64ValueHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantInt64ValueHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x26000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantInt64ValueHandle)
		{
			return _value == ((ConstantInt64ValueHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantInt64ValueHandle handle)
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

	public static implicit operator Handle(ConstantInt64ValueHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantInt64Value GetConstantInt64Value(MetadataReader reader)
	{
		return new ConstantInt64Value(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 19)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
