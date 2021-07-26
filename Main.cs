using System;
using System.Threading;

/*      *      *      *      *      *      *      *      *      *      *      */
/*                       Get asynchronous task started                        */
/*      *      *      *      *      *      *      *      *      *      *      */
var task = new Interruptable(innerLoop: () => {
	Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));
	Thread.Sleep(333);
});

task.Start();
/*      *      *      *      *      *      *      *      *      *      *      */


/*      *      *      *      *      *      *      *      *      *      *      */
/*                   Control asynchronous task via console                    */
/*      *      *      *      *      *      *      *      *      *      *      */
Console.WriteLine("Command keys: " + new { p = "Pause", r = "Resume", s = "SingleStep", a = "Abort", x = "Exit", });

while (true) {
	Action action = Console.ReadKey(intercept: true).KeyChar switch {
		'p' => task.Pause,
		'r' => task.Resume,
		's' => task.SingleStep,
		'a' => task.Abort,
		'x' => () => { Environment.Exit(0); },
		_ => default,
	};
	action?.Invoke();
}
/*      *      *      *      *      *      *      *      *      *      *      */
