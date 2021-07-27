using System.Diagnostics;

namespace Omnibus.Threading.Tasks {
	public class DutyCycle {
		/// <summary>
		/// <code>new DutyCycle(100, 0.6f)</code>
		///   <---- waveLength  100ms ----> 
		///    ________________             __
		///   |                |           |
		/// __|                |___________|
		///   <--on=0.6/60ms--><-off/40ms->
		///   <-- rising edge  <-- trailing edge
		/// </summary>
		/// <param name="waveLength">in milliseconds</param>
		/// <param name="on">factor 0.0f .. 1.0f</param>
		public DutyCycle(long waveLength, float on = 1.0f) {
			WaveLength = waveLength < 0 ? 0 : waveLength;
			On = on > 1.0f ? 1.0f : on < 0.0f ? 0.0f : on;
			Off = 1.0f - on;
			FullDutyCycle = !(Off >= float.Epsilon);
			Timer = new Stopwatch();
			CycleCount = 0;
			Overruns = 0;
			RisingEdge = true;
		}
		public void Trigger(out long millisecondsRemaining) {
			var remaining = MillisecondsRemaining();
			if (!Timer.IsRunning) Timer.Start();
			millisecondsRemaining = remaining % WaveLength;
			if (remaining < 0) {
				if (RisingEdge) Overruns++;
				millisecondsRemaining = 0;
			}
			if (RisingEdge) CycleCount++;
			if (!FullDutyCycle) RisingEdge = !RisingEdge;
		}
		public long MillisecondsRemaining() {
			var elapsed = Timer.ElapsedMilliseconds;
			var elapsedThreshold = WaveLength * CycleCount;
			return elapsedThreshold - elapsed;
		}
		public long WaveLength { get; init; }
		public float On { get; init; }
		public float Off { get; init; }
		public bool FullDutyCycle { get; init; }
		public Stopwatch Timer { get; init; }
		public long CycleCount { get; private set; }
		public long Overruns { get; private set; }
		public long Remaining { get; private set; }
		public bool RisingEdge { get; private set; }
		public bool TrailingEdge { get => !RisingEdge; }
		public override string ToString()
			=> new {
				CycleCount,
				Overruns,
				Timer.ElapsedMilliseconds,
				Remaining = MillisecondsRemaining(),
			}.ToString();
	}

}

