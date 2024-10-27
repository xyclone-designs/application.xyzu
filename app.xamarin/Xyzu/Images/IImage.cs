using System;
using System.Linq;
using System.Security.Cryptography;
using Xyzu.Library.Models;

namespace Xyzu.Images
{
	public interface IImage<T>
	{
		T Id { get; set; }
		T Buffer { get; set; }
		T BufferKey { get; set; }
		T Filepath { get; set; }
		T IsCorrupt { get; set; }
		T Uri { get; set; }
	}
	public interface IImage
	{
		string? Id { get; set; }
		byte[]? Buffer { get; set; }
		string? BufferKey { get; set; }
		string? Filepath { get; set; }
		bool IsCorrupt { get; set; }
		Uri? Uri { get; set; }

		public static IImage FromAlbum(IAlbum album)
		{
			return new IImage.Default
			{
				Id = album.Id,
			};
		}
		public static IImage FromArtist(IArtist artist)
		{
			return new IImage.Default
			{
				Id = artist.Id,
			};
		}
		public static IImage FromSong(ISong song)
		{
			return new IImage.Default
			{
				Id = song.Id,
				Uri = song.Uri,
				Filepath = song.Filepath,
				IsCorrupt = song.IsCorrupt,
			};
		}

		public class Default : IImage
		{
			public string? Id { get; set; }
			public byte[]? Buffer { get; set; }
			public string? BufferKey { get; set; }
			public string? Filepath { get; set; }
			public bool IsCorrupt { get; set; }
			public Uri? Uri { get; set; }
		}
		public class Default<T> : IImage<T>
		{
			public Default(T defaultvalue)
			{
				Id = defaultvalue;
				Buffer = defaultvalue;
				BufferKey = defaultvalue;
				Uri = defaultvalue;
				Filepath = defaultvalue;
				IsCorrupt = defaultvalue;
			}

			public T Id { get; set; }
			public T Buffer { get; set; }
			public T BufferKey { get; set; }
			public T Uri { get; set; }
			public T IsCorrupt { get; set; }
			public T Filepath { get; set; }
		}

		public static class Utils
		{
			public static byte[] BufferToHash(byte[] buffer)
			{
				//return SHA1.HashData(buffer);
				//return SHA256.HashData(buffer);
				return MD5.HashData(buffer);
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
