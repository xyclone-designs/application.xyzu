#nullable enable

using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;

using Java.IO;

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Xyzu.Droid;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Views.Setting
{
	public class DirectoryItem : ConstraintLayout
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_setting_directoryitem;

			public const int DirectoryIsSelected_AppCompatCheckBox = Resource.Id.xyzu_view_setting_directoryitem_directoryisselected_appcompatcheckbox;
			public const int DirectoryTitle_AppCompatButton = Resource.Id.xyzu_view_setting_directoryitem_directorytitle_appcompatbutton;
			public const int DirectoryChildren_RecursiveItemsRecyclerView = Resource.Id.xyzu_view_setting_directoryitem_directorychildren_recursiveitemsrecyclerview;
		}

		public DirectoryItem(Context context) : this(context, null!) { }
		public DirectoryItem(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Setting_DirectoryItem) { }
		public DirectoryItem(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected void Init(Context context, IAttributeSet? _)
		{
			Inflate(context, Ids.Layout, this);

			_DirectoryIsSelected = FindViewById(Ids.DirectoryIsSelected_AppCompatCheckBox) as AppCompatCheckBox;
			_DirectoryTitle = FindViewById(Ids.DirectoryTitle_AppCompatButton) as AppCompatButton;
			_DirectoryChildren = FindViewById(Ids.DirectoryChildren_RecursiveItemsRecyclerView) as RecursiveItemsRecyclerView;
		}
		protected void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			switch(propertyname)
			{
				case nameof(Directory):
					DirectoryTitle.Text = DirectoryTitleText;
					LoadLayout();
					break;
				case nameof(DirectoryHasChildren):
					LoadLayout();
					break;	   
				case nameof(DirectoryLevel):
					DirectoryTitle.Text = DirectoryTitleText;
					break;

				default: break;
			}
		}

		private bool _Expanded = false;
		private Drawable? _DirectoryChildrenDrawableGone;
		private Drawable? _DirectoryChildrenDrawableVisible;

		private string? DirectoryTitleText
		{
			get => Directory is null
				? null
				: Directory.ParentFile is null
					? Directory.AbsolutePath
					: Directory.AbsolutePath.Replace(Directory.ParentFile.AbsolutePath, string.Join(string.Empty, Enumerable.Range(0, DirectoryLevel).Select(num => "    ")) + "..");
		}

		private File? _Directory;
		private int _DirectoryLevel;
		private bool _DirectoryHasChildren;
		private AppCompatCheckBox? _DirectoryIsSelected;
		private AppCompatButton? _DirectoryTitle;
		private RecursiveItemsRecyclerView? _DirectoryChildren;

		public File? Directory 
		{
			get => _Directory;
			set
			{
				_Directory = value;

				OnPropertyChanged();
			}
		}					   	
		public bool DirectoryHasChildren
		{
			get => _DirectoryHasChildren;
			set
			{
				_DirectoryHasChildren = value;

				OnPropertyChanged();
			}
		}
		public int DirectoryLevel
		{
			get => _DirectoryLevel;
			set
			{
				_DirectoryLevel = value;

				OnPropertyChanged();
			}
		}

		public AppCompatCheckBox DirectoryIsSelected 
		{
			get => _DirectoryIsSelected ?? throw new InflateException();
		}
		public AppCompatButton DirectoryTitle 
		{
			get => _DirectoryTitle ?? throw new InflateException();
		}
		public RecursiveItemsRecyclerView DirectoryChildren 
		{
			get => _DirectoryChildren ?? throw new InflateException();
		}

		public Action<object, CompoundButton.CheckedChangeEventArgs>? DirectoryIsSelectedCheckChange { get; set; }
		public Action<object, EventArgs>? DirectoryTitleClick { get; set; }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			DirectoryIsSelected.CheckedChange += DirectoryIsSelected_CheckedChange;
			DirectoryTitle.Click += DirectoryTitle_Click; 
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			DirectoryIsSelected.CheckedChange -= DirectoryIsSelected_CheckedChange;
			DirectoryTitle.Click -= DirectoryTitle_Click;
		}

		private void DirectoryIsSelected_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
		{
			DirectoryIsSelectedCheckChange?.Invoke(sender, args);
		}
		private void DirectoryTitle_Click(object sender, EventArgs args)
		{
			_Expanded = !_Expanded;

			LoadLayout();

			DirectoryTitleClick?.Invoke(sender, args);
		}

		private void LoadLayout()
		{
			_DirectoryChildrenDrawableGone ??= Context?.Resources?.GetDrawable(Resource.Drawable.mtrl_ic_arrow_drop_down, Context.Theme);
			_DirectoryChildrenDrawableVisible ??= Context?.Resources?.GetDrawable(Resource.Drawable.mtrl_ic_arrow_drop_up, Context.Theme);

			switch (true)
			{					
				case true when Directory is null || DirectoryHasChildren is false:
					DirectoryChildren.Visibility = ViewStates.Gone;
					DirectoryTitle.SetDrawableEnd(null);
					break;

				case true when _Expanded:
					DirectoryChildren.Visibility = ViewStates.Visible;
					DirectoryTitle.SetDrawableEnd(_DirectoryChildrenDrawableVisible);
					break;				

				default:
					DirectoryChildren.Visibility = ViewStates.Gone;
					DirectoryTitle.SetDrawableEnd(_DirectoryChildrenDrawableGone);
					break;
			}
		}
	}
}