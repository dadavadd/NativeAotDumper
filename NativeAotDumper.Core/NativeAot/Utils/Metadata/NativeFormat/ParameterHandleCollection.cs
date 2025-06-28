using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ParameterHandleCollection
{
	[CLSCompliant(false)]
	public struct Enumerator
	{
		private readonly NativeReader _reader;

		private uint _offset;

		private uint _remaining;

		private ParameterHandle _current;

		public ParameterHandle Current => _current;

		internal Enumerator(NativeReader reader, uint offset)
		{
			_reader = reader;
			_offset = reader.DecodeUnsigned(offset, out _remaining);
			_current = default(ParameterHandle);
		}

		public bool MoveNext()
		{
			if (_remaining == 0)
			{
				return false;
			}
			_remaining--;
			_offset = _reader.Read(_offset, out _current);
			return true;
		}

		public void Dispose()
		{
		}
	}

	private readonly NativeReader _reader;

	private readonly uint _offset;

	public int Count
	{
		get
		{
			_reader.DecodeUnsigned(_offset, out var count);
			return (int)count;
		}
	}

	internal ParameterHandleCollection(NativeReader reader, uint offset)
	{
		_offset = offset;
		_reader = reader;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(_reader, _offset);
	}
}
