#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using Java.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Settings.Enums;
using Xyzu.Settings.Files;
using Xyzu.Views.Setting;
using Xyzu.Widgets.RecyclerViews;

using AndroidOSEnvironment = Android.OS.Environment;
using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuDialogPreference = Xyzu.Preference.DialogPreference;
using XyzuSeekBarPreference = Xyzu.Preference.SeekBarPreference;
using XyzuMultiSelectListPreference = Xyzu.Preference.MultiSelectListPreference;

namespace Xyzu.Fragments.Settings.Files
{
	[Register(FragmentName)]
	public class FilesPreferenceFragment : BasePreferenceFragment, IFilesSettingsDroid
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Files.FilesPreferenceFragment";

		public static class Keys
		{
			public const string TrackLengthIgnore = "settings_system_errorlogs_dialogpreference_key";
			public const string Directories = "settings_files_directories_dialogpreference_key";
			public const string Mimetypes = "settings_files_mimetypes_multiselectlistpreference_key";
		}

		private int _TrackLengthIgnore;
		private IEnumerable<string>? _Directories;
		private IEnumerable<MimeTypes>? _Mimetypes;
		private Func<File, bool>? _FilesSettingsDirectoryPredicate;

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
			get => _Directories ?? Enumerable.Empty<string>();
			set
			{

				_Directories = value;

				OnPropertyChanged();
			}
		}
		public IEnumerable<MimeTypes> Mimetypes
		{
			get => _Mimetypes ?? Enumerable.Empty<MimeTypes>();
			set
			{

				_Mimetypes = value;

				OnPropertyChanged();
			}
		}

		public XyzuDialogPreference? DirectoriesPreference { get; set; }
		public XyzuMultiSelectListPreference? MimetypesPreference { get; set; }
		public XyzuSeekBarPreference? TrackLengthIgnorePreference { get; set; }

		public RecursiveItemsRecyclerView? DirectoriesPreferenceView { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_files_title);

			AddPreferenceChangeHandler(
				TrackLengthIgnorePreference,
				DirectoriesPreference,
				MimetypesPreference);

			IFilesSettingsDroid settings = XyzuSettings.Instance.GetFilesDroid();

			_FilesSettingsDirectoryPredicate = IFilesSettingsDroid.FilesSettingsDirectoryPredicate(settings);

			TrackLengthIgnore = settings.TrackLengthIgnore;
			Directories = settings.Directories;
			Mimetypes = settings.Mimetypes;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				TrackLengthIgnorePreference,
				DirectoriesPreference,
				MimetypesPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutFilesDroid(this)
				.Apply();
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_files, rootKey);
			InitPreferences(
				TrackLengthIgnorePreference = FindPreference(Keys.TrackLengthIgnore) as XyzuSeekBarPreference,
				DirectoriesPreference = FindPreference(Keys.Directories) as XyzuDialogPreference,
				MimetypesPreference = FindPreference(Keys.Mimetypes) as XyzuMultiSelectListPreference);

			if (TrackLengthIgnorePreference != null)
			{
				TrackLengthIgnorePreference.Min = 0;
				TrackLengthIgnorePreference.Max = 60;
			}

			if (DirectoriesPreference != null && Context != null)
			{
				DirectoriesPreference.DialogOnBuild = alertdialogbuilder =>
				{
					alertdialogbuilder.SetNeutralButton(Resource.String.settings_files_directories_dialog_neutralbutton, (sender, args) =>
					{
						if (AppCompatActivity is null)
							return;

						AppCompatActivity.RegisterForActivityResult((contract, callback) =>
						{
							contract.CreateIntentAction = (context, input) =>
							{
								Intent actionpickcontent = new Intent(Intent.ActionOpenDocumentTree);

								return actionpickcontent;
							};
							contract.ParseResultAction = (resultcode, intent) =>
							{
								return intent;
							};
							callback.OnActivityResultAction = result =>
							{
								Intent? intentresult = result as Intent;

								if (intentresult is null)
									return;

								if (intentresult.Data?.Path is string directory && Directories.Contains(directory) is false)
									Directories = Directories.Append(directory);

								DirectoriesPreferenceView?.RecursiveAdapter.NotifyDataSetChanged();
							};

						})?.Launch(null, null);
					});

					alertdialogbuilder.SetOnDismissListener(new DialogInterfaceOnDismissListener
					{
						OnDismissAction = dialoginterface => DirectoriesPreferenceView = null
					});
					alertdialogbuilder.SetView(DirectoriesPreferenceView ??= new RecursiveItemsRecyclerView(Context)
					{
						RecursiveAdapter = new DirectoriesRecyclerView.Adapter(Context)
						{
							Directories = AndroidOSEnvironment.StorageDirectory
								.ListFilesSettingsDiretories(XyzuSettings.Instance.GetFilesDroid() ?? IFilesSettingsDroid.Defaults.FilesSettingsDroid, Context)
								.Where(directory =>
								{
									bool predicate = Directories.Any(dir => directory.AbsolutePath.Contains(string.Format("/{0}/", dir), StringComparison.OrdinalIgnoreCase));
									
									return predicate is false;

								}).OrderBy(file => file.AbsolutePath).ToList(),

							ViewHolderOnBind = OnBindViewHolder,
						}
					});

					DirectoriesPreferenceView.RecursiveAdapter.NotifyDataSetChanged();
				};
			}

			MimetypesPreference?.SetDefaultValue(Array.Empty<string>());
			MimetypesPreference?.SetEntries(
				entries: IFilesSettings.Options.Mimetypes.AsEnumerable()
					.Select(mimetype => mimetype.AsStringTitle(Context) ?? mimetype.ToString())
					.ToArray());
			MimetypesPreference?.SetEntryValues(
				entryValues: IFilesSettings.Options.Mimetypes.AsEnumerable()
					.Select(mimetype => mimetype.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(TrackLengthIgnore) when
				TrackLengthIgnorePreference != null &&
				TrackLengthIgnorePreference.Value != TrackLengthIgnore:
					TrackLengthIgnorePreference.Value = TrackLengthIgnore;
					break;

				case nameof(Mimetypes) when
				MimetypesPreference != null:
					MimetypesPreference.Values = Mimetypes.Select(mimetype => mimetype.ToString()).ToList();
					break;

				default: break;
			}
		}
		public override bool OnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			switch (true)
			{
				case true when
				preference == TrackLengthIgnorePreference &&
				newvalue != null:
					TrackLengthIgnore = newvalue.JavaCast<Java.Lang.Integer>().IntValue();
					return true;

				case true when
				preference == MimetypesPreference &&
				Mimetypes.Select(mimetype => mimetype.ToString()).SequenceEqual(MimetypesPreference.Values) is false:
					Mimetypes = MimetypesPreference.Values
						.Select(mimetype => Enum.TryParse(mimetype, out MimeTypes outmimetypes) ? outmimetypes : new MimeTypes?())
						.OfType<MimeTypes>();
					return true;

				default: return result;
			}
		}

		private void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			DirectoriesRecyclerView.ViewHolder viewholder = (DirectoriesRecyclerView.ViewHolder)holder;

			viewholder.ItemChildrenAdapter.Directories = viewholder.ItemView.Directory?
				.ListFiles()?
				.Where(_FilesSettingsDirectoryPredicate)
				.OrderBy(file => file.AbsolutePath)
				.ToList();

			viewholder.ItemView.DirectoryHasChildren = viewholder.ItemChildrenAdapter.Directories?.Any() ?? false;
		}

		public static class DirectoriesRecyclerView 
		{
			public class Adapter : RecursiveItemsRecyclerView.Adapter
			{
				public Adapter(Context context) : base(context) { }
																   
				public IList<File>? Directories { get; set; }
				public bool? ParentChecked { get; set; }
				public int? ParentLevel { get; set; }

				public override int ItemCount
				{
					get => Directories?.Count ?? base.ItemCount;
				}

				public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
				{
					ViewHolder viewholder = (ViewHolder)holder;

					if ((viewholder.ItemView.Directory = Directories?[position]) is null)
						return;

					viewholder.ItemView.DirectoryLevel = ParentLevel + 1 ?? 0;
					viewholder.ItemView.DirectoryIsSelected.Checked = ParentChecked ?? true;
					viewholder.ItemView.DirectoryIsSelectedCheckChange = (sender, args) =>
					{
						viewholder.ItemChildrenAdapter.ParentChecked = args.IsChecked;
						viewholder.ItemChildrenAdapter.NotifyDataSetChanged();
					};

					viewholder.ItemChildrenAdapter = new Adapter(Context)
					{
						ParentChecked = viewholder.ItemView.DirectoryIsSelected.Checked,
						ParentLevel = ParentLevel + 1 ?? 0,
						ViewHolderOnBind = ViewHolderOnBind,
					};

					base.OnBindViewHolder(holder, position);
				}
				public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
				{
					return new ViewHolder(Context);
				}
			}
			public class ViewHolder : RecursiveItemsRecyclerView.ViewHolder
			{
				private static DirectoryItem ItemViewDefault(Context context)
				{
					return new DirectoryItem(context);
				}

				public ViewHolder(Context context) : this(ItemViewDefault(context)) { }
				public ViewHolder(DirectoryItem itemView) : base(itemView) 
				{
					ItemChildren = itemView.DirectoryChildren;
				}

				public new DirectoryItem ItemView
				{
					set => base.ItemView = value;
					get => (DirectoryItem)base.ItemView;
				}			

				public Adapter ItemChildrenAdapter
				{
					set => ItemView.DirectoryChildren.RecursiveAdapter = value;
					get => (Adapter)ItemView.DirectoryChildren.RecursiveAdapter;
				}
			}
		}
	}
}