﻿using Com.Sothree.Slidinguppanel;

using Xyzu.Library.Models;
using Xyzu.Views.Library;

namespace Xyzu.Activities
{
	public partial class LibraryActivity : ILibrary.INavigatable
	{
		protected void Navigate()
		{
			ActivityCancellationTokenSourceCancel();

			if (SlidingUpPanel != null && SlidingUpPanel.GetPanelState() != SlidingUpPanelLayout.PanelState.Hidden)
				SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);
		}

		public virtual void NavigateModel(IModel? model)
		{
			switch (true)
			{
				case true when model is IAlbum album:
					NavigateAlbum(album);
					break;

				case true when model is IArtist artist:
					NavigateArtist(artist);
					break;

				case true when model is IGenre genre:
					NavigateGenre(genre);
					break;

				case true when model is IPlaylist playlist:
					NavigatePlaylist(playlist);
					break;

				case true when model is ISong song:
					NavigateSong(song);
					break;

				default:
					break;
			}
		}
		public virtual void NavigateAlbum(IAlbum? album)
		{
			Navigate();
		}
		public virtual void NavigateArtist(IArtist? artist)
		{
			Navigate();
		}
		public virtual void NavigateGenre(IGenre? genre)
		{
			Navigate();
		}
		public virtual void NavigatePlaylist(IPlaylist? playlist)
		{
			Navigate();
		}
		public virtual void NavigateQueue()
		{
			Navigate();
		}
		public virtual void NavigateSearch()
		{
			Navigate();
		}
		public virtual void NavigateSong(ISong? song)
		{
			Navigate();

			if (SlidingUpPanel != null && SlidingUpPanel.GetPanelState() != SlidingUpPanelLayout.PanelState.Hidden)
				SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
		}
	}
}