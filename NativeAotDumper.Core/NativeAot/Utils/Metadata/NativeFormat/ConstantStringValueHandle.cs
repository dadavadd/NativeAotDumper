using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ConstantStringValueHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	public bool StringEquals(string value, MetadataReader reader)
	{
		return reader.StringEquals(this, value);
	}

	internal ConstantStringValueHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal ConstantStringValueHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x34000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConstantStringValueHandle)
		{
			return _value == ((ConstantStringValueHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(ConstantStringValueHandle handle)
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

	public static implicit operator Handle(ConstantStringValueHandle handle)
	{
		return new Handle(handle._value);
	}

	public ConstantStringValue GetConstantStringValue(MetadataReader reader)
	{
		return new ConstantStringValue(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 26)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
