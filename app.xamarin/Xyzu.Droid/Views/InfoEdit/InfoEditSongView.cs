#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public class InfoEditSongView : InfoEditView, IInfoEditSong
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_infoedit_song;

			public const int Title = Resource.Id.xyzu_view_infoedit_song_title_appcompattextview;
			public const int SongArtwork = Resource.Id.xyzu_view_infoedit_song_songartwork_appcompatimageview;
			public const int SongTitle_Value = Resource.Id.xyzu_view_infoedit_song_songtitle_value_appcompatedittext;
			public const int SongTitle_Title = Resource.Id.xyzu_view_infoedit_song_songtitle_title_appcompattextview;
			public const int SongArtist_Value = Resource.Id.xyzu_view_infoedit_song_songartist_value_appcompatedittext;
			public const int SongArtist_Title = Resource.Id.xyzu_view_infoedit_song_songartist_title_appcompattextview;
			public const int SongAlbum_Value = Resource.Id.xyzu_view_infoedit_song_songalbum_value_appcompatedittext;
			public const int SongAlbum_Title = Resource.Id.xyzu_view_infoedit_song_songalbum_title_appcompattextview;
			public const int SongAlbumArtist_Value = Resource.Id.xyzu_view_infoedit_song_songalbumartist_value_appcompatedittext;
			public const int SongAlbumArtist_Title = Resource.Id.xyzu_view_infoedit_song_songalbumartist_title_appcompattextview;
			public const int SongGenre_Value = Resource.Id.xyzu_view_infoedit_song_songgenre_value_appcompatedittext;
			public const int SongGenre_Title = Resource.Id.xyzu_view_infoedit_song_songgenre_title_appcompattextview;
			public const int SongReleaseDate_Value = Resource.Id.xyzu_view_infoedit_song_songreleasedate_value_appcompateditdate;
			public const int SongReleaseDate_Title = Resource.Id.xyzu_view_infoedit_song_songreleasedate_title_appcompattextview;
			public const int SongCopyright_Value = Resource.Id.xyzu_view_infoedit_song_songcopyright_value_appcompatedittext;
			public const int SongCopyright_Title = Resource.Id.xyzu_view_infoedit_song_songcopyright_title_appcompattextview;
			public const int SongTrackNumber_Value = Resource.Id.xyzu_view_infoedit_song_songtracknumber_value_appcompatedittext;
			public const int SongTrackNumber_Title = Resource.Id.xyzu_view_infoedit_song_songtracknumber_title_appcompattextview;
			public const int SongDiscNumber_Value = Resource.Id.xyzu_view_infoedit_song_songdiscnumber_value_appcompatedittext;
			public const int SongDiscNumber_Title = Resource.Id.xyzu_view_infoedit_song_songdiscnumber_title_appcompattextview;
		}

		public InfoEditSongView(Context context) : base(context) { }
		public InfoEditSongView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoEditSongView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

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
				FindViewById<AppCompatTextView>(Ids.SongCopyright_Title),
				FindViewById<AppCompatTextView>(Ids.SongTrackNumber_Title),
				FindViewById<AppCompatTextView>(Ids.SongDiscNumber_Title));


			base.Init(context, attrs);
		}

		private ISong? _Song;
		private AppCompatImageView? _SongArtwork;
		private AppCompatEditText? _SongTitle;
		private AppCompatEditText? _SongArtist;
		private AppCompatEditText? _SongAlbum;
		private AppCompatEditText? _SongAlbumArtist;
		private AppCompatEditText? _SongGenre;
		private AppCompatEditDate? _SongReleaseDate;
		private AppCompatEditText? _SongCopyright;
		private AppCompatEditText? _SongTrackNumber;
		private AppCompatEditText? _SongDiscNumber;


		public ISong? Song
		{
			get
			{
				if (_Song != null)
				{
					_Song.Title = SongTitle?.Text;
					_Song.Artist = SongArtist?.Text;
					_Song.Album = SongAlbum?.Text;
					_Song.AlbumArtist = SongAlbumArtist?.Text;
					_Song.Genre = SongGenre?.Text;
					_Song.Copyright = SongCopyright?.Text;
					_Song.ReleaseDate = SongReleaseDate?.Date;
					_Song.TrackNumber = int.TryParse(SongTrackNumber?.Text ?? string.Empty, out int tracknumber) ? tracknumber : new int?();
					_Song.DiscNumber = int.TryParse(SongDiscNumber?.Text ?? string.Empty, out int discnumber) ? discnumber : new int?();
				}

				return _Song;
			}
			set
			{
				_Song = value;

				SongTitle?.SetText(_Song?.Title, null);
				SongArtist?.SetText(_Song?.Artist, null);
				SongAlbum?.SetText(_Song?.Album, null);
				SongAlbumArtist?.SetText(_Song?.AlbumArtist, null);
				SongGenre?.SetText(_Song?.Genre, null);
				SongCopyright?.SetText(_Song?.Copyright, null);
				SongReleaseDate?.SetDate(_Song?.ReleaseDate);
				SongTrackNumber?.SetText(_Song?.TrackNumber?.ToString(), null);
				SongDiscNumber?.SetText(_Song?.DiscNumber?.ToString(), null);

				ReloadImage();
			}
		}
		public AppCompatImageView SongArtwork 
		{
			get => _SongArtwork
				??= FindViewById<AppCompatImageView>(Ids.SongArtwork) ??
				throw new InflateException("SongArtwork");
		}
		public AppCompatEditText SongTitle 
		{
			get => _SongTitle
				??= FindViewById<AppCompatEditText>(Ids.SongTitle_Value) ??
				throw new InflateException("SongTitle_Value");
		}
		public AppCompatEditText SongArtist 
		{
			get => _SongArtist
				??= FindViewById<AppCompatEditText>(Ids.SongArtist_Value) ??
				throw new InflateException("SongArtist_Value");
		}
		public AppCompatEditText SongAlbum 
		{
			get => _SongAlbum
				??= FindViewById<AppCompatEditText>(Ids.SongAlbum_Value) ??
				throw new InflateException("SongAlbum_Value");
		}
		public AppCompatEditText SongAlbumArtist 
		{
			get => _SongAlbumArtist 
				??= FindViewById<AppCompatEditText>(Ids.SongAlbumArtist_Value) ??
				throw new InflateException("SongAlbumArtist_Value");
		}
		public AppCompatEditText SongGenre 
		{
			get => _SongGenre 
				??= FindViewById<AppCompatEditText>(Ids.SongGenre_Value) ??
				throw new InflateException("SongGenre_Value");
		}
		public AppCompatEditDate SongReleaseDate 
		{
			get => _SongReleaseDate
				??= FindViewById<AppCompatEditDate>(Ids.SongReleaseDate_Value) ??
				throw new InflateException("SongReleaseDate_Value");
		}
		public AppCompatEditText SongCopyright 
		{
			get => _SongCopyright
				??= FindViewById<AppCompatEditText>(Ids.SongCopyright_Value) ??
				throw new InflateException("SongCopyright_Value");
		}
		public AppCompatEditText SongTrackNumber 
		{
			get => _SongTrackNumber 
				??= FindViewById<AppCompatEditText>(Ids.SongTrackNumber_Value) ??
				throw new InflateException("SongTrackNumber_Value");
		}
		public AppCompatEditText SongDiscNumber 
		{
			get => _SongDiscNumber 
				??= FindViewById<AppCompatEditText>(Ids.SongDiscNumber_Value) ??
				throw new InflateException("SongDiscNumber_Value");
		}

		public Action<InfoEditSongView, object?, EventArgs>? SongArtworkClick { get; set; }

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Song)
			{
				ImageView = SongArtwork,
				Operations = IImages.DefaultOperations.RoundedDownsample,
				OnPalette = palette => Palette = palette
			});
		}

		protected void OnSongArtworkClick(object? sender, EventArgs args)
		{
			SongArtworkClick?.Invoke(this, sender, args);
		}
	}
}