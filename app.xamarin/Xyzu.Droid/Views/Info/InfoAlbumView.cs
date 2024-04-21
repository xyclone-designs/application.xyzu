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

			Title = FindViewById(Ids.Title) as AppCompatTextView;
			AlbumArtwork = FindViewById(Ids.AlbumArtwork) as AppCompatImageView;

			AlbumTitle = FindViewById(Ids.AlbumTitle_Value) as AppCompatTextView;
			AlbumArtist = FindViewById(Ids.AlbumArtist_Value) as AppCompatTextView;
			AlbumReleaseDate = FindViewById(Ids.AlbumReleaseDate_Value) as AppCompatTextView;
			AlbumDuration = FindViewById(Ids.AlbumDuration_Value) as AppCompatTextView;
			AlbumSongCount = FindViewById(Ids.AlbumSongCount_Value) as AppCompatTextView;
			AlbumDiscCount = FindViewById(Ids.AlbumDiscCount_Value) as AppCompatTextView;	   
			AlbumTitle_Title = FindViewById(Ids.AlbumTitle_Title) as AppCompatTextView;
			AlbumArtist_Title = FindViewById(Ids.AlbumArtist_Title) as AppCompatTextView;
			AlbumReleaseDate_Title = FindViewById(Ids.AlbumReleaseDate_Title) as AppCompatTextView;
			AlbumDuration_Title = FindViewById(Ids.AlbumDuration_Title) as AppCompatTextView;
			AlbumSongCount_Title = FindViewById(Ids.AlbumSongCount_Title) as AppCompatTextView;
			AlbumDiscCount_Title = FindViewById(Ids.AlbumDiscCount_Title) as AppCompatTextView;
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
			get => _Album;
			set
			{
				_Album = value;

				AlbumTitle?.SetText(_Album?.Title, null);
				AlbumArtist?.SetText(_Album?.Artist, null);
				AlbumReleaseDate?.SetText(_Album?.ReleaseDate?.ToString("dd/MM/yyyy"), null);
				AlbumDuration?.SetText(_Album?.Duration.ToMicrowaveFormat(), null);
				AlbumSongCount?.SetText(_Album?.SongIds.Count().ToString(), null);
				AlbumDiscCount?.SetText(_Album?.DiscCount?.ToString(), null);

				ReloadImage();
			}
		}

		public AppCompatTextView? Title { get; protected set; }
		public AppCompatImageView? AlbumArtwork { get; protected set; }
		public AppCompatTextView? AlbumTitle { get; protected set; }
		public AppCompatTextView? AlbumArtist { get; protected set; }
		public AppCompatTextView? AlbumReleaseDate { get; protected set; }
		public AppCompatTextView? AlbumDuration { get; protected set; }
		public AppCompatTextView? AlbumSongCount { get; protected set; }
		public AppCompatTextView? AlbumDiscCount { get; protected set; }

		public AppCompatTextView? AlbumTitle_Title { get; protected set; }
		public AppCompatTextView? AlbumArtist_Title { get; protected set; }
		public AppCompatTextView? AlbumReleaseDate_Title { get; protected set; }
		public AppCompatTextView? AlbumDuration_Title { get; protected set; }
		public AppCompatTextView? AlbumSongCount_Title { get; protected set; }
		public AppCompatTextView? AlbumDiscCount_Title { get; protected set; }

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
					AlbumDuration_Title?.SetTextColor(color);
					AlbumSongCount_Title?.SetTextColor(color);
					AlbumDiscCount_Title?.SetTextColor(color);
				}
			}
		}
	}
}