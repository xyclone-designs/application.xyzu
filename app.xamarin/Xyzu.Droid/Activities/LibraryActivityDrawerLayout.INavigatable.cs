#nullable enable

using Xyzu.Droid;
using Xyzu.Library.Models;

using JavaRunnable = Java.Lang.Runnable;

namespace Xyzu.Activities
{
	public partial class LibraryActivityDrawerLayout 
	{
		public override void NavigateAlbum(IAlbum? album)
		{
			base.NavigateAlbum(album);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryAlbum, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryAlbum(album))
				.Commit();
		}
		public override void NavigateArtist(IArtist? artist)
		{
			base.NavigateArtist(artist);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryArtist, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryArtist(artist))
				.Commit();
		}
		public override void NavigateGenre(IGenre? genre)
		{
			base.NavigateGenre(genre);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryGenre, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryGenre(genre))
				.Commit();
		}
		public override void NavigatePlaylist(IPlaylist? playlist)
		{
			base.NavigatePlaylist(playlist);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryPlaylist, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryPlaylist(playlist))
				.Commit();
		}
		public override void NavigateQueue()
		{
			base.NavigateQueue();

			ToDrawerLayoutable(FragmentLibraryQueue);
		}
		public override void NavigateSearch()
		{
			base.NavigateSearch();

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibrarySearch, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, FragmentLibrarySearch)
				.Commit();
		}
	}
}