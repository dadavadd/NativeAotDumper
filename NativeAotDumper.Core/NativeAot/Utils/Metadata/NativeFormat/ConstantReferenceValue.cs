using System;

namespace Internal.Metadata.NativeFormat;

/// <summary>
/// ConstantReferenceValue can only be used to encapsulate null reference values,
/// and therefore does not actually store the value.
/// </summary>
[CLSCompliant(false)]
public readonly struct ConstantReferenceValue
{
	private readonly MetadataReader _reader;

	private readonly ConstantReferenceValueHandle _handle;

	/// Always returns null value.
	public object Value => null;

	public ConstantReferenceValueHandle Handle => _handle;

	internal ConstantReferenceValue(MetadataReader reader, ConstantReferenceValueHandle handle)
	{
		_reader = reader;
		_handle = handle;
		_ = handle.Offset;
		_ = reader._streamReader;
	}
}
