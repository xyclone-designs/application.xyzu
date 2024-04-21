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
	public class HeaderAlbumView : HeaderView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_header_album;

			public const int Artwork_AppCompatImageView = Resource.Id.xyzu_view_libraryitem_header_album_artwork_appcompatimageview;
			public const int LineOne_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_album_lineone_appcompattextview;
			public const int LineTwo_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_album_linetwo_appcompattextview;
			public const int LineThree_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_album_linethree_appcompattextview;
			public const int LineFour_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_album_linefour_appcompattextview;
			public const int AlbumSongs_LibraryHeaderLibraryItemsView = Resource.Id.xyzu_view_libraryitem_header_album_albumsongs_headerlibraryitemsview;
		}

		public HeaderAlbumView(Context context) : base(context) { }
		public HeaderAlbumView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public HeaderAlbumView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

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

		private IAlbum? _Album;
		private HeaderLibraryItemsView? _AlbumSongs;

		public IAlbum? Album
		{
			get => _Album;
			set
			{
				_Album = value;

				SetAlbum(_Album, Defaults.Album(Context));
			}
		}
		public HeaderLibraryItemsView AlbumSongs
		{
			get => _AlbumSongs ??= FindViewById(Ids.AlbumSongs_LibraryHeaderLibraryItemsView) as HeaderLibraryItemsView ?? throw new InflateException();
		}

		public Action<object, EventArgs>? OnClickOptions
		{																																		 
			get => AlbumSongs.OptionsClick;
			set => AlbumSongs.OptionsClick = value;
		}
		public Action<object, EventArgs>? OnClickPlay
		{
			get => AlbumSongs.PlayClick;
			set => AlbumSongs.PlayClick = value;
		}

		protected void SetAlbum(IAlbum? album, IAlbum? defaults = null)
		{
			string? lineone = album?.Title ?? defaults?.Title;
			string? linetwo = album?.Artist ?? defaults?.Artist;
			string? linethree = (album?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? "00:00";

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
			if (LineThree != null)
			{
				LineThree.Visibility = ViewStates.Visible;
				LineThree.SetText(linethree, null);
			}

			SetArtwork(album);

			if (Images != null)
				Images.SetToViewBackground(IImagesDroid.DefaultOperations.BlurDownsample, this, null, default, album);
		}
	}
}