#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
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
			base.Init(context, attrs);

			Inflate(Context, Ids.Layout, this);

			SongLyrics = FindViewById(Ids.SongLyrics) as AppCompatEditText;
		}

		private ISong? _Song;

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

		public AppCompatEditText? SongLyrics { get; protected set; }
	}
}