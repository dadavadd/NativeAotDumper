namespace Internal.NativeFormat;

internal struct NativeHashtable
{
	public struct Enumerator
	{
		private NativeParser _parser;

		private uint _endOffset;

		private byte _lowHashcode;

		internal Enumerator(NativeParser parser, uint endOffset, byte lowHashcode)
		{
			_parser = parser;
			_endOffset = endOffset;
			_lowHashcode = lowHashcode;
		}

		public NativeParser GetNext()
		{
			while (_parser.Offset < _endOffset)
			{
				byte lowHashcode = _parser.GetUInt8();
				if (lowHashcode == _lowHashcode)
				{
					return _parser.GetParserFromRelativeOffset();
				}
				if (lowHashcode > _lowHashcode)
				{
					_endOffset = _parser.Offset;
					break;
				}
				_parser.SkipInteger();
			}
			return default(NativeParser);
		}
	}

	public struct AllEntriesEnumerator
	{
		private NativeHashtable _table;

		private NativeParser _parser;

		private uint _currentBucket;

		private uint _endOffset;

		internal AllEntriesEnumerator(NativeHashtable table)
		{
			_table = table;
			_currentBucket = 0u;
			_parser = _table.GetParserForBucket(_currentBucket, out _endOffset);
		}

		public NativeParser GetNext()
		{
			while (true)
			{
				if (_parser.Offset < _endOffset)
				{
					_parser.GetUInt8();
					return _parser.GetParserFromRelativeOffset();
				}
				if (_currentBucket >= _table._bucketMask)
				{
					break;
				}
				_currentBucket++;
				_parser = _table.GetParserForBucket(_currentBucket, out _endOffset);
			}
			return default(NativeParser);
		}
	}

	private NativeReader _reader;

	private uint _baseOffset;

	private uint _bucketMask;

	private byte _entryIndexSize;

	public bool IsNull => _reader == null;

	public NativeHashtable(NativeParser parser)
	{
		byte uInt = parser.GetUInt8();
		_reader = parser.Reader;
		_baseOffset = parser.Offset;
		int numberOfBucketsShift = uInt >>> 2;
		if (numberOfBucketsShift > 31)
		{
			NativeReader.ThrowBadImageFormatException();
		}
		_bucketMask = (uint)((1 << numberOfBucketsShift) - 1);
		byte entryIndexSize = (byte)(uInt & 3);
		if (entryIndexSize > 2)
		{
			NativeReader.ThrowBadImageFormatException();
		}
		_entryIndexSize = entryIndexSize;
	}

	private NativeParser GetParserForBucket(uint bucket, out uint endOffset)
	{
		uint start;
		uint end;
		if (_entryIndexSize == 0)
		{
			uint bucketOffset = _baseOffset + bucket;
			start = _reader.ReadUInt8(bucketOffset);
			end = _reader.ReadUInt8(bucketOffset + 1);
		}
		else if (_entryIndexSize == 1)
		{
			uint bucketOffset2 = _baseOffset + 2 * bucket;
			start = _reader.ReadUInt16(bucketOffset2);
			end = _reader.ReadUInt16(bucketOffset2 + 2);
		}
		else
		{
			uint bucketOffset3 = _baseOffset + 4 * bucket;
			start = _reader.ReadUInt32(bucketOffset3);
			end = _reader.ReadUInt32(bucketOffset3 + 4);
		}
		endOffset = end + _baseOffset;
		return new NativeParser(_reader, _baseOffset + start);
	}

	public Enumerator Lookup(int hashcode)
	{
		uint bucket = (uint)(hashcode >>> 8) & _bucketMask;
		uint endOffset;
		return new Enumerator(GetParserForBucket(bucket, out endOffset), endOffset, (byte)hashcode);
	}

	public AllEntriesEnumerator EnumerateAllEntries()
	{
		return new AllEntriesEnumerator(this);
	}
}
