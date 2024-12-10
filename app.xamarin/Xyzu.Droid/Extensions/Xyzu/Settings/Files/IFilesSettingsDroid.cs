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
		private readonly static File _FileIntenalStorage = new(IntenalStorage);

		public const string NoMediaName = ".nomedia";
		public const string IntenalStorage = "/storage";
		public const string IntenalStorageSelf = IntenalStorage + "/self";
		public const string IntenalStorageEmulated = IntenalStorage + "/emulated";
		public const string IntenalStorageEmulated0 = IntenalStorageEmulated + "/0";

		public readonly static string? DirectoryAudiobooks = "audiobooks"; //AndroidOSEnvironment.DirectoryAudiobooks;
		public readonly static string? DirectoryDownloads = AndroidOSEnvironment.DirectoryDownloads;
		public readonly static string? DirectoryMusic = AndroidOSEnvironment.DirectoryMusic;
		public readonly static string? DirectoryPodcasts = AndroidOSEnvironment.DirectoryPodcasts;

		public static File? FileIntenalStorage
		{
			get => _FileIntenalStorage.Exists() ? _FileIntenalStorage : null;
		}
		
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

		public IEnumerable<File> Files()
		{
			return FilesDirectories()
				.SelectMany(directoryfile =>
				{
					if (directoryfile.ListFiles() is not File[] directoryfiles)
						return Array.Empty<File>();

					if (directoryfiles.Any(file => file.AbsolutePath.EndsWith(NoMediaName)))
						return Array.Empty<File>();

					return directoryfiles;
				
				}).Where(PredicateFiles(this));
		}
		public IEnumerable<File> FilesDirectories()
		{
			IEnumerable<File> 
				directoriesfiles = Enumerable.Empty<File>(),
				filesnomedia = Enumerable.Empty<File>();

			if (Directories != null)
				directoriesfiles = Directories.SelectMany(dir =>
				{
					File file = new (dir);

					if (file.Exists() is false || file.IsDirectory is false)
						return Enumerable.Empty<File>();

					return file.ListAllFiles().Where(dirr => dirr.IsDirectory);
				});

			if (DirectoriesExclude != null)
				directoriesfiles = directoriesfiles.Where(dir =>
				{
					return DirectoriesExclude.Any(dirr => dir.AbsolutePath.StartsWith(dirr)) is false;
				});

			filesnomedia = directoriesfiles
				.SelectMany(_ => _.ListAllParentFiles())
				.DistinctBy(_ => _.AbsolutePath)
				.Select(_ =>
				{
					File file = new(_, NoMediaName);

					if (file.Exists() is false)
						return null;

					return file.ParentFile;

				}).DistinctBy(_ => _?.AbsolutePath).OfType<File>();

			directoriesfiles = directoriesfiles.Where(_ =>
			{
				return filesnomedia.Any(filenomedia => _.AbsolutePath.StartsWith(filenomedia.AbsolutePath)) is false;
			});

			return directoriesfiles;
		}

		public static IEnumerable<File> Storages()
		{
			File intenalstorage = new(IntenalStorage);

			if (intenalstorage.Exists() && intenalstorage.ListFiles() is File[] files)
				foreach (File file in files)
					if (file.AbsolutePath == "/storage/self")
						continue;
					else if (file.AbsolutePath == IntenalStorageEmulated)
						yield return new File(IntenalStorageEmulated0);
					else yield return file;
		}
		public static IEnumerable<string> DefaultDirectories()
		{
			return Enumerable.Empty<string?>()
				.Append(DirectoryAudiobooks)
				.Append(DirectoryDownloads)
				.Append(DirectoryMusic)
				.Append(DirectoryPodcasts)
				.OfType<string>();
		}

		public static Func<File, bool> Predicate(IFilesSettings filessettings)
		{
			return javafile =>
			{
				bool isapplicable = false;

				if (filessettings.Directories?.GetEnumerator() is IEnumerator<string> enumeratordirectories)
				{
					isapplicable = false;

					while (isapplicable is false && enumeratordirectories.MoveNext())
						isapplicable = javafile.AbsolutePath.StartsWith(enumeratordirectories.Current, StringComparison.OrdinalIgnoreCase);

					if (isapplicable is false)
						return isapplicable;
				}

				if (filessettings.DirectoriesExclude?.GetEnumerator() is IEnumerator<string> enumeratordirectoriesexclude)
				{
					isapplicable = true;

					while (isapplicable is true && enumeratordirectoriesexclude.MoveNext())
						isapplicable = javafile.AbsolutePath.StartsWith(enumeratordirectoriesexclude.Current, StringComparison.OrdinalIgnoreCase) is false;

					if (isapplicable is true)
						return isapplicable;
				}

				if (filessettings.Mimetypes?.GetEnumerator() is IEnumerator<string> enumeratormimetypes)
				{
					isapplicable = false;

					while (isapplicable is false && enumeratormimetypes.MoveNext())
						if (isapplicable is false)
						{
							string applicabilitystring = string.Format(".{0}", enumeratormimetypes.Current);
							isapplicable =
								javafile.AbsolutePath.EndsWith(applicabilitystring, StringComparison.OrdinalIgnoreCase);
						}
				}

				return isapplicable;
			};
		}
		public static Func<File, bool> PredicateFiles(IFilesSettings filessettings)
		{
			return javafile =>
			{
				bool isapplicable = false;

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