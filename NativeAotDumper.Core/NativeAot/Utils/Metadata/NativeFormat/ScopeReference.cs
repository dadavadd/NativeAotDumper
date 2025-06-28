using System;
using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public readonly struct ScopeReference
{
	private readonly MetadataReader _reader;

	private readonly ScopeReferenceHandle _handle;

	private readonly AssemblyFlags _flags;

	private readonly ConstantStringValueHandle _name;

	private readonly ushort _majorVersion;

	private readonly ushort _minorVersion;

	private readonly ushort _buildNumber;

	private readonly ushort _revisionNumber;

	private readonly ByteCollection _publicKeyOrToken;

	private readonly ConstantStringValueHandle _culture;

	public ScopeReferenceHandle Handle => _handle;

	public AssemblyFlags Flags => _flags;

	public ConstantStringValueHandle Name => _name;

	public ushort MajorVersion => _majorVersion;

	public ushort MinorVersion => _minorVersion;

	public ushort BuildNumber => _buildNumber;

	public ushort RevisionNumber => _revisionNumber;

	public ByteCollection PublicKeyOrToken => _publicKeyOrToken;

	public ConstantStringValueHandle Culture => _culture;

	internal ScopeReference(MetadataReader reader, ScopeReferenceHandle handle)
	{
		_reader = reader;
		_handle = handle;
		uint offset = (uint)handle.Offset;
		NativeReader streamReader = reader._streamReader;
		offset = streamReader.Read(offset, out _flags);
		offset = streamReader.Read(offset, out _name);
		offset = streamReader.Read(offset, out _majorVersion);
		offset = streamReader.Read(offset, out _minorVersion);
		offset = streamReader.Read(offset, out _buildNumber);
		offset = streamReader.Read(offset, out _revisionNumber);
		offset = streamReader.Read(offset, out _publicKeyOrToken);
		offset = streamReader.Read(offset, out _culture);
	}
}
