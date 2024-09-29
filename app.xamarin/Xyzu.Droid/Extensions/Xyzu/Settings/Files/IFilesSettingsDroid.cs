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
		private static File _DirectoryStorage = new(IntenalStorage);

		public const string IntenalStorage = "/storage";
		public const string IntenalStorageSelf = IntenalStorage + "/self";
		public const string IntenalStorageEmulated = IntenalStorage + "/emulated";
		public const string IntenalStorageEmulated0 = IntenalStorageEmulated + "/0";

		public readonly static string? DirectoryAudiobooks = "audiobooks"; //AndroidOSEnvironment.DirectoryAudiobooks;
		public readonly static string? DirectoryDownloads = AndroidOSEnvironment.DirectoryDownloads;
		public readonly static string? DirectoryMusic = AndroidOSEnvironment.DirectoryMusic;
		public readonly static string? DirectoryPodcasts = AndroidOSEnvironment.DirectoryPodcasts;

		public static File? DirectoryStorage
		{
			get => _DirectoryStorage.Exists() ? _DirectoryStorage : null;
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
				.SelectMany(directoryfile => directoryfile.ListFiles() ?? Array.Empty<File>())
				.Where(PredicateFiles(this));
		}
		public IEnumerable<File> FilesDirectories()
		{
			IEnumerable<File> directoriesfiles = Enumerable.Empty<File>();

			if (Directories != null)
				directoriesfiles = Directories.SelectMany(dir =>
				{
					File file = new (dir);
					if (file.Exists() && file.IsDirectory)
						return file.ListAllFiles().Where(dirr => dirr.IsDirectory);
					return Enumerable.Empty<File>();
				});

			if (DirectoriesExclude != null)
				directoriesfiles = directoriesfiles.Where(dir =>
				{
					return DirectoriesExclude.Any(dirr => dir.AbsolutePath.StartsWith(dirr)) is false;
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
		public static IEnumerable<File> StoragesDirectories()
		{
			foreach (File storage in Storages())
				foreach (File storagefile in storage.ListAllFiles())
					if (storagefile.AbsolutePath.StartsWith(storage.AbsolutePath + "/Android") is false)
						yield return storagefile;
		}
		public static IEnumerable<File> StoragesFiles()
		{
			foreach (File storage in Storages())
				foreach (File storagefile in storage.ListAllFiles())
					if (storagefile.AbsolutePath.StartsWith(storage.AbsolutePath + "/Android") is false)
						yield return storagefile;
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

		public async IAsyncEnumerable<File> FilesAsync()
		{
			Func<File, bool> predicate = PredicateFiles(this);

			await foreach (File directoryfile in FilesDirectoriesAsync())
				if (await directoryfile.ListFilesAsync() is File[] files)
					foreach (File file in files)
						if (predicate.Invoke(file))
							yield return file;
		}
		public IAsyncEnumerable<File> FilesDirectoriesAsync()
		{
			IAsyncEnumerable<File> directoriesfiles = AsyncEnumerable.Empty<File>();

			if (Directories != null)
				directoriesfiles = Directories
					.ToAsyncEnumerable()
					.SelectMany(dir =>
					{
						File file = new (dir);
						if (file.Exists() && file.IsDirectory)
							return file.ListAllFilesAsync().Where(dirr => dirr.IsDirectory);
						return AsyncEnumerable.Empty<File>();
					});

			if (DirectoriesExclude != null)
				directoriesfiles = directoriesfiles.Where(dir =>
				{
					return DirectoriesExclude.Any(dirr => dir.AbsolutePath.StartsWith(dirr)) is false;
				});

			return directoriesfiles;
		}
		public static async IAsyncEnumerable<File> StoragesAsync()
		{
			File intenalstorage = new(IntenalStorage);

			if (intenalstorage.Exists() && await intenalstorage.ListFilesAsync() is File[] files)
				foreach (File file in files)
					if (file.AbsolutePath == IntenalStorageSelf)
						continue;
					else if (file.AbsolutePath == IntenalStorageEmulated)
						yield return new File(IntenalStorageEmulated0);
					else yield return file;
		}
		public static async IAsyncEnumerable<File> StoragesDirectoriesAsync()
		{
			await foreach (File storage in StoragesAsync())
				await foreach (File storagefile in storage.ListAllFilesAsync())
					if (storagefile.AbsolutePath.StartsWith(storage.AbsolutePath + "/Android") is false)
						yield return storagefile;
		}
		public static async IAsyncEnumerable<File> StoragesFilesAsync()
		{
			await foreach (File storage in StoragesAsync())
				await foreach (File storagefile in storage.ListAllFilesAsync())
					if (storagefile.AbsolutePath.StartsWith(storage.AbsolutePath + "/Android") is false)
						yield return storagefile;
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
