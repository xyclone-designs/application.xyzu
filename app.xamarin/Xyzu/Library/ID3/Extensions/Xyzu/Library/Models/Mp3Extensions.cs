using Id3;
using Id3.Frames;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Images;

namespace Xyzu.Library.Models
{
	public static class Mp3Extensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, Mp3 mp3, IAlbum<bool>? retriever, bool overwrite = false)
		{
			IEnumerable<Id3Tag>? _tags = null;
			IEnumerable<Id3Tag> tags() => _tags ??= mp3.GetAllTags();

			if ((retriever?.ReleaseDate ?? true) && (retrieved.ReleaseDate is null || overwrite) && tags().Select(tag => tag.Year).FirstOrDefault(frame => frame.IsAssigned) is YearFrame yearframe && yearframe.Value.HasValue) 
				retrieved.ReleaseDate = new DateTime(yearframe.Value.Value, 1, 1);

			if ((retriever?.Title ?? true) && (retrieved.Title is null || overwrite) && tags().Select(tag => tag.Album).FirstOrDefault(frame => frame.IsAssigned) is AlbumFrame albumframe) 
				retrieved.Title = albumframe.Value;

			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, Mp3 mp3, IArtist<bool>? retriever, bool overwrite = false)
		{
			IEnumerable<Id3Tag>? _tags = null;
			IEnumerable<Id3Tag> tags() => _tags ??= mp3.GetAllTags();

			if ((retriever?.Name ?? true) && (retrieved.Name is null || overwrite) && tags().Select(tag => tag.Artists).FirstOrDefault(frame => frame.IsAssigned) is ArtistsFrame artistsframe) 
				retrieved.Name = artistsframe.Value.FirstOrDefault();

			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, Mp3 mp3, IGenre<bool>? retriever, bool overwrite = false)
		{
			IEnumerable<Id3Tag>? _tags = null;
			IEnumerable<Id3Tag> tags() => _tags ??= mp3.GetAllTags();

			if ((retriever?.Name ?? true) && (retrieved.Name is null || overwrite) && tags().Select(tag => tag.Genre).FirstOrDefault(frame => frame.IsAssigned) is GenreFrame genreframe) 
				retrieved.Name = genreframe.Value;

			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, Mp3 mp3, IPlaylist<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, Mp3 mp3, ISong<bool>? retriever, bool overwrite = false)
		{
			IEnumerable<Id3Tag>? _tags = null;
			IEnumerable<Id3Tag> tags() => _tags ??= mp3.GetAllTags();

			if ((retriever?.Album ?? true) && (retrieved.Album is null || overwrite) && tags().Select(tag => tag.Album).FirstOrDefault(frame => frame.IsAssigned) is AlbumFrame albumframe)
				retrieved.Album = albumframe.Value;   

			if ((retriever?.Artist ?? true) && (retrieved.Artist is null || overwrite) && tags().Select(tag => tag.Artists).FirstOrDefault(frame => frame.IsAssigned) is ArtistsFrame artistsframe)
				retrieved.Artist = artistsframe.Value.FirstOrDefault();

			if ((retriever?.Bitrate ?? true) && (retrieved.Bitrate is null || overwrite))
				retrieved.Bitrate = mp3.Audio.Bitrate;

			if ((retriever?.Channels ?? true) && (retrieved.Channels is null || overwrite))
				retrieved.Channels = mp3.Audio.Mode.ToAudioChannel();

			if ((retriever?.Copyright ?? true) && (retrieved.Copyright is null || overwrite) && tags().Select(tag => tag.Copyright).FirstOrDefault(frame => frame.IsAssigned) is CopyrightFrame copyrightframe)
				retrieved.Copyright = copyrightframe.Value;

			if ((retriever?.Duration ?? true) && (retrieved.Duration is null || overwrite))
				retrieved.Duration = mp3.Audio.Duration;

			if ((retriever?.Filepath ?? true) && (retrieved.Filepath is null || overwrite) && tags().Select(tag => tag.AudioFileUrl).FirstOrDefault(frame => frame.IsAssigned) is AudioFileUrlFrame audiofileurlframe)
				retrieved.Filepath = audiofileurlframe.Url;

			if ((retriever?.Genre ?? true) && (retrieved.Genre is null || overwrite) && tags().Select(tag => tag.Genre).FirstOrDefault(frame => frame.IsAssigned) is GenreFrame genreframe)
				retrieved.Genre = genreframe.Value;

			if ((retriever?.Lyrics ?? true) && (retrieved.Lyrics is null || overwrite) && tags().SelectMany(tag => tag.Lyrics).FirstOrDefault(frame => frame.IsAssigned) is LyricsFrame lyricsframe)
				retrieved.Lyrics = lyricsframe.Lyrics;

			if ((retriever?.MimeType ?? true) && (retrieved.MimeType is null || overwrite) && tags().Select(tag => tag.FileType).FirstOrDefault(frame => frame.IsAssigned) is FileTypeFrame filetypeframe)
				retrieved.MimeType = filetypeframe.Value.ToMimeType();

			if ((retriever?.ReleaseDate ?? true) && (retrieved.ReleaseDate is null || overwrite) && tags().Select(tag => tag.Year).FirstOrDefault(frame => frame.IsAssigned) is YearFrame yearframe && yearframe.Value.HasValue)
				retrieved.ReleaseDate = new DateTime(yearframe.Value.Value, 1, 1);

			if ((retriever?.Title ?? true) && (retrieved.Title is null || overwrite) && tags().Select(tag => tag.Title).FirstOrDefault(frame => frame.IsAssigned) is TitleFrame titleframe)
				retrieved.Title = titleframe.Value;

			if ((retriever?.TrackNumber ?? true) && (retrieved.TrackNumber is null || overwrite) && tags().Select(tag => tag.Track).FirstOrDefault(frame => frame.IsAssigned) is TrackFrame trackframe)
				retrieved.TrackNumber = trackframe.Value;

			_tags = null;

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, Mp3 mp3, PictureTypeComparer? picturetypecomparer)
		{
			if (retrieved.Buffer is null)
			{
				PictureFrame? pictureframe = mp3.GetAllTags()
					.SelectMany(tag => tag.Pictures)
					.OrderBy(pictureframe => pictureframe.PictureType, picturetypecomparer ?? new PictureTypeComparer())
					.Where(frame => frame.IsAssigned && (frame.PictureData?.Any() ?? false))
					.FirstOrDefault();

				retrieved.Buffer = pictureframe?.PictureData;
			}

			return retrieved;
		}
	}
}
