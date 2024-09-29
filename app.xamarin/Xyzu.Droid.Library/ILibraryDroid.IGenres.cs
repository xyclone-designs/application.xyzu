#nullable enable

using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibraryDroid
	{
		public partial class Default : ILibraryDroid.IGenres
		{
			IGenre? ILibraryDroid.IGenres.Random(ILibraryDroid.IIdentifiers? identifiers)
			{
				TableQuery<GenreEntity> genres = identifiers is null
					? SQLiteLibrary.GenresTable
					: SQLiteLibrary.GenresTable.Where(identifiers.MatchesGenre<GenreEntity>());

				Random random = new();
				int index = random.Next(0, genres.Count() - 1);
				IGenre genre = genres.ElementAt(index);

				return genre;
			}
			async Task<IGenre?> ILibraryDroid.IGenres.Random(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				AsyncTableQuery<GenreEntity> genres = identifiers is null
					? SQLiteLibrary.GenresTableAsync
					: SQLiteLibrary.GenresTableAsync.Where(identifiers.MatchesGenre<GenreEntity>());

				Random random = new();
				int index = random.Next(0, await genres.CountAsync() - 1);
				IGenre genre = await genres.ElementAtAsync(index);

				return genre;
			}

			IGenre? ILibraryDroid.IGenres.GetGenre(ILibraryDroid.IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IGenre genre = SQLiteLibrary.GenresTable.FirstOrDefault(identifiers.MatchesGenre<GenreEntity>());

				return genre;
			}
			async Task<IGenre?> ILibraryDroid.IGenres.GetGenre(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IGenre genre = await SQLiteLibrary.GenresTableAsync.FirstOrDefaultAsync(identifiers.MatchesGenre<GenreEntity>());

				return genre;
			}
			IEnumerable<IGenre> ILibraryDroid.IGenres.GetGenres(ILibraryDroid.IIdentifiers? identifiers)
			{
				TableQuery<GenreEntity> genres = identifiers is null
					? SQLiteLibrary.GenresTable
					: SQLiteLibrary.GenresTable.Where(identifiers.MatchesGenre<GenreEntity>());

				foreach (IGenre genre in genres)
					yield return genre;
			}
			async IAsyncEnumerable<IGenre> ILibraryDroid.IGenres.GetGenres(ILibraryDroid.IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				AsyncTableQuery<GenreEntity> genres = identifiers is null
					? SQLiteLibrary.GenresTableAsync
					: SQLiteLibrary.GenresTableAsync.Where(identifiers.MatchesGenre<GenreEntity>());

				foreach (IGenre genre in await genres.ToListAsync())
					yield return genre;
			}

			IGenre? ILibraryDroid.IGenres.PopulateGenre(IGenre? genre)
			{
				if (genre is null)
					return null;

				if (SQLiteLibrary.GenresTable.FirstOrDefault(sqlgenre => sqlgenre.Id == genre.Id) is GenreEntity genreentity)
					genre.Populate(genreentity);

				return genre;
			}
			async Task<IGenre?> ILibraryDroid.IGenres.PopulateGenre(IGenre? genre, CancellationToken cancellationToken)
			{
				if (genre is null)
					return null;

				if (await SQLiteLibrary.GenresTableAsync.FirstOrDefaultAsync(sqlgenre => sqlgenre.Id == genre.Id) is GenreEntity genreentity)
					genre.Populate(genreentity);

				return genre;
			}
			IEnumerable<IGenre>? ILibraryDroid.IGenres.PopulateGenres(IEnumerable<IGenre>? genres)
			{
				if (genres is null)
					return null;

				foreach (IGenre genre in genres)
					(this as ILibraryDroid.IGenres).PopulateGenre(genre);

				return genres;
			}
			async Task<IEnumerable<IGenre>?> ILibraryDroid.IGenres.PopulateGenres(IEnumerable<IGenre>? genres, CancellationToken cancellationToken)
			{
				if (genres is null)
					return null;

				foreach (IGenre genre in genres)
					await (this as ILibraryDroid.IGenres).PopulateGenre(genre, cancellationToken);

				return genres;
			}

			bool ILibraryDroid.IGenres.DeleteGenre(IGenre genre)
			{
				if (Actions?.OnDelete != null)
					foreach (ILibraryDroid.IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Genre(genre);

				GenreEntity genreentity = genre as GenreEntity ?? new GenreEntity(genre);

				SQLiteLibrary.Connection.Delete(genreentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IGenres.DeleteGenre(IGenre genre, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Genre(genre)));

				GenreEntity genreentity = genre as GenreEntity ?? new GenreEntity(genre);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(genreentity);

				return true;
			}

			bool ILibraryDroid.IGenres.UpdateGenre(IGenre old, IGenre updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (ILibraryDroid.IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Genre(old, updated);

				GenreEntity genreentity = updated as GenreEntity ?? new GenreEntity(updated);

				SQLiteLibrary.Connection.Update(genreentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IGenres.UpdateGenre(IGenre old, IGenre updated, CancellationToken cancellationToken)
			{
				if (Actions?.OnUpdate != null)
					await Task.WhenAll(Actions.OnUpdate.Select(_ => _.Genre(old, updated)));

				GenreEntity genreentity = updated as GenreEntity ?? new GenreEntity(updated);

				await SQLiteLibrary.ConnectionAsync.UpdateAsync(genreentity);

				return true;
			}
		}
	}
}