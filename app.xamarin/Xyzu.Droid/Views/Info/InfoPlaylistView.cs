#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;
using System.Linq;

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

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			PlaylistName = FindViewById(Ids.PlaylistName_Value) as AppCompatTextView;
			PlaylistDateCreated = FindViewById(Ids.PlaylistDateCreated_Value) as AppCompatTextView;
			PlaylistDateModified = FindViewById(Ids.PlaylistDateModified_Value) as AppCompatTextView;
			PlaylistDuration = FindViewById(Ids.PlaylistDuration_Value) as AppCompatTextView;
			PlaylistSongCount = FindViewById(Ids.PlaylistSongCount_Value) as AppCompatTextView;			 
			PlaylistName_Title = FindViewById(Ids.PlaylistName_Title) as AppCompatTextView;
			PlaylistDateCreated_Title = FindViewById(Ids.PlaylistDateCreated_Title) as AppCompatTextView;
			PlaylistDateModified_Title = FindViewById(Ids.PlaylistDateModified_Title) as AppCompatTextView;
			PlaylistDuration_Title = FindViewById(Ids.PlaylistDuration_Title) as AppCompatTextView;
			PlaylistSongCount_Title = FindViewById(Ids.PlaylistSongCount_Title) as AppCompatTextView;
		}


		private IPlaylist? _Playlist;
		public IPlaylist? Playlist
		{
			get => _Playlist;
			set
			{
				_Playlist = value;

				PlaylistName?.SetText(_Playlist?.Name, null);
				PlaylistDateCreated?.SetText(_Playlist?.DateCreated.ToString("dd/MM/yyyy"), null);
				PlaylistDateModified?.SetText(_Playlist?.DateModified?.ToString("dd/MM/yyyy"), null);
				PlaylistDuration?.SetText(_Playlist?.Duration.ToMicrowaveFormat(), null);
				PlaylistSongCount?.SetText(_Playlist?.SongIds.Count().ToString(), null);
			}
		}

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatTextView? PlaylistName { get; protected set; }
		public AppCompatTextView? PlaylistDateCreated { get; protected set; }
		public AppCompatTextView? PlaylistDateModified { get; protected set; }
		public AppCompatTextView? PlaylistDuration { get; protected set; }
		public AppCompatTextView? PlaylistSongCount { get; protected set; }					
		public AppCompatTextView? PlaylistName_Title { get; protected set; }
		public AppCompatTextView? PlaylistDateCreated_Title { get; protected set; }
		public AppCompatTextView? PlaylistDateModified_Title { get; protected set; }
		public AppCompatTextView? PlaylistDuration_Title { get; protected set; }
		public AppCompatTextView? PlaylistSongCount_Title { get; protected set; }
	}
}