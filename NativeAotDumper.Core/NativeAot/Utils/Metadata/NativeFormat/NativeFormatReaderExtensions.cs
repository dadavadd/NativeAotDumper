using System;

namespace Internal.Metadata.NativeFormat;

[CLSCompliant(false)]
public static class NativeFormatReaderExtensions
{
	public static string GetString(this MetadataReader reader, ConstantStringValueHandle handle)
	{
		return reader.GetConstantStringValue(handle).Value;
	}
}
