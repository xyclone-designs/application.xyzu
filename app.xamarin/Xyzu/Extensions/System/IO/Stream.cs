#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	public static class StreamExtensions
	{
		public static byte[] ToBytes(this Stream stream)
		{
			using MemoryStream memorystream = new ();

			stream.CopyTo(memorystream);

			byte[] buffer = memorystream.ToArray();

			return buffer;
		}			  
		public static async Task<byte[]> ToBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
		{
			using MemoryStream memorystream = new();

			await stream.CopyToAsync(memorystream, cancellationToken);

			byte[] buffer = memorystream.ToArray();

			return buffer;
		}
	}
}