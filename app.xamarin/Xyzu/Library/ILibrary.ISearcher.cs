using Xyzu.Library.Enums;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface ISearcher<T>
		{
			T String { get; set; }

			T SearchAlbums { get; set; }
			T SearchArtists { get; set; }
			T SearchGenres { get; set; }
			T SearchPlaylists { get; set; }
			T SearchSongs { get; set; }
		}
		public interface ISearcher 
		{
			string String { get; set; }

			bool SearchAlbums { get; set; } 
			bool SearchArtists { get; set; }
			bool SearchGenres { get; set; }
			bool SearchPlaylists { get; set; }
			bool SearchSongs { get; set; }
			
			public class Default : ISearcher
			{
				public string String { get; set; } = string.Empty;

				public bool SearchAlbums { get; set; } = true;
				public bool SearchArtists { get; set; } = true;
				public bool SearchGenres { get; set; } = true;
				public bool SearchPlaylists { get; set; } = true;
				public bool SearchSongs { get; set; } = true;
			}	   
			public class Default<T> : ISearcher<T>
			{
				public Default(T defaultvalue)
				{
					String = defaultvalue;
					SearchAlbums = defaultvalue;
					SearchArtists = defaultvalue;
					SearchGenres = defaultvalue;
					SearchPlaylists = defaultvalue;
					SearchSongs = defaultvalue;
				}

				public T String { get; set; }

				public T SearchAlbums { get; set; }
				public T SearchArtists { get; set; }
				public T SearchGenres { get; set; }
				public T SearchPlaylists { get; set; }
				public T SearchSongs { get; set; }
			}
		}		
		public interface ISearchResult
		{
			string Id { get; }
			ModelTypes ModelType { get; }

			public class Default : ISearchResult
			{
				public Default(string id, ModelTypes modeltype)
				{
					Id = id;
					ModelType = modeltype;
				}

				public string Id { get; }
				public ModelTypes ModelType { get; }
			}
		}
	}
}
