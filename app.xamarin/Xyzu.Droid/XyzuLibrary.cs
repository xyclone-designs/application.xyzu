using Android.Content;

using System;

using Xyzu.Library;

using Exception = System.Exception;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : ILibraryDroid
	{
		private XyzuLibrary(Context context)
		{
			Context = context;
			Library = new ILibraryDroid.Default();
		}

		private static XyzuLibrary? _Instance;
		
		public static XyzuLibrary Instance
		{
			get => _Instance ?? throw new Exception("Instane is null. Init AppLibrary before use");
		}
		public static bool Inited => _Instance != null;

		public static void Init(Context context, Action<XyzuLibrary>? action = null) 
		{
			_Instance = new XyzuLibrary(context) { };

			action?.Invoke(_Instance);
		}

		public Context Context { get; set; }
		private ILibraryDroid Library { get; set; }
	}
}