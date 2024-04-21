using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface IMisc
		{
			void SetImage(IModel? model);
			Task SetImage(IModel? model, CancellationToken cancellationToken = default);

			void SetImages(IEnumerable<IModel> models);
			Task SetImages(IEnumerable<IModel> models, CancellationToken cancellationToken = default);

			IImage? GetImage(IIdentifiers? identifiers, params ModelTypes[] modeltypes);
			Task<IImage?> GetImage(IIdentifiers? identifiers, CancellationToken cancellationToken = default, params ModelTypes[] modeltypes);		   

			IEnumerable<ISearchResult> Search(ISearcher searcher);
			IAsyncEnumerable<ISearchResult> SearchAsync(ISearcher searcher, CancellationToken cancellationtoken = default);
		}
	}
}
