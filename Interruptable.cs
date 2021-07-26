using System;
using System.Threading.Tasks;
using Omnibus.Threading.Tasks;

class Interruptable : PauseStepResume {

	public Interruptable(Action innerLoop) => InnerLoop = innerLoop;

	public async void Start(bool initialyPaused = false) {

		Console.WriteLine($"{nameof(Interruptable)}: Started");

		if (initialyPaused) Pause();

		/*      *      *      *      *      *      *      *      *      *      */
		/*                Pause, Resume, Single step inner loop                */
		/*      *      *      *      *      *      *      *      *      *      */
		while (await IfPaused()) {
			await Task.Factory.StartNew(InnerLoop);
		}
		/*      *      *      *      *      *      *      *      *      *      */

		Console.WriteLine($"{nameof(Interruptable)}: Finished");
	}

	public Action InnerLoop { get; init; }
}
