#nullable enable

using Android.Runtime;

using System;

namespace Android.Media.Audiofx
{
	public class LoudnessEnhancerEffect : LoudnessEnhancer
	{
		public LoudnessEnhancerEffect(int audioSession) : base(audioSession) { }
		protected LoudnessEnhancerEffect(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		private Settings? _Properties;
		public Settings? Properties
		{
			get => _Properties;
			set
			{
				_Properties = value;

				SetTargetGain(_Properties?.TargetGain ?? 0);
			}
		}

		public class Settings : Java.Lang.Object
		{
			public int TargetGain { get; set; }
		}
	}
}