using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public enum AssemblyHashAlgorithm : uint
{
	None = 0u,
	Reserved = 32771u,
	SHA1 = 32772u
}
