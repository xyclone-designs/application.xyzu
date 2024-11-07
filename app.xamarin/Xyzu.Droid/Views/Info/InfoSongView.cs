using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;
using static Xyzu.Menus.LibraryItem;

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
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.SongTitle_Title),
				FindViewById<AppCompatTextView>(Ids.SongArtist_Title),
				FindViewById<AppCompatTextView>(Ids.SongAlbum_Title),
				FindViewById<AppCompatTextView>(Ids.SongAlbumArtist_Title),
				FindViewById<AppCompatTextView>(Ids.SongGenre_Title),
				FindViewById<AppCompatTextView>(Ids.SongReleaseDate_Title),
				FindViewById<AppCompatTextView>(Ids.SongDuration_Title),
				FindViewById<AppCompatTextView>(Ids.SongTrackNumber_Title),
				FindViewById<AppCompatTextView>(Ids.SongDiscNumber_Title),
				FindViewById<AppCompatTextView>(Ids.SongCopyright_Title),
				FindViewById<AppCompatTextView>(Ids.SongBitrate_Title),
				FindViewById<AppCompatTextView>(Ids.SongFilepath_Title),
				FindViewById<AppCompatTextView>(Ids.SongMimeType_Title),
				FindViewById<AppCompatTextView>(Ids.SongSize_Title));

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch(propertyname)
			{
				case nameof(Song):
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
					break;

				default: break;
			}
		}

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Song)
			{
				ImageView = SongArtwork,
				Operations = IImages.DefaultOperations.RoundedDownsample,
				OnPalette = palette => Palette = palette
			});
		}

		private ISong? _Song;
		protected AppCompatImageView? _SongArtwork;
		protected AppCompatTextView? _SongTitle;
		protected AppCompatTextView? _SongArtist;
		protected AppCompatTextView? _SongAlbum;
		protected AppCompatTextView? _SongAlbumArtist;
		protected AppCompatTextView? _SongGenre;
		protected AppCompatTextView? _SongReleaseDate;
		protected AppCompatTextView? _SongDuration;
		protected AppCompatTextView? _SongTrackNumber;
		protected AppCompatTextView? _SongDiscNumber;
		protected AppCompatTextView? _SongCopyright;
		protected AppCompatTextView? _SongBitrate;
		protected AppCompatTextView? _SongFilepath;
		protected AppCompatTextView? _SongMimeType;
		protected AppCompatTextView? _SongSize;


		public ISong? Song
		{
			get => _Song;
			set
			{
				_Song = value;

				OnPropertyChanged();
			}
		}
		public AppCompatImageView SongArtwork 
		{
			get => _SongArtwork ??=
				FindViewById(Ids.SongArtwork) as AppCompatImageView ??
				throw new InflateException("SongArtwork");
		}
		public AppCompatTextView SongTitle 
		{
			get => _SongTitle ??=
				FindViewById(Ids.SongTitle_Value) as AppCompatTextView ??
				throw new InflateException("SongTitle");
		}
		public AppCompatTextView SongArtist 
		{
			get => _SongArtist ??=
				FindViewById(Ids.SongArtist_Value) as AppCompatTextView ??
				throw new InflateException("SongArtist");
		}
		public AppCompatTextView SongAlbum 
		{
			get => _SongAlbum ??=
				FindViewById(Ids.SongAlbum_Value) as AppCompatTextView ??
				throw new InflateException("SongAlbum");
		}
		public AppCompatTextView SongAlbumArtist 
		{
			get => _SongAlbumArtist ??=
				FindViewById(Ids.SongAlbumArtist_Value) as AppCompatTextView ??
				throw new InflateException("SongAlbumArtist");
		}
		public AppCompatTextView SongGenre 
		{
			get => _SongGenre ??=
				FindViewById(Ids.SongGenre_Value) as AppCompatTextView ??
				throw new InflateException("SongGenre");
		}
		public AppCompatTextView SongReleaseDate 
		{
			get => _SongReleaseDate ??=
				FindViewById(Ids.SongReleaseDate_Value) as AppCompatTextView ??
				throw new InflateException("SongReleaseDate");
		}
		public AppCompatTextView SongDuration 
		{
			get => _SongDuration ??=
				FindViewById(Ids.SongDuration_Value) as AppCompatTextView ??
				throw new InflateException("SongDuration");
		}
		public AppCompatTextView SongTrackNumber 
		{
			get => _SongTrackNumber ??=
				FindViewById(Ids.SongTrackNumber_Value) as AppCompatTextView ??
				throw new InflateException("SongTrackNumber");
		}
		public AppCompatTextView SongDiscNumber 
		{
			get => _SongDiscNumber ??=
				FindViewById(Ids.SongDiscNumber_Value) as AppCompatTextView ??
				throw new InflateException("SongDiscNumber");
		}
		public AppCompatTextView SongCopyright 
		{
			get => _SongCopyright ??=
				FindViewById(Ids.SongCopyright_Value) as AppCompatTextView ??
				throw new InflateException("SongCopyright");
		}
		public AppCompatTextView SongBitrate 
		{
			get => _SongBitrate ??=
				FindViewById(Ids.SongBitrate_Value) as AppCompatTextView ??
				throw new InflateException("SongBitrate");
		}
		public AppCompatTextView SongFilepath 
		{
			get => _SongFilepath ??=
				FindViewById(Ids.SongFilepath_Value) as AppCompatTextView ??
				throw new InflateException("SongFilepath");
		}
		public AppCompatTextView SongMimeType 
		{
			get => _SongMimeType ??=
				FindViewById(Ids.SongMimeType_Value) as AppCompatTextView ??
				throw new InflateException("SongMimeType");
		}
		public AppCompatTextView SongSize 
		{
			get => _SongSize ??=
				FindViewById(Ids.SongSize_Value) as AppCompatTextView ??
				throw new InflateException("SongSize");
		}
	}
}