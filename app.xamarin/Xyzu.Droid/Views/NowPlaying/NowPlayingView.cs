#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Motion.Widget;
using AndroidX.Palette.Graphics;
using AndroidX.RecyclerView.Widget;

using Oze.Music.MusicBarLib;

using System;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Views.Insets;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Views.NowPlaying
{
	public partial class NowPlayingView : MotionLayout
	{
		public static partial class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_nowplaying;
			public const int Layout_Collapsed = Resource.Layout.xyzu_view_nowplaying_collapsed;
			public const int Layout_Expanded = Resource.Layout.xyzu_view_nowplaying_expanded;

			public const int View = Resource.Id.xyzu_view_nowplaying;
			public const int BackgroundBlur_AppCompatImageView = Resource.Id.xyzu_view_nowplaying_backgroundblur_appcompatimageview;
			public const int StatusbarInset = Resource.Id.xyzu_view_nowplaying_statusbarinsetview;
			public const int NavigationbarInset = Resource.Id.xyzu_view_nowplaying_navigationbarinsetview;
			public const int Artwork_SimpleHorizontalRecyclerView = Resource.Id.xyzu_view_nowplaying_artwork_simplehorizontalrecyclerview;
			public const int ArtworkDetails_Space = Resource.Id.xyzu_view_nowplaying_artworkdetails_space;
			public const int Detail_One_AppCompatTextView = Resource.Id.xyzu_view_nowplaying_detail_one_appcompattextview;
			public const int Detail_Two_AppCompatTextView = Resource.Id.xyzu_view_nowplaying_detail_two_appcompattextview;
			public const int Position_FixedMusicBar = Resource.Id.xyzu_view_nowplaying_position_fixedmusicbar;
			public const int PositionText_AppCompatTextView = Resource.Id.xyzu_view_nowplaying_positiontext_appcompattextview;
			public const int Buttons_Player_Previous_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_player_previous_appcompatimagebutton;
			public const int Buttons_Player_PlayPause_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_player_playpause_appcompatimagebutton;
			public const int Buttons_Player_Next_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_player_next_appcompatimagebutton;
			public const int Buttons_Menu_Queue_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_menu_queue_appcompatimagebutton;
			public const int Buttons_Menu_AudioEffects_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_menu_audioeffects_appcompatimagebutton;
			public const int Buttons_Menu_PlayerSettings_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_menu_playersettings_appcompatimagebutton;
			public const int Buttons_Menu_Options_AppCompatImageButton = Resource.Id.xyzu_view_nowplaying_buttons_menu_options_appcompatimagebutton;
		}

		public NowPlayingView(Context context) : this(context, null!) { }
		public NowPlayingView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlayling) { }
		public NowPlayingView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected void Configure(Context context)
		{
			Position.SetBarWidth(8);
			Position.SetSpaceBetweenBar(2);
			Position.SetProgressChangeListener(this);

			Artwork.SimpleLayoutManager.SpanCount = 1;
			Artwork.SimpleAdapter.GetItemCount = () => Math.Max(Player?.Queue.Count ?? 0, 1);
			Artwork.SimpleAdapter.ViewHolderOnCreate = (viewgroup, type) => new ArtworkViewHolder(context);
			Artwork.SimpleAdapter.ViewHolderOnBind = async (viewholderdefault, index) =>
			{
				ArtworkViewHolder viewholder = (ArtworkViewHolder)viewholderdefault;

				switch (true)
				{
					case true when
					Images is null ||
					Player?.Queue.CurrentIndex is null:
						viewholder.ItemView.SetImageBitmap(null);
						break;

					case true when index == Player.Queue.CurrentIndex.Value - 1:
						await Images.SetToImageView(IImages.DefaultOperations.Rounded, viewholder.ItemView, null, default, _SongPreviousBitmap, SongPrevious);
						break;
									
					case true when index == Player.Queue.CurrentIndex.Value:
						SetBlur(SongCurrent);
						await Images.SetToImageView(IImages.DefaultOperations.Rounded, viewholder.ItemView, null, default, _SongCurrentBitmap, SongCurrent);						
						break;										 
																	
					case true when index == Player.Queue.CurrentIndex.Value + 1:
						await Images.SetToImageView(IImages.DefaultOperations.Rounded, viewholder.ItemView, null, default, _SongNextBitmap, SongNext);
						break;

					default:
						viewholder.ItemView.SetImageBitmap(null);
						break;
				}
			};

			Artwork.AddOnScrollListener(new ArtworkOnScrollListener(this));

			ArtworkSnapHelper.AttachToRecyclerView(Artwork);

			InitConstraintSets(context);
		}
		protected void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			SetClipChildren(false);

			_BackgroundBlur = FindViewById(Ids.BackgroundBlur_AppCompatImageView) as AppCompatImageView;

			_StatusbarInset = FindViewById(Ids.StatusbarInset) as StatusBarInsetView;
			_NavigationbarInset = FindViewById(Ids.NavigationbarInset) as NavigationBarInsetView;

			_Artwork = FindViewById(Ids.Artwork_SimpleHorizontalRecyclerView) as SimpleHorizontalRecyclerView;
			_Detail_One = FindViewById(Ids.Detail_One_AppCompatTextView) as AppCompatTextView;
			_Detail_Two = FindViewById(Ids.Detail_Two_AppCompatTextView) as AppCompatTextView;

			_Buttons_Player_Previous = FindViewById(Ids.Buttons_Player_Previous_AppCompatImageButton) as AppCompatImageButton;
			_Buttons_Player_PlayPause = FindViewById(Ids.Buttons_Player_PlayPause_AppCompatImageButton) as AppCompatImageButton;
			_Buttons_Player_Next = FindViewById(Ids.Buttons_Player_Next_AppCompatImageButton) as AppCompatImageButton;

			_PositionText = FindViewById(Ids.PositionText_AppCompatTextView) as AppCompatTextView;
			_Position = FindViewById(Ids.Position_FixedMusicBar) as FixedMusicBar;
			
			_Buttons_Menu_Queue = FindViewById(Ids.Buttons_Menu_Queue_AppCompatImageButton) as AppCompatImageButton;
			_Buttons_Menu_AudioEffects = FindViewById(Ids.Buttons_Menu_AudioEffects_AppCompatImageButton) as AppCompatImageButton;
			_Buttons_Menu_PlayerSettings = FindViewById(Ids.Buttons_Menu_PlayerSettings_AppCompatImageButton) as AppCompatImageButton;
			_Buttons_Menu_Options = FindViewById(Ids.Buttons_Menu_Options_AppCompatImageButton) as AppCompatImageButton;

			Configure(context);
		}

		private AppCompatImageView? _BackgroundBlur;
		private StatusBarInsetView? _StatusbarInset;
		private NavigationBarInsetView? _NavigationbarInset;
		private SimpleHorizontalRecyclerView? _Artwork;
		private PagerSnapHelper? _ArtworkSnapHelper;
		private Palette? _ArtworkPalette;
		private AppCompatTextView? _Detail_One;
		private AppCompatTextView? _Detail_Two;
		private AppCompatImageButton? _Buttons_Player_Previous;
		private AppCompatImageButton? _Buttons_Player_PlayPause;
		private AppCompatImageButton? _Buttons_Player_Next;
		private FixedMusicBar? _Position;
		private AppCompatTextView? _PositionText;
		private AppCompatImageButton? _Buttons_Menu_Queue;
		private AppCompatImageButton? _Buttons_Menu_AudioEffects;
		private AppCompatImageButton? _Buttons_Menu_PlayerSettings;
		private AppCompatImageButton? _Buttons_Menu_Options;

		public AppCompatImageView BackgroundBlur 
		{ 
			get => _BackgroundBlur ?? throw new InflateException("Could not find 'BackgroundBlur'");
		}
		public StatusBarInsetView StatusbarInset 
		{ 
			get => _StatusbarInset ?? throw new InflateException("Could not find 'StatusbarInset'");
		}
		public NavigationBarInsetView NavigationbarInset 
		{ 
			get => _NavigationbarInset ?? throw new InflateException("Could not find 'NavigationbarInset'");
		}
		public SimpleHorizontalRecyclerView Artwork 
		{ 
			get => _Artwork ?? throw new InflateException("Could not find 'Artwork'");
		}								  
		public Palette? ArtworkPalette
		{
			get => _ArtworkPalette;
			set => _ArtworkPalette = value;
		}
		public PagerSnapHelper ArtworkSnapHelper
		{
			set => _ArtworkSnapHelper = value;
			get => _ArtworkSnapHelper ??= new PagerSnapHelper();
		}
		public View? ArtworkViewCurrent
		{
			get; protected set;
		}									   
		public bool ArtworkLayoutFull
		{
			get; set;
		}
		public AppCompatTextView Detail_One 
		{ 
			get => _Detail_One ?? throw new InflateException("Could not find 'Detail_One'");
		}
		public AppCompatTextView Detail_Two 
		{ 
			get => _Detail_Two ?? throw new InflateException("Could not find 'Detail_Two'");
		}
		public AppCompatImageButton Buttons_Player_Previous 
		{ 
			get => _Buttons_Player_Previous ?? throw new InflateException("Could not find 'Buttons_Player_Previous'");
		}
		public AppCompatImageButton Buttons_Player_PlayPause 
		{ 
			get => _Buttons_Player_PlayPause ?? throw new InflateException("Could not find 'Buttons_Player_PlayPause'");
		}
		public AppCompatImageButton Buttons_Player_Next 
		{ 
			get => _Buttons_Player_Next ?? throw new InflateException("Could not find 'Buttons_Player_Next'");
		}
		public FixedMusicBar Position 
		{ 
			get => _Position ?? throw new InflateException("Could not find 'Position'");
		}
		public AppCompatTextView PositionText 
		{ 
			get => _PositionText ?? throw new InflateException("Could not find 'PositionText'");
		}
		public AppCompatImageButton Buttons_Menu_Queue 
		{ 
			get => _Buttons_Menu_Queue ?? throw new InflateException("Could not find 'Buttons_Menu_Queue'");
		}
		public AppCompatImageButton Buttons_Menu_AudioEffects 
		{ 
			get => _Buttons_Menu_AudioEffects ?? throw new InflateException("Could not find 'Buttons_Menu_AudioEffects'");
		}
		public AppCompatImageButton Buttons_Menu_PlayerSettings 
		{ 
			get => _Buttons_Menu_PlayerSettings ?? throw new InflateException("Could not find 'Buttons_Menu_PlayerSettings'");
		}
		public AppCompatImageButton Buttons_Menu_Options
		{
			get => _Buttons_Menu_Options ?? throw new InflateException("Could not find 'Buttons_Menu_Options'");
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			SetTransitionListener(this);

			if (Buttons_Player_Previous != null) Buttons_Player_Previous.Click += OnClick;
			if (Buttons_Player_PlayPause != null) Buttons_Player_PlayPause.Click += OnClick;
			if (Buttons_Player_Next != null) Buttons_Player_Next.Click += OnClick;

			if (Buttons_Menu_AudioEffects != null) Buttons_Menu_AudioEffects.Click += OnClick;
			if (Buttons_Menu_Options != null) Buttons_Menu_Options.Click += OnClick;
			if (Buttons_Menu_Queue != null) Buttons_Menu_Queue.Click += OnClick;
			if (Buttons_Menu_PlayerSettings != null) Buttons_Menu_PlayerSettings.Click += OnClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			SetTransitionListener(null);

			if (Buttons_Player_Previous != null) Buttons_Player_Previous.Click -= OnClick;
			if (Buttons_Player_PlayPause != null) Buttons_Player_PlayPause.Click -= OnClick;
			if (Buttons_Player_Next != null) Buttons_Player_Next.Click -= OnClick;

			if (Buttons_Menu_AudioEffects != null) Buttons_Menu_AudioEffects.Click -= OnClick;
			if (Buttons_Menu_Options != null) Buttons_Menu_Options.Click -= OnClick;
			if (Buttons_Menu_Queue != null) Buttons_Menu_Queue.Click -= OnClick;
			if (Buttons_Menu_PlayerSettings != null) Buttons_Menu_PlayerSettings.Click -= OnClick;
		}

		public virtual void OnClick(object sender, EventArgs args)
		{
			if (true switch
			{
				true when sender == Buttons_Player_Previous => ViewOperations.PressPrevious,
				true when sender == Buttons_Player_PlayPause => ViewOperations.PressPlayPause,
				true when sender == Buttons_Player_Next => ViewOperations.PressNext,

				true when sender == Buttons_Menu_AudioEffects => ViewOperations.PressEffects,
				true when sender == Buttons_Menu_Options => ViewOperations.PressOptions,
				true when sender == Buttons_Menu_Queue => ViewOperations.PressQueue,
				true when sender == Buttons_Menu_PlayerSettings => ViewOperations.PressPlayerSettings,

				_ => new ViewOperations?()

			} is ViewOperations viewoperation) InvokeOnViewOperation(sender, new ViewOperationEventArgs(viewoperation));
		}

		public void OnClick_Buttons_Menu_AudioEffects()
		{
			if (Context is null || MenuAudioEffectsView is null)
				return;

			ShowDialog(MenuAudioEffectsView, menudialog =>
			{
				menudialog.SetOnDismissListener(new DialogInterfaceOnDismissListener
				{
					OnDismissAction = dialoginterface =>
					{
						_MenuAudioEffectsView = null;
					}
				});
			});
		}
		public void OnClick_Buttons_Menu_Options()
		{
			if (Context is null || MenuOptionsView is null)
				return;

			ShowDialog(MenuOptionsView, menudialog =>
			{
				menudialog.SetOnDismissListener(new DialogInterfaceOnDismissListener
				{
					OnDismissAction = dialoginterface =>
					{
						_MenuOptionsView = null;
					}
				});
			});
		}
		public void OnClick_Buttons_Menu_Queue()
		{
			OnMenuOptionClick(Menus.NowPlaying.GoToQueue);
		}			
		public void OnClick_Buttons_Menu_PlayerSettings()
		{
			if (Context is null || MenuPlayerSettingsView is null)
				return;

			ShowDialog(MenuPlayerSettingsView, menudialog =>
			{
				menudialog.SetOnDismissListener(new DialogInterfaceOnDismissListener
				{
					OnDismissAction = dialoginterface =>
					{
						_MenuPlayerSettingsView = null;
					}
				});
			});
		}

		public class ArtworkViewHolder : RecyclerViewViewHolderDefault
		{
			private static AppCompatImageView ItemViewDefault(Context context)
			{
				ContextThemeWrapper contextthemewrapper = new ContextThemeWrapper(context, Resource.Style.Xyzu_View_NowPlayling_Image);
				AppCompatImageView itemview = new AppCompatImageView(contextthemewrapper)
				{
					LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
				};

				return itemview;
			}

			public ArtworkViewHolder(Context context) : this(ItemViewDefault(context)) { }
			public ArtworkViewHolder(AppCompatImageView itemView) : base(itemView) { }

			public new AppCompatImageView ItemView
			{
				set => base.ItemView = value;
				get => (AppCompatImageView)base.ItemView;
			}
		}
	}
}