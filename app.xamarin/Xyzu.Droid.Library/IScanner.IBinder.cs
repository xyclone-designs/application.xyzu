using Android.Content;
using Android.Database;

using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.MediaStore;

namespace Xyzu.Library
{
	public partial interface IScanner
	{
		public class ServiceBinder : Android.OS.Binder 
		{
			public ServiceBinder(IScanner scannerservice)
			{
				Service = scannerservice;
				SQLiteLibrary = new SQLiteLibraryConnection();
			}

			public IScanner Service { get; }
			public IServiceConnection? ServiceConnection { get; set; }
			public SQLiteLibraryConnection SQLiteLibrary { get; set; }
			public CursorBuilder? DefaultCursorBuilder { get; set; }
			public IEnumerable<string>? Filepaths { get; set; }
			public ILibraryDroid.IActions.Container? Actions { get; set; }
			public ICursor? CursorToActions(Func<ICursor?> getcursor)
			{
				ICursor? cursor = null;

				if (Actions?.OnCreate is MediaStoreActions.OnCreate mediastoreoncreateaction)
					mediastoreoncreateaction.Cursor = cursor ??= getcursor.Invoke();

				if (Actions?.OnRetrieve?.OfType<MediaStoreActions.OnRetrieve>().FirstOrDefault() is MediaStoreActions.OnRetrieve mediastoreonretrieveaction)
					mediastoreonretrieveaction.Cursor = cursor ??= getcursor.Invoke();

				return cursor;
			}
		}
	}
}