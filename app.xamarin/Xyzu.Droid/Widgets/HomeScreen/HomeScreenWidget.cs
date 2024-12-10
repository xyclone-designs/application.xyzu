using Android.Appwidget;
using Android.Content;
using Android.Widget;

using JavaClass = Java.Lang.Class;

namespace Xyzu.Widgets.HomeScreen
{
	public abstract class HomeScreenWidget : AppWidgetProvider 
	{
		public abstract int LayoutRes { get; }

		protected virtual RemoteViews OnRemoteViews(Context context)
		{
			return new(context.PackageName, LayoutRes);
		}

		public override void OnReceive(Context? context, Intent? intent)
		{
			base.OnReceive(context, intent);

			OnUpdate(context, AppWidgetManager.GetInstance(context), null);
		}
		public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
		{
			base.OnUpdate(context, appWidgetManager, appWidgetIds);

			if (context is null || appWidgetManager is null)
				return;

			RemoteViews remoteviews = OnRemoteViews(context);
			ComponentName componentname = new(context, JavaClass.FromType(GetType()));

			appWidgetManager.UpdateAppWidget(componentname, remoteviews);
		}
	}
}