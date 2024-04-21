#nullable enable

using AndroidX.Core.OS;

namespace System.Threading
{
	public static class CancellationTokenExtensions
	{
		public static CancellationSignal AsCancellationSignal(this CancellationToken cancellationtoken)
		{
			CancellationSignal cancellationsignal = new CancellationSignal();

			cancellationtoken.Register(() =>
			{
				if (cancellationtoken.IsCancellationRequested)
					cancellationsignal.Cancel();
			});

			return cancellationsignal;
		}
		public static CancellationSignal AsCancellationSignal(this CancellationToken cancellationtoken, Action OnCancelAction)
		{
			CancellationSignal cancellationsignal = new CancellationSignal();
			cancellationsignal.SetOnCancelListener(new CancellationSignalOnCancelListener
			{
				OnCancelAction = OnCancelAction
			});

			cancellationtoken.Register(() =>
			{
				if (cancellationtoken.IsCancellationRequested)
					cancellationsignal.Cancel();
			});

			return cancellationsignal;
		}
	}
}