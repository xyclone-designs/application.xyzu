#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Runtime.CompilerServices;

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
			base.Init(context, attrs);

			Inflate(Context, Ids.Layout, this);

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			ArtistImage = FindViewById(Ids.ArtistImage) as AppCompatImageView;
			ArtistName = FindViewById(Ids.ArtistName) as AppCompatEditText;
			ArtistName_Title = FindViewById(Ids.ArtistName) as AppCompatTextView;
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Images):
					ReloadImage();
					break;

				default: break;
			}
		}


		private IArtist? _Artist;

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

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatImageView? ArtistImage { get; protected set; }
		public AppCompatEditText? ArtistName { get; protected set; }
		public AppCompatTextView? ArtistName_Title { get; protected set; }

		public Action<InfoEditArtistView, object, EventArgs>? ArtistImageClick { get; set; }

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
				ArtistImage.Click -= OnArtistImageClick;
		}

		protected void OnArtistImageClick(object sender, EventArgs args)
		{
			ArtistImageClick?.Invoke(this, sender, args);
		}

		public async void ReloadImage()
		{
			if (Images != null)
			{
				await Images.SetToImageView(IImages.DefaultOperations.CirculariseDownsample, ArtistImage, null, default, Artist);

				if (Context != null && Images.GetPalette(Artist)?.GetColorForBackground(Context, Resource.Color.ColorSurface) is Color color)
				{
					Title?.SetTextColor(color);

					ArtistName_Title?.SetTextColor(color);
				}
			}
		}
	}
}