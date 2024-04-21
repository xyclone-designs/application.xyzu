#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public static class LanguageModesExtensions
	{  
		public static int AsResoureIdTitle(this LanguageModes languagemode)
		{
			return languagemode switch
			{
				LanguageModes.FollowSystem => Resource.String.enums_languagemodes_followsystem_title,
				LanguageModes.ForceChosen => Resource.String.enums_languagemodes_forcechosen_title,

				_ => throw new ArgumentException(string.Format("Invalid LanguageModes '{0}'", languagemode))
			};
		}	
		public static int AsResoureIdDescription(this LanguageModes languagemode)
		{
			return languagemode switch
			{
				LanguageModes.FollowSystem => Resource.String.enums_languagemodes_followsystem_description,
				LanguageModes.ForceChosen => Resource.String.enums_languagemodes_forcechosen_description,

				_ => throw new ArgumentException(string.Format("Invalid LanguageModes '{0}'", languagemode))
			};
		}
		public static string? AsStringTitle(this LanguageModes languagemodes, Context? context)
		{
			if (languagemodes.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this LanguageModes languagemodes, Context? context)
		{
			if (languagemodes.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}