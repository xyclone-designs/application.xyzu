using System.Collections.Generic;

using Xyzu.Library.Enums;

namespace Xyzu.Library
{
	public partial interface ILibrary 
	{
		public interface ISettings
		{
			IEnumerable<string>? Directories { get; set; }
			IEnumerable<MimeTypes>? Mimetypes { get; set; }

			public class Default : ISettings
			{
				public IEnumerable<string>? Directories { get; set; }
				public IEnumerable<MimeTypes>? Mimetypes { get; set; }
			}
		}
	}
}