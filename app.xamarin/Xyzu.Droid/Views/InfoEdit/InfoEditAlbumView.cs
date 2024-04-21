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
			base.Init(context, attrs);

			Inflate(Context, Ids.Layout, this);

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			AlbumArtwork = FindViewById(Ids.AlbumArtwork) as AppCompatImageView;
			AlbumTitle = FindViewById(Ids.AlbumTitle_Value) as AppCompatEditText;
			AlbumArtist = FindViewById(Ids.AlbumArtist_Value) as AppCompatEditText;
			AlbumReleaseDate = FindViewById(Ids.AlbumReleaseDate_Value) as AppCompatEditDate;			  
			AlbumTitle_Title = FindViewById(Ids.AlbumTitle_Title) as AppCompatTextView;
			AlbumArtist_Title = FindViewById(Ids.AlbumArtist_Title) as AppCompatTextView;
			AlbumReleaseDate_Title = FindViewById(Ids.AlbumReleaseDate_Title) as AppCompatTextView;
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


		private IAlbum? _Album;

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

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatImageView? AlbumArtwork { get; protected set; }
		public AppCompatEditText? AlbumTitle { get; protected set; }
		public AppCompatEditText? AlbumArtist { get; protected set; }
		public AppCompatEditDate? AlbumReleaseDate { get; protected set; }									  
		public AppCompatTextView? AlbumTitle_Title { get; protected set; }
		public AppCompatTextView? AlbumArtist_Title { get; protected set; }
		public AppCompatTextView? AlbumReleaseDate_Title { get; protected set; }
		public Action<InfoEditAlbumView, object, EventArgs>? AlbumArtworkClick { get; set; }

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

		protected void OnAlbumArtworkClick(object sender, EventArgs args)
		{
			AlbumArtworkClick?.Invoke(this, sender, args);
		}

		public async void ReloadImage()
		{
			if (Images != null)
			{
				await Images.SetToImageView(IImages.DefaultOperations.RoundedDownsample, AlbumArtwork, null, default, Album);

				if (Context != null && Images.GetPalette(Album)?.GetColorForBackground(Context, Resource.Color.ColorSurface) is Color color)
				{
					Title?.SetTextColor(color);

					AlbumTitle_Title?.SetTextColor(color);
					AlbumArtist_Title?.SetTextColor(color);
					AlbumReleaseDate_Title?.SetTextColor(color);
				}
			}
		}
	}
}