#nullable enable

using Android.Views;
using Android.Widget;

using System;
using System.Linq;

namespace AndroidX.RecyclerView.Widget
{
	public class RecyclerViewViewHolderDefault : RecyclerView.ViewHolder
	{
		public RecyclerViewViewHolderDefault(View itemView) : base(itemView) { }

		private bool CheckedChangeAttatched = false;
		private bool ClickAttatched = false;
		private bool LongClickAttatched = false;

		private event EventHandler<ViewHolderEventArgs>? CheckedChange;
		private event EventHandler<ViewHolderEventArgs>? Click;
		private event EventHandler<ViewHolderEventArgs>? LongClick;
		   
		public event EventHandler<ViewHolderEventArgs> OnCheckedChange
		{
			add
			{
				if (ItemView is CompoundButton itemviewcompoundbutton)
				{
					if (CheckedChangeAttatched is false)
					{
						itemviewcompoundbutton.CheckedChange += ItemView_CheckedChange;
						CheckedChangeAttatched = true;
					}

					CheckedChange += value;
				}
			}
			remove
			{
				if (ItemView is CompoundButton itemviewcompoundbutton)
				{
					CheckedChange -= value;

					if (CheckedChangeAttatched is true && (CheckedChange?.GetInvocationList().Any() ?? false) is true)
					{
						itemviewcompoundbutton.CheckedChange -= ItemView_CheckedChange;
						CheckedChangeAttatched = false;
					}
				}
			}
		}			
		public event EventHandler<ViewHolderEventArgs> OnClick
		{
			add
			{
				if (ClickAttatched is false)
				{
					ItemView.Click += ItemView_Click; 
					ClickAttatched = true;
				}

				Click += value;
			}
			remove
			{
				Click -= value;

				if (ClickAttatched is true && (Click?.GetInvocationList().Any() ?? false) is true)
				{
					ItemView.Click -= ItemView_Click;
					ClickAttatched = false;
				}
			}
		}
		public event EventHandler<ViewHolderEventArgs> OnLongClick
		{
			add
			{
				if (LongClickAttatched is false)
				{
					ItemView.LongClick += ItemView_LongClick;
					LongClickAttatched = true;
				}

				LongClick += value;
			}
			remove
			{
				LongClick -= value;

				if (LongClickAttatched is true && (LongClick?.GetInvocationList().Any() ?? false) is true)
				{
					ItemView.LongClick -= ItemView_LongClick;
					LongClickAttatched = false;
				}
			}
		}

		public Action<ViewHolderEventArgs>? OnCheckedChangeAction { get; set; }
		public Action<ViewHolderEventArgs>? OnClickAction { get; set; }
		public Action<ViewHolderEventArgs>? OnLongClickAction { get; set; }

		protected virtual void ItemView_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
		{
			bool stop =
				CheckedChange is null &&
				OnCheckedChangeAction is null;

			if (stop)
				return;

			ViewHolderEventArgs viewholdereventargs = new ViewHolderEventArgs(this)
			{
				CheckedChangeEventArgs = args,
			};

			CheckedChange?.Invoke(sender, viewholdereventargs);
			OnCheckedChangeAction?.Invoke(viewholdereventargs);
		}						   					   
		protected virtual void ItemView_Click(object sender, EventArgs args)
		{
			bool stop =
				Click is null &&
				OnClickAction is null;

			if (stop)
				return;

			ViewHolderEventArgs viewholdereventargs = new ViewHolderEventArgs(this)
			{
				Gesture = Gestures.Click,
				ClickEventArgs = args,
			};

			Click?.Invoke(sender, viewholdereventargs);
			OnClickAction?.Invoke(viewholdereventargs);
		}						   
		protected virtual void ItemView_LongClick(object sender, View.LongClickEventArgs args)
		{
			bool stop =
				LongClick is null &&
				OnLongClickAction is null;

			if (stop)
				return;

			ViewHolderEventArgs viewholdereventargs = new ViewHolderEventArgs(this)
			{
				Gesture = Gestures.LongPress,
				LongClickEventArgs = args,
			};

			LongClick?.Invoke(sender, viewholdereventargs);
			OnLongClickAction?.Invoke(viewholdereventargs);
		}

		public class ViewHolderEventArgs
		{
			public ViewHolderEventArgs(RecyclerView.ViewHolder viewholder)
			{
				ViewHolder = viewholder;
			}

			public RecyclerView.ViewHolder ViewHolder { get; }

			public Gestures Gesture { get; set; }
			public EventArgs? ClickEventArgs { get; set; }
			public CompoundButton.CheckedChangeEventArgs? CheckedChangeEventArgs { get; set; }
			public View.LongClickEventArgs? LongClickEventArgs { get; set; }
			public View.TouchEventArgs? TouchEventArgs { get; set; }
		}
	}
}