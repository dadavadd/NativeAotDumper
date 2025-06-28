using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public enum NamedArgumentMemberKind : byte
{
	/// Specifies the name of a property
	Property,
	/// Specifies the name of a field
	Field
}
