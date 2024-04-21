#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.AppCompat.Widget;

using Xyzu.Droid;
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

			base.Init(context, attrs);

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			SongLyrics = FindViewById(Ids.SongLyrics) as AppCompatTextView;
		}

		private ISong? _Song;

		public ISong? Song
		{
			get => _Song;
			set
			{
				_Song = value;

				SongLyrics?.SetText(_Song?.Lyrics ?? Context?.Resources?.GetString(Resource.String.library_no_lyrics), null);
			}
		}

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatTextView? SongLyrics { get; protected set; }
	}
}