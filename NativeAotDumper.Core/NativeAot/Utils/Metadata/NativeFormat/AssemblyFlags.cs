using System;

namespace Internal.Metadata.NativeFormat;

[Flags]
[CLSCompliant(false)]
public enum AssemblyFlags : uint
{
	/// The assembly reference holds the full (unhashed) public key.
	PublicKey = 1u,
	/// The implementation of this assembly used at runtime is not expected to match the version seen at compile time.
	Retargetable = 0x100u,
	/// Content type mask. Masked bits correspond to values of System.Reflection.AssemblyContentType
	ContentTypeMask = 0xE00u
}
