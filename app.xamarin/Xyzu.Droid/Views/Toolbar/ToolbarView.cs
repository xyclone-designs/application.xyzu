#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.ConstraintLayout.Widget;

using System.Runtime.CompilerServices;

using Xyzu.Droid;

namespace Xyzu.Views.Toolbar
{
	public class ToolbarView : ConstraintLayout
	{
		public ToolbarView(Context context) : this(context, null!)
		{ }
		public ToolbarView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Toolbar)
		{ }
		public ToolbarView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_Toolbar)
		{ }
		public ToolbarView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}

		protected virtual void Init(Context context, IAttributeSet? attrs)
		{ }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{ }
	}
}