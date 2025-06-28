using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public enum GenericParameterKind : byte
{
	/// Represents a type parameter for a generic type.
	GenericTypeParameter,
	/// Represents a type parameter from a generic method.
	GenericMethodParameter
}
