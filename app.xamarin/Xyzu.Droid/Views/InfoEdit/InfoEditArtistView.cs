using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public class InfoEditArtistView : InfoEditView, IInfoEditArtist
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_infoedit_artist;

			public const int Title = Resource.Id.xyzu_view_infoedit_artist_title_appcompattextview;
			public const int ArtistImage = Resource.Id.xyzu_view_infoedit_artist_artistimage_appcompatimageview;
			public const int ArtistName = Resource.Id.xyzu_view_infoedit_artist_artistname_value_appcompatedittext;
			public const int ArtistName_Title = Resource.Id.xyzu_view_infoedit_artist_artistname_title_appcompattextview;
		}

		public InfoEditArtistView(Context context) : base(context) { }
		public InfoEditArtistView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoEditArtistView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);
			SetPaletteTextViews(
				FindViewById<AppCompatTextView>(Ids.Title),
				FindViewById<AppCompatTextView>(Ids.ArtistName_Title));

			base.Init(context, attrs);
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (ArtistImage != null)
				ArtistImage.Click += OnArtistImageClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			if (ArtistImage != null)
				ArtistImage.Click += OnArtistImageClick;
		}

		private IArtist? _Artist;
		private AppCompatImageView? _ArtistImage;
		private AppCompatEditText? _ArtistName;

		public IArtist? Artist
		{
			get
			{
				if (_Artist != null)
				{
					_Artist.Name = ArtistName?.Text;
				}

				return _Artist;
			}
			set
			{
				_Artist = value;

				ArtistName?.SetText(_Artist?.Name, null);

				ReloadImage();
			}
		}
		public AppCompatImageView ArtistImage
		{
			get => _ArtistImage
				??= FindViewById<AppCompatImageView>(Ids.ArtistImage) ??
				throw new InflateException("ArtistName_Value");
		}
		public AppCompatEditText ArtistName
		{
			get => _ArtistName
				??= FindViewById<AppCompatEditText>(Ids.ArtistName) ??
				throw new InflateException("ArtistName_Value");
		}

		public Action<InfoEditArtistView, object?, EventArgs>? ArtistImageClick { get; set; }

		public override async void ReloadImage()
		{
			if (Images is not null) await Images.SetToImageView(new IImagesDroid.Parameters(Artist)
			{
				ImageView = ArtistImage,
				Operations = IImages.DefaultOperations.CirculariseDownsample,
				OnPalette = palette => Palette = palette
			});
		}

		protected void OnArtistImageClick(object? sender, EventArgs args)
		{
			ArtistImageClick?.Invoke(this, sender, args);
		}
	}
}