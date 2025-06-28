namespace Internal.NativeFormat;

internal struct NativeParser
{
	private readonly NativeReader _reader;

	private uint _offset;

	public bool IsNull => _reader == null;

	public NativeReader Reader => _reader;

	public uint Offset
	{
		get
		{
			return _offset;
		}
		set
		{
			_offset = value;
		}
	}

	public NativeParser(NativeReader reader, uint offset)
	{
		_reader = reader;
		_offset = offset;
	}

	public static void ThrowBadImageFormatException()
	{
		NativeReader.ThrowBadImageFormatException();
	}

	public byte GetUInt8()
	{
		byte result = _reader.ReadUInt8(_offset);
		_offset++;
		return result;
	}

	public uint GetUnsigned()
	{
		_offset = _reader.DecodeUnsigned(_offset, out var value);
		return value;
	}

	public ulong GetUnsignedLong()
	{
		_offset = _reader.DecodeUnsignedLong(_offset, out var value);
		return value;
	}

	public int GetSigned()
	{
		_offset = _reader.DecodeSigned(_offset, out var value);
		return value;
	}

	public uint GetRelativeOffset()
	{
		uint offset = _offset;
		_offset = _reader.DecodeSigned(_offset, out var delta);
		return offset + (uint)delta;
	}

	public void SkipInteger()
	{
		_offset = _reader.SkipInteger(_offset);
	}

	public NativeParser GetParserFromRelativeOffset()
	{
		return new NativeParser(_reader, GetRelativeOffset());
	}

	public uint GetSequenceCount()
	{
		return GetUnsigned();
	}

	public string GetString()
	{
		_offset = _reader.DecodeString(_offset, out var value);
		return value;
	}

	public void SkipString()
	{
		_offset = _reader.SkipString(_offset);
	}
}
