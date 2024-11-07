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
			public const int SongLyrics = Resource.Id.xyzu_view_info_song_lyrics_songlyrics_appcompattextview;
		}

		public InfoSongLyricsView(Context context) : base(context) { }
		public InfoSongLyricsView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoSongLyricsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(FindViewById<AppCompatTextView>(Ids.Title));

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Song):
					SongLyrics?.SetText(_Song?.Lyrics ?? Context?.Resources?.GetString(Resource.String.library_no_lyrics), null);
					ReloadImage();
					break;

				default: break;
			}
		}

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.Operate(new IImagesDroid.Parameters
			{
				Sources = new object?[] { _Song },
				OnPalette = palette => Palette = palette
			});
		}

		private ISong? _Song;
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
		public AppCompatTextView SongLyrics
		{
			get => _SongLyrics ??= 
				FindViewById(Ids.SongLyrics) as AppCompatTextView ?? 
				throw new InflateException("SongLyrics");
		}
	}
}