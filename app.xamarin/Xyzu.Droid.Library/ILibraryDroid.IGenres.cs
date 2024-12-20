﻿using System;
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
		public partial class Default : IGenres
		{
			IGenre? IGenres.Random(IIdentifiers? identifiers)
			{
				IEnumerable<IGenre> genres = identifiers is null
					? SQLiteLibrary.GenresTable
					: SQLiteLibrary.GenresTable.AsEnumerable().Where(_ => identifiers.MatchesGenre(_));

				Random random = new();
				int index = random.Next(0, genres.Count() - 1);
				IGenre genre = genres.ElementAt(index);

				return genre;
			}
			async Task<IGenre?> IGenres.Random(IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				IEnumerable<IGenre> genres = identifiers is null
					? SQLiteLibrary.GenresTable
					: SQLiteLibrary.GenresTable.AsEnumerable().Where(_ => identifiers.MatchesGenre(_));

				Random random = new();
				int index = random.Next(0, genres.Count() - 1);
				IGenre genre = await Task.FromResult(genres.ElementAt(index));

				return genre;
			}

			IGenre? IGenres.GetGenre(IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IGenre? genre = SQLiteLibrary.GenresTable.AsEnumerable().FirstOrDefault(genre => identifiers.MatchesGenre(genre));

				return genre;
			}
			async Task<IGenre?> IGenres.GetGenre(IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IGenre? genre = SQLiteLibrary.GenresTable.AsEnumerable().FirstOrDefault(genre => identifiers.MatchesGenre(genre));

				return await Task.FromResult(genre);
			}
			IEnumerable<IGenre> IGenres.GetGenres(IIdentifiers? identifiers)
			{
				IEnumerable<IGenre> genres = identifiers is null
					? SQLiteLibrary.GenresTable
					: SQLiteLibrary.GenresTable.AsEnumerable().Where(_ => identifiers.MatchesGenre(_));

				foreach (IGenre genre in genres)
					yield return genre;
			}
			async IAsyncEnumerable<IGenre> IGenres.GetGenres(IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				IEnumerable<IGenre> genres = identifiers is null
					? SQLiteLibrary.GenresTable
					: SQLiteLibrary.GenresTable.AsEnumerable().Where(_ => identifiers.MatchesGenre(_));

				foreach (IGenre genre in genres)
					yield return await Task.FromResult(genre);
			}

			bool IGenres.DeleteGenre(IGenre genre)
			{
				if (Actions?.OnDelete != null)
					foreach (IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Genre(genre);

				GenreEntity genreentity = genre as GenreEntity ?? new GenreEntity(genre);

				SQLiteLibrary.Connection.Delete(genreentity);

				return true;
			}
			async Task<bool> IGenres.DeleteGenre(IGenre genre, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Genre(genre)));

				GenreEntity genreentity = genre as GenreEntity ?? new GenreEntity(genre);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(genreentity);

				return true;
			}

			bool IGenres.UpdateGenre(IGenre old, IGenre updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Genre(old, updated);

				GenreEntity genreentity = updated as GenreEntity ?? new GenreEntity(updated);

				SQLiteLibrary.Connection.Update(genreentity);

				return true;
			}
			async Task<bool> IGenres.UpdateGenre(IGenre old, IGenre updated, CancellationToken cancellationToken)
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