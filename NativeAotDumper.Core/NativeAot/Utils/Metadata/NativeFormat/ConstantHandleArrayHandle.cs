using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantHandleArrayHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal ConstantHandleArrayHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantHandleArrayHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x1A000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantHandleArrayHandle)
		{
			return _value == ((ConstantHandleArrayHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantHandleArrayHandle handle)
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

	public static implicit operator Handle(ConstantHandleArrayHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantHandleArray GetConstantHandleArray(MetadataReader reader)
	{
		return new ConstantHandleArray(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 13)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
