#nullable enable

using Google.Android.Material.Snackbar;

namespace Xyzu.Views.Library
{
	public partial class LibraryView 
	{
		protected virtual void OnSnackbarCreated(Snackbar snackbar)
		{
			if (InsetBottomView != null)
				snackbar.View.TranslationY = -InsetBottomView.InsetLargest;
		}
	}
}