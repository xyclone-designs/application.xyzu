#nullable enable

using Android.App;
using Android.Content;
using Android.Provider;
using AndroidX.Core.Content;
using AndroidX.Core.OS;

using Xyzu.Library;
using Xyzu.Library.MediaStore;
using Xyzu.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using AndroidUri = Android.Net.Uri;

namespace Android.Database
{
	public class CursorBuilder
	{
		private CancellationSignal? CancellationSignal { get; set; }
		private CancellationToken? CancellationToken { get; set; }
		private ContentResolver? ContentResolver { get; set; }
		private bool? IdentifiersExclusive { get; set; }
		private ILibrary.IIdentifiers? Identifiers { get; set; }
		private string[]? Projection { get; set; }
		private string? Selection { get; set; }
		private string[]? SelectionArgs { get; set; }
		private ILibrary.ISettings? Settings { get; set; }
		private string? SortOrder { get; set; }
		private AndroidUri? Uri { get; set; }

		public ICursor? Build()
		{
			(string selection, string[] selectionArgs) = Identifiers.ToSelectionAndArgs(IdentifiersExclusive ?? true, Settings);

			CancellationSignal ??= CancellationToken?.AsCancellationSignal() ?? new CancellationSignal();
			Projection ??= Array.Empty<string>();
			Selection ??= selection;
			SelectionArgs ??= selectionArgs;
			SortOrder ??= MediaStore.Audio.Media.DefaultSortOrder;
			Uri ??= true switch
			{
				true when Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q
					=> MediaStore.Audio.Media.GetContentUri(MediaStore.VolumeExternal),

				_ => MediaStore.Audio.Media.ExternalContentUri
			};

			if (ContentResolver is null || Uri is null)
				return null;

			return ContentResolverCompat
				.Query(ContentResolver, Uri, Projection, Selection, SelectionArgs, SortOrder, CancellationSignal);
		}

		public CursorBuilder WithCancellationSignal(CancellationSignal? cancellationsignal)
		{
			CancellationSignal = cancellationsignal;

			return this;
		}
		public CursorBuilder WithCancellationToken(CancellationToken? cancellationtoken)
		{
			CancellationToken = cancellationtoken;

			return this;
		}
		public CursorBuilder WithContentResolver(ContentResolver? contentresolver)
		{
			ContentResolver = contentresolver;

			return this;
		}
		public CursorBuilder WithLibraryIdentifiers(ILibrary.IIdentifiers? identifiers, bool exclusive = true)
		{
			Identifiers = identifiers;
			IdentifiersExclusive = exclusive;

			return this;
		}									
		public CursorBuilder WithProjection(params string[] projection)
		{
			Projection = projection;

			return this;
		}
		public CursorBuilder WithRetriever<TRetriever>(TRetriever? retriever) where TRetriever : class
		{
			IEnumerable<string> projection = Projection?.AsEnumerable() ?? Enumerable.Empty<string>();

			switch (true)
			{						  
				case true when retriever is IAlbum<bool> albumretriever:
					Projection = projection.Concat(albumretriever.ToProjection())
						.Distinct()
						.ToArray();
					break;
									  
				case true when retriever is IArtist<bool> artistretriever:
					Projection = projection.Concat(artistretriever.ToProjection())
						.Distinct()
						.ToArray();
					break;
									  
				case true when retriever is IGenre<bool> genreretriever:
					Projection = projection.Concat(genreretriever.ToProjection())
						.Distinct()
						.ToArray();
					break;
									  
				case true when retriever is IPlaylist<bool> playlistretriever:
					Projection = projection.Concat(playlistretriever.ToProjection())
						.Distinct()
						.ToArray();
					break;
									  
				case true when retriever is ISong<bool> songretriever:
					Projection = projection.Concat(songretriever.ToProjection())
						.Distinct()
						.ToArray();
					break;

				default: break;
			}

			return this;
		}				   
		public CursorBuilder WithSelection(string? selection)
		{
			Selection = selection;

			return this;
		}
		public CursorBuilder WithSelectionArgs(string[]? selectionargs)
		{
			SelectionArgs = selectionargs;

			return this;
		}				  								  
		public CursorBuilder WithSettings(ILibrary.ISettings? settings)
		{
			Settings = settings;

			return this;
		}									  
		public CursorBuilder WithSortOrder(string? sortorder)
		{
			SortOrder = sortorder;

			return this;
		}
		public CursorBuilder WithUri(AndroidUri? uri)
		{
			Uri = uri;

			return this;
		}
	}
}