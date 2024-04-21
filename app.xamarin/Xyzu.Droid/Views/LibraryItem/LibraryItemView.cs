#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Images.Enums;
using Xyzu.Library;
using Xyzu.Library.Models;

namespace Xyzu.Views.LibraryItem
{
	public partial class LibraryItemView : ConstraintLayout, ILibraryItem
	{
		public LibraryItemView(Context context) : this(context, null!)
		{ }
		public LibraryItemView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_LibraryItem)
		{ }
		public LibraryItemView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_LibraryItem)
		{ }
		public LibraryItemView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs); 
		}

		protected virtual void Init(Context context, IAttributeSet? attrs)
		{ }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null) 
		{
			switch (propertyname)
			{
				case nameof(Details):

					string? lineone = Details?.ElementAtOrDefault(0);
					string? linetwo = Details?.ElementAtOrDefault(1);
					string? linethree = Details?.ElementAtOrDefault(2);
					string? linefour = Details?.ElementAtOrDefault(3);

					LineOne?.SetText(lineone, null);
					LineTwo?.SetText(linetwo, null);
					LineThree?.SetText(linethree, null);
					LineFour?.SetText(linefour, null);

					break;

				default: break;
			}
		}

		private bool _Playing;
		private bool _Selected;
		private string? _LibraryItemId;
		private IEnumerable<string>? _Details;

		public IImages? Images { get; set; }
		public ILibrary? Library { get; set; }
		public Operations[]? ImagesOperations { get; set; }

		public AppCompatImageView? Artwork { get; protected set; }
		public AppCompatImageView? Equaliser { get; protected set; }
		public AppCompatTextView? LineOne { get; protected set; }
		public AppCompatTextView? LineTwo { get; protected set; }
		public AppCompatTextView? LineThree { get; protected set; }
		public AppCompatTextView? LineFour { get; protected set; }

		public override bool Selected 
		{ 
			get => _Selected; 
			set => _Selected = base.Selected = value;
		}

		public bool Playing
		{
			get => _Playing;
			set
			{
				_Playing = value;

				OnPropertyChanged();
			}
		}
		public string? LibraryItemId
		{
			get => _LibraryItemId;
			set
			{
				_LibraryItemId = value;

				OnPropertyChanged();
			}
		}
		public IEnumerable<string>? Details
		{
			get => _Details;
			set
			{
				_Details = value;

				OnPropertyChanged();
			}
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			UpdateState();
		}

		public virtual void Reset()
		{
			LibraryItemId = null;
			Playing = false;
			Selected = false;
			Details = null;

			Artwork?.SetImageDrawable(null);
			LineOne?.SetText(null as string, null);
			LineTwo?.SetText(null as string, null);
			LineThree?.SetText(null as string, null);
			LineFour?.SetText(null as string, null);
		}
		public virtual void UpdateState()
		{			
			if (Equaliser != null)
				Equaliser.Visibility = Playing
					? ViewStates.Visible
					: ViewStates.Gone;
		}

		public virtual async void SetArtwork(IImage? image)
		{
			if (Images != null)
				await Images.SetToImageView(ImagesOperations ?? IImages.DefaultOperations.RoundedDownsample, Artwork, null, default, image);
		}									
		public virtual async void SetArtwork(IModel? model)
		{
			if (Images is null)
				return;

			ILibrary.IIdentifiers? iidentifiers = true switch
			{
				true when model is IGenre genre => ILibrary.IIdentifiers.FromGenre(genre),
				true when model is IPlaylist playlist => ILibrary.IIdentifiers.FromPlaylist(playlist),

				_ => null,
			};

			if (iidentifiers is null)
				await Images.SetToImageView(ImagesOperations ?? IImages.DefaultOperations.RoundedDownsample, Artwork, null, default, model);
			else if (Library != null && Bitmap.Config.Argb8888 != null)
			{
				int index = 0;
				Bitmap[] bitmaps = new Bitmap[4];

				await foreach (ISong song in Library.Songs.GetSongs(
					identifiers: iidentifiers,
					cancellationToken: default,
					retriever: new ISong.Default<bool>(false)
					{
						Artwork = new IImage.Default<bool>(true),

					})) if ((Images.GetBitmap(IImages.DefaultOperations.Downsample, null, song) ?? BitmapFactory.DecodeResource(Context?.Resources, Resource.Drawable.icon_xyzu)) is Bitmap songbitmap)
					{
						bitmaps[index] = songbitmap;

						if (index == 3)
							break;

						index++;
					}

				if (await Task.Run(() => Images.MergeBitmaps(bitmaps[0], bitmaps[1], bitmaps[2], bitmaps[3])) is Bitmap bitmap)
					await Images.SetToImageView(ImagesOperations ?? IImages.DefaultOperations.RoundedDownsample, Artwork, null, default, bitmap);
			}
		}
	}
}