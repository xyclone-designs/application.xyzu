using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public class InfoPlaylistView : InfoView, IInfoPlaylist
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_info_playlist;

			public const int Title = Resource.Id.xyzu_view_info_playlist_title_appcompattextview;
			
			public const int PlaylistName_Title = Resource.Id.xyzu_view_info_playlist_playlistname_title_appcompattextview; 
			public const int PlaylistName_Value = Resource.Id.xyzu_view_info_playlist_playlistname_value_appcompattextview;
			public const int PlaylistDateCreated_Title = Resource.Id.xyzu_view_info_playlist_playlistdatecreated_title_appcompattextview; 
			public const int PlaylistDateCreated_Value = Resource.Id.xyzu_view_info_playlist_playlistdatecreated_value_appcompattextview;
			public const int PlaylistDateModified_Title = Resource.Id.xyzu_view_info_playlist_playlistdatemodified_title_appcompattextview; 
			public const int PlaylistDateModified_Value = Resource.Id.xyzu_view_info_playlist_playlistdatemodified_value_appcompattextview;
			public const int PlaylistDuration_Title = Resource.Id.xyzu_view_info_playlist_playlistduration_title_appcompattextview; 
			public const int PlaylistDuration_Value = Resource.Id.xyzu_view_info_playlist_playlistduration_value_appcompattextview;
			public const int PlaylistSongCount_Title = Resource.Id.xyzu_view_info_playlist_playlistsongcount_title_appcompattextview; 
			public const int PlaylistSongCount_Value = Resource.Id.xyzu_view_info_playlist_playlistsongcount_value_appcompattextview;
		}

		public InfoPlaylistView(Context context) : base(context) { }
		public InfoPlaylistView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoPlaylistView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.PlaylistName_Title),
				FindViewById<AppCompatTextView>(Ids.PlaylistDateCreated_Title),
				FindViewById<AppCompatTextView>(Ids.PlaylistDateModified_Title),
				FindViewById<AppCompatTextView>(Ids.PlaylistDuration_Title),
				FindViewById<AppCompatTextView>(Ids.PlaylistSongCount_Title));

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Playlist):
					PlaylistName?.SetText(_Playlist?.Name, null);
					PlaylistDateCreated?.SetText(_Playlist?.DateCreated.ToString("dd/MM/yyyy"), null);
					PlaylistDateModified?.SetText(_Playlist?.DateModified?.ToString("dd/MM/yyyy"), null);
					PlaylistDuration?.SetText(_Playlist?.Duration.ToMicrowaveFormat(), null);
					PlaylistSongCount?.SetText(_Playlist?.SongIds.Count().ToString(), null);
					break;

				default: break;
			}
		}

		private IPlaylist? _Playlist;
		private AppCompatTextView? _PlaylistName;
		private AppCompatTextView? _PlaylistDateCreated;
		private AppCompatTextView? _PlaylistDateModified;
		private AppCompatTextView? _PlaylistDuration;
		private AppCompatTextView? _PlaylistSongCount;


		public IPlaylist? Playlist
		{
			get => _Playlist;
			set
			{
				_Playlist = value;

				OnPropertyChanged();
			}
		}
		public AppCompatTextView PlaylistName
		{
			get => _PlaylistName
				??= FindViewById<AppCompatTextView>(Ids.PlaylistName_Value) ??
				throw new InflateException("PlaylistName");
		}
		public AppCompatTextView PlaylistDateCreated
		{
			get => _PlaylistDateCreated
				??= FindViewById<AppCompatTextView>(Ids.PlaylistDateCreated_Value) ??
				throw new InflateException("PlaylistDateCreated");
		}
		public AppCompatTextView PlaylistDateModified
		{
			get => _PlaylistDateModified
				??= FindViewById<AppCompatTextView>(Ids.PlaylistDateModified_Value) ??
				throw new InflateException("PlaylistDateModified");
		}
		public AppCompatTextView PlaylistDuration
		{
			get => _PlaylistDuration
				??= FindViewById<AppCompatTextView>(Ids.PlaylistDuration_Value) ??
				throw new InflateException("PlaylistDuration");
		}
		public AppCompatTextView PlaylistSongCount
		{
			get => _PlaylistSongCount
				??= FindViewById<AppCompatTextView>(Ids.PlaylistSongCount_Value) ??
				throw new InflateException("PlaylistSongCount");
		}
	}
}