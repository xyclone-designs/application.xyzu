using Android.Database;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

using JavaFile = Java.IO.File;

namespace Xyzu.Library
{
	public partial interface ILibraryDroid
	{
		public interface IParameters : IDisposable
		{
			ICursor? Cursor { get; set; }
			Func<IParameters, ICursor?>? CursorLazy { get; set; }
			IDictionary<string, int>? CursorPositions { get; set; }
			ILibrary.IIdentifiers? Identifiers { get; set; }
			JavaFile? Directory { get; set; }

			IScanner.ServiceNotification? Notification { get; set; }

			ModelTypes[]? ImageModelTypes { get; set; }
			ISong? RetrievedSongExtra { get; set; }

			IEnumerable<string> Filepaths();

			public class Default : IParameters, IDisposable
			{
				public ICursor? Cursor { get; set; }
				public JavaFile? Directory { get; set; }
				public Func<IParameters, ICursor?>? CursorLazy { get; set; }
				public IDictionary<string, int>? CursorPositions { get; set; }
				public ILibrary.IIdentifiers? Identifiers { get; set; }
				public IScanner.ServiceNotification? Notification { get; set; }
				public ILibrary.IOnCreateActions? OnCreateAction { get; set; }

				public ModelTypes[]? ImageModelTypes { get; set; }
				public ISong? RetrievedSongExtra { get; set; }

				public IEnumerable<string> Filepaths()
				{
					switch (true)
					{
						case true when OnCreateAction is MediaStore.MediaStoreActions.OnCreate mediastoreoncreateaction:

							if ((mediastoreoncreateaction.Cursor = Cursor) != null && mediastoreoncreateaction.Cursor.MoveToFirst())
								do
								{
									if (mediastoreoncreateaction.Cursor.GetId() is string id &&
										(Identifiers?.SongIds is null || Identifiers.SongIds.Contains(id)) &&
										(Identifiers?.WithoutSongIds is null || Identifiers.WithoutSongIds.Contains(id) is false))
										yield return id;
								}
								while (mediastoreoncreateaction.Cursor.MoveToNext());

							break;

						case true when OnCreateAction is TagLibSharp.TagLibSharpActions.OnCreate taglibsharponcreateaction:

							if (taglibsharponcreateaction.Paths != null)
								foreach (string key in taglibsharponcreateaction.Paths.Keys)
									if ((Identifiers?.SongIds is null || Identifiers.SongIds.Contains(key)) &&
										(Identifiers?.WithoutSongIds is null || Identifiers.WithoutSongIds.Contains(key) is false))
										yield return key;

							break;

						default: yield break;
					}
				}

				public void Dispose()
				{
					Cursor?.Close();
					Cursor?.Dispose();
					CursorPositions?.Clear();

					Cursor = null;
					CursorLazy = null;
					CursorPositions = null;
					Identifiers = null;
					RetrievedSongExtra = null;
				}
			}
		}
	}
}