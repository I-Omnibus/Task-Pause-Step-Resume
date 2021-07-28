using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Omnibus.Threading.Tasks {
	public abstract partial class PauseStepResume {
#if DEBUG
#if false
		partial void DebugPause(bool start) => Say(nameof(DebugPause), start);
		partial void DebugCycle(bool start) => Say(nameof(DebugCycle), start);
#endif
		private void Say(string name, bool start) {

			if (!Timers.TryGetValue(name, out Stopwatch timer)) {
				Timers.Add(name, new());
				timer = Timers[name];
				if (start) timer.Restart();
			}
			var delta = string.Empty;
			if (!start) {
				delta = $"+{timer.ElapsedMilliseconds}ms";
			}
			lock (Interlock) {
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine($"{name}: {delta}");
				Console.ForegroundColor = ConsoleColor.White;
			}
			timer.Restart();
		}

		private Dictionary<string, Stopwatch> Timers { get; } = new();
#endif
	}
}
