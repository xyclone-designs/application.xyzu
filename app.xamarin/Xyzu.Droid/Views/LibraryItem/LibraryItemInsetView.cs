#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;

using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Views.LibraryItem
{
	public class LibraryItemInsetView : LibraryItemView
	{
		public LibraryItemInsetView(Context context) : base(context) { }
		public LibraryItemInsetView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public LibraryItemInsetView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
		public LibraryItemInsetView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

		private IDictionary<string, int>? _Insets;

		protected IDictionary<string, int> Insets
		{
			set => _Insets = value;
			get => _Insets ??= new Dictionary<string, int>();
		}

		public int InsetLargest
		{
			get => Insets.Values.Max();
		}
		public new ViewGroup.LayoutParams LayoutParameters
		{
			set => base.LayoutParameters = value;
			get => base.LayoutParameters ??= new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
		}

		public int? GetInset(string key)
		{
			if (Insets.TryGetValue(key, out int inset))
				return inset;

			return new int?();
		}
		public void AddInset(string key, int inset)
		{
			Insets.AddOrReplace(key, inset);

			int max = Insets.Values.Count == 0 ? 0 : Insets.Values.Max();

			if (Height > max)
				return;

			SetHeight(max);
		}
		public void RemoveInset(string key)
		{
			if (Insets.ContainsKey(key) is false)
				return;

			Insets.Remove(key);

			int max = Insets.Values.Count == 0 ? 0 : Insets.Values.Max();

			if (Height < max)
				return;

			SetHeight(max);
		}
		public void SetHeight(int height)
		{
			ViewGroup.LayoutParams layoutparams = LayoutParameters;

			layoutparams.Height = height;

			LayoutParameters = layoutparams;
		}
	}
}