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
using static Xyzu.Menus.LibraryItem;

namespace Xyzu.Views.Info
{
	public class InfoArtistView : InfoView, IInfoArtist
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_info_artist;

			public const int Title = Resource.Id.xyzu_view_info_artist_title_appcompattextview;
			public const int ArtistImage = Resource.Id.xyzu_view_info_artist_artistimage_appcompatimageview;

			public const int ArtistName_Title = Resource.Id.xyzu_view_info_artist_artistname_title_appcompattextview;
			public const int ArtistName_Value = Resource.Id.xyzu_view_info_artist_artistname_value_appcompattextview;
			public const int ArtistSongCount_Title = Resource.Id.xyzu_view_info_artist_artistsongcount_title_appcompattextview;
			public const int ArtistSongCount_Value = Resource.Id.xyzu_view_info_artist_artistsongcount_value_appcompattextview;
			public const int ArtistAlbumCount_Title = Resource.Id.xyzu_view_info_artist_artistalbumcount_title_appcompattextview;
			public const int ArtistAlbumCount_Value = Resource.Id.xyzu_view_info_artist_artistalbumcount_value_appcompattextview;
		}

		public InfoArtistView(Context context) : base(context) { }
		public InfoArtistView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoArtistView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.ArtistName_Title),
				FindViewById<AppCompatTextView>(Ids.ArtistSongCount_Title),
				FindViewById<AppCompatTextView>(Ids.ArtistAlbumCount_Title));

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Artist):
					ArtistName?.SetText(_Artist?.Name, null);
					ArtistSongCount?.SetText(_Artist?.SongIds.Count().ToString(), null);
					ArtistAlbumCount?.SetText(_Artist?.AlbumIds.Count().ToString(), null);
					ReloadImage();
					break;

				default: break;
			}
		}

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Artist)
			{
				ImageView = ArtistImage,
				Operations = IImages.DefaultOperations.CirculariseDownsample,
				OnPalette = palette => Palette = palette
			});
		}

		private IArtist? _Artist;
		private AppCompatImageView? _ArtistImage;
		private AppCompatTextView? _ArtistName;
		private AppCompatTextView? _ArtistSongCount;
		private AppCompatTextView? _ArtistAlbumCount;


		public IArtist? Artist
		{
			get => _Artist;
			set
			{
				_Artist = value;
				
				OnPropertyChanged();
			}
		}
		public AppCompatImageView ArtistImage
		{
			get => _ArtistImage
				??= FindViewById<AppCompatImageView>(Ids.ArtistImage) ??
				throw new InflateException("ArtistImage");
		}
		public AppCompatTextView ArtistName
		{
			get => _ArtistName
				??= FindViewById<AppCompatTextView>(Ids.ArtistName_Title) ??
				throw new InflateException("ArtistName");
		}
		public AppCompatTextView ArtistSongCount
		{
			get => _ArtistSongCount
				??= FindViewById<AppCompatTextView>(Ids.ArtistSongCount_Title) ??
				throw new InflateException("ArtistSongCount");
		}
		public AppCompatTextView ArtistAlbumCount
		{
			get => _ArtistAlbumCount
				??= FindViewById<AppCompatTextView>(Ids.ArtistAlbumCount_Title) ??
				throw new InflateException("ArtistAlbumCount");
		}
	}
}