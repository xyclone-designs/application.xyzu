#nullable enable

using Android.Content;
using Java.IO;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

using AndroidOSEnvironment = Android.OS.Environment;

namespace Xyzu.Settings.Files
{
	public interface IFilesSettingsDroid<T> : IFilesSettings<T> { } 
	public interface IFilesSettingsDroid : IFilesSettings 
	{
		public new class Defaults : IFilesSettings.Defaults
		{
			public static readonly IFilesSettingsDroid FilesSettingsDroid = new Default
			{
				Directories = Directories,
				DirectoriesExclude = DirectoriesExclude,
				Mimetypes = Mimetypes,
				TrackLengthIgnore = TrackLengthIgnore,
			};
		}			  

		public new class Default : IFilesSettings.Default, IFilesSettingsDroid { }
		public new class Default<T> : IFilesSettings.Default<T>, IFilesSettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}

		public static IEnumerable<File> Storages()
		{
			if (AndroidOSEnvironment.StorageDirectory.ListFiles() is File[] files)
				foreach (File? file in files)
					if (file.AbsolutePath == "/storage/self")
						continue;
					else if (file.AbsolutePath == "/storage/emulated")
						yield return new File("/storage/emulated/0");
					else yield return file;
		}

		public static Func<File, bool> FilesSettingsDirectoryPredicate(IFilesSettings filessettings)
		{
			return javafile =>
			{
				if (javafile.IsDirectory is false)
					return false;

				bool isapplicable = false;

				if (filessettings.Directories != null)
				{
					foreach (string directory in filessettings.Directories)
						if (isapplicable is false)
							isapplicable = javafile.AbsolutePath.StartsWith(directory, StringComparison.OrdinalIgnoreCase);

					if (isapplicable is false)
						return isapplicable;
				}

				return isapplicable;
			};
		}

		public static Func<File, bool> PredicateDirectories(IFilesSettings filessettings)
		{
			return javafile =>
			{
				bool isapplicable = false;

				if (filessettings.Directories != null)
				{
					foreach (string directory in filessettings.Directories)
						if (isapplicable is false)
							isapplicable = javafile.AbsolutePath.StartsWith(directory, StringComparison.OrdinalIgnoreCase);

					if (isapplicable is false)
						return isapplicable;
				}

				if (filessettings.Mimetypes != null)
				{
					isapplicable = false;

					foreach (MimeTypes mimetype in filessettings.Mimetypes)
						if (isapplicable is false)
						{
							string applicabilitystring = string.Format(".{0}", mimetype);
							isapplicable =
								javafile.AbsolutePath.EndsWith(applicabilitystring, StringComparison.OrdinalIgnoreCase);
						}
				}

				return isapplicable;
			};
		}
		public static Func<File, bool> PredicateDirectoriesExclude(IFilesSettings filessettings)
		{
			return javafile =>
			{
				bool isapplicable = true;

				if (filessettings.DirectoriesExclude != null)
				{
					foreach (string directoryexclude in filessettings.DirectoriesExclude)
						if (isapplicable is true)
							isapplicable = javafile.AbsolutePath.StartsWith(directoryexclude, StringComparison.OrdinalIgnoreCase) is false;

					if (isapplicable is true)
						return isapplicable;
				}

				return isapplicable;
			};
		}
	}
}
