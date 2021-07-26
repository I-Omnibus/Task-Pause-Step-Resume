using System.Threading;
using System.Threading.Tasks;

namespace Omnibus.Threading.Tasks {
	public abstract class PauseStepResume {
		public void Pause() => IsPaused = true;
		public void SingleStep() {
			CancelPause();
			IsPaused = true;
			IsSingleStep = true;
		}
		public void Resume() {
			CancelPause();
			IsPaused = false;
			IsSingleStep = false;
		}
		public void Abort() {
			IsAbort = true;
			CancelPause();
		}
		public async Task<bool> IfPaused() {
			if (IsSingleStep) IsSingleStep = false;
			if (IsPaused) {
				TokenSource?.Dispose();
				TokenSource = new();
				await Indefinitely(TokenSource);
			}
			return !IsAbort;
		}

		private static async Task<bool> Indefinitely(CancellationTokenSource tokenSource) {
			await HellFreezesOver(tokenSource).ContinueWith(_ => HellFreezesOver());
			return true;
		}

		private static Task HellFreezesOver(CancellationTokenSource tokenSource = null)
			=> tokenSource switch {
				not null => Task.Delay(int.MaxValue, tokenSource.Token),
				null => Task.Delay(int.MaxValue),
			};

		private void CancelPause() {
			if (IsPaused) TokenSource?.Cancel();
		}

		private CancellationTokenSource TokenSource { get; set; }
		private bool IsPaused { get; set; }
		private bool IsSingleStep { get; set; }
		private bool IsAbort { get; set; }

	}
}
