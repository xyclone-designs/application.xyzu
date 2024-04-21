
#nullable enable

using Android.Content;

using Java.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Enums;
using Xyzu.Settings.Files;

using AndroidEnvironment = Android.OS.Environment;
using JavaNioFiles = Java.Nio.FileNio.Files;
using IJavaNioPath = Java.Nio.FileNio.IPath;

using SystemIODirectory = System.IO.Directory;
using SystemIOFile = System.IO.File;
using SystemIOFileStream = System.IO.FileStream;
using SystemIOPath = System.IO.Path;
using SystemIOStreamWriter = System.IO.StreamWriter;

namespace Xyzu.Settings.System
{
	public interface ISystemSettingsDroid<T> : ISystemSettings<T> { }
	public interface ISystemSettingsDroid : ISystemSettings
	{
		public static string DataDirectory_ErrorLog_PathName = "errorlogs";

		public static Task ClearErrorLogs(Context context)
		{
			File? packagedirectory = AndroidEnvironment.StorageDirectory.GetPackageDirectory(context);

			if (packagedirectory is null)
				return Task.CompletedTask;

			string errorlogdirectorypath = string.Format("{0}/{1}", packagedirectory.AbsolutePath, DataDirectory_ErrorLog_PathName);
			return new File(errorlogdirectorypath).ClearDirectoryAsync();
		}
		public static IEnumerable<IErrorLog> GetErrorLogs(Context context)
		{
			File? packagedirectory = AndroidEnvironment.StorageDirectory.GetPackageDirectory(context);

			if (packagedirectory is null)
				yield break;

			string errorlogdirectorypath = string.Format("{0}/{1}", packagedirectory.AbsolutePath, DataDirectory_ErrorLog_PathName);
			File[] errorlogfiles = new File(errorlogdirectorypath).ListFiles() ?? Array.Empty<File>();

			StringBuilder stringbuilder = new StringBuilder();

			foreach (File errorlogfile in errorlogfiles)
				if (errorlogfile.IsFile)
				{
					IJavaNioPath path = errorlogfile.ToPath();
					BufferedReader? bufferedreader = JavaNioFiles.NewBufferedReader(path);

					if (bufferedreader is null)
						continue;

					stringbuilder.Clear();

					while (bufferedreader.ReadLine() is string line)
						stringbuilder.AppendLine(line);

					bufferedreader.Close();

					string text = stringbuilder.ToString();
					IErrorLog errorlog = IErrorLog.FromText(text);
					errorlog.Path = errorlogfile.Path;

					yield return errorlog;
				}
		}
		public static Task AddErrorLog(Context context, IErrorLog errorlog)
		{
			File? packagedirectory = AndroidEnvironment.StorageDirectory.GetPackageDirectory(context);

			if (packagedirectory is null)
				return Task.CompletedTask;

			string errorlogdirectorypath = string.Format("{0}/{1}", packagedirectory.AbsolutePath, DataDirectory_ErrorLog_PathName);
			string errorlogfilepath = string.Format("{0}/{1}", errorlogdirectorypath, errorlog.FileName);

			if (SystemIODirectory.Exists(errorlogdirectorypath) is false)
				SystemIODirectory.CreateDirectory(errorlogdirectorypath);

			using SystemIOFileStream systemiofilestream = SystemIOFile.Create(errorlogfilepath);
			using SystemIOStreamWriter systemiostreamwriter = new SystemIOStreamWriter(systemiofilestream);

			systemiostreamwriter.Write(errorlog.AsText());

			return Task.CompletedTask;
		}

		public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			if (args.ExceptionObject is Exception exception && sender is Context context)
				AddErrorLog(context, IErrorLog.FromException(exception));
		}
		public static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
		{
			if (sender is Context context)
				AddErrorLog(context, IErrorLog.FromException(args.Exception));
		}

		public new class Default : ISystemSettings.Default, ISystemSettingsDroid
		{
			public Default() : base() { }
		}
		public new class Default<T> : ISystemSettings.Default<T>, ISystemSettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
