using System;
using System.Collections.Generic;

/*      *      *      *      *      *      *      *      *      *      *      */
/*                       Get asynchronous tasks started                       */
/*      *      *      *      *      *      *      *      *      *      *      */
var task1 = new Interruptable {
	InnerLoop = (task) => {
		task.ColourOn();
		Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));
		task.ColourReset();
	},
	CycleTime = 333,
	ForegroundColour = ConsoleColor.Green
};

var task2 = new Interruptable {
	InnerLoop = (task) => {
		task.ColourOn();
		Console.WriteLine("\t\t" + DateTime.Now.ToString("HH:mm:ss.fff"));
		task.ColourReset();
	},
	CycleTime = 555,
	ForegroundColour = ConsoleColor.Red
};

var tasks = new List<Interruptable> { task1, task2 };
tasks.ForEach(task => task.Start());
/*      *      *      *      *      *      *      *      *      *      *      */


/*      *      *      *      *      *      *      *      *      *      *      */
/*                   Control asynchronous task via console                    */
/*      *      *      *      *      *      *      *      *      *      *      */
Console.WriteLine("Command keys: " + new { p = "Pause", r = "Resume", s = "SingleStep", a = "Abort", c = "Cycle", x = "Exit", });

var selectedTask = 0;
while (true) {
	Action action = Console.ReadKey(intercept: true).KeyChar switch {
		'p' => tasks[selectedTask].Pause,
		'r' => tasks[selectedTask].Resume,
		's' => tasks[selectedTask].SingleStep,
		'a' => tasks[selectedTask].Abort,
		'c' => () => { selectedTask = ++selectedTask % tasks.Count; },
		'x' => () => { Environment.Exit(0); },
		_ => default,
	};
	action?.Invoke();
}
/*      *      *      *      *      *      *      *      *      *      *      */
