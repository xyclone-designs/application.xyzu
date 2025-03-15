#nullable enable

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Preference;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.Preference
{
	public class PreferenceView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_preference_preference;

			public const int IconFrame_FrameLayout = Android.Resource.Id.IconFrame;
			public const int Icon_AppCompatImageView = Android.Resource.Id.Icon;
			public const int Title_AppCompatTextView = Android.Resource.Id.Title;
			public const int AdditionalItem_FrameLayout = Resource.Id.xyzu_preference_preference_additionalitem_framelayout;
			public const int Checkbox_SwitchCompat = Resource.Id.switchWidget;
			public const int Dropdown_AppCompatImageView = Resource.Id.xyzu_preference_preference_dropdown_appcomptimagebutton;
			public const int ValueSummary_AppCompatTextView = Resource.Id.xyzu_preference_preference_valuesummary_appcompattextview;
			public const int Summary_AppCompatTextView = Android.Resource.Id.Summary;
			public const int AdditionalContent_FrameLayout = Resource.Id.xyzu_preference_preference_additionalcontent_framelayout;
		}

		private bool _Expanded = false;
		private bool _IsDropdown = false;
		private bool _IsSwitch = false;

		private ConstraintLayout? _ViewContainer;
		private FrameLayout? _ViewIconFrame;
		private AppCompatImageView? _ViewIcon;
		private AppCompatTextView? _ViewTitle;
		private FrameLayout? _ViewAdditionalItem;
		private AppCompatImageButton? _ViewDropDown;
		private SwitchCompat? _ViewSwitch;
		private AppCompatTextView? _ViewValueSummary;
		private AppCompatTextView? _ViewSummary;
		private FrameLayout? _ViewAdditionalContent;
		private View? _ViewAdditionalContentView;

		public ConstraintLayout? ViewContainer 
		{ 
			get => _ViewContainer;
			set => _ViewContainer = value; 
		}
		public FrameLayout? ViewIconFrame 
		{ 
			get => _ViewIconFrame;
			set => _ViewIconFrame = value; 
		}
		public AppCompatImageView? ViewIcon 
		{ 
			get => _ViewIcon;
			set => _ViewIcon = value; 
		}
		public AppCompatTextView? ViewTitle 
		{ 
			get => _ViewTitle;
			set => _ViewTitle = value; 
		}
		public FrameLayout? ViewAdditionalItem 
		{ 
			get => _ViewAdditionalItem;
			set => _ViewAdditionalItem = value; 
		}
		public SwitchCompat? ViewSwitch
		{ 
			get => _ViewSwitch;
			set => _ViewSwitch = value; 
		}
		public AppCompatImageButton? ViewDropDown
		{ 
			get => _ViewDropDown;
			set => _ViewDropDown = value; 
		}
		public AppCompatTextView? ViewValueSummary 
		{ 
			get => _ViewValueSummary;
			set => _ViewValueSummary = value; 
		}
		public AppCompatTextView? ViewSummary 
		{ 
			get => _ViewSummary;
			set => _ViewSummary = value; 
		}
		public FrameLayout? ViewAdditionalContent 
		{ 
			get => _ViewAdditionalContent;
			set
			{
				_ViewAdditionalContent = value;
				_ViewAdditionalContent?.RemoveAllViews();

				if (ViewAdditionalContentView is null)
					return;

				_ViewAdditionalContent?.RemoveView(ViewAdditionalContentView);
				_ViewAdditionalContent?.AddView(ViewAdditionalContentView);
			}
		}
		public View? ViewAdditionalContentView
		{
			get => _ViewAdditionalContentView;
			set
			{
				_ViewAdditionalContentView = value;
				ViewAdditionalContent?.RemoveAllViews();

				if (_ViewAdditionalContentView is null)
					return;

				ViewAdditionalContent?.RemoveView(_ViewAdditionalContentView);
				ViewAdditionalContent?.AddView(_ViewAdditionalContentView);
			}
		}

		public bool Expanded
		{
			get => _Expanded;
			set
			{
				_Expanded = value;

				ViewAdditionalContent?.SetVisibility(_Expanded
					? ViewStates.Visible
					: ViewStates.Gone);
				ViewDropDown?.SetImageResource(_Expanded
					? Resource.Drawable.icon_general_chevron_up
					: Resource.Drawable.icon_general_chevron_down);
			}
		}
		public bool IsDropdown
		{
			get => _IsDropdown;
			protected set => _IsDropdown = value;
		}
		public bool IsSwitch
		{
			get => _IsSwitch;
			protected set => _IsSwitch = value;
		}

		public void OnBindViewHolder(PreferenceViewHolder holder)
		{
			ViewContainer = holder.ItemView as ConstraintLayout;
			ViewIconFrame = holder.FindViewById(Ids.IconFrame_FrameLayout) as FrameLayout;
			ViewIcon = holder.FindViewById(Ids.Icon_AppCompatImageView) as AppCompatImageView;
			ViewTitle = holder.FindViewById(Ids.Title_AppCompatTextView) as AppCompatTextView;
			ViewAdditionalItem = holder.FindViewById(Ids.AdditionalItem_FrameLayout) as FrameLayout;
			ViewSwitch = holder.FindViewById(Ids.Checkbox_SwitchCompat) as SwitchCompat;
			ViewDropDown = holder.FindViewById(Ids.Dropdown_AppCompatImageView) as AppCompatImageButton;
			ViewValueSummary = holder.FindViewById(Ids.ValueSummary_AppCompatTextView) as AppCompatTextView;
			ViewSummary = holder.FindViewById(Ids.Summary_AppCompatTextView) as AppCompatTextView;
			ViewAdditionalContent = holder.FindViewById(Ids.AdditionalContent_FrameLayout) as FrameLayout;
		}
		public void SetIsDropDown(bool isdropdown, Action<View?>? ondropdownclick)
		{
			IsDropdown = isdropdown;

			if (ViewDropDown is null) return;

			Expanded = _Expanded;
			
			if (ViewDropDown is null) return;
			else if (IsDropdown)
			{
				ViewDropDown.Visibility = ViewStates.Visible;
				ViewDropDown.SetOnClickListener(new OnClickListener
				{
					OnClickAction = ondropdownclick
				});
			}
			else
			{
				ViewDropDown.Visibility = ViewStates.Gone;
				ViewDropDown.SetOnClickListener(null);
			}
		}
		public void SetIsSwitch(bool isswitch, Action<bool>? onswitch)
		{
			IsSwitch = isswitch;

			if (ViewSwitch is null) return;
			else if (IsSwitch)
			{
				ViewSwitch.Visibility = ViewStates.Visible;
				ViewSwitch.SetOnCheckedChangeListener(new OnCheckedChangeListener
				{
					OnCheckedChangedAction = (_, ischecked) => onswitch?.Invoke(ischecked)
				});
			}
			else
			{
				ViewSwitch.Visibility = ViewStates.Gone;
				ViewSwitch.SetOnCheckedChangeListener(null);
			}
		}
	}
}