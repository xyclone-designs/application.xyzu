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
		T Mimetypes { get; set; }
	}
	public interface IFilesSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(IFilesSettings);
			
			public const string TrackLengthIgnore = Base + "." + nameof(TrackLengthIgnore);
			public const string Directories = Base + "." + nameof(Directories);
			public const string Mimetypes = Base + "." + nameof(Mimetypes);
		}
		public new class Defaults : ISettings.Defaults
		{
			public const int TrackLengthIgnore = 5;
			public static readonly IEnumerable<string> Directories = Enumerable.Empty<string>();										
			public static readonly IEnumerable<MimeTypes> Mimetypes = Enum
				.GetValues(typeof(MimeTypes))
				.Cast<MimeTypes>();

			public static readonly IFilesSettings FilesSettings = new Default
			{
				Directories = Directories,
				Mimetypes = Mimetypes,
				TrackLengthIgnore = TrackLengthIgnore,
			};
		}			  
		public new class Options : ISettings.Options
		{
			public class Mimetypes
			{
				public const MimeTypes AAC = MimeTypes.AAC;
				public const MimeTypes FLAC = MimeTypes.FLAC;
				public const MimeTypes M4A = MimeTypes.M4A;
				public const MimeTypes MP3 = MimeTypes.MP3;
				public const MimeTypes MP4 = MimeTypes.MP4;
				public const MimeTypes WAV = MimeTypes.WAV;

				public static IEnumerable<MimeTypes> AsEnumerable()
				{
					return Enumerable.Empty<MimeTypes>()
						.Append(AAC)
						.Append(FLAC)
						.Append(M4A)
						.Append(MP3)
						.Append(MP4)
						.Append(WAV);
				}
			}
		}

		int TrackLengthIgnore { get; set; }
		IEnumerable<string> Directories { get; set; }
		IEnumerable<MimeTypes> Mimetypes { get; set; }

		public new class Default : ISettings.Default, IFilesSettings 
		{
			public Default()
			{
				_Directories = Enumerable.Empty<string>();
				_Mimetypes = Enumerable.Empty<MimeTypes>();
			}

			private int _TrackLengthIgnore;
			private IEnumerable<string> _Directories;
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
				Mimetypes = defaultvalue;
			}

			public T TrackLengthIgnore { get; set; }
			public T Directories { get; set; }
			public T Mimetypes { get; set; }
		}
	}
}
