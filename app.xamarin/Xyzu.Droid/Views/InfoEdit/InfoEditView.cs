
#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.ConstraintLayout.Widget;

using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;

namespace Xyzu.Views.InfoEdit
{
	public class InfoEditView : ConstraintLayout, IInfoEdit
	{
		public InfoEditView(Context context) : this(context, null!) { }
		public InfoEditView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_InfoEdit) { }
		public InfoEditView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected virtual void Init(Context context, IAttributeSet? attrs) { }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null) { }

		private IImages? _Images;

		public IImages? Images
		{
			get => _Images;
			set
			{
				_Images = value;

				OnPropertyChanged();
			}
		}
	}
}