using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Internal.NativeFormat;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct NativePrimitiveDecoder
{
	public static void ThrowBadImageFormatException()
	{
		throw new BadImageFormatException();
	}

	public unsafe static uint DecodeUnsigned(ref byte* stream, byte* streamEnd)
	{
		if (stream >= streamEnd)
		{
			ThrowBadImageFormatException();
		}
		uint value = 0u;
		uint val = *stream;
		if ((val & 1) == 0)
		{
			value = val >> 1;
			stream++;
		}
		else if ((val & 2) == 0)
		{
			if (stream + 1 >= streamEnd)
			{
				ThrowBadImageFormatException();
			}
			value = (val >> 2) | (uint)(stream[1] << 6);
			stream += 2;
		}
		else if ((val & 4) == 0)
		{
			if (stream + 2 >= streamEnd)
			{
				ThrowBadImageFormatException();
			}
			value = (val >> 3) | (uint)(stream[1] << 5) | (uint)(stream[2] << 13);
			stream += 3;
		}
		else if ((val & 8) == 0)
		{
			if (stream + 3 >= streamEnd)
			{
				ThrowBadImageFormatException();
			}
			value = (val >> 4) | (uint)(stream[1] << 4) | (uint)(stream[2] << 12) | (uint)(stream[3] << 20);
			stream += 4;
		}
		else
		{
			if ((val & 0x10) != 0)
			{
				ThrowBadImageFormatException();
				return 0u;
			}
			stream++;
			value = ReadUInt32(ref stream);
		}
		return value;
	}

	public unsafe static int DecodeSigned(ref byte* stream, byte* streamEnd)
	{
		if (stream >= streamEnd)
		{
			ThrowBadImageFormatException();
		}
		int value = 0;
		int val = *stream;
		if ((val & 1) == 0)
		{
			value = (sbyte)val >> 1;
			stream++;
		}
		else if ((val & 2) == 0)
		{
			if (stream + 1 >= streamEnd)
			{
				ThrowBadImageFormatException();
			}
			value = (val >> 2) | (stream[1] << 6);
			stream += 2;
		}
		else if ((val & 4) == 0)
		{
			if (stream + 2 >= streamEnd)
			{
				ThrowBadImageFormatException();
			}
			value = (val >> 3) | (stream[1] << 5) | (stream[2] << 13);
			stream += 3;
		}
		else if ((val & 8) == 0)
		{
			if (stream + 3 >= streamEnd)
			{
				ThrowBadImageFormatException();
			}
			value = (val >> 4) | (stream[1] << 4) | (stream[2] << 12) | (stream[3] << 20);
			stream += 4;
		}
		else
		{
			if ((val & 0x10) != 0)
			{
				ThrowBadImageFormatException();
				return 0;
			}
			stream++;
			value = (int)ReadUInt32(ref stream);
		}
		return value;
	}

	public unsafe static ulong DecodeUnsignedLong(ref byte* stream, byte* streamEnd)
	{
		if (stream >= streamEnd)
		{
			ThrowBadImageFormatException();
		}
		ulong value = 0uL;
		byte val = *stream;
		if ((val & 0x1F) != 31)
		{
			return DecodeUnsigned(ref stream, streamEnd);
		}
		if ((val & 0x20) == 0)
		{
			stream++;
			return ReadUInt64(ref stream);
		}
		ThrowBadImageFormatException();
		return 0uL;
	}

	public unsafe static long DecodeSignedLong(ref byte* stream, byte* streamEnd)
	{
		if (stream >= streamEnd)
		{
			ThrowBadImageFormatException();
		}
		long value = 0L;
		byte val = *stream;
		if ((val & 0x1F) != 31)
		{
			return DecodeSigned(ref stream, streamEnd);
		}
		if ((val & 0x20) == 0)
		{
			stream++;
			return (long)ReadUInt64(ref stream);
		}
		ThrowBadImageFormatException();
		return 0L;
	}

	public unsafe static void SkipInteger(ref byte* stream)
	{
		byte val = *stream;
		if ((val & 1) == 0)
		{
			stream++;
		}
		else if ((val & 2) == 0)
		{
			stream += 2;
		}
		else if ((val & 4) == 0)
		{
			stream += 3;
		}
		else if ((val & 8) == 0)
		{
			stream += 4;
		}
		else if ((val & 0x10) == 0)
		{
			stream += 5;
		}
		else if ((val & 0x20) == 0)
		{
			stream += 9;
		}
		else
		{
			ThrowBadImageFormatException();
		}
	}

	public unsafe static byte ReadUInt8(ref byte* stream)
	{
		byte result = *stream;
		stream++;
		return result;
	}

	public unsafe static ushort ReadUInt16(ref byte* stream)
	{
		ushort result = Unsafe.ReadUnaligned<ushort>(stream);
		stream += 2;
		return result;
	}

	public unsafe static uint ReadUInt32(ref byte* stream)
	{
		uint result = Unsafe.ReadUnaligned<uint>(stream);
		stream += 4;
		return result;
	}

	public unsafe static ulong ReadUInt64(ref byte* stream)
	{
		ulong result = Unsafe.ReadUnaligned<ulong>(stream);
		stream += 8;
		return result;
	}

	public unsafe static float ReadFloat(ref byte* stream)
	{
		uint value = ReadUInt32(ref stream);
		return *(float*)(&value);
	}

	public unsafe static double ReadDouble(ref byte* stream)
	{
		ulong value = ReadUInt64(ref stream);
		return *(double*)(&value);
	}

	public static uint GetUnsignedEncodingSize(uint value)
	{
		if (value < 128)
		{
			return 1u;
		}
		if (value < 16384)
		{
			return 2u;
		}
		if (value < 2097152)
		{
			return 3u;
		}
		if (value < 268435456)
		{
			return 4u;
		}
		return 5u;
	}

	public unsafe static uint DecodeUnsigned(ref byte* stream)
	{
		uint value = 0u;
		uint val = *stream;
		if ((val & 1) == 0)
		{
			value = val >> 1;
			stream++;
		}
		else if ((val & 2) == 0)
		{
			value = (val >> 2) | (uint)(stream[1] << 6);
			stream += 2;
		}
		else if ((val & 4) == 0)
		{
			value = (val >> 3) | (uint)(stream[1] << 5) | (uint)(stream[2] << 13);
			stream += 3;
		}
		else if ((val & 8) == 0)
		{
			value = (val >> 4) | (uint)(stream[1] << 4) | (uint)(stream[2] << 12) | (uint)(stream[3] << 20);
			stream += 4;
		}
		else
		{
			if ((val & 0x10) != 0)
			{
				return 0u;
			}
			stream++;
			value = ReadUInt32(ref stream);
		}
		return value;
	}

	public unsafe static int DecodeSigned(ref byte* stream)
	{
		int value = 0;
		int val = *stream;
		if ((val & 1) == 0)
		{
			value = (sbyte)val >> 1;
			stream++;
		}
		else if ((val & 2) == 0)
		{
			value = (val >> 2) | (stream[1] << 6);
			stream += 2;
		}
		else if ((val & 4) == 0)
		{
			value = (val >> 3) | (stream[1] << 5) | (stream[2] << 13);
			stream += 3;
		}
		else if ((val & 8) == 0)
		{
			value = (val >> 4) | (stream[1] << 4) | (stream[2] << 12) | (stream[3] << 20);
			stream += 4;
		}
		else
		{
			if ((val & 0x10) != 0)
			{
				return 0;
			}
			stream++;
			value = (int)ReadUInt32(ref stream);
		}
		return value;
	}

	public unsafe static ulong DecodeUnsignedLong(ref byte* stream)
	{
		ulong value = 0uL;
		byte val = *stream;
		if ((val & 0x1F) != 31)
		{
			return DecodeUnsigned(ref stream);
		}
		if ((val & 0x20) == 0)
		{
			stream++;
			return ReadUInt64(ref stream);
		}
		return 0uL;
	}

	public unsafe static long DecodeSignedLong(ref byte* stream)
	{
		long value = 0L;
		byte val = *stream;
		if ((val & 0x1F) != 31)
		{
			return DecodeSigned(ref stream);
		}
		if ((val & 0x20) == 0)
		{
			stream++;
			return (long)ReadUInt64(ref stream);
		}
		return 0L;
	}
}
