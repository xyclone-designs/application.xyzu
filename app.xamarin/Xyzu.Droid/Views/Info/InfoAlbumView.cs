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
	public class InfoAlbumView : InfoView, IInfoAlbum
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_info_album;

			public const int Title = Resource.Id.xyzu_view_info_album_title_appcompattextview;
			public const int AlbumArtwork = Resource.Id.xyzu_view_info_album_albumartwork_appcompatimageview;

			public const int AlbumTitle_Title = Resource.Id.xyzu_view_info_album_albumtitle_title_appcompattextview; 
			public const int AlbumTitle_Value = Resource.Id.xyzu_view_info_album_albumtitle_value_appcompattextview;
			public const int AlbumArtist_Title = Resource.Id.xyzu_view_info_album_albumartist_title_appcompattextview; 
			public const int AlbumArtist_Value = Resource.Id.xyzu_view_info_album_albumartist_value_appcompattextview;
			public const int AlbumReleaseDate_Title = Resource.Id.xyzu_view_info_album_albumreleasedate_title_appcompattextview; 
			public const int AlbumReleaseDate_Value = Resource.Id.xyzu_view_info_album_albumreleasedate_value_appcompattextview;
			public const int AlbumDuration_Title = Resource.Id.xyzu_view_info_album_albumduration_title_appcompattextview; 
			public const int AlbumDuration_Value = Resource.Id.xyzu_view_info_album_albumduration_value_appcompattextview;
			public const int AlbumSongCount_Title = Resource.Id.xyzu_view_info_album_albumsongcount_title_appcompattextview; 
			public const int AlbumSongCount_Value = Resource.Id.xyzu_view_info_album_albumsongcount_value_appcompattextview;
			public const int AlbumDiscCount_Title = Resource.Id.xyzu_view_info_album_albumdisccount_title_appcompattextview; 
			public const int AlbumDiscCount_Value = Resource.Id.xyzu_view_info_album_albumdisccount_value_appcompattextview;
		}

		public InfoAlbumView(Context context) : base(context) { }
		public InfoAlbumView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoAlbumView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.AlbumTitle_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumArtist_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumReleaseDate_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumDuration_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumSongCount_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumDiscCount_Title));

			base.Init(context, attrs);
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Album):
					AlbumTitle?.SetText(_Album?.Title, null);
					AlbumArtist?.SetText(_Album?.Artist, null);
					AlbumReleaseDate?.SetText(_Album?.ReleaseDate?.ToString("dd/MM/yyyy"), null);
					AlbumDuration?.SetText(_Album?.Duration.ToMicrowaveFormat(), null);
					AlbumSongCount?.SetText(_Album?.SongIds.Count().ToString(), null);
					AlbumDiscCount?.SetText(_Album?.DiscCount?.ToString(), null);
					ReloadImage();
					break;

				default: break;
			}
		}

		private IAlbum? _Album;
		private AppCompatImageView? _AlbumArtwork;
		private AppCompatTextView? _AlbumTitle;
		private AppCompatTextView? _AlbumArtist;
		private AppCompatTextView? _AlbumReleaseDate;
		private AppCompatTextView? _AlbumDuration;
		private AppCompatTextView? _AlbumSongCount;
		private AppCompatTextView? _AlbumDiscCount;


		public IAlbum? Album
		{
			get => _Album;
			set
			{
				_Album = value;

				OnPropertyChanged();
			}
		}
		public AppCompatImageView AlbumArtwork
		{
			get => _AlbumArtwork
				??= FindViewById<AppCompatImageView>(Ids.AlbumArtwork) ??
				throw new InflateException("AlbumTitle_Value");
		}
		public AppCompatTextView AlbumTitle
		{
			get => _AlbumTitle
				??= FindViewById<AppCompatTextView>(Ids.AlbumTitle_Value) as AppCompatTextView ??
				throw new InflateException("AlbumTitle_Value");
		}
		public AppCompatTextView AlbumArtist
		{
			get => _AlbumArtist
				??= FindViewById<AppCompatTextView>(Ids.AlbumArtist_Value) as AppCompatTextView ??
				throw new InflateException("AlbumArtist_Value");
		}
		public AppCompatTextView AlbumReleaseDate
		{
			get => _AlbumReleaseDate
				??= FindViewById<AppCompatTextView>(Ids.AlbumReleaseDate_Value) as AppCompatTextView ??
				throw new InflateException("AlbumReleaseDate_Value");
		}
		public AppCompatTextView AlbumDuration
		{
			get => _AlbumDuration
				??= FindViewById<AppCompatTextView>(Ids.AlbumDuration_Value) as AppCompatTextView ??
				throw new InflateException("AlbumDuration_Value");
		}
		public AppCompatTextView AlbumSongCount
		{
			get => _AlbumSongCount
				??= FindViewById<AppCompatTextView>(Ids.AlbumSongCount_Value) as AppCompatTextView ??
				throw new InflateException("AlbumSongCount_Value");
		}
		public AppCompatTextView AlbumDiscCount
		{
			get => _AlbumDiscCount
				??= FindViewById<AppCompatTextView>(Ids.AlbumDiscCount_Value) as AppCompatTextView ??
				throw new InflateException("AlbumDiscCount_Value");
		}

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Album)
			{
				ImageView = AlbumArtwork,
				Operations = IImages.DefaultOperations.RoundedDownsample,
				OnPalette = palette => Palette = palette
			});
		}
	}
}