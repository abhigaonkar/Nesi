using System;
using System.IO;
using System.Text;

namespace NESI.Common.Extensions
{
	public static class StreamExtensions
	{
		public static readonly Encoding DefaultEncoding = (Encoding)new UTF8Encoding(false, true);

		public static BinaryReader CreateReader(this Stream stream)
		{
			return new BinaryReader(stream, StreamExtensions.DefaultEncoding, true);
		}

		public static BinaryWriter CreateWriter(this Stream stream)
		{
			return new BinaryWriter(stream, StreamExtensions.DefaultEncoding, true);
		}

		public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader)
		{
			return new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
		}

		public static void Write(this BinaryWriter writer, DateTimeOffset value)
		{
			writer.Write(value.UtcTicks);
		}
	}
}