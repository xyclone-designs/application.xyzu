#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Runtime.CompilerServices;
using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public class InfoSongView : InfoView, IInfoSong
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_info_song;

			public const int Title = Resource.Id.xyzu_view_info_song_title_appcompattextview;
			public const int SongArtwork = Resource.Id.xyzu_view_info_song_songartwork_appcompatimageview;

			public const int SongTitle_Title = Resource.Id.xyzu_view_info_song_songtitle_title_appcompattextview; 
			public const int SongTitle_Value = Resource.Id.xyzu_view_info_song_songtitle_value_appcompattextview;
			public const int SongArtist_Title = Resource.Id.xyzu_view_info_song_songartist_title_appcompattextview; 
			public const int SongArtist_Value = Resource.Id.xyzu_view_info_song_songartist_value_appcompattextview;
			public const int SongAlbum_Title = Resource.Id.xyzu_view_info_song_songalbum_title_appcompattextview; 
			public const int SongAlbum_Value = Resource.Id.xyzu_view_info_song_songalbum_value_appcompattextview;
			public const int SongAlbumArtist_Title = Resource.Id.xyzu_view_info_song_songalbumartist_title_appcompattextview; 
			public const int SongAlbumArtist_Value = Resource.Id.xyzu_view_info_song_songalbumartist_value_appcompattextview;
			public const int SongGenre_Title = Resource.Id.xyzu_view_info_song_songgenre_title_appcompattextview; 
			public const int SongGenre_Value = Resource.Id.xyzu_view_info_song_songgenre_value_appcompattextview;
			public const int SongReleaseDate_Title = Resource.Id.xyzu_view_info_song_songreleasedate_title_appcompattextview; 
			public const int SongReleaseDate_Value = Resource.Id.xyzu_view_info_song_songreleasedate_value_appcompattextview;
			public const int SongDuration_Title = Resource.Id.xyzu_view_info_song_songduration_title_appcompattextview; 
			public const int SongDuration_Value = Resource.Id.xyzu_view_info_song_songduration_value_appcompattextview;
			public const int SongTrackNumber_Title = Resource.Id.xyzu_view_info_song_songtracknumber_title_appcompattextview; 
			public const int SongTrackNumber_Value = Resource.Id.xyzu_view_info_song_songtracknumber_value_appcompattextview;
			public const int SongDiscNumber_Title = Resource.Id.xyzu_view_info_song_songdiscnumber_title_appcompattextview; 
			public const int SongDiscNumber_Value = Resource.Id.xyzu_view_info_song_songdiscnumber_value_appcompattextview;
			public const int SongCopyright_Title = Resource.Id.xyzu_view_info_song_songcopyright_title_appcompattextview; 
			public const int SongCopyright_Value = Resource.Id.xyzu_view_info_song_songcopyright_value_appcompattextview;
			public const int SongBitrate_Title = Resource.Id.xyzu_view_info_song_songbitrate_title_appcompattextview; 
			public const int SongBitrate_Value = Resource.Id.xyzu_view_info_song_songbitrate_value_appcompattextview;
			public const int SongFilepath_Title = Resource.Id.xyzu_view_info_song_songfilepath_title_appcompattextview; 
			public const int SongFilepath_Value = Resource.Id.xyzu_view_info_song_songfilepath_value_appcompattextview;
			public const int SongMimeType_Title = Resource.Id.xyzu_view_info_song_songmimetype_title_appcompattextview; 
			public const int SongMimeType_Value = Resource.Id.xyzu_view_info_song_songmimetype_value_appcompattextview;
			public const int SongSize_Title = Resource.Id.xyzu_view_info_song_songsize_title_appcompattextview; 
			public const int SongSize_Value = Resource.Id.xyzu_view_info_song_songsize_value_appcompattextview;
		}

		public InfoSongView(Context context) : base(context) { }
		public InfoSongView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoSongView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			SongArtwork = FindViewById(Ids.SongArtwork) as AppCompatImageView;

			SongTitle = FindViewById(Ids.SongTitle_Value) as AppCompatTextView;
			SongArtist = FindViewById(Ids.SongArtist_Value) as AppCompatTextView;
			SongAlbum = FindViewById(Ids.SongAlbum_Value) as AppCompatTextView;
			SongAlbumArtist = FindViewById(Ids.SongAlbumArtist_Value) as AppCompatTextView;
			SongGenre = FindViewById(Ids.SongGenre_Value) as AppCompatTextView;
			SongReleaseDate = FindViewById(Ids.SongReleaseDate_Value) as AppCompatTextView;
			SongDuration = FindViewById(Ids.SongDuration_Value) as AppCompatTextView;
			SongTrackNumber = FindViewById(Ids.SongTrackNumber_Value) as AppCompatTextView;
			SongDiscNumber = FindViewById(Ids.SongDiscNumber_Value) as AppCompatTextView;
			SongCopyright = FindViewById(Ids.SongCopyright_Value) as AppCompatTextView;
			SongBitrate = FindViewById(Ids.SongBitrate_Value) as AppCompatTextView;
			SongFilepath = FindViewById(Ids.SongFilepath_Value) as AppCompatTextView;
			SongMimeType = FindViewById(Ids.SongMimeType_Value) as AppCompatTextView;
			SongSize = FindViewById(Ids.SongSize_Value) as AppCompatTextView;					   
			SongTitle_Title = FindViewById(Ids.SongTitle_Title) as AppCompatTextView;
			SongArtist_Title = FindViewById(Ids.SongArtist_Title) as AppCompatTextView;
			SongAlbum_Title = FindViewById(Ids.SongAlbum_Title) as AppCompatTextView;
			SongAlbumArtist_Title = FindViewById(Ids.SongAlbumArtist_Title) as AppCompatTextView;
			SongGenre_Title = FindViewById(Ids.SongGenre_Title) as AppCompatTextView;
			SongReleaseDate_Title = FindViewById(Ids.SongReleaseDate_Title) as AppCompatTextView;
			SongDuration_Title = FindViewById(Ids.SongDuration_Title) as AppCompatTextView;
			SongTrackNumber_Title = FindViewById(Ids.SongTrackNumber_Title) as AppCompatTextView;
			SongDiscNumber_Title = FindViewById(Ids.SongDiscNumber_Title) as AppCompatTextView;
			SongCopyright_Title = FindViewById(Ids.SongCopyright_Title) as AppCompatTextView;
			SongBitrate_Title = FindViewById(Ids.SongBitrate_Title) as AppCompatTextView;
			SongFilepath_Title = FindViewById(Ids.SongFilepath_Title) as AppCompatTextView;
			SongMimeType_Title = FindViewById(Ids.SongMimeType_Title) as AppCompatTextView;
			SongSize_Title = FindViewById(Ids.SongSize_Title) as AppCompatTextView;
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch(propertyname)
			{
				case nameof(Images):
					ReloadImage();
					break;

				default: break;
			}
		}

		private ISong? _Song;

		public ISong? Song
		{
			get => _Song;
			set
			{
				_Song = value;

				SongTitle?.SetText(_Song?.Title, null);
				SongArtist?.SetText(_Song?.Artist, null);
				SongAlbum?.SetText(_Song?.Album, null);
				SongAlbumArtist?.SetText(_Song?.AlbumArtist, null);
				SongGenre?.SetText(_Song?.Genre, null);
				SongReleaseDate?.SetText(_Song?.ReleaseDate?.ToString("dd/MM/yyyy"), null);
				SongDuration?.SetText(_Song?.Duration?.ToMicrowaveFormat(), null);
				SongTrackNumber?.SetText(_Song?.TrackNumber?.ToString(), null);
				SongDiscNumber?.SetText(_Song?.DiscNumber?.ToString(), null);
				SongCopyright?.SetText(_Song?.Copyright, null);
				SongBitrate?.SetText(_Song?.Bitrate?.ToString(), null);
				SongFilepath?.SetText(_Song?.Filepath, null);
				SongMimeType?.SetText(_Song?.MimeType?.ToString(), null);
				SongSize?.SetText(string.Format("{0} MB", (_Song?.Size ?? 0) / 1024 / 1024), null);

				ReloadImage();
			}
		}

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatImageView? SongArtwork { get; protected set; }
		public AppCompatTextView? SongTitle { get; protected set; }
		public AppCompatTextView? SongArtist { get; protected set; }
		public AppCompatTextView? SongAlbum { get; protected set; }
		public AppCompatTextView? SongAlbumArtist { get; protected set; }
		public AppCompatTextView? SongGenre { get; protected set; }
		public AppCompatTextView? SongReleaseDate { get; protected set; }
		public AppCompatTextView? SongDuration { get; protected set; }
		public AppCompatTextView? SongTrackNumber { get; protected set; }
		public AppCompatTextView? SongDiscNumber { get; protected set; }
		public AppCompatTextView? SongCopyright { get; protected set; }
		public AppCompatTextView? SongBitrate { get; protected set; }
		public AppCompatTextView? SongFilepath { get; protected set; }
		public AppCompatTextView? SongMimeType { get; protected set; }
		public AppCompatTextView? SongSize { get; protected set; }				  
		public AppCompatTextView? SongTitle_Title { get; protected set; }
		public AppCompatTextView? SongArtist_Title { get; protected set; }
		public AppCompatTextView? SongAlbum_Title { get; protected set; }
		public AppCompatTextView? SongAlbumArtist_Title { get; protected set; }
		public AppCompatTextView? SongGenre_Title { get; protected set; }
		public AppCompatTextView? SongReleaseDate_Title { get; protected set; }
		public AppCompatTextView? SongDuration_Title { get; protected set; }
		public AppCompatTextView? SongTrackNumber_Title { get; protected set; }
		public AppCompatTextView? SongDiscNumber_Title { get; protected set; }
		public AppCompatTextView? SongCopyright_Title { get; protected set; }
		public AppCompatTextView? SongBitrate_Title { get; protected set; }
		public AppCompatTextView? SongFilepath_Title { get; protected set; }
		public AppCompatTextView? SongMimeType_Title { get; protected set; }
		public AppCompatTextView? SongSize_Title { get; protected set; }

		public async void ReloadImage()
		{
			if (Images != null)
			{
				await Images.SetToImageView(IImages.DefaultOperations.RoundedDownsample, SongArtwork, null, default, Song);

				if (Context != null && Images.GetPalette(Song)?.GetColorForBackground(Context, Resource.Color.ColorSurface) is Color color)
				{
					Title?.SetTextColor(color);

					SongTitle_Title?.SetTextColor(color);
					SongArtist_Title?.SetTextColor(color);
					SongAlbum_Title?.SetTextColor(color);
					SongAlbumArtist_Title?.SetTextColor(color);
					SongGenre_Title?.SetTextColor(color);
					SongReleaseDate_Title?.SetTextColor(color);
					SongDuration_Title?.SetTextColor(color);
					SongTrackNumber_Title?.SetTextColor(color);
					SongDiscNumber_Title?.SetTextColor(color);
					SongCopyright_Title?.SetTextColor(color);
					SongBitrate_Title?.SetTextColor(color);
					SongFilepath_Title?.SetTextColor(color);
					SongMimeType_Title?.SetTextColor(color);
					SongSize_Title?.SetTextColor(color);
				}
			}
		}
	}
}