using System;
using System.Text;

namespace Internal.NativeFormat;

internal sealed class NativeReader
{
	private unsafe readonly byte* _base;

	private readonly uint _size;

	public uint Size => _size;

	public unsafe NativeReader(byte* base_, uint size)
	{
		if (size >= 1073741823)
		{
			ThrowBadImageFormatException();
		}
		_base = base_;
		_size = size;
	}

	public unsafe uint AddressToOffset(nint address)
	{
		return (uint)((byte*)address - _base);
	}

	public unsafe nint OffsetToAddress(uint offset)
	{
		return new IntPtr(_base + offset);
	}

	public static void ThrowBadImageFormatException()
	{
		throw new BadImageFormatException();
	}

	private uint EnsureOffsetInRange(uint offset, uint lookAhead)
	{
		if ((int)offset < 0 || offset + lookAhead >= _size)
		{
			ThrowBadImageFormatException();
		}
		return offset;
	}

	public unsafe byte ReadUInt8(uint offset)
	{
		EnsureOffsetInRange(offset, 0u);
		byte* data = _base + offset;
		return NativePrimitiveDecoder.ReadUInt8(ref data);
	}

	public unsafe ushort ReadUInt16(uint offset)
	{
		EnsureOffsetInRange(offset, 1u);
		byte* data = _base + offset;
		return NativePrimitiveDecoder.ReadUInt16(ref data);
	}

	public unsafe uint ReadUInt32(uint offset)
	{
		EnsureOffsetInRange(offset, 3u);
		byte* data = _base + offset;
		return NativePrimitiveDecoder.ReadUInt32(ref data);
	}

	public unsafe ulong ReadUInt64(uint offset)
	{
		EnsureOffsetInRange(offset, 7u);
		byte* data = _base + offset;
		return NativePrimitiveDecoder.ReadUInt64(ref data);
	}

	public unsafe float ReadFloat(uint offset)
	{
		EnsureOffsetInRange(offset, 3u);
		byte* data = _base + offset;
		return NativePrimitiveDecoder.ReadFloat(ref data);
	}

	public unsafe double ReadDouble(uint offset)
	{
		EnsureOffsetInRange(offset, 7u);
		byte* data = _base + offset;
		return NativePrimitiveDecoder.ReadDouble(ref data);
	}

	public unsafe uint DecodeUnsigned(uint offset, out uint value)
	{
		EnsureOffsetInRange(offset, 0u);
		byte* data = _base + offset;
		value = NativePrimitiveDecoder.DecodeUnsigned(ref data, _base + _size);
		return (uint)(data - _base);
	}

	public unsafe uint DecodeSigned(uint offset, out int value)
	{
		EnsureOffsetInRange(offset, 0u);
		byte* data = _base + offset;
		value = NativePrimitiveDecoder.DecodeSigned(ref data, _base + _size);
		return (uint)(data - _base);
	}

	public unsafe uint DecodeUnsignedLong(uint offset, out ulong value)
	{
		EnsureOffsetInRange(offset, 0u);
		byte* data = _base + offset;
		value = NativePrimitiveDecoder.DecodeUnsignedLong(ref data, _base + _size);
		return (uint)(data - _base);
	}

	public unsafe uint DecodeSignedLong(uint offset, out long value)
	{
		EnsureOffsetInRange(offset, 0u);
		byte* data = _base + offset;
		value = NativePrimitiveDecoder.DecodeSignedLong(ref data, _base + _size);
		return (uint)(data - _base);
	}

	public unsafe uint SkipInteger(uint offset)
	{
		EnsureOffsetInRange(offset, 0u);
		byte* data = _base + offset;
		NativePrimitiveDecoder.SkipInteger(ref data);
		return (uint)(data - _base);
	}

	public string ReadString(uint offset)
	{
		DecodeString(offset, out var value);
		return value;
	}

	public unsafe uint DecodeString(uint offset, out string value)
	{
		offset = DecodeUnsigned(offset, out var numBytes);
		if (numBytes == 0)
		{
			value = string.Empty;
			return offset;
		}
		uint endOffset = offset + numBytes;
		if (endOffset < numBytes || endOffset > _size)
		{
			ThrowBadImageFormatException();
		}
		value = Encoding.UTF8.GetString(_base + offset, (int)numBytes);
		return endOffset;
	}

	public uint SkipString(uint offset)
	{
		offset = DecodeUnsigned(offset, out var numBytes);
		if (numBytes == 0)
		{
			return offset;
		}
		uint endOffset = offset + numBytes;
		if (endOffset < numBytes || endOffset > _size)
		{
			ThrowBadImageFormatException();
		}
		return endOffset;
	}

	public unsafe bool StringEquals(uint offset, string value)
	{
		uint originalOffset = offset;
		offset = DecodeUnsigned(offset, out var numBytes);
		if (offset + numBytes < numBytes || offset > _size)
		{
			ThrowBadImageFormatException();
		}
		if (numBytes < value.Length)
		{
			return false;
		}
		for (int i = 0; i < value.Length; i++)
		{
			int ch = (_base + offset)[i];
			if (ch > 127)
			{
				return ReadString(originalOffset) == value;
			}
			if (ch != value[i])
			{
				return false;
			}
		}
		return numBytes == value.Length;
	}
}
