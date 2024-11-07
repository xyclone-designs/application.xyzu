using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Settings.Files
{
	public interface IFilesSettings<T> : ISettings<T> 
	{
		T TrackLengthIgnore { get; set; }
		T Directories { get; set; }
		T DirectoriesExclude { get; set; }
		T Mimetypes { get; set; }
	}
	public interface IFilesSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(IFilesSettings);
			
			public const string TrackLengthIgnore = Base + "." + nameof(TrackLengthIgnore);
			public const string Directories = Base + "." + nameof(Directories);
			public const string DirectoriesExclude = Base + "." + nameof(DirectoriesExclude);
			public const string Mimetypes = Base + "." + nameof(Mimetypes);
		}
		public new class Defaults : ISettings.Defaults
		{
			public const int TrackLengthIgnore = 5;
			public static readonly IEnumerable<string> Directories = Enumerable.Empty<string>();										
			public static readonly IEnumerable<string> DirectoriesExclude = Enumerable.Empty<string>();										
			public static readonly IEnumerable<MimeTypes> Mimetypes = Enum
				.GetValues(typeof(MimeTypes))
				.Cast<MimeTypes>();

			public static readonly IFilesSettings FilesSettings = new Default
			{
				Directories = Directories,
				DirectoriesExclude = DirectoriesExclude,
				Mimetypes = Mimetypes,
				TrackLengthIgnore = TrackLengthIgnore,
			};
		}			  
		public new class Options : ISettings.Options
		{
			public class Mimetypes
			{
				public const MimeTypes aac = MimeTypes.aac;
				public const MimeTypes flac = MimeTypes.flac;
				public const MimeTypes m4a = MimeTypes.m4a;
				public const MimeTypes mp3 = MimeTypes.mp3;
				public const MimeTypes wav = MimeTypes.wav;

				public static IEnumerable<MimeTypes> AsEnumerable()
				{
					return Enumerable.Empty<MimeTypes>()
						.Append(aac)
						.Append(flac)
						.Append(m4a)
						.Append(mp3)
						.Append(wav);
				}
			}
		}

		int TrackLengthIgnore { get; set; }
		IEnumerable<string> Directories { get; set; }
		IEnumerable<string> DirectoriesExclude { get; set; }
		IEnumerable<MimeTypes> Mimetypes { get; set; }

		public new class Default : ISettings.Default, IFilesSettings 
		{
			public Default()
			{
				_Directories = Enumerable.Empty<string>();
				_DirectoriesExclude = Enumerable.Empty<string>();
				_Mimetypes = Enumerable.Empty<MimeTypes>();
			}

			private int _TrackLengthIgnore;
			private IEnumerable<string> _Directories;
			private IEnumerable<string> _DirectoriesExclude;
			private IEnumerable<MimeTypes> _Mimetypes;

			public int TrackLengthIgnore
			{
				get => _TrackLengthIgnore;
				set
				{
					_TrackLengthIgnore = value;

					OnPropertyChanged();
				}
			}
			public IEnumerable<string> Directories
			{
				get => _Directories;
				set
				{
					_Directories = value;

					OnPropertyChanged();
				}
			}
			public IEnumerable<string> DirectoriesExclude
			{
				get => _DirectoriesExclude;
				set
				{
					_DirectoriesExclude = value;

					OnPropertyChanged();
				}
			}
			public IEnumerable<MimeTypes> Mimetypes
			{
				get => _Mimetypes;
				set
				{
					_Mimetypes = value;

					OnPropertyChanged();
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, IFilesSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				TrackLengthIgnore = defaultvalue;
				Directories = defaultvalue;
				DirectoriesExclude = defaultvalue;
				Mimetypes = defaultvalue;
			}

			public T TrackLengthIgnore { get; set; }
			public T Directories { get; set; }
			public T DirectoriesExclude { get; set; }
			public T Mimetypes { get; set; }
		}
	}
}
