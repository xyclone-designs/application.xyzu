#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
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
			base.Init(context, attrs);

			Inflate(Context, Ids.Layout, this);

			PlaylistName = FindViewById(Ids.PlaylistName_Value) as AppCompatEditText;
			PlaylistName_Title = FindViewById(Ids.PlaylistName_Title) as AppCompatTextView;
		}
		
		private IPlaylist? _Playlist;

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

		public AppCompatEditText? PlaylistName { get; protected set; }
		public AppCompatTextView? PlaylistName_Title { get; protected set; }
	}
}