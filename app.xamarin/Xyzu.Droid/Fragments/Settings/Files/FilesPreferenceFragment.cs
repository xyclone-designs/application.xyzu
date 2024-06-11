#nullable enable

using Android.Content;
using Android.OS;
using Android.Provider;
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

using AndroidNetUri = Android.Net.Uri;
using AndroidOSEnvironment = Android.OS.Environment;
using AndroidXPreference = AndroidX.Preference.Preference;
using SystemIOPath = System.IO.Path;
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
		private IEnumerable<string>? _DirectoriesExclude;
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
		public IEnumerable<string> DirectoriesExclude
		{
			get => _DirectoriesExclude ?? Enumerable.Empty<string>();
			set
			{

				_DirectoriesExclude = value;

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

			_FilesSettingsDirectoryPredicate = IFilesSettingsDroid.PredicateDirectories(settings);

			TrackLengthIgnore = settings.TrackLengthIgnore;
			Directories = settings.Directories;
			DirectoriesExclude = settings.DirectoriesExclude;
			Mimetypes = settings.Mimetypes;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				TrackLengthIgnorePreference,
				DirectoriesPreference,
				MimetypesPreference);

			XyzuSettings.Instance
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
					alertdialogbuilder.SetNeutralButton(Resource.String.settings_files_directories_dialog_neutralbutton, DirectoriesPreferenceOnNeutralButton);
					alertdialogbuilder.SetPositiveButton(Resource.String.settings_files_directories_dialog_positivebutton, DirectoriesPreferenceOnPositiveButton);
					alertdialogbuilder.SetOnDismissListener(new DialogInterfaceOnDismissListener
					{
						OnDismissAction = dialoginterface => DirectoriesPreferenceView = null
					});
					alertdialogbuilder.SetView(DirectoriesPreferenceView ??= new RecursiveItemsRecyclerView(Context)
					{
						RecursiveAdapter = new DirectoriesRecyclerView.Adapter(Context)
						{
							Directories = IFilesSettingsDroid.Storages()
								.OrderBy(file => file.AbsolutePath)
								.ToList(),

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

		private void DirectoriesPreferenceOnNeutralButton(object sender, EventArgs args) 
		{
			AppCompatActivity?.RegisterForActivityResult((contract, callback) =>
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
					AndroidNetUri? documenturi = DocumentsContract.BuildDocumentUriUsingTree(intentresult?.Data, intentresult?.Identifier);

					if (documenturi?.PathSegments is null)
						return;

					string path = SystemIOPath.Combine(
						AndroidOSEnvironment.StorageDirectory.AbsolutePath,
						documenturi.PathSegments[1]
							.Replace(':', '/')
							.Replace("primary", "emulated/0"));

					Directories = Directories
						.Append(path)
						.OrderBy(dir => dir);

					DirectoriesPreferenceView?.RecursiveAdapter.NotifyDataSetChanged();
				};

			})?.Launch(null, null);
		}
		private void DirectoriesPreferenceOnPositiveButton(object sender, EventArgs args) { }

		private void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			DirectoriesRecyclerView.ViewHolder viewholder = (DirectoriesRecyclerView.ViewHolder)holder;
			File? file = viewholder.Parent?.Directories?[position];

			if (Context is null || viewholder.Parent is null || file is null) return;

			viewholder.ItemChildrenAdapter = new DirectoriesRecyclerView.Adapter(Context)
			{
				ParentChecked = viewholder.Parent.ParentChecked ?? true,
				ParentLevel = viewholder.Parent.ParentLevel + 1 ?? 0,
				ViewHolderOnBind = OnBindViewHolder,
				Directories = file
					.ListFiles()?
					.Where(file => file.IsDirectory)
					.OrderBy(file => file.AbsolutePath)
					.ToList()
			};

			viewholder.ItemView.Directory = file;
			viewholder.ItemView.DirectoryLevel = viewholder.Parent.ParentLevel + 1 ?? 0;
			viewholder.ItemView.DirectoryTitleText = file.AbsolutePath == IFilesSettingsDroid.IntenalStorageFilespath0 ? "Internal" : null;
			viewholder.ItemView.DirectoryHasChildren = viewholder.ItemChildrenAdapter.Directories?.Any() ?? false;
			viewholder.ItemView.DirectoryIsSelected.Checked = _FilesSettingsDirectoryPredicate?.Invoke(file) ?? true;
			viewholder.ItemView.DirectoryIsSelectedCheckChange = (sender, args) =>
			{
				viewholder.ItemChildrenAdapter.ParentChecked = args.IsChecked;
				viewholder.ItemChildrenAdapter.NotifyDataSetChanged();
			};
		}

		public  class DirectoriesRecyclerView 
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
				public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
				{
					return new ViewHolder(Context)
					{
						Parent = this
					};
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

				public Adapter? Parent { get; set; }

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