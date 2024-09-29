
namespace Xyzu.Library
{
	public partial interface ILibrary 
	{
		IAlbums Albums { get; }
		IArtists Artists { get; }
		IGenres Genres { get; }
		IPlaylists Playlists { get; }
		ISongs Songs { get; }
		IMisc Misc { get; }

		ISettings? Settings { get; set; }

		public static string? IdFromAlbumTitle(string? albumtitle, string? albumartistname)
		{
			return albumtitle + albumartistname;
		}
		public static string? IdFromArtistName(string? artistname)
		{
			return artistname;
		}
		public static string? IdFromGenreName(string? genrename)
		{
			return genrename;
		}
		public static string? IdFromPlaylistTitle(string? playlisttitle)
		{
			return playlisttitle;
		}
	}
}