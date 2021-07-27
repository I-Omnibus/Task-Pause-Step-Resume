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
			await InnerLoopTask;
		}
		/*      *      *      *      *      *      *      *      *      *      */

		Say($"{nameof(Interruptable)}: ...Finished");
	}

	private Task InnerLoopTask { 
		get=> Task.Factory.StartNew(() => InnerLoop(this)); 
	}
	public Interruptable(Action<Interruptable> innerLoop) => InnerLoop = innerLoop;
	public Interruptable() { }
	public Action<Interruptable> InnerLoop { get; init; } = (_) => { throw new ArgumentException("Missing property:", nameof(InnerLoop)); };
	public ConsoleColor ForegroundColour { get; init; } = ConsoleColor.White;
	public void ColourOn() => Console.ForegroundColor = ForegroundColour;
	public void ColourReset() => Console.ForegroundColor = ConsoleColor.White;
	private void Say(string narrative) {
		ColourOn();
		Console.WriteLine(narrative);
		ColourReset();
	}
}
