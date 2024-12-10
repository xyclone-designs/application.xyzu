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
	public class InfoEditAlbumView : InfoEditView, IInfoEditAlbum
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_infoedit_album;

			public const int Title = Resource.Id.xyzu_view_infoedit_album_title_appcompattextview;
			public const int AlbumArtwork = Resource.Id.xyzu_view_infoedit_album_albumartwork_appcompatimageview;
			public const int AlbumTitle_Value = Resource.Id.xyzu_view_infoedit_album_albumtitle_value_appcompatedittext; 
			public const int AlbumTitle_Title = Resource.Id.xyzu_view_infoedit_album_albumtitle_title_appcompattextview;
			public const int AlbumArtist_Value = Resource.Id.xyzu_view_infoedit_album_albumartist_value_appcompatedittext; 
			public const int AlbumArtist_Title = Resource.Id.xyzu_view_infoedit_album_albumartist_title_appcompattextview;
			public const int AlbumReleaseDate_Value = Resource.Id.xyzu_view_infoedit_album_albumreleasedate_value_appcompateditdate; 
			public const int AlbumReleaseDate_Title = Resource.Id.xyzu_view_infoedit_album_albumreleasedate_title_appcompattextview;
		}

		public InfoEditAlbumView(Context context) : base(context) { }
		public InfoEditAlbumView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoEditAlbumView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.AlbumTitle_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumArtist_Title),
				FindViewById<AppCompatTextView>(Ids.AlbumReleaseDate_Title));

			base.Init(context, attrs);
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (AlbumArtwork != null)
				AlbumArtwork.Click += OnAlbumArtworkClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			if (AlbumArtwork != null)
				AlbumArtwork.Click += OnAlbumArtworkClick;
		}

		private IAlbum? _Album;
		private AppCompatImageView? _AlbumArtwork;
		private AppCompatEditText? _AlbumTitle;
		private AppCompatEditText? _AlbumArtist;
		private AppCompatEditDate? _AlbumReleaseDate;


		public IAlbum? Album
		{
			get
			{
				if (_Album != null)
				{
					_Album.Title = AlbumTitle?.Text;
					if (_Album.Artist != null) _Album.Artist = AlbumArtist?.Text;
					_Album.ReleaseDate = AlbumReleaseDate?.Date;
				}

				return _Album;
			}
			set
			{
				_Album = value;

				AlbumTitle?.SetText(_Album?.Title, null);
				AlbumArtist?.SetText(_Album?.Artist, null);
				AlbumReleaseDate?.SetDate(_Album?.ReleaseDate);

				ReloadImage();
			}
		}
		public AppCompatImageView AlbumArtwork
		{
			get => _AlbumArtwork
				??= FindViewById<AppCompatImageView>(Ids.AlbumArtwork) ??
				throw new InflateException("AlbumArtwork");
		}
		public AppCompatEditText AlbumTitle
		{
			get => _AlbumTitle
				??= FindViewById<AppCompatEditText>(Ids.AlbumTitle_Value) ??
				throw new InflateException("AlbumTitle_Value");
		}
		public AppCompatEditText AlbumArtist
		{
			get => _AlbumArtist
				??= FindViewById<AppCompatEditText>(Ids.AlbumArtist_Value) ??
				throw new InflateException("AlbumArtist_Value");
		}
		public AppCompatEditDate AlbumReleaseDate
		{
			get => _AlbumReleaseDate
				??= FindViewById<AppCompatEditDate>(Ids.AlbumReleaseDate_Value) ??
				throw new InflateException("AlbumReleaseDate_Value");
		}

		public Action<InfoEditAlbumView, object?, EventArgs>? AlbumArtworkClick { get; set; }

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Album)
			{
				ImageView = AlbumArtwork,
				Operations = IImages.DefaultOperations.RoundedDownsample,
				OnPalette = palette => Palette = palette
			});
		}

		protected void OnAlbumArtworkClick(object? sender, EventArgs args)
		{
			AlbumArtworkClick?.Invoke(this, sender, args);
		}
	}
}