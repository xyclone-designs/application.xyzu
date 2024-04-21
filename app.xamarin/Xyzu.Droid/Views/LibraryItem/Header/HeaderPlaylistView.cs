#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;

using System;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.LibraryItem.Header
{
	public partial class HeaderPlaylistView : HeaderView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_header_playlist;

			public const int Artwork_AppCompatImageView = Resource.Id.xyzu_view_libraryitem_header_playlist_artwork_appcompatimageview;
			public const int LineOne_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_playlist_lineone_appcompattextview;
			public const int LineTwo_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_playlist_linetwo_appcompattextview;
			public const int LineThree_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_playlist_linethree_appcompattextview;
			public const int LineFour_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_playlist_linefour_appcompattextview;
			public const int PlaylistSongs_LibraryHeaderLibraryItemsView = Resource.Id.xyzu_view_libraryitem_header_playlist_playlistsongs_headerlibraryitemsview;
		}

		public HeaderPlaylistView(Context context) : base(context) { }
		public HeaderPlaylistView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public HeaderPlaylistView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);
		}

		private IPlaylist? _Playlist;
		private HeaderLibraryItemsView? _PlaylistSongs;

		public IPlaylist? Playlist
		{
			get => _Playlist;
			set
			{
				_Playlist = value;

				SetPlaylist(_Playlist, Defaults.Playlist(Context));
			}
		}

		public HeaderLibraryItemsView PlaylistSongs
		{
			get => _PlaylistSongs ??= FindViewById(Ids.PlaylistSongs_LibraryHeaderLibraryItemsView) as HeaderLibraryItemsView ?? throw new InflateException();
		}

		public Action<object, EventArgs>? OnClickOptions
		{
			get => PlaylistSongs.OptionsClick;
			set => PlaylistSongs.OptionsClick = value;
		}
		public Action<object, EventArgs>? OnClickPlay
		{
			get => PlaylistSongs.PlayClick;
			set => PlaylistSongs.PlayClick = value;
		}

		protected void SetPlaylist(IPlaylist? playlist, IPlaylist? defaults = null)
		{
			string? lineone = playlist?.Name ?? defaults?.Name;
			string? linetwo = playlist?.Duration.ToMicrowaveFormat() ?? defaults?.Duration.ToMicrowaveFormat();

			if (LineOne != null)
			{
				LineOne.Visibility = ViewStates.Visible;
				LineOne.SetText(linetwo, null);
			}
			if (LineTwo != null)
			{
				LineTwo.Visibility = ViewStates.Visible;
				LineTwo.SetText(linetwo, null);
			}

			SetArtwork(playlist);

			if (Images != null)
				Images.SetToViewBackground(IImagesDroid.DefaultOperations.BlurDownsample, this, null, default, playlist);
		}
	}
}