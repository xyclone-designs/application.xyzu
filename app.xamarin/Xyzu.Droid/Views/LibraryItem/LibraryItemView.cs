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

		private bool _IsCorrupt;
		private bool _IsPlaying;
		private bool _Selected;
		private string? _LibraryItemId;
		private IEnumerable<string>? _Details;

		public IImages? Images { get; set; }
		public ILibrary? Library { get; set; }
		public Operations[]? ImagesOperations { get; set; }

		public AppCompatImageView? Artwork { get; protected set; }
		public AppCompatImageView? Equaliser { get; protected set; }
		public AppCompatImageView? Corrupt { get; protected set; }
		public AppCompatTextView? LineOne { get; protected set; }
		public AppCompatTextView? LineTwo { get; protected set; }
		public AppCompatTextView? LineThree { get; protected set; }
		public AppCompatTextView? LineFour { get; protected set; }

		public override bool Selected 
		{ 
			get => _Selected; 
			set => _Selected = base.Selected = value;
		}

		public bool IsCorrupt
		{
			get => _IsCorrupt;
			set
			{
				_IsCorrupt = value;

				OnPropertyChanged();
			}
		}
		public bool IsPlaying
		{
			get => _IsPlaying;
			set
			{
				_IsPlaying = value;

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
			IsPlaying = false;
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
				Equaliser.Visibility = IsPlaying
					? ViewStates.Visible
					: ViewStates.Gone;

			if (Corrupt != null)
				Corrupt.Visibility = IsCorrupt
					? ViewStates.Visible
					: ViewStates.Gone;
		}

		public virtual async void SetArtwork(IImage? image)
		{
			if (Images != null)
				await Images.SetToImageView(new IImagesDroid.Parameters(image)
				{
					ImageView = Artwork,
					Operations = IImages.DefaultOperations.RoundedDownsample,
				});
		}									
		public virtual async void SetArtwork(IModel? model)
		{
			if (Images is null)
				return;

			ILibrary.IIdentifiers? identifiers = true switch
			{
				true when model is IGenre genre => ILibrary.IIdentifiers.FromGenre(genre),
				true when model is IPlaylist playlist => ILibrary.IIdentifiers.FromPlaylist(playlist),

				_ => null,
			};

			if (identifiers is null)
				await Images.SetToImageView(new IImagesDroid.Parameters(model)
				{
					ImageView = Artwork,
					Operations = IImages.DefaultOperations.RoundedDownsample,
				});
			else if (Library?.Songs.GetSongs(identifiers, default) is IAsyncEnumerable<ISong> songs)
				await Images.SetToImageView(new IImagesDroid.Parameters(songs)
				{
					ImageView = Artwork,
					Operations = ImagesOperations ?? IImages.DefaultOperations.MergeRoundedDownsample,
				});
		}
	}
}