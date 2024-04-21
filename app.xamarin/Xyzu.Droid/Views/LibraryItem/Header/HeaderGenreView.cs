#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.LibraryItem.Header
{
	public partial class HeaderGenreView : HeaderView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_header_genre;

			public const int Artwork_AppCompatImageView = Resource.Id.xyzu_view_libraryitem_header_genre_artwork_appcompatimageview;
			public const int LineOne_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_genre_lineone_appcompattextview;
			public const int LineTwo_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_genre_linetwo_appcompattextview;
			public const int LineThree_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_genre_linethree_appcompattextview;
			public const int LineFour_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_genre_linefour_appcompattextview;
			public const int GenreSongs_LibraryHeaderLibraryItemsView = Resource.Id.xyzu_view_libraryitem_header_genre_genresongs_headerlibraryitemsview;
		}

		public HeaderGenreView(Context context) : base(context) { }
		public HeaderGenreView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public HeaderGenreView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			Artwork = FindViewById<AppCompatImageView>(Ids.Artwork_AppCompatImageView);
			LineOne = FindViewById<AppCompatTextView>(Ids.LineOne_AppCompatTextView);
			LineTwo = FindViewById<AppCompatTextView>(Ids.LineTwo_AppCompatTextView);
			LineThree = FindViewById<AppCompatTextView>(Ids.LineThree_AppCompatTextView);
			LineFour = FindViewById<AppCompatTextView>(Ids.LineFour_AppCompatTextView);
		}

		private IGenre? _Genre;
		private HeaderLibraryItemsView? _GenreSongs;

		public IGenre? Genre
		{
			get => _Genre;
			set
			{
				_Genre = value;
				
				SetGenre(_Genre, Defaults.Genre(Context));
			}
		}
		public HeaderLibraryItemsView GenreSongs
		{
			get => _GenreSongs ??= FindViewById(Ids.GenreSongs_LibraryHeaderLibraryItemsView) as HeaderLibraryItemsView ?? throw new InflateException();
		}

		public Action<object, EventArgs>? OnClickOptions
		{
			get => GenreSongs.OptionsClick;
			set => GenreSongs.OptionsClick = value;
		}
		public Action<object, EventArgs>? OnClickPlay
		{
			get => GenreSongs.PlayClick;
			set => GenreSongs.PlayClick = value;
		}

		public void SetGenre(IGenre? genre, IGenre? defaults = null)
		{
			string? lineone = genre?.Name ?? defaults?.Name;
			string? linetwo = genre?.Duration.ToMicrowaveFormat() ?? defaults?.Duration.ToMicrowaveFormat();

			if (LineOne != null)
			{
				LineOne.Visibility = ViewStates.Visible;
				LineOne.SetText(lineone, null);
			}
			if (LineTwo != null)
			{
				LineTwo.Visibility = ViewStates.Visible;
				LineTwo.SetText(linetwo, null);
			}

			SetArtwork(genre);

			if (Images != null)
				Images.SetToViewBackground(IImagesDroid.DefaultOperations.BlurDownsample, this, null, default, genre);
		}
	}
}