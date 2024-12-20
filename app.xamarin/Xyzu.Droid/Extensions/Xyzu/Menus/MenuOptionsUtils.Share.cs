﻿using Android.App;
using Android.Content;
using Android.Net;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Library;
using Xyzu.Library.Models;

using JavaUri = Java.Net.URI;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public static void Share(VariableContainer variables)
		{
			if (variables.Activity is null)
				return;

			switch (true)
			{
				case true when variables.Albums.Any():
					Share(variables.Activity, variables.Albums.ToArray());
					break;

				case true when variables.Artists.Any():
					Share(variables.Activity, variables.Artists.ToArray());
					break;

				case true when variables.Genres.Any():
					Share(variables.Activity, variables.Genres.ToArray());
					break;

				case true when variables.Playlists.Any():
					Share(variables.Activity, variables.Playlists.ToArray());
					break;

				case true when variables.Songs.Any():
					Share(variables.Activity, variables.Songs.ToArray());
					break;

				default: break;
			}
		}
		public static void Share(Activity activity, params IAlbum[] albums)
		{
			Share(activity, albums
				.SelectMany(album => album.SongIds)
				.Distinct());
		}
		public static void Share(Activity activity, params IArtist[] artists)
		{
			Share(activity, artists
				.SelectMany(artist => artist.SongIds)
				.Distinct());
		}
		public static void Share(Activity activity, params IGenre[] genres)
		{
			Share(activity, genres
				.SelectMany(genre => genre.SongIds)
				.Distinct());
		}
		public static void Share(Activity activity, params IPlaylist[] playlists)
		{
			Share(activity, playlists
				.SelectMany(playlist => playlist.SongIds)
				.Distinct());
		}
		public static void Share(Activity activity, params string[] songids)
		{
			Share(activity, songids as IEnumerable<string>);
		}
		public static void Share(Activity activity, IEnumerable<string> songids)
		{
			Share(activity, XyzuLibrary.Instance.Songs.GetSongs(
				new ILibrary.IIdentifiers.Default 
				{ 
					SongIds = songids 
				}));
		}
		public static void Share(Activity activity, params ISong[] songs)
		{
			Share(activity, songs as IEnumerable<ISong>);
		}
		public static void Share(Activity activity, IEnumerable<ISong> songs)
		{
			Intent? intentchooser = XyzuUtils.Intents.Chooser_ShareFiles(activity, songs
				.Distinct(new EqualityComparerDefault<ISong>((x, y) => x?.Uri == y?.Uri))
				.Select(song => song.Uri?.ToJavaUri())
				.OfType<JavaUri>()
				.ToArray());

			activity.StartActivityForResult(intentchooser, -1);
		}
	}
}