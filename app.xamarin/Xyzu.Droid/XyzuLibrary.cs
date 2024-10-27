using Android.Content;

using System;
using System.IO;

using Xyzu.Library;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : ILibraryDroid
	{
		private XyzuLibrary(Context context)
		{
			Context = context;
			Library = new ILibraryDroid.Default(Paths.Databases.XyzuLibrary);
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

		public static class Paths
		{
			public static readonly string _Directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			public static class Databases
			{
				static Databases() { Directory.CreateDirectory(_Directory); }

				public static readonly string _Directory = Path.Combine(Paths._Directory, "databases");

				public static readonly string XyzuLibrary = Path.Combine(_Directory, "xyzu.library.db3");
			}
		}
	}
}