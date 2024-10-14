using System;

using Xyzu.Library.Models;

namespace Xyzu.Views.Library
{
	public interface ILibrary 
	{
		INavigatable? Navigatable { get; set; }

		public interface INavigatable
		{
			public Action? OnNavigateQueue => NavigateQueue;
			public Action<IModel?>? OnNavigateModel => NavigateModel;
			public Action<IAlbum?>? OnNavigateAlbum => NavigateAlbum;
			public Action<IArtist?>? OnNavigateArtist => NavigateArtist;
			public Action<IGenre?>? OnNavigateGenre => NavigateGenre;
			public Action<IPlaylist?>? OnNavigatePlaylist => NavigatePlaylist;
			public Action<ISong?>? OnNavigateSong => NavigateSong;

			void NavigateQueue();
			void NavigateModel(IModel? model);
			void NavigateAlbum(IAlbum? album);
			void NavigateArtist(IArtist? artist);
			void NavigateGenre(IGenre? genre);
			void NavigatePlaylist(IPlaylist? playlist);
			void NavigateSong(ISong? song);
		}
	}
}
