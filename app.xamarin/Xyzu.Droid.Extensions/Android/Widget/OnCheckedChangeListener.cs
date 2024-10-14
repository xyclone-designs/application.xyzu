using System;

namespace Android.Widget
{
	public class OnCheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
	{
		public void OnCheckedChanged(CompoundButton? buttonView, bool isChecked)
		{
			OnCheckedChangedAction?.Invoke(buttonView, isChecked);
		}

		public Action<CompoundButton?, bool>? OnCheckedChangedAction { get; set; }
	}
}