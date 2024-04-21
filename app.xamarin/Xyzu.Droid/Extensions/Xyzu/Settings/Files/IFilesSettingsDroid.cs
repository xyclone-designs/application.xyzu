#nullable enable

using Android.Content;

using Java.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Enums;

using AndroidEnvironment = Android.OS.Environment;

namespace Xyzu.Settings.Files
{
	public interface IFilesSettingsDroid<T> : IFilesSettings<T> { } 
	public interface IFilesSettingsDroid : IFilesSettings 
	{
		public new class Defaults : IFilesSettings.Defaults
		{
			public new static readonly IEnumerable<string> Directories = Enumerable.Empty<string>()
				.Append(AndroidEnvironment.DirectoryAudiobooks)
				.Append(AndroidEnvironment.DirectoryDownloads)
				.Append(AndroidEnvironment.DirectoryMusic)
				.Append(AndroidEnvironment.DirectoryPodcasts)
				.OfType<string>();

			public static readonly IFilesSettingsDroid FilesSettingsDroid = new Default
			{
				Directories = Directories,
				Mimetypes = Mimetypes,
				TrackLengthIgnore = TrackLengthIgnore,
			};
		}			  

		public new class Default : IFilesSettings.Default, IFilesSettingsDroid { }
		public new class Default<T> : IFilesSettings.Default<T>, IFilesSettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
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
							isapplicable =
								javafile.AbsolutePath.Contains(string.Format("/{0}/", directory), StringComparison.OrdinalIgnoreCase) ||
								javafile.AbsolutePath.EndsWith(string.Format("/{0}", directory), StringComparison.OrdinalIgnoreCase);

					if (isapplicable is false)
						return isapplicable;
				}

				return isapplicable;
			};
		}
		public static Func<File, bool> FilesSettingsFilePredicate(IFilesSettings filessettings)
		{
			return javafile =>
			{
				bool isapplicable = false;

				if (filessettings.Directories != null)
				{
					foreach (string directory in filessettings.Directories)
						if (isapplicable is false)
						{
							string applicabilitystring = string.Format("/{0}/", directory);
							isapplicable =
								javafile.AbsolutePath.Contains(applicabilitystring, StringComparison.OrdinalIgnoreCase) ||
								javafile.AbsolutePath.EndsWith(applicabilitystring, StringComparison.OrdinalIgnoreCase);
						}

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
	}

	public static class IFilesSettingsDroidExtensions
	{
		private static readonly string AndroidDirectory = "/Android";

		public static File? GetPackageDirectory(this File file, Context? context)
		{
			IEnumerable<File> list = file.ListAllFiles();

			string[] files = list.Select(l => l.AbsolutePath).ToArray();

			string? packagename = context?.ApplicationInfo?.PackageName;
			File? packagedirectory = packagename is null ? null : list.FirstOrDefault(file => file.AbsolutePath.EndsWith(packagename, StringComparison.OrdinalIgnoreCase));

			return packagedirectory;
		}
		public static File? GetAndroidDirectory(this File file, Context? context)
		{
			IEnumerable<File> list = file.ListAllFiles();

			string? packagename = context?.ApplicationInfo?.PackageName;
			File? packagedirectory = packagename is null ? null : list.FirstOrDefault(file => file.AbsolutePath.EndsWith(packagename, StringComparison.OrdinalIgnoreCase));
			File? androiddirectory = packagedirectory?.ParentFile;

			while (androiddirectory != null && androiddirectory?.AbsolutePath.EndsWith(AndroidDirectory) is false)
				androiddirectory = androiddirectory.ParentFile;

			return androiddirectory;
		}				  			
		public static async Task<File?> GetAndroidDirectory(this File file, Context? context, CancellationToken cancellationtoken = default)
		{
			IAsyncEnumerable<File> list = file.ListAllFilesAsync();

			string? packagename = context?.ApplicationInfo?.PackageName;
			File? packagedirectory = packagename is null ? null : await list.FirstOrDefaultAsync(file => file.AbsolutePath.EndsWith(packagename, StringComparison.OrdinalIgnoreCase), cancellationtoken);
			File? androiddirectory = packagedirectory?.ParentFile;

			while (androiddirectory != null && androiddirectory?.AbsolutePath.EndsWith(AndroidDirectory) is false)
				androiddirectory = androiddirectory.ParentFile;

			return androiddirectory;
		}				  
		public static IEnumerable<File> ListFilesSettingsDiretories(this File file, IFilesSettings filessettings, Context? context)
		{
			IEnumerable<File> list = file.ListAllFiles();

			string? packagename = context?.ApplicationInfo?.PackageName;
			File? packagedirectory = packagename is null ? null : list.FirstOrDefault(file => file.AbsolutePath.EndsWith(packagename, StringComparison.OrdinalIgnoreCase));
			File? androiddirectory = packagedirectory?.ParentFile;

			while (androiddirectory != null && androiddirectory?.AbsolutePath.EndsWith(AndroidDirectory) is false)
				androiddirectory = androiddirectory.ParentFile;

			if (androiddirectory != null && androiddirectory.AbsolutePath.EndsWith(AndroidDirectory))
				list = list.Where(file => file.AbsolutePath.StartsWith(androiddirectory.AbsolutePath) is false);

			foreach (File directory in list.Where(IFilesSettingsDroid.FilesSettingsDirectoryPredicate(filessettings)))
				yield return directory;
		}
		public static async IAsyncEnumerable<File> ListFilesSettingsDiretoriesAsync(this File file, IFilesSettings filessettings, Context? context, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
		{
			IAsyncEnumerable<File> list = file.ListAllFilesAsync();

			string? packagename = context?.ApplicationInfo?.PackageName;
			File? packagedirectory = packagename is null ? null : await list.FirstOrDefaultAsync(file => file.AbsolutePath.EndsWith(packagename, StringComparison.OrdinalIgnoreCase), cancellationtoken);
			File? androiddirectory = packagedirectory?.ParentFile;

			while (androiddirectory != null && androiddirectory?.AbsolutePath.EndsWith(AndroidDirectory) is false)
				androiddirectory = androiddirectory.ParentFile;

			if (androiddirectory != null && androiddirectory.AbsolutePath.EndsWith(AndroidDirectory))
				list = list.Where(file => file.AbsolutePath.StartsWith(androiddirectory.AbsolutePath));

			await foreach (File directory in list.Where(IFilesSettingsDroid.FilesSettingsDirectoryPredicate(filessettings)))
				yield return directory;
		}

		public static IEnumerable<File> ListFilesSettingsFiles(this File file, IFilesSettings filessettings)
		{
			if (file.ListAllFiles() is IEnumerable<File> listfiles)
				foreach (File listfile in listfiles.Where(IFilesSettingsDroid.FilesSettingsFilePredicate(filessettings)))
					yield return listfile;
		}
		public static async IAsyncEnumerable<File> ListFilesSettingsFilesAsync(this File file, IFilesSettings filessettings, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
		{
			if (file.ListAllFilesAsync() is IAsyncEnumerable<File> listfiles)
				await foreach (File listfile in listfiles.Where(IFilesSettingsDroid.FilesSettingsFilePredicate(filessettings)))
					yield return listfile;
		}
	}
}
