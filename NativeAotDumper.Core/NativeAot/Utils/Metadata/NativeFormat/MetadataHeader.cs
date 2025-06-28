using Internal.NativeFormat;

namespace Internal.Metadata.NativeFormat;

internal sealed class MetadataHeader
{
	/// <todo>
	/// Signature should be updated every time the metadata schema changes.
	/// </todo>
	public const uint Signature = 3735937021u;

	/// <summary>
	/// The set of ScopeDefinitions contained within this metadata resource.
	/// </summary>
	public ScopeDefinitionHandleCollection ScopeDefinitions;

	public void Decode(NativeReader reader)
	{
		if (reader.ReadUInt32(0u) != 3735937021u)
		{
			NativeReader.ThrowBadImageFormatException();
		}
		reader.Read(4u, out ScopeDefinitions);
	}
}
