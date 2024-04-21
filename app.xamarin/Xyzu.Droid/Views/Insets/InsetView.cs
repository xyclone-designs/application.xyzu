#nullable enable

using Android.Animation;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

using System;
using System.Collections.Generic;
using System.Linq;

using JavaInteger = Java.Lang.Integer;

namespace Xyzu.Views.Insets
{
	[Register("xyzu/views/insets/InsetView")]
	public class InsetView : View, ValueAnimator.IAnimatorUpdateListener
	{
		public InsetView(Context context) : base(context) { }
		public InsetView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InsetView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		private IDictionary<string, int>? _Insets;

		protected IDictionary<string, int> Insets
		{
			set => _Insets = value;
			get => _Insets ??= new Dictionary<string, int>();
		}

		public new ViewGroup.LayoutParams LayoutParameters 
		{  
			set => base.LayoutParameters = value;
			get => base.LayoutParameters ??= new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
		}
		public long? AnimationDuration { get; set; }

		public void AddInset(string key, int inset)
		{
			Insets.AddOrReplace(key, inset);

			int max = Insets.Values.Count == 0 ? 0 : Insets.Values.Max();

			if (Height > max)
				return;

			if (AnimationDuration.HasValue && ValueAnimator.OfInt(Height, max) is ValueAnimator valueanimator)
			{
				valueanimator.AddUpdateListener(this);
				valueanimator.SetDuration(AnimationDuration.Value);
				
				valueanimator.Start();
			}
			else SetHeight(max);
		}
		public void RemoveInset(string key)
		{
			if (Insets.ContainsKey(key) is false)
				return;

			Insets.Remove(key);

			int max = Insets.Values.Count == 0 ? 0 : Insets.Values.Max();

			if (Height < max)
				return;

			if (AnimationDuration.HasValue && ValueAnimator.OfInt(Height, max) is ValueAnimator valueanimator)
			{
				valueanimator.AddUpdateListener(this);
				valueanimator.SetDuration(AnimationDuration.Value);

				valueanimator.Start();
			}
			else SetHeight(max);
		}

		public void SetHeight(int height)
		{
			ViewGroup.LayoutParams layoutparams = LayoutParameters;

			layoutparams.Height = height;

			LayoutParameters = layoutparams;
		}
		public void OnAnimationUpdate(ValueAnimator? animation)
		{
			if (animation?.AnimatedValue.JavaCast<JavaInteger>()?.IntValue() is int animatedvalue)
				SetHeight(animatedvalue);
		}
	}
}