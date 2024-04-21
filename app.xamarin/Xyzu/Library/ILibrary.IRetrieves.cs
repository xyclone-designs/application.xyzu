using System;
using System.Collections.Generic;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary 
	{
		public interface IRetrieves
		{
			Func<IAlbum, IAlbum<bool>?, IAlbum>? RetrievedAlbum { get; set; }
			Func<IArtist, IArtist<bool>?, IArtist>? RetrievedArtist { get; set; }
			Func<IGenre, IGenre<bool>?, IGenre>? RetrievedGenre { get; set; }
			Func<IPlaylist, IPlaylist<bool>?, IPlaylist>? RetrievedPlaylist { get; set; }
			Func<ISong, ISong<bool>?, ISong>? RetrievedSong { get; set; }
		}		  
		public interface IRetrieves<T> : IRetrieves
		{
			public Func<IIdentifiers?, T?>? GetSingle { get; set; }
			public Func<IIdentifiers?, IEnumerable<T>>? GetMultiple { get; set; }
		}
	}
}
