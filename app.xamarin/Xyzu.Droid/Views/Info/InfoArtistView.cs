#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

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

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			ArtistImage = FindViewById(Ids.ArtistImage) as AppCompatImageView;
			ArtistName = FindViewById(Ids.ArtistName_Value) as AppCompatTextView;
			ArtistSongCount = FindViewById(Ids.ArtistSongCount_Value) as AppCompatTextView;
			ArtistAlbumCount = FindViewById(Ids.ArtistAlbumCount_Value) as AppCompatTextView;		 
			ArtistName_Title = FindViewById(Ids.ArtistName_Title) as AppCompatTextView;
			ArtistSongCount_Title = FindViewById(Ids.ArtistSongCount_Title) as AppCompatTextView;
			ArtistAlbumCount_Title = FindViewById(Ids.ArtistAlbumCount_Title) as AppCompatTextView;
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
			get => _Artist;
			set
			{
				_Artist = value;
				
				ArtistName?.SetText(_Artist?.Name, null);
				ArtistSongCount?.SetText(_Artist?.SongIds.Count().ToString(), null);
				ArtistAlbumCount?.SetText(_Artist?.AlbumIds.Count().ToString(), null);

				ReloadImage();
			}
		}

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatImageView? ArtistImage { get; protected set; }
		public AppCompatTextView? ArtistName { get; protected set; }
		public AppCompatTextView? ArtistSongCount { get; protected set; }
		public AppCompatTextView? ArtistAlbumCount { get; protected set; }			 
		public AppCompatTextView? ArtistName_Title { get; protected set; }
		public AppCompatTextView? ArtistSongCount_Title { get; protected set; }
		public AppCompatTextView? ArtistAlbumCount_Title { get; protected set; }

		public async void ReloadImage()
		{
			if (Images != null)
			{
				await Images.SetToImageView(IImages.DefaultOperations.CirculariseDownsample, ArtistImage, null, default, Artist);

				if (Context != null && Images.GetPalette(Artist)?.GetColorForBackground(Context, Resource.Color.ColorSurface) is Color color)
				{
					Title?.SetTextColor(color);

					ArtistName_Title?.SetTextColor(color);
					ArtistSongCount_Title?.SetTextColor(color);
					ArtistAlbumCount_Title?.SetTextColor(color);
				}
			}
		}
	}
}