#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.ConstraintLayout.Widget;

using System;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;

namespace Xyzu.Views.Info
{
	public class InfoView : ConstraintLayout, IInfo
	{
		public InfoView(Context context) : this(context, null!) { }
		public InfoView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Info) { }
		public InfoView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
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