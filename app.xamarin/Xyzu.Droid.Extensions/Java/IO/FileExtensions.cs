using System.Collections.Generic;
using System.Threading.Tasks;

namespace Java.IO
{
	public static class FileExtensions
	{
		public static IEnumerable<File> ListAllParentFiles(this File file)
		{
			File? parentfile = file.ParentFile;

			while (parentfile is not null)
			{
				yield return parentfile;
				parentfile = parentfile.ParentFile;
			}
		}

		public static IEnumerable<File> ListAllFiles(this File file)
		{
			if (file.ListFiles() is File[] files)
				foreach (File f in files)
				{
					foreach (File fi in f.ListAllFiles())
						yield return fi;

					yield return f;
				}
		}
		public static async IAsyncEnumerable<File> ListAllFilesAsync(this File file)
		{
			if (await file.ListFilesAsync() is IEnumerable<File> files)
				foreach (File f in files)
				{
					await foreach (File fi in f.ListAllFilesAsync())
						yield return fi;

					yield return f;
				}
		}

		public static void ClearDirectory(this File file)
		{
			if (file.ListFiles() is IEnumerable<File> files)
				foreach (File innerfile in files)
					innerfile.Delete();
		}
		public static async Task ClearDirectoryAsync(this File file)
		{
			if (await file.ListFilesAsync() is IEnumerable<File> files)
				foreach (File innerfile in files)
					innerfile.Delete();
		}
	}
}