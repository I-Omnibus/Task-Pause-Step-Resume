using System;
using System.Threading.Tasks;
using Omnibus.Threading.Tasks;

class Interruptable : PauseStepResume {

	public async void Start(bool initialyPaused = false) {

		Say($"{nameof(Interruptable)}: Started...");

		if (initialyPaused) Pause();

		/*      *      *      *      *      *      *      *      *      *      */
		/*                Pause, Resume, Single step inner loop                */
		/*      *      *      *      *      *      *      *      *      *      */
		while (await IfPaused()) {
			await LoopTask;
		}
		/*      *      *      *      *      *      *      *      *      *      */

		Say($"{nameof(Interruptable)}: ...Finished");
	}

	private Task LoopTask {
		get => Task.Factory.StartNew(() => Loop(this));
	}

	public Action<Interruptable> Loop { get; init; } = (_) => { throw new ArgumentException("Missing property:", nameof(Loop)); };
	public ConsoleColor ForegroundColour { get; init; } = ConsoleColor.White;
	public void ColourOn() => Console.ForegroundColor = ForegroundColour;
	public void ColourReset() => Console.ForegroundColor = ConsoleColor.White;
	private void Say(string narrative) {
		lock (Interlock) {
			ColourOn();
			Console.WriteLine(narrative);
			ColourReset();
		}
	}
}
