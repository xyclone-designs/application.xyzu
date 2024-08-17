#nullable enable

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Preference;
using AndroidX.Preference.Internal;

using System;
using System.Linq;

using Xyzu.Droid;

namespace Xyzu.Views.Preference
{
	public class PreferenceView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_preference_preference;

			public const int IconFrame_ContentFrameLayout = Android.Resource.Id.IconFrame;
			public const int Icon_PreferenceImageView = Android.Resource.Id.Icon;
			public const int Title_AppCompatTextView = Android.Resource.Id.Title;
			public const int AdditionalItem_ContentFrameLayout = Resource.Id.xyzu_preference_preference_additionalitem_contentframelayout;
			public const int Checkbox_SwitchCompat = Resource.Id.switchWidget;
			public const int Dropdown_AppCompatImageView = Resource.Id.xyzu_preference_preference_dropdown_appcomptimagebutton;
			public const int ValueSummary_AppCompatTextView = Resource.Id.xyzu_preference_preference_valuesummary_appcompattextview;
			public const int Summary_AppCompatTextView = Android.Resource.Id.Summary;
			public const int AdditionalContent_ContentFrameLayout = Resource.Id.xyzu_preference_preference_additionalcontent_contentframelayout;
		}

		private bool _Expanded = false;
		private bool _IsDropdown = false;
		private bool _IsSwitch = false;

		private ConstraintLayout? _ViewContainer;
		private ContentFrameLayout? _ViewIconFrame;
		private PreferenceImageView? _ViewIcon;
		private AppCompatTextView? _ViewTitle;
		private ContentFrameLayout? _ViewAdditionalItem;
		private AppCompatImageButton? _ViewDropDown;
		private SwitchCompat? _ViewSwitch;
		private AppCompatTextView? _ViewValueSummary;
		private AppCompatTextView? _ViewSummary;
		private ContentFrameLayout? _ViewAdditionalContent;
		private View? _ViewAdditionalContentView;

		public ConstraintLayout? ViewContainer 
		{ 
			get => _ViewContainer;
			set => _ViewContainer = value; 
		}
		public ContentFrameLayout? ViewIconFrame 
		{ 
			get => _ViewIconFrame;
			set => _ViewIconFrame = value; 
		}
		public PreferenceImageView? ViewIcon 
		{ 
			get => _ViewIcon;
			set => _ViewIcon = value; 
		}
		public AppCompatTextView? ViewTitle 
		{ 
			get => _ViewTitle;
			set => _ViewTitle = value; 
		}
		public ContentFrameLayout? ViewAdditionalItem 
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
		public ContentFrameLayout? ViewAdditionalContent 
		{ 
			get => _ViewAdditionalContent;
			set
			{
				_ViewAdditionalContent = value;

				if (_ViewAdditionalContent is null || ViewAdditionalContentView is null)
					return;

				_ViewAdditionalContent.RemoveAllViews();
				_ViewAdditionalContent.AddView(ViewAdditionalContentView);
			}
		}
		public View? ViewAdditionalContentView
		{
			get => _ViewAdditionalContentView;
			set
			{
				_ViewAdditionalContentView = value;

				if (_ViewAdditionalContentView is null)
					return;

				ViewAdditionalContent?.RemoveAllViews();
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
			ViewIconFrame = holder.FindViewById(Ids.IconFrame_ContentFrameLayout) as ContentFrameLayout;
			ViewIcon = holder.FindViewById(Ids.Icon_PreferenceImageView) as PreferenceImageView;
			ViewTitle = holder.FindViewById(Ids.Title_AppCompatTextView) as AppCompatTextView;
			ViewAdditionalItem = holder.FindViewById(Ids.AdditionalItem_ContentFrameLayout) as ContentFrameLayout;
			ViewSwitch = holder.FindViewById(Ids.Checkbox_SwitchCompat) as SwitchCompat;
			ViewDropDown = holder.FindViewById(Ids.Dropdown_AppCompatImageView) as AppCompatImageButton;
			ViewValueSummary = holder.FindViewById(Ids.ValueSummary_AppCompatTextView) as AppCompatTextView;
			ViewSummary = holder.FindViewById(Ids.Summary_AppCompatTextView) as AppCompatTextView;
			ViewAdditionalContent = holder.FindViewById(Ids.AdditionalContent_ContentFrameLayout) as ContentFrameLayout;
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