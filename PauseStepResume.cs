using System;
using System.Threading;
using System.Threading.Tasks;

namespace Omnibus.Threading.Tasks {
	public abstract partial class PauseStepResume {

		partial void DebugPause(bool start = true);
		partial void DebugCycle(bool start = true);

		public void Pause() {
			DebugPause();
			IsPaused = true;
		}
		public void SingleStep() {
			IsPaused = true;
			IsSingleStep = true;
			CancelPause();
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
			if (IsPaused) {
				PauseTokenSource?.Dispose();
				PauseTokenSource = new();
				await Indefinitely(PauseTokenSource);
				DebugPause(start: false);
			}
			if (IsSingleStep) {
				IsSingleStep = false;
			} else {

				DebugCycle();
				await Task.Delay(CycleTime);
				DebugCycle(start: false);

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
			if (IsPaused) PauseTokenSource?.Cancel();
		}

		private CancellationTokenSource PauseTokenSource { get; set; }

		private bool IsPaused { get; set; }
		private bool IsSingleStep { get; set; }
		private bool IsAbort { get; set; }
		public int CycleTime { get; init; }
		public static readonly object Interlock = new();

	}
}
