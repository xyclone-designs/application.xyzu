using Laerdal.FFmpeg.Android;

using System;
using System.Threading.Tasks;

using Xyzu.Library.Models;

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
					Uri = Uri.TryCreate(id, UriKind.Absolute, out Uri? _uri) ? _uri : null,

				}.Retrieve(mediainformation);
			}
		}
		public class OnDelete : ILibrary.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default { }
		public class OnUpdate : ILibrary.IOnUpdateActions.Default { }
	}
}
