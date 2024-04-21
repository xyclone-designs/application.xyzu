#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.ConstraintLayout.Widget;

using System.Runtime.CompilerServices;

using Xyzu.Droid;

namespace Xyzu.Views.Option
{
	public class OptionsView : ConstraintLayout
	{
		public OptionsView(Context context) : this(context, null!)
		{ }
		public OptionsView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Option)
		{ }
		public OptionsView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_Option)
		{ }
		public OptionsView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}

		protected virtual void Init(Context context, IAttributeSet? attrs)
		{ }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{ }
	}
}