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
	public class InfoGenreView : InfoView, IInfoGenre
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_info_genre;

			public const int Title = Resource.Id.xyzu_view_info_genre_title_appcompattextview;

			public const int GenreName_Title = Resource.Id.xyzu_view_info_genre_genrename_title_appcompattextview; 
			public const int GenreName_Value = Resource.Id.xyzu_view_info_genre_genrename_value_appcompattextview;
			public const int GenreSongCount_Title = Resource.Id.xyzu_view_info_genre_genresongcount_title_appcompattextview; 
			public const int GenreSongCount_Value = Resource.Id.xyzu_view_info_genre_genresongcount_value_appcompattextview;
		}

		public InfoGenreView(Context context) : base(context) { }
		public InfoGenreView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoGenreView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			GenreName = FindViewById(Ids.GenreName_Value) as AppCompatTextView;
			GenreSongCount = FindViewById(Ids.GenreSongCount_Value) as AppCompatTextView;	 
			GenreName_Title = FindViewById(Ids.GenreName_Title) as AppCompatTextView;
			GenreSongCount_Title = FindViewById(Ids.GenreSongCount_Title) as AppCompatTextView;
		}

		private IGenre? _Genre;

		public IGenre? Genre
		{
			get => _Genre;
			set
			{
				_Genre = value;

				GenreName?.SetText(_Genre?.Name, null);
				GenreSongCount?.SetText(_Genre?.SongIds.Count().ToString(), null);
			}
		}

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatTextView? GenreName { get; protected set; }
		public AppCompatTextView? GenreSongCount { get; protected set; }			
		public AppCompatTextView? GenreName_Title { get; protected set; }
		public AppCompatTextView? GenreSongCount_Title { get; protected set; }
	}
}