using System;
using System.Diagnostics;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct MethodSemanticsHandle
{
	internal readonly int _value;

	internal int Offset => _value & 0x1FFFFFF;

	public bool IsNil => (_value & 0x1FFFFFF) == 0;

	internal MethodSemanticsHandle(Handle handle)
		: this(handle._value)
	{
	}

	internal MethodSemanticsHandle(int value)
	{
		_ = (byte)(value >>> 25);
		_value = (value & 0x1FFFFFF) | 0x54000000;
	}

	public override bool Equals(object obj)
	{
		if (obj is MethodSemanticsHandle)
		{
			return _value == ((MethodSemanticsHandle)obj)._value;
		}
		if (obj is Handle)
		{
			return _value == ((Handle)obj)._value;
		}
		return false;
	}

	public bool Equals(MethodSemanticsHandle handle)
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

	public static implicit operator Handle(MethodSemanticsHandle handle)
	{
		return new Handle(handle._value);
	}

	public MethodSemantics GetMethodSemantics(MetadataReader reader)
	{
		return new MethodSemantics(reader, this);
	}

	[Conditional("DEBUG")]
	internal void _Validate()
	{
		if ((byte)(_value >>> 25) != 42)
		{
			throw new ArgumentException();
		}
	}

	public override string ToString()
	{
		return $"{_value:X8}";
	}
}
