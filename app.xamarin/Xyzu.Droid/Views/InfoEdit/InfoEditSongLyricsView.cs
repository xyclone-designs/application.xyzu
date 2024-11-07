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
	public class InfoEditSongLyricsView : InfoEditView, IInfoEditSongLyrics
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_infoedit_song_lyrics;

			public const int SongLyrics = Resource.Id.xyzu_view_infoedit_song_lyrics_songlyrics_value_appcompatedittext;
		}

		public InfoEditSongLyricsView(Context context) : base(context) { }
		public InfoEditSongLyricsView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoEditSongLyricsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			//SetPaletteTextViews(FindViewById<AppCompatTextView>(Ids.Title));

			base.Init(context, attrs);
		}

		private ISong? _Song;
		private AppCompatTextView? _SongLyrics;

		public ISong? Song
		{
			get
			{
				if (_Song != null)
				{
					_Song.Lyrics = SongLyrics?.Text;
				}

				return _Song;
			}
			set
			{
				_Song = value;

				SongLyrics?.SetText(_Song?.Lyrics, null);
			}
		}

		public AppCompatTextView SongLyrics
		{
			get => _SongLyrics ??=
				FindViewById(Ids.SongLyrics) as AppCompatTextView ??
				throw new InflateException("SongLyrics");
		}

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.Operate(new IImagesDroid.Parameters(Song)
			{
				Operations = IImages.DefaultOperations.Downsample,
				OnPalette = palette => Palette = palette
			});
		}
	}
}