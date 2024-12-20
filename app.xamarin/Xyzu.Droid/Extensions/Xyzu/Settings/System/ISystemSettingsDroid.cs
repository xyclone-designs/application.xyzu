﻿using Android.Runtime;

using Java.IO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using JavaNioFiles = Java.Nio.FileNio.Files;
using IJavaNioPath = Java.Nio.FileNio.IPath;

using SystemIODirectory = System.IO.Directory;
using SystemIOFile = System.IO.File;
using SystemIOFileStream = System.IO.FileStream;
using SystemIOStreamWriter = System.IO.StreamWriter;

namespace Xyzu.Settings.System
{
	public interface ISystemSettingsDroid<T> : ISystemSettings<T> { }
	public interface ISystemSettingsDroid : ISystemSettings
	{
		public readonly static string DataDirectory_ErrorLog_PathName = "errorlogs";

		public static File? PackageDirectory { get; set; }

		public static Task ClearErrorLogs()
		{
			if (PackageDirectory is null)
				return Task.CompletedTask;

			string errorlogdirectorypath = string.Format("{0}/{1}", PackageDirectory.AbsolutePath, DataDirectory_ErrorLog_PathName);
			return new File(errorlogdirectorypath).ClearDirectoryAsync();
		}
		public static IEnumerable<IErrorLog> GetErrorLogs()
		{
			if (PackageDirectory is null)
				yield break;

			string errorlogdirectorypath = string.Format("{0}/{1}", PackageDirectory.AbsolutePath, DataDirectory_ErrorLog_PathName);
			File[] errorlogfiles = new File(errorlogdirectorypath).ListFiles() ?? Array.Empty<File>();

			StringBuilder stringbuilder = new ();

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
		public static Task AddErrorLog(IErrorLog errorlog)
		{
			if (PackageDirectory is null)
				return Task.CompletedTask;

			string errorlogdirectorypath = string.Format("{0}/{1}", PackageDirectory.AbsolutePath, DataDirectory_ErrorLog_PathName);
			string errorlogfilepath = string.Format("{0}/{1}", errorlogdirectorypath, errorlog.FileName);

			if (SystemIODirectory.Exists(errorlogdirectorypath) is false)
				SystemIODirectory.CreateDirectory(errorlogdirectorypath);

			using SystemIOFileStream systemiofilestream = SystemIOFile.Create(errorlogfilepath);
			using SystemIOStreamWriter systemiostreamwriter = new (systemiofilestream);

			systemiostreamwriter.Write(errorlog.AsText());

			return Task.CompletedTask;
		}

		public static void UnhandledExceptionRaiser(object? sender, RaiseThrowableEventArgs args)
		{
			AddErrorLog(IErrorLog.FromException(args.Exception));
		}
		public static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs args)
		{
			if (args.ExceptionObject is Exception exception)
				AddErrorLog(IErrorLog.FromException(exception));
		}
		public static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs args)
		{
			AddErrorLog(IErrorLog.FromException(args.Exception));
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
