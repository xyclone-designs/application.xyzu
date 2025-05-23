﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Xyzu.Settings
{
	public partial interface ISettings
	{				  
		public interface IPresetable<T> : ISettings where T : IPreset
		{
			bool IsEnabled { get; set; }
			T CurrentPreset { get; set; }
			IEnumerable<T> AllPresets { get; set; }
		}
		public interface IPresetable : ISettings
		{
			public new class Default<T> : Default, IPresetable<T> where T : class, IPreset
			{
				public Default(T currentpreset, IEnumerable<T>? allpresets)
				{
					_CurrentPreset = currentpreset;
					_AllPresets = allpresets ?? Enumerable.Empty<T>()
						.Append(currentpreset);
				}

				private bool _IsEnabled;	  
				private T _CurrentPreset;	  
				private IEnumerable<T> _AllPresets;

				public bool IsEnabled
				{
					get => _IsEnabled;
					set
					{
						_IsEnabled = value;

						OnPropertyChanged();
					}
				}	 
				public T CurrentPreset
				{
					get => _CurrentPreset;
					set
					{
						if (_CurrentPreset is not null)
							_CurrentPreset.PropertyChanged -= CurrentPresetPropertyChanged; 

						_CurrentPreset = value;

						if (_CurrentPreset is not null)
							_CurrentPreset.PropertyChanged += CurrentPresetPropertyChanged;

						OnPropertyChanged();
					}
				}

				private void CurrentPresetPropertyChanged(object? sender, PropertyChangedEventArgs args)
				{
					OnPropertyChanged(args.PropertyName);
				}

				public IEnumerable<T> AllPresets
				{
					get => _AllPresets;
					set
					{
						_AllPresets = value;

						OnPropertyChanged();
					}
				}
			}
		}
	}
}
