using System.Linq;

using Xyzu.Library.Models;

using Laerdal.FFmpeg.Android;
using System.Threading.Tasks;
using System;

namespace Xyzu.Library.FFProbe
{
	public partial class FFProbeActions
	{
		public class OnCreate : ILibrary.IOnCreateActions.Default 
		{
			public override async Task<ISong?> Song(string id)
			{
				await Task.CompletedTask;

				Paths ??= OnPaths?.Invoke();

				if (Paths?[id] is not string filepath)
					return null;

				MediaInformation mediainformation = FFprobe.GetMediaInformation(filepath);

				return new ISong.Default(id)
				{
					Filepath = filepath,
					IsCorrupt = mediainformation.Format.Contains(filepath.Split('.').Last()) is false,
					Uri = Uri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out Uri? outuri) ? outuri : null,
				};
			}
		}
		public class OnDelete : ILibrary.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default { }
		public class OnUpdate : ILibrary.IOnUpdateActions.Default { }
	}
}
