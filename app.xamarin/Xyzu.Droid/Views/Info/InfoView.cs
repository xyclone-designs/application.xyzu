using Android.Content;
using Android.Graphics;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;

namespace Xyzu.Views.Info
{
	public class InfoView : ConstraintLayout, IInfo
	{
		public InfoView(Context context) : this(context, null!) { }
		public InfoView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Info) { }
		public InfoView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected virtual void Init(Context context, IAttributeSet? attrs) { }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null) 
		{
			switch (propertyname)
			{
				case nameof(Palette):

					if (PaletteTextViews is not null)
					{
						Color? color = Context is null ? null : Palette?.GetColorForBackground(Context, Resource.Color.ColorSurface);

						foreach (KeyValuePair<AppCompatTextView, Color> title in PaletteTextViews)
							title.Key.SetTextColor(color ?? title.Value);
					}

					OnPalette?.Invoke(Palette);
					break;

				default: break;
			}
		}

		private IImages? _Images;
		private Palette? _Palette;
		private Action<Palette?>? _OnPalette;

		protected IDictionary<AppCompatTextView, Color>? PaletteTextViews { get; set; }

		public IImages? Images
		{
			get => _Images;
			set
			{
				_Images = value;

				OnPropertyChanged();
			}
		}
		public Palette? Palette
		{
			get => _Palette;
			set
			{
				_Palette = value;

				OnPropertyChanged();
			}
		}
		public Action<Palette?>? OnPalette
		{
			get => _OnPalette;
			set
			{
				_OnPalette = value;

				OnPropertyChanged();
			}
		}

		public virtual void ReloadImage() { }
		protected virtual void SetPaletteTextViews(params AppCompatTextView?[] palettetextviews)
		{
			PaletteTextViews ??= new Dictionary<AppCompatTextView, Color>();

			foreach (AppCompatTextView palettetextview in palettetextviews.OfType<AppCompatTextView>())
				PaletteTextViews.TryAdd(palettetextview, new Color(palettetextview.CurrentTextColor));
		}
	}
}