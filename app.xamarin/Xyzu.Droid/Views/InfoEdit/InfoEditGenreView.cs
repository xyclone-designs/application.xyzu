#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public class InfoEditGenreView : InfoEditView, IInfoEditGenre
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_infoedit_genre;

			public const int GenreName_Value = Resource.Id.xyzu_view_infoedit_genre_genrename_value_appcompatedittext;
			public const int GenreName_Title = Resource.Id.xyzu_view_infoedit_genre_genrename_title_appcompattextview;
		}

		public InfoEditGenreView(Context context) : base(context) { }
		public InfoEditGenreView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public InfoEditGenreView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(Context, Ids.Layout, this);

			GenreName = FindViewById(Ids.GenreName_Value) as AppCompatEditText;
			GenreName_Title = FindViewById(Ids.GenreName_Title) as AppCompatTextView;
		}

		private IGenre? _Genre;

		public IGenre? Genre
		{
			get
			{
				if (_Genre != null)
				{
					_Genre.Name = GenreName?.Text;
				}

				return _Genre;
			}
			set
			{
				_Genre = value;

				GenreName?.SetText(_Genre?.Name, null);
			}
		}

		public AppCompatEditText? GenreName { get; protected set; }
		public AppCompatTextView? GenreName_Title { get; protected set; }
	}
}