#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.LibraryItem.Header
{
	public partial class HeaderArtistView : HeaderView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_header_artist;

			public const int Artwork_AppCompatImageView = Resource.Id.xyzu_view_libraryitem_header_artist_artwork_appcompatimageview;
			public const int LineOne_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_artist_lineone_appcompattextview;
			public const int LineTwo_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_artist_linetwo_appcompattextview;
			public const int LineThree_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_artist_linethree_appcompattextview;
			public const int LineFour_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_artist_linefour_appcompattextview;
			public const int ArtistItems_LibraryHeaderLibraryItemsView = Resource.Id.xyzu_view_libraryitem_header_artist_artistitems_headerlibraryitemsview;
		}

		public HeaderArtistView(Context context) : base(context) { }
		public HeaderArtistView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public HeaderArtistView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			ImagesOperations = IImages.DefaultOperations.Circularise;

			Artwork = FindViewById<AppCompatImageView>(Ids.Artwork_AppCompatImageView);
			LineOne = FindViewById<AppCompatTextView>(Ids.LineOne_AppCompatTextView);
			LineTwo = FindViewById<AppCompatTextView>(Ids.LineTwo_AppCompatTextView);
			LineThree = FindViewById<AppCompatTextView>(Ids.LineThree_AppCompatTextView);
			LineFour = FindViewById<AppCompatTextView>(Ids.LineFour_AppCompatTextView);
		}

		private IArtist? _Artist;
		private HeaderLibraryItemsView? _ArtistItems;

		public IArtist? Artist
		{
			get => _Artist;
			set
			{
				_Artist = value;

				SetArtist(_Artist, Defaults.Artist(Context));
			}
		}

		public HeaderLibraryItemsView ArtistItems
		{
			get => _ArtistItems ??= FindViewById(Ids.ArtistItems_LibraryHeaderLibraryItemsView) as HeaderLibraryItemsView ?? throw new InflateException();
		}

		public Action<object, EventArgs>? OnClickOptions
		{
			get => ArtistItems.OptionsClick;
			set => ArtistItems.OptionsClick = value;
		}
		public Action<object, EventArgs>? OnClickPlay
		{
			get => ArtistItems.PlayClick;
			set => ArtistItems.PlayClick = value;
		}

		protected void SetArtist(IArtist? artist, IArtist? defaults = null)
		{
			string? lineone = artist?.Name ?? defaults?.Name;
			
			if (LineOne != null)
			{
				LineOne.Visibility = ViewStates.Visible;
				LineOne.SetText(lineone, null);
			}

			SetArtwork(artist);

			if (Images != null)
				Images.SetToViewBackground(IImagesDroid.DefaultOperations.BlurDownsample, this, null, default, artist);
		}
	}
}