using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
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
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);
		}

		private IGenre? _Genre;
		private AppCompatEditText? _GenreName;

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

		public AppCompatEditText GenreName
		{
			get => _GenreName
				??= FindViewById<AppCompatEditText>(Ids.GenreName_Value) ??
				throw new InflateException("GenreName_Value");
		}
	}
}