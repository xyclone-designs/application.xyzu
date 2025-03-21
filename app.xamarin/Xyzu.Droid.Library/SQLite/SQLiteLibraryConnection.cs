﻿using Xyzu.Library.Models;

namespace SQLite
{
	public class SQLiteLibraryConnection
	{
		public SQLiteLibraryConnection(string path)
		{
			Path = path;

			Connection ??= new SQLiteConnection(Path);
			ConnectionAsync ??= new SQLiteAsyncConnection(Path);

			Connection.CreateTable<AlbumEntity>();
			Connection.CreateTable<ArtistEntity>();
			Connection.CreateTable<GenreEntity>();
			Connection.CreateTable<PlaylistEntity>();
			Connection.CreateTable<SongEntity>();
		}

		public string Path { get; }
		public SQLiteConnection Connection { get; }
		public SQLiteAsyncConnection ConnectionAsync { get; }

		public TableQuery<AlbumEntity> AlbumsTable
		{
			get => Connection.Table<AlbumEntity>();
		}
		public TableQuery<ArtistEntity> ArtistsTable
		{
			get => Connection.Table<ArtistEntity>();
		}
		public TableQuery<GenreEntity> GenresTable
		{
			get => Connection.Table<GenreEntity>();
		}
		public TableQuery<PlaylistEntity> PlaylistsTable
		{
			get => Connection.Table<PlaylistEntity>();
		}
		public TableQuery<SongEntity> SongsTable
		{
			get => Connection.Table<SongEntity>();
		}

		public AsyncTableQuery<AlbumEntity> AlbumsTableAsync
		{
			get => ConnectionAsync.Table<AlbumEntity>();
		}
		public AsyncTableQuery<ArtistEntity> ArtistsTableAsync
		{
			get => ConnectionAsync.Table<ArtistEntity>();
		}
		public AsyncTableQuery<GenreEntity> GenresTableAsync
		{
			get => ConnectionAsync.Table<GenreEntity>();
		}
		public AsyncTableQuery<PlaylistEntity> PlaylistsTableAsync
		{
			get => ConnectionAsync.Table<PlaylistEntity>();
		}
		public AsyncTableQuery<SongEntity> SongsTableAsync
		{
			get => ConnectionAsync.Table<SongEntity>();
		}
	}
}