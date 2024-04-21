#nullable enable

using Xyzu.Settings.Audio;

namespace Android.Media.Audiofx
{
	public static class EnvironmentalReverbExtensions
	{
		public static EnvironmentalReverb SetSettingsPreset(this EnvironmentalReverb environmentalreverb, IEnvironmentalReverbSettings.IPreset? settings, IEnvironmentalReverbSettings.IPreset? @default = null)
		{
			if ((settings?.DecayHFRatio ?? @default?.DecayHFRatio) is short decayhfratio) 
				environmentalreverb.DecayHFRatio = decayhfratio;

			if ((settings?.DecayTime ?? @default?.DecayTime) is int decaytime) 
				environmentalreverb.DecayTime = decaytime;

			if ((settings?.Density ?? @default?.Density) is short density) 
				environmentalreverb.Density = density;

			if ((settings?.Diffusion ?? @default?.Diffusion) is short diffusion) 
				environmentalreverb.Diffusion = diffusion;

			if ((settings?.ReflectionsDelay ?? @default?.ReflectionsDelay) is int reflectionsdelay) 
				environmentalreverb.ReflectionsDelay = reflectionsdelay;

			if ((settings?.ReflectionsLevel ?? @default?.ReflectionsLevel) is short reflectionslevel) 
				environmentalreverb.ReflectionsLevel = reflectionslevel;

			if ((settings?.ReverbDelay ?? @default?.ReverbDelay) is int reverbdelay) 
				environmentalreverb.ReverbDelay = reverbdelay;

			if ((settings?.ReverbLevel ?? @default?.ReverbLevel) is short reverblevel) 
				environmentalreverb.ReverbLevel = reverblevel;

			if ((settings?.RoomHFLevel ?? @default?.RoomHFLevel) is short roomhflevel) 
				environmentalreverb.RoomHFLevel = roomhflevel;

			if ((settings?.RoomLevel ?? @default?.RoomLevel) is short roomlevel) 
				environmentalreverb.RoomLevel = roomlevel;

			return environmentalreverb;
		}
	}
}