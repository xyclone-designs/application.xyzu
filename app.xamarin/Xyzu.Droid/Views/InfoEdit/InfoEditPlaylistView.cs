using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public class InfoEditPlaylistView : InfoEditView, IInfoEditPlaylist
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_infoedit_playlist;

			public const int PlaylistName_Value = Resource.Id.xyzu_view_infoedit_playlist_playlistname_value_appcompatedittext;
			public const int PlaylistName_Title = Resource.Id.xyzu_view_infoedit_playlist_playlistname_title_appcompattextview;
		}

		public InfoEditPlaylistView(Context context) : base(context) { }
		public InfoEditPlaylistView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoEditPlaylistView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(FindViewById<AppCompatTextView>(Ids.PlaylistName_Title));

			base.Init(context, attrs);
		}

		private IPlaylist? _Playlist;
		private AppCompatTextView? _PlaylistName;

		public IPlaylist? Playlist
		{
			get
			{
				if (_Playlist != null)
				{
					_Playlist.Name = PlaylistName?.Text;
				}

				return _Playlist;
			}
			set
			{
				_Playlist = value;

				PlaylistName?.SetText(_Playlist?.Name, null);
			}
		}
		public AppCompatTextView PlaylistName
		{
			get => _PlaylistName
				??= FindViewById<AppCompatTextView>(Ids.PlaylistName_Title) ??
				throw new InflateException("PlaylistName");
		}
	}
}