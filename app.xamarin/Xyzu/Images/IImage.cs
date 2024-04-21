using System;
using System.Linq;
using System.Security.Cryptography;

namespace Xyzu.Images
{
	public interface IImage<T>
	{
		T Id { get; set; }
		T Buffer { get; set; }
		T BufferHash { get; set; }
		T Uri { get; set; }
	}
	public interface IImage
	{
		string? Id { get; set; }
		byte[]? Buffer { get; set; }
		byte[]? BufferHash { get; set; }
		Uri? Uri { get; set; }

		public class Default : IImage
		{
			public string? Id { get; set; }
			public byte[]? Buffer { get; set; }
			public byte[]? BufferHash { get; set; }
			public Uri? Uri { get; set; }
		}
		public class Default<T> : IImage<T>
		{
			public Default(T defaultvalue)
			{
				Id = defaultvalue;
				Buffer = defaultvalue;
				BufferHash = defaultvalue;
				Uri = defaultvalue;
			}

			public T Id { get; set; }
			public T Buffer { get; set; }
			public T BufferHash { get; set; }
			public T Uri { get; set; }
		}
		public static class Utils
		{
			private static MD5? _MD5;

			public static byte[] BufferToHash(byte[] buffer)
			{
				byte[] hash = (_MD5 ??= MD5.Create())
					.ComputeHash(buffer);

				return hash;
			}
			public static string BufferToHashString(byte[] buffer)
			{
				return HashToString(BufferToHash(buffer));
			}
			public static string HashToString(byte[] hash)
			{
				return string.Join(".", hash);

			}
			public static byte[] StringToHash(string str)
			{
				return str
					.Split('.')
					.Select(chr => byte.TryParse(chr, out byte outbyte) ? outbyte : new byte?())
					.OfType<byte>()
					.ToArray();
			}
		}
	}
}
