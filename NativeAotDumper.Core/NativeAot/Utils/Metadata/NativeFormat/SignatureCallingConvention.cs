using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public enum SignatureCallingConvention : byte
{
	HasThis = 32,
	ExplicitThis = 64,
	Default = 0,
	Vararg = 5,
	Cdecl = 1,
	StdCall = 2,
	ThisCall = 3,
	FastCall = 4,
	Unmanaged = 9,
	UnmanagedCallingConventionMask = 15
}
