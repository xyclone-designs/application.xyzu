#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library;
using Xyzu.Library.Models;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : ILibrary.IGenres
	{
		IGenre? ILibrary.IGenres.Random(ILibrary.IIdentifiers? identifiers, IGenre<bool>? retriever)
		{
			IEnumerable<IGenre> genres = SqliteGenresTableQuery;

			if (identifiers != null)
				genres = genres.Where(genre => identifiers.MatchesGenre(genre));

			Random random = new Random();
			int index = random.Next(0, genres.Count() - 1);
			IGenre genre = genres.ElementAt(index);

			return genre;
		}
		async Task<IGenre?> ILibrary.IGenres.Random(ILibrary.IIdentifiers? identifiers, IGenre<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IGenres).Random(identifiers, retriever);
			});
		}

		IGenre? ILibrary.IGenres.GetGenre(ILibrary.IIdentifiers? identifiers, IGenre<bool>? retriever)
		{
			if (identifiers is null)
				return null;

			IEnumerable<IGenre> genres = SqliteGenresTableQuery;
			IGenre? genre = genres.FirstOrDefault(genre => identifiers.MatchesGenre(genre));

			return genre;
		}
		async Task<IGenre?> ILibrary.IGenres.GetGenre(ILibrary.IIdentifiers? identifiers, IGenre<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IGenres).GetGenre(identifiers, retriever);
			});
		}
		IEnumerable<IGenre> ILibrary.IGenres.GetGenres(ILibrary.IIdentifiers? identifiers, IGenre<bool>? retriever)
		{
			IEnumerable<IGenre> genres = SqliteGenresTableQuery;

			if (identifiers != null)
				genres = genres.Where(genre => identifiers.MatchesGenre(genre));

			if (retriever != null)
				genres = genres.Select(genre =>
				{
					return genre;
				});

			foreach (IGenre genre in genres)
				yield return genre;
		}
		async IAsyncEnumerable<IGenre> ILibrary.IGenres.GetGenres(ILibrary.IIdentifiers? identifiers, IGenre<bool>? retriever, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			IEnumerable<IGenre> genres = await Task.Run(() =>
			{
				return (this as ILibrary.IGenres).GetGenres(identifiers, retriever);
			});

			foreach (IGenre genre in genres)
				yield return genre;
		}

		IGenre? ILibrary.IGenres.PopulateGenre(IGenre? genre)
		{
			if (genre is null)
				return null;

			IEnumerable<IGenre> genres = SqliteGenresTableQuery;

			if (genres.FirstOrDefault(predicate: sqlgenre => sqlgenre.Id == genre.Id) is GenreEntity genreentity)
				genre.Populate(genreentity);

			return genre;
		}
		async Task<IGenre?> ILibrary.IGenres.PopulateGenre(IGenre? genre, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IGenres).PopulateGenre(genre);
			});
		}
		IEnumerable<IGenre>? ILibrary.IGenres.PopulateGenres(IEnumerable<IGenre>? genres)
		{
			if (genres is null)
				return null;

			foreach (IGenre genre in genres)
				(this as ILibrary.IGenres).PopulateGenre(genre);

			return genres;
		}
		async Task<IEnumerable<IGenre>?> ILibrary.IGenres.PopulateGenres(IEnumerable<IGenre>? genres, CancellationToken cancellationToken)
		{
			if (genres is null)
				return null;

			foreach (IGenre genre in genres)
				await (this as ILibrary.IGenres).PopulateGenre(genre, cancellationToken);

			return genres;
		}

		bool ILibrary.IGenres.DeleteGenre(IGenre genre)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Genre(genre);

			GenreEntity genreentity = genre as GenreEntity ?? new GenreEntity(genre);

			SqliteConnection.Delete(genreentity);

			return true;
		}
		async Task<bool> ILibrary.IGenres.DeleteGenre(IGenre genre, CancellationToken cancellationToken)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Genre(genre);

			GenreEntity genreentity = genre as GenreEntity ?? new GenreEntity(genre);

			await SqliteConnectionAsync.DeleteAsync(genreentity);

			return true;
		}

		bool ILibrary.IGenres.UpdateGenre(IGenre old, IGenre updated)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Genre(old, updated);

			GenreEntity genreentity = updated as GenreEntity ?? new GenreEntity(updated);

			SqliteConnection.Update(genreentity);

			return true;
		}
		async Task<bool> ILibrary.IGenres.UpdateGenre(IGenre old, IGenre updated, CancellationToken cancellationToken)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Genre(old, updated);

			GenreEntity genreentity = updated as GenreEntity ?? new GenreEntity(updated);

			await SqliteConnectionAsync.UpdateAsync(genreentity);

			return true;
		}
	}
}