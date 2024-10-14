using Android.Content;
using Android.Database;

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
			}

			public IScanner Service { get; }
			public ILibraryDroid? Library { get; set; }
			public IServiceConnection? ServiceConnection { get; set; }
			public IEnumerable<string>? Filepaths { get; set; }
			
			public ICursor? CursorToActions(Func<ICursor?> getcursor)
			{
				ICursor? cursor = null;

				if (Library?.Actions?.OnCreate is MediaStoreActions.OnCreate mediastoreoncreateaction)
					mediastoreoncreateaction.Cursor = cursor ??= getcursor.Invoke();

				if (Library?.Actions?.OnRetrieve?.OfType<MediaStoreActions.OnRetrieve>().FirstOrDefault() is MediaStoreActions.OnRetrieve mediastoreonretrieveaction)
					mediastoreonretrieveaction.Cursor = cursor ??= getcursor.Invoke();

				return cursor;
			}
		}
	}
}