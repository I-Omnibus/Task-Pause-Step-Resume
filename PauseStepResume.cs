using System.Threading;
using System.Threading.Tasks;

namespace Omnibus.Threading.Tasks {
	public abstract class PauseStepResume {

		public void Pause() {
			IsPaused = true;
			//CancelCycle();
		}
		public void SingleStep() {
			IsPaused = true;
			IsSingleStep = true;
			CancelPause();
			//CancelCycle();
		}
		public void Resume() {
			CancelPause();
			IsPaused = false;
			IsSingleStep = false;
		}
		public void Abort() {
			IsAbort = true;
			//CancelCycle();
			CancelPause();
		}
		public async Task<bool> IfPaused() {
			if (IsPaused) {
				PauseTokenSource?.Dispose();
				PauseTokenSource = new();
				await Indefinitely(PauseTokenSource);
			}
			if (IsSingleStep) {
				IsSingleStep = false;
			} else {
				//IsCycling = true;
				//CycleTokenSource?.Dispose();
				//CycleTokenSource = new();
				await Task.Delay(CycleTime);//, CycleTokenSource.Token
				//IsCycling = false;
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
		//private void CancelCycle() {
		//	if (IsCycling) CycleTokenSource?.Cancel();
		//}

		private CancellationTokenSource PauseTokenSource { get; set; }
		//private CancellationTokenSource CycleTokenSource { get; set; }
		private bool IsPaused { get; set; }
		private bool IsSingleStep { get; set; }
		private bool IsAbort { get; set; }
		//private bool IsCycling { get; set; }
		public int CycleTime { get; init; }

	}
}
