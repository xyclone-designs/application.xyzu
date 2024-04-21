#nullable enable

using SQLite;

using System;
using System.Collections.Generic;
using System.IO;

using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu
{
	public sealed partial class XyzuLibrary
	{
		private readonly string _Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Xyzu.db3");

		public SQLiteConnection SqliteConnection { get; private set; }
		public SQLiteAsyncConnection SqliteConnectionAsync { get; private set; }

		public TableQuery<AlbumEntity> SqliteAlbumsTableQuery
		{
			get => SqliteConnection.Table<AlbumEntity>();
		}												 
		public TableQuery<ArtistEntity> SqliteArtistsTableQuery
		{
			get => SqliteConnection.Table<ArtistEntity>();
		}												 
		public TableQuery<GenreEntity> SqliteGenresTableQuery
		{
			get => SqliteConnection.Table<GenreEntity>();
		}												 
		public TableQuery<PlaylistEntity> SqlitePlaylistsTableQuery
		{
			get => SqliteConnection.Table<PlaylistEntity>();
		}												 
		public TableQuery<SongEntity> SqliteSongsTableQuery
		{
			get => SqliteConnection.Table<SongEntity>();
		}			 

		public AsyncTableQuery<AlbumEntity> SqliteAlbumsTableQueryAsync
		{
			get => SqliteConnectionAsync.Table<AlbumEntity>();
		}												 
		public AsyncTableQuery<ArtistEntity> SqliteArtistsTableQueryAsync
		{
			get => SqliteConnectionAsync.Table<ArtistEntity>();
		}												 
		public AsyncTableQuery<GenreEntity> SqliteGenresTableQueryAsync
		{
			get => SqliteConnectionAsync.Table<GenreEntity>();
		}												 
		public AsyncTableQuery<PlaylistEntity> SqlitePlaylistsTableQueryAsync
		{
			get => SqliteConnectionAsync.Table<PlaylistEntity>();
		}												 
		public AsyncTableQuery<SongEntity> SqliteSongsTableQueryAsync
		{
			get => SqliteConnectionAsync.Table<SongEntity>();
		}

		public class AlbumEntity : IAlbum.Default
		{
			public AlbumEntity() : this(Guid.NewGuid().ToString()) { }
			public AlbumEntity(string id) : base(id) { }
			public AlbumEntity(IAlbum album) : base(album) { }

			[PrimaryKey]
			public new string Id
			{
				get => base.Id;
				set => base.Id = value;
			}

			public Uri? ArtworkUri
			{
				get => base.Artwork?.Uri;
				set => (base.Artwork ??= new IImage.Default()).Uri = value;
			}
			public byte[]? ArtworkBufferHash
			{
				get => base.Artwork?.BufferHash;
				set => (base.Artwork ??= new IImage.Default()).BufferHash = value;
			}
			
			public new string SongIds
			{
				get => EntityUtils.IdsToString(base.SongIds);
				set => base.SongIds = EntityUtils.StringToIds(value);
			}

			[Ignore]
			public new IImage? Artwork
			{
				get => base.Artwork;
				set => base.Artwork = value;
			}
		}
		public class ArtistEntity : IArtist.Default
		{
			public ArtistEntity() : this(Guid.NewGuid().ToString()) { }
			public ArtistEntity(string id) : base(id) { }
			public ArtistEntity(IArtist artist) : base(artist) { }

			[PrimaryKey]
			public new string Id
			{
				get => base.Id;
				set => base.Id = value;
			}

			[Ignore]
			public new IImage? Image
			{
				get => base.Image;
				set => base.Image = value;
			}
			public Uri? ImageUri
			{
				get => base.Image?.Uri;
				set => (base.Image ??= new IImage.Default()).Uri = value;
			}
			public byte[]? ImageBufferHash
			{
				get => base.Image?.BufferHash;
				set => (base.Image ??= new IImage.Default()).BufferHash = value;
			}

			public new string AlbumIds
			{
				get => EntityUtils.IdsToString(base.AlbumIds);
				set => base.AlbumIds = EntityUtils.StringToIds(value);
			}
			public new string SongIds
			{
				get => EntityUtils.IdsToString(base.SongIds);
				set => base.SongIds = EntityUtils.StringToIds(value);
			}
		}
		public class GenreEntity : IGenre.Default
		{
			public GenreEntity() : this(Guid.NewGuid().ToString()) { }
			public GenreEntity(string id) : base(id) { }
			public GenreEntity(IGenre genre) : base(genre) { }

			[PrimaryKey]
			public new string Id
			{
				get => base.Id;
				set => base.Id = value;
			}

			public new string SongIds
			{
				get => EntityUtils.IdsToString(base.SongIds);
				set => base.SongIds = EntityUtils.StringToIds(value);
			}
		}
		public class PlaylistEntity : IPlaylist.Default
		{
			public PlaylistEntity() : this(Guid.NewGuid().ToString()) { }
			public PlaylistEntity(string id) : base(id) { }
			public PlaylistEntity(IPlaylist playlist) : base(playlist) { }

			[PrimaryKey]
			public new string Id
			{
				get => base.Id;
				set => base.Id = value;
			}

			public new string SongIds
			{
				get => EntityUtils.IdsToString(base.SongIds);
				set => base.SongIds = EntityUtils.StringToIds(value);
			}
		}
		public class SongEntity : ISong.Default
		{
			public SongEntity() : this(Guid.NewGuid().ToString()) { }
			public SongEntity(string id) : base(id) { }
			public SongEntity(ISong song) : base(song) { }

			[PrimaryKey]
			public new string Id
			{
				get => base.Id;
				set => base.Id = value;
			}

			public Uri? ArtworkUri
			{
				get => base.Artwork?.Uri;
				set => (base.Artwork ??= new IImage.Default()).Uri = value;
			}
			public byte[]? ArtworkBufferHash
			{
				get => base.Artwork?.BufferHash;
				set => (base.Artwork ??= new IImage.Default()).BufferHash = value;
			}

			[Ignore]
			public new IImage? Artwork
			{
				get => base.Artwork;
				set => base.Artwork = value;
			}
		}

		public class EntityUtils
		{
			private const string Delim = ";";

			public static string IdsToString(IEnumerable<string> ids)
			{
				return string.Join(Delim, ids);
			}
			public static IEnumerable<string> StringToIds(string ids)
			{
				return ids.Split(Delim);
			}
		}
	}
}