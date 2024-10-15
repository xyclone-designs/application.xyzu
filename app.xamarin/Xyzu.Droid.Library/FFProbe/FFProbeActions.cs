using Laerdal.FFmpeg.Android;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
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
					IsCorrupt = mediainformation.Format.Contains(filepath.Split('.').Last()) is false,
					Uri = Uri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out Uri? outuri) ? outuri : null,
				};
			}
		}
		public class OnDelete : ILibrary.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default 
		{
			public override Task Song(ISong? retrieved)
			{
				if (retrieved?.Filepath is not null)
				{
					MediaInformation mediainformation = FFprobe.GetMediaInformation(retrieved.Filepath);

					retrieved.IsCorrupt = mediainformation.Format.Contains(retrieved.Filepath.Split('.').Last()) is false;
				}

				return base.Song(retrieved);
			}
			public override Task Image(IImage? retrieved, IEnumerable<ModelTypes>? modeltypes)
			{
				if (retrieved?.Filepath is not null)
				{
					MediaInformation mediainformation = FFprobe.GetMediaInformation(retrieved.Filepath);

					retrieved.IsCorrupt = mediainformation.Format.Contains(retrieved.Filepath.Split('.').Last()) is false;
				}

				return base.Image(retrieved, modeltypes);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default { }
	}
}
