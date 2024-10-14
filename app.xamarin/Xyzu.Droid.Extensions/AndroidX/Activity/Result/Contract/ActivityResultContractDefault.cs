using Android.Content;

using System;

using JavaObject = Java.Lang.Object;

namespace AndroidX.Activity.Result.Contract
{
	public class ActivityResultContractDefault : ActivityResultContract
	{
		public Func<Context, JavaObject?, Intent>? CreateIntentAction { get; set; }
		public Func<int, Intent?, JavaObject?>? ParseResultAction { get; set; }

		public override Intent CreateIntent(Context context, JavaObject? input)
		{
			return CreateIntentAction?.Invoke(context, input) ?? new Intent();
		}
		public override JavaObject? ParseResult(int resultCode, Intent? intent)
		{
			return ParseResultAction?.Invoke(resultCode, intent) ?? intent;
		}
	}
}