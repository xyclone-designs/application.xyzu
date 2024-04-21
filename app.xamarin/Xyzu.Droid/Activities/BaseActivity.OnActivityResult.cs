#nullable enable

using Android.App;
using Android.Content;
using Android.Runtime;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;

using System;
using System.ComponentModel;

namespace Xyzu.Activities
{
	public partial class BaseActivity
	{
		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			OnActivityResultEvent?.Invoke(this, new OnActivityResultArgs(requestCode, resultCode, data));
		}

		protected ActivityResultLauncher? ResultLauncher { get; set; }
		protected ActivityResultCallback ResultCallback { get; set; } = new ActivityResultCallback();
		protected ActivityResultContractDefault ResultContract { get; set; } = new ActivityResultContractDefault();

		public event EventHandler<OnActivityResultArgs>? OnActivityResultEvent;
		public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

		public ActivityResultLauncher? RegisterForActivityResult(Action<ActivityResultContractDefault, ActivityResultCallback> action)
		{
			action.Invoke(ResultContract, ResultCallback);

			return ResultLauncher;
		}
		public class OnActivityResultArgs : EventArgs
		{
			public OnActivityResultArgs(int requestCode, [GeneratedEnum] Result resultCode, Intent? data) 
			{
				Data = data;
				RequestCode = requestCode;
				ResultCode = resultCode;
			}

			public Intent? Data { get; }
			public int RequestCode { get; }
			public Result ResultCode { get; }
		}
	}
}