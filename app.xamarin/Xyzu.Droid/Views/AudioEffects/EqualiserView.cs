using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;

using System;
using System.ComponentModel;

using Xyzu.Droid;
using Xyzu.Settings.Audio;
using Xyzu.Widgets.Controls;

namespace Xyzu.Views.AudioEffects
{
	[Register("xyzu/views/audioeffects/EqualiserView")]
	public class EqualiserView : AudioEffectsView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_audioeffects_equaliser;

			public const int AudioPlot_PlotView = Resource.Id.xyzu_view_audioeffects_equaliser_audioplot_plotview;
			public const int AudioBands_HorizontalScrollView = Resource.Id.xyzu_view_audioeffects_equaliser_audiobands_horizontalscrollview;
			public const int Preamp_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_preamp_audioband;
			public const int BandOne_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandone_audioband;
			public const int BandTwo_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandtwo_audioband;
			public const int BandThree_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandthree_audioband;
			public const int BandFour_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandfour_audioband;
			public const int BandFive_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandfive_audioband;
			public const int BandSix_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandsix_audioband;
			public const int BandSeven_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandseven_audioband;
			public const int BandEight_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandeight_audioband;
			public const int BandNine_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandnine_audioband;
			public const int BandTen_AudioBand = Resource.Id.xyzu_view_audioeffects_equaliser_bandten_audioband;
		}

		public EqualiserView(Context context) : this(context, null) { }
		public EqualiserView(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_View_AudioEffects_Equliser) { }
		public EqualiserView(Context context, IAttributeSet? attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_AudioEffects_Equliser) { }
		public EqualiserView(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			AudioPlot.SetOnTouchListener(new OnTouchListener
			{
				OnTouchFunc = (view, motionevent) => true,
			});

			Preset = _Preset;
		}

		protected PlotView? _AudioPlot;
		protected HorizontalScrollView? _AudioBands;
		protected View? _AudioBandsScrollbar;
		protected AudioBand? _Preamp;
		protected AudioBand? _BandOne; protected DataPoint _BandOneDataPoint;
		protected AudioBand? _BandTwo; protected DataPoint _BandTwoDataPoint;
		protected AudioBand? _BandThree; protected DataPoint _BandThreeDataPoint;
		protected AudioBand? _BandFour; protected DataPoint _BandFourDataPoint;
		protected AudioBand? _BandFive; protected DataPoint _BandFiveDataPoint;
		protected AudioBand? _BandSix; protected DataPoint _BandSixDataPoint;
		protected AudioBand? _BandSeven; protected DataPoint _BandSevenDataPoint;
		protected AudioBand? _BandEight; protected DataPoint _BandEightDataPoint;
		protected AudioBand? _BandNine; protected DataPoint _BandNineDataPoint;
		protected AudioBand? _BandTen; protected DataPoint _BandTenDataPoint;
		protected bool _PresetSwapped = false;
		protected IEqualiserSettings.IPreset _Preset = new IEqualiserSettings.IPreset.Default(string.Empty);
		protected IEqualiserSettings.IPreset? _PresetEdited = null;

		public PlotView AudioPlot
		{
			get => _AudioPlot ??= FindViewById(Ids.AudioPlot_PlotView) as PlotView ?? throw new InflateException("AudioPlot_PlotView");
		}
		public PlotModel AudioPlotModel
		{
			get
			{
				OxyColor border = Resources?.GetColor(Resource.Color.ColorPrimary, Context?.Theme) is Color colorprimary
					? OxyColor.FromArgb(colorprimary.A, colorprimary.R, colorprimary.G, colorprimary.B)
					: OxyColors.Transparent;

				LineSeries lineseries = new()
				{
					MarkerSize = 2,
					MarkerType = MarkerType.Circle,
					Smooth = true,
					Color = border,
					MarkerFill = border,
					MarkerStroke = border,
				};
				AreaSeries areaseries = new()
				{
					Color = OxyColors.Transparent,
					Fill = Resources?.GetColor(Resource.Color.ColorOnPrimarySurface, Context?.Theme) is Color coloronprimarysurface
						? OxyColor.FromArgb(coloronprimarysurface.A, coloronprimarysurface.R, coloronprimarysurface.G, coloronprimarysurface.B)
						: OxyColors.Transparent,
					Smooth = true,
				};

				areaseries.Points.Add(_BandOneDataPoint);
				areaseries.Points.Add(_BandTwoDataPoint);
				areaseries.Points.Add(_BandThreeDataPoint);
				areaseries.Points.Add(_BandFourDataPoint);
				areaseries.Points.Add(_BandFiveDataPoint);
				areaseries.Points.Add(_BandSixDataPoint);
				areaseries.Points.Add(_BandSevenDataPoint);
				areaseries.Points.Add(_BandEightDataPoint);
				areaseries.Points.Add(_BandNineDataPoint);
				areaseries.Points.Add(_BandTenDataPoint);

				lineseries.Points.Add(_BandOneDataPoint);
				lineseries.Points.Add(_BandTwoDataPoint);
				lineseries.Points.Add(_BandThreeDataPoint);
				lineseries.Points.Add(_BandFourDataPoint);
				lineseries.Points.Add(_BandFiveDataPoint);
				lineseries.Points.Add(_BandSixDataPoint);
				lineseries.Points.Add(_BandSevenDataPoint);
				lineseries.Points.Add(_BandEightDataPoint);
				lineseries.Points.Add(_BandNineDataPoint);
				lineseries.Points.Add(_BandTenDataPoint);

				PlotModel plotmodel = new() 
				{
					Padding = new OxyThickness(0),
					PlotAreaBorderThickness = new OxyThickness(0),
					PlotMargins = new OxyThickness(0),
				};

				plotmodel.Series.Add(areaseries);
				plotmodel.Series.Add(lineseries);
				plotmodel.Axes.Add(new LinearAxis { IsAxisVisible = false, Position = AxisPosition.Bottom, });
				plotmodel.Axes.Add(new LinearAxis { IsAxisVisible = false, Position = AxisPosition.Left, Maximum = 100, Minimum = -100 });

				return plotmodel;
			}
		}
		public HorizontalScrollView AudioBands
		{
			get => _AudioBands ??= FindViewById(Ids.AudioBands_HorizontalScrollView) as HorizontalScrollView ?? throw new InflateException("AudioBands_HorizontalScrollView");
		}
		public AudioBand Preamp
		{
			get 
			{
				if (_Preamp is not null) return _Preamp; 
				
				_Preamp = FindViewById(Ids.Preamp_AudioBand) as AudioBand ?? throw new InflateException("Preamp_AudioBand");
				_Preamp.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_Preamp.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_Preamp.OnProgress = (sender, value) => Preset.PreAmp = (float)value / 100;
				_Preamp.OnProgressValue = (value) => string.Format("{0:0.00}", Preset.PreAmp);

				return _Preamp;
			}
		}
		public AudioBand BandOne 
		{
			get 
			{
				if (_BandOne is not null) return _BandOne; 
				
				_BandOne = FindViewById(Ids.BandOne_AudioBand) as AudioBand ?? throw new InflateException("BandOne_AudioBand");
				_BandOne.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandOne.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandOne.OnProgress = (sender, value) =>
				{
					_BandOneDataPoint = new DataPoint(
						x: 00,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[00] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandOne;
			}
		}
		public AudioBand BandTwo 
		{
			get 
			{
				if (_BandTwo is not null) return _BandTwo; 
				
				_BandTwo = FindViewById(Ids.BandTwo_AudioBand) as AudioBand ?? throw new InflateException("BandTwo_AudioBand");
				_BandTwo.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandTwo.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandTwo.OnProgress = (sender, value) =>
				{
					_BandTwoDataPoint = new DataPoint(
						x: 01,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[01] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandTwo;
			}
		}
		public AudioBand BandThree 
		{
			get 
			{
				if (_BandThree is not null) return _BandThree; 
				
				_BandThree = FindViewById(Ids.BandThree_AudioBand) as AudioBand ?? throw new InflateException("BandThree_AudioBand");
				_BandThree.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandThree.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandThree.OnProgress = (sender, value) =>
				{
					_BandThreeDataPoint = new DataPoint(
						x: 02,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[02] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandThree;
			}
		}
		public AudioBand BandFour 
		{
			get 
			{
				if (_BandFour is not null) return _BandFour; 
				
				_BandFour = FindViewById(Ids.BandFour_AudioBand) as AudioBand ?? throw new InflateException("BandFour_AudioBand");
				_BandFour.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandFour.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandFour.OnProgress = (sender, value) =>
				{
					_BandFourDataPoint = new DataPoint(
						x: 03,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[03] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandFour;
			}
		}
		public AudioBand BandFive 
		{
			get 
			{
				if (_BandFive is not null) return _BandFive; 
				
				_BandFive = FindViewById(Ids.BandFive_AudioBand) as AudioBand ?? throw new InflateException("BandFive_AudioBand");
				_BandFive.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandFive.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandFive.OnProgress = (sender, value) =>
				{
					_BandFiveDataPoint = new DataPoint(
						x: 04,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[04] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandFive;
			}
		}
		public AudioBand BandSix 
		{
			get 
			{
				if (_BandSix is not null) return _BandSix; 
				
				_BandSix = FindViewById(Ids.BandSix_AudioBand) as AudioBand ?? throw new InflateException("BandSix_AudioBand");
				_BandSix.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandSix.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandSix.OnProgress = (sender, value) =>
				{
					_BandSixDataPoint = new DataPoint(
						x: 05,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[05] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandSix;
			}
		}
		public AudioBand BandSeven 
		{
			get 
			{
				if (_BandSeven is not null) return _BandSeven; 
				
				_BandSeven = FindViewById(Ids.BandSeven_AudioBand) as AudioBand ?? throw new InflateException("BandSeven_AudioBand");
				_BandSeven.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandSeven.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandSeven.OnProgress = (sender, value) =>
				{
					_BandSevenDataPoint = new DataPoint(
						x: 06,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[06] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandSeven;
			}
		}
		public AudioBand BandEight 
		{
			get 
			{
				if (_BandEight is not null) return _BandEight; 
				
				_BandEight = FindViewById(Ids.BandEight_AudioBand) as AudioBand ?? throw new InflateException("BandEight_AudioBand");
				_BandEight.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandEight.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandEight.OnProgress = (sender, value) =>
				{
					_BandEightDataPoint = new DataPoint(
						x: 07,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[07] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandEight;
			}
		}
		public AudioBand BandNine 
		{
			get 
			{
				if (_BandNine is not null) return _BandNine; 
				
				_BandNine = FindViewById(Ids.BandNine_AudioBand) as AudioBand ?? throw new InflateException("BandNine_AudioBand");
				_BandNine.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandNine.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandNine.OnProgress = (sender, value) =>
				{
					_BandNineDataPoint = new DataPoint(
						x: 08,
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[08] = (short)value);

					AudioPlot.Model = AudioPlotModel;
				};

				return _BandNine;
			}
		}
		public AudioBand BandTen 
		{
			get 
			{
				if (_BandTen is not null) return _BandTen; 
				
				_BandTen = FindViewById(Ids.BandTen_AudioBand) as AudioBand ?? throw new InflateException("BandTen_AudioBand");
				_BandTen.Value.ValueMin = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
				_BandTen.Value.ValueMax = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
				_BandTen.OnProgress = (sender, value) =>
				{
					_BandTenDataPoint = new DataPoint(
						x: 09, 
						y: (PresetEdited ??= new IEqualiserSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset).FrequencyLevels[09] = (short)value); 

					AudioPlot.Model = AudioPlotModel; 
				};
				
				return _BandTen;
			}
		}
		public IEqualiserSettings.IPreset Preset
		{
			get => _Preset;
			set
			{
				_Preset.PropertyChanged -= PresetPropertyChanged;
				_Preset = value;
				_PresetEdited = null;
				_Preset.PropertyChanged += PresetPropertyChanged;

				Preamp.SetValue((int)(_Preset.PreAmp * 100));
				BandOne.SetValue(_Preset.FrequencyLevels[00]);
				BandTwo.SetValue(_Preset.FrequencyLevels[01]);
				BandThree.SetValue(_Preset.FrequencyLevels[02]);
				BandFour.SetValue(_Preset.FrequencyLevels[03]);
				BandFive.SetValue(_Preset.FrequencyLevels[04]);
				BandSix.SetValue(_Preset.FrequencyLevels[05]);
				BandSeven.SetValue(_Preset.FrequencyLevels[06]);
				BandEight.SetValue(_Preset.FrequencyLevels[07]);
				BandNine.SetValue(_Preset.FrequencyLevels[08]);
				BandTen.SetValue(_Preset.FrequencyLevels[09]);

				AudioPlot.Model = AudioPlotModel;
			}
		}
		public IEqualiserSettings.IPreset? PresetEdited
		{
			get => _PresetEdited;
			set
			{
				if (_PresetEdited is not null)
					_PresetEdited.PropertyChanged -= PresetPropertyChanged;

				_PresetEdited = value;

				if (_PresetEdited is not null)
					_PresetEdited.PropertyChanged += PresetPropertyChanged;

				PresetPropertyChanged(this, new PropertyChangedEventArgs(nameof(PresetEdited)));
			}
		}

		public Action<object?, PropertyChangedEventArgs>? OnPresetPropertyChanged { get; set; }

		private void PresetPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			OnPresetPropertyChanged?.Invoke(sender, args);
		}

		public class AudioBandHolder : RecyclerViewViewHolderDefault
		{
			private static AudioBand ItemViewDefault(Context context)
			{
				ContextThemeWrapper contextthemewrapper = new(context, Resource.Style.Xyzu_View_AudioEffects_AudioBand);
				AudioBand itemview = new(contextthemewrapper)
				{
					LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent)
				};

				return itemview;
			}

			public AudioBandHolder(Context context) : this(ItemViewDefault(context)) { }
			public AudioBandHolder(AudioBand itemView) : base(itemView) { }

			public new AudioBand ItemView
			{
				set => base.ItemView = value;
				get => (AudioBand)base.ItemView;
			}
		}
	}
}