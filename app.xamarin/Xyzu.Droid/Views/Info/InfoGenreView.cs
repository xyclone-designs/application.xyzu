using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
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
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.GenreName_Title),
				FindViewById<AppCompatTextView>(Ids.GenreSongCount_Title));

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Genre):
					GenreName?.SetText(_Genre?.Name, null);
					GenreSongCount?.SetText(_Genre?.SongIds.Count().ToString(), null);
					ReloadImage();
					break;

				default: break;
			}
		}

		private IGenre? _Genre;
		private AppCompatTextView? _GenreName;
		private AppCompatTextView? _GenreSongCount;


		public IGenre? Genre
		{
			get => _Genre;
			set
			{
				_Genre = value;

				OnPropertyChanged();
			}
		}
		public AppCompatTextView GenreName
		{
			get => _GenreName
				??= FindViewById<AppCompatTextView>(Ids.GenreName_Title) ??
				throw new InflateException("GenreName");
		}
		public AppCompatTextView GenreSongCount
		{
			get => _GenreSongCount
				??= FindViewById<AppCompatTextView>(Ids.GenreSongCount_Title) ??
				throw new InflateException("GenreSongCount");
		}
	}
}