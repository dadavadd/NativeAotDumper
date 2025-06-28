using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct TypeForwarderHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal TypeForwarderHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal TypeForwarderHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x76000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is TypeForwarderHandle)
		{
			return _value == ((TypeForwarderHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(TypeForwarderHandle handle)
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

	public static implicit operator Handle(TypeForwarderHandle handle)
	{
		return new Handle(handle._value);
	}

	public TypeForwarder GetTypeForwarder(MetadataReader reader)
	{
		return new TypeForwarder(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 59)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
