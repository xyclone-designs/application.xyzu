
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
	}
}