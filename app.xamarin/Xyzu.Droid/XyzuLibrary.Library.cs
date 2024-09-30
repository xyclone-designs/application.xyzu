#nullable enable

using Android.Database;
using SQLite;
using Xyzu.Library;

namespace Xyzu
{
	public sealed partial class XyzuLibrary 
	{
		public ILibrary.IAlbums Albums => Library.Albums;
		public ILibrary.IArtists Artists => Library.Artists;
		public ILibrary.IGenres Genres => Library.Genres;
		public ILibrary.IPlaylists Playlists => Library.Playlists;
		public ILibrary.ISongs Songs => Library.Songs;
		public ILibrary.IMisc Misc => Library.Misc;

		public ILibrary.ISettings? Settings 
		{ 
			get => Library.Settings; 
			set => Library.Settings = value; 
		}
		public ILibrary.IActions.Container? Actions 
		{ 
			get => Library.Actions; 
			set => Library.Actions = value; 
		}

		public CursorBuilder DefaultCursorBuilder => Library.DefaultCursorBuilder;

		public SQLiteLibraryConnection SQLiteLibrary { get => Library.SQLiteLibrary; set => Library.SQLiteLibrary = value; }
	}
}