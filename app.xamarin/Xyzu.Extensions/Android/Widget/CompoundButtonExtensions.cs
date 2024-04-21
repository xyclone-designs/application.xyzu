#nullable enable

namespace Android.Widget
{
	public static class CompoundButtonExtensions
	{
		public static void SetChecked(this CompoundButton compoundbutton, bool cheked)
		{
			compoundbutton.Checked = cheked;
		}
	}
}