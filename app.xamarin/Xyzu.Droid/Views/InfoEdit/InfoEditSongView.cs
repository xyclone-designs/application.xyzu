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
			Inflate(Context, Ids.Layout, this);

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			SongArtwork = FindViewById(Ids.SongArtwork) as AppCompatImageView;
			SongTitle = FindViewById(Ids.SongTitle_Value) as AppCompatEditText;
			SongArtist = FindViewById(Ids.SongArtist_Value) as AppCompatEditText;
			SongAlbum = FindViewById(Ids.SongAlbum_Value) as AppCompatEditText;
			SongAlbumArtist = FindViewById(Ids.SongAlbumArtist_Value) as AppCompatEditText;
			SongGenre = FindViewById(Ids.SongGenre_Value) as AppCompatEditText;
			SongReleaseDate = FindViewById(Ids.SongReleaseDate_Value) as AppCompatEditDate;
			SongCopyright = FindViewById(Ids.SongCopyright_Value) as AppCompatEditText;
			SongTrackNumber = FindViewById(Ids.SongTrackNumber_Value) as AppCompatEditText;
			SongDiscNumber = FindViewById(Ids.SongDiscNumber_Value) as AppCompatEditText;	  
			SongTitle_Title = FindViewById(Ids.SongTitle_Title) as AppCompatTextView;
			SongArtist_Title = FindViewById(Ids.SongArtist_Title) as AppCompatTextView;
			SongAlbum_Title = FindViewById(Ids.SongAlbum_Title) as AppCompatTextView;
			SongAlbumArtist_Title = FindViewById(Ids.SongAlbumArtist_Title) as AppCompatTextView;
			SongGenre_Title = FindViewById(Ids.SongGenre_Title) as AppCompatTextView;
			SongReleaseDate_Title = FindViewById(Ids.SongReleaseDate_Title) as AppCompatTextView;
			SongCopyright_Title = FindViewById(Ids.SongCopyright_Title) as AppCompatTextView;
			SongTrackNumber_Title = FindViewById(Ids.SongTrackNumber_Title) as AppCompatTextView;
			SongDiscNumber_Title = FindViewById(Ids.SongDiscNumber_Title) as AppCompatTextView;
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
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

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatButton? SongLyrics { get; protected set; }
		public AppCompatImageView? SongArtwork { get; protected set; }
		public AppCompatEditText? SongTitle { get; protected set; }
		public AppCompatEditText? SongArtist { get; protected set; }
		public AppCompatEditText? SongAlbum { get; protected set; }
		public AppCompatEditText? SongAlbumArtist { get; protected set; }
		public AppCompatEditText? SongGenre { get; protected set; }
		public AppCompatEditDate? SongReleaseDate { get; protected set; }
		public AppCompatEditText? SongCopyright { get; protected set; }
		public AppCompatEditText? SongTrackNumber { get; protected set; }
		public AppCompatEditText? SongDiscNumber { get; protected set; }		   
		public AppCompatTextView? SongTitle_Title { get; protected set; }
		public AppCompatTextView? SongArtist_Title { get; protected set; }
		public AppCompatTextView? SongAlbum_Title { get; protected set; }
		public AppCompatTextView? SongAlbumArtist_Title { get; protected set; }
		public AppCompatTextView? SongGenre_Title { get; protected set; }
		public AppCompatTextView? SongReleaseDate_Title { get; protected set; }
		public AppCompatTextView? SongCopyright_Title { get; protected set; }
		public AppCompatTextView? SongTrackNumber_Title { get; protected set; }
		public AppCompatTextView? SongDiscNumber_Title { get; protected set; }

		public Action<InfoEditSongView, object, EventArgs>? SongArtworkClick { get; set; }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (SongArtwork != null)
				SongArtwork.Click += OnSongArtworkClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			if (SongArtwork != null)
				SongArtwork.Click -= OnSongArtworkClick;
		}

		private void OnSongArtworkClick(object sender, EventArgs args)
		{
			SongArtworkClick?.Invoke(this, sender, args);
		}

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
					SongCopyright_Title?.SetTextColor(color);
					SongTrackNumber_Title?.SetTextColor(color);
					SongDiscNumber_Title?.SetTextColor(color);
				}
			}
		}
	}
}