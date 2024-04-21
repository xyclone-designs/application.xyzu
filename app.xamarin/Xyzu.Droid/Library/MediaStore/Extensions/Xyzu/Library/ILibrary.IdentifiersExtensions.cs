#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

namespace Xyzu.Library
{
	public static class IdentifiersExtensions
	{
		public static void ToSelectionAndArgs(this ILibrary.IIdentifiers? identifiers, bool exclusive, ILibrary.ISettings? settings, out string selection, out string[] selectionargs)
		{
			string selectionvaluesseperator = " OR ";
			string selectionvalueslikeelementformat = "{0} like ?";
			string selectionvaluesnotlikeelementformat = "{0} not like ?";
			string selectionargsformat = "%{0}%";

			StringBuilder selectionstringbuilder = new StringBuilder();
			IEnumerable<string> selectionargsenumerable = Enumerable.Empty<string>();

			selectionstringbuilder.AppendFormat("{0} = ?", MediaStoreUtils.ColumnNames.IsMusic);
			selectionargsenumerable = selectionargsenumerable.Append("1");
			if (settings?.Directories?.Any() ?? false)
			{
				selectionstringbuilder.AppendFormat(" AND ({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: settings.Directories.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.RelativePath))));
				selectionargsenumerable = selectionargsenumerable.Concat(settings.Directories.Select(directory => string.Format(selectionargsformat, directory)));
			}  
			if (settings?.Mimetypes?.Any() ?? false)
			{
				selectionstringbuilder.AppendFormat(" AND ({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: settings.Mimetypes.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.DisplayName))));
				selectionargsenumerable = selectionargsenumerable.Concat(settings.Mimetypes.Select(mimetype => string.Format("%.{0}", mimetype.ToString())));
			}

			IEnumerable<string> selectionenumerable = Enumerable.Empty<string>();

			if (identifiers?.AlbumArtistNames?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.AlbumArtistNames.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.AlbumArtist)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.AlbumArtistNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.AlbumIds?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.AlbumIds.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.AlbumId)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.AlbumIds);
			}
			if (identifiers?.AlbumTitles?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.AlbumTitles.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.Album)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.AlbumTitles.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.ArtistIds?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.ArtistIds.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.ArtistId)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.ArtistIds);
			}
			if (identifiers?.ArtistNames?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.ArtistNames.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.Artist)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.ArtistNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.GenreIds?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.GenreIds.Count(),
				//	element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.GenreId)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.GenreIds);
			}
			if (identifiers?.GenreNames?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.GenreNames.Count(),
				//	element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.Genre)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.GenreNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.PlaylistIds?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.PlaylistIds.Count(),
				//	element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.PlaylistId)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.PlaylistIds);
			}
			if (identifiers?.PlaylistNames?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.PlaylistNames.Count(),
				//	element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.Playlist)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.PlaylistNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.SongIds.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.SongIds.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.Id)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.SongIds);
			}
			if (identifiers?.SongTitles?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.SongTitles.Count(),
					element: string.Format(selectionvalueslikeelementformat, MediaStoreUtils.ColumnNames.Title)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.SongTitles.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.WithoutAlbumArtistNames?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutAlbumArtistNames.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.AlbumArtist)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutAlbumArtistNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.WithoutAlbumIds?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutAlbumIds.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.AlbumId)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutAlbumIds);
			}
			if (identifiers?.WithoutAlbumTitles?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutAlbumTitles.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.Album)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutAlbumTitles.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.WithoutArtistIds?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutArtistIds.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.ArtistId)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutArtistIds);
			}
			if (identifiers?.WithoutArtistNames?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutArtistNames.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.Artist)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutArtistNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.WithoutGenreIds?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.WithoutGenreIds.Count(),
				//	element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.GenreId)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutGenreIds);
			}
			if (identifiers?.WithoutGenreNames?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.WithoutGenreNames.Count(),
				//	element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.Genre)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutGenreNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.WithoutPlaylistIds?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.WithoutPlaylistIds.Count(),
				//	element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.PlaylistId)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutPlaylistIds);
			}
			if (identifiers?.WithoutPlaylistNames?.Any() ?? false)
			{
				//selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
				//	count: identifiers.WithoutPlaylistNames.Count(),
				//	element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.Playlist)))));
				//selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutPlaylistNames.Select(_ => string.Format(selectionargsformat, _)));
			}
			if (identifiers?.WithoutSongIds?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutSongIds.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.Id)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutSongIds);
			}
			if (identifiers?.WithoutSongTitles?.Any() ?? false)
			{
				selectionenumerable = selectionenumerable.Append(string.Format("({0})", string.Join(selectionvaluesseperator, Enumerable.Repeat(
					count: identifiers.WithoutSongTitles.Count(),
					element: string.Format(selectionvaluesnotlikeelementformat, MediaStoreUtils.ColumnNames.Title)))));
				selectionargsenumerable = selectionargsenumerable.Concat(identifiers.WithoutSongTitles.Select(_ => string.Format(selectionargsformat, _)));
			}

			if (selectionenumerable.Any())
				selectionstringbuilder
					.Insert(0, "(")
					.AppendFormat(") AND (")
					.AppendJoin(exclusive ? " AND " : " OR ", selectionenumerable)
					.Append(")");

			selection = selectionstringbuilder.ToString();
			selectionargs = selectionargsenumerable.ToArray();
		}
		public static (string selection, string[] selectionargs) ToSelectionAndArgs(this ILibrary.IIdentifiers? identifiers, bool exclusive, ILibrary.ISettings? settings)
		{
			identifiers.ToSelectionAndArgs(exclusive, settings, out string s, out string[] sa);

			return (s, sa);
		}
	}
}