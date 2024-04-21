#nullable enable

using Android.Content;

using SQLite;

using System;
using System.Linq;
using System.Threading.Tasks;

using JavaFile = Java.IO.File;
using Exception = System.Exception;

namespace Xyzu
{
	public sealed partial class XyzuLibrary 
	{
		private XyzuLibrary(Context context)
		{
			Context = context;

			SqliteConnection = new SQLiteConnection(_Path);
			SqliteConnectionAsync = new SQLiteAsyncConnection(_Path);

			SqliteConnection.CreateTable<AlbumEntity>();
			SqliteConnection.CreateTable<ArtistEntity>();
			SqliteConnection.CreateTable<GenreEntity>();
			SqliteConnection.CreateTable<PlaylistEntity>();
			SqliteConnection.CreateTable<SongEntity>();
		}

		private static XyzuLibrary? _Instance;
		public static XyzuLibrary Instance
		{
			get => _Instance ?? throw new Exception("Instane is null. Init AppLibrary before use");
		}

		public static void Init(Context context, Action<XyzuLibrary>? action = null) 
		{
			_Instance = new XyzuLibrary(context) { };

			action?.Invoke(_Instance);
		}

		public Context Context { get; set; }

		private bool SettingsDirectoriesSelector(JavaFile javafile) 
		{
			bool isapplicable = false;

			if (Settings?.Directories != null)
			{
				foreach (string directory in Settings.Directories)
					if (isapplicable is false)
						isapplicable =
							javafile.AbsolutePath.Contains(string.Format("/{0}/", directory), StringComparison.OrdinalIgnoreCase) ||
							javafile.AbsolutePath.EndsWith(string.Format("/{0}", directory), StringComparison.OrdinalIgnoreCase);

				if (isapplicable is false)
					return isapplicable;
			}

			return isapplicable;
		}
		private bool SettingsMimetypesSelector(JavaFile javafile) 
		{
			bool isapplicable = false;

			if (Settings?.Mimetypes != null)
			{
				isapplicable = false;

				foreach (string mimetype in Settings.Mimetypes.Select(_mimetype => _mimetype.ToString()))
					if (isapplicable is false)
						isapplicable =
							javafile.AbsolutePath.EndsWith(string.Format(".{0}", mimetype), StringComparison.OrdinalIgnoreCase);
			}

			return isapplicable;
		}
	}
}