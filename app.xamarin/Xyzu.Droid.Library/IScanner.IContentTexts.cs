using Android.Content;
using Android.Content.Res;
using Android.OS;

namespace Xyzu.Library
{
	public partial interface IScanner
	{
		public interface IContentTexts
		{
			string? Title { get; set; }
			string? TextAlbums { get; set; }
			string? TextArtists { get; set; }
			string? TextGenres { get; set; }
			string? TextPlaylists { get; set; }
			string? TextSongs { get; set; }

			public class Default : IContentTexts
			{
				public string? Title { get; set; }
				public string? TextAlbums { get; set; }
				public string? TextArtists { get; set; }
				public string? TextGenres { get; set; }
				public string? TextPlaylists { get; set; }
				public string? TextSongs { get; set; }
			}
		}
	}
}