#nullable enable

namespace Xyzu.Library.Models
{
	public enum CursorRetrieveTypes
	{
		/// <summary>
		/// For Retrieval of Id Only
		/// </summary>
		Id,
		/// <summary>
		/// For Retrieval of Id, URI, Filepath and Ids if present
		/// </summary>
		Data,
		/// <summary>
		/// For Retrieval of All Values
		/// </summary>
		All,
	}
}
