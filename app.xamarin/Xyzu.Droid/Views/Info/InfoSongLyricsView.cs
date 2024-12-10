using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public class InfoSongLyricsView : InfoView, IInfoSongLyrics
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_info_song_lyrics;

			public const int Title = Resource.Id.xyzu_view_info_song_lyrics_title_appcompattextview;
			public const int SongTitle = Resource.Id.xyzu_view_info_song_lyrics_songtitle_appcompattextview;
			public const int SongArtwork = Resource.Id.xyzu_view_info_song_lyrics_songartwork_appcompatimageview;
			public const int SongLyrics = Resource.Id.xyzu_view_info_song_lyrics_songlyrics_appcompattextview;
		}

		public InfoSongLyricsView(Context context) : base(context) { }
		public InfoSongLyricsView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoSongLyricsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(FindViewById<AppCompatTextView>(Ids.Title), SongTitle);

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Song):
					ReloadImage();
					SongLyrics.SetText(Song?.Lyrics ?? Context?.Resources?.GetString(Resource.String.library_no_lyrics), null);
					SongTitle.SetText(
						type: null,
						text: Song is null ? string.Empty : string.Format(
							"{0} - {1}",
							Song.Title ?? Context?.Resources?.GetString(Resource.String.library_unknown_title),
							Song.Artist ?? Context?.Resources?.GetString(Resource.String.library_unknown_artist)));
					break;

				default: break;
			}
		}

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Song)
			{
				ImageView = SongArtwork,
				Operations = IImagesDroid.DefaultOperations.RoundedDownsample,
				OnPalette = palette => Palette = palette
			});
		}

		private ISong? _Song;
		private AppCompatTextView? _SongTitle;
		private AppCompatImageView? _SongArtwork;
		private AppCompatTextView? _SongLyrics;

		public ISong? Song
		{
			get => _Song;
			set
			{
				_Song = value;

				OnPropertyChanged();
			}
		}
		public AppCompatTextView SongTitle
		{
			get => _SongTitle ??= 
				FindViewById(Ids.SongTitle) as AppCompatTextView ?? 
				throw new InflateException("SongTitle");
		}
		public AppCompatImageView SongArtwork
		{
			get => _SongArtwork ??=
				FindViewById(Ids.SongArtwork) as AppCompatImageView ??
				throw new InflateException("SongArtwork");
		}
		public AppCompatTextView SongLyrics
		{
			get => _SongLyrics ??=
				FindViewById(Ids.SongLyrics) as AppCompatTextView ??
				throw new InflateException("SongLyrics");
		}
	}
}