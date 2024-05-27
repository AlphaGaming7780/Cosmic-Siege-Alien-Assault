using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace K8055Velleman.Lib;

// SOURCE ; https://stackoverflow.com/questions/26964192/c-sharp-how-to-pause-a-timer

public class PausableTimer : Timer
{
    public double RemainingAfterPause { get; private set; }

    private readonly Stopwatch _stopwatch;
    private readonly double _initialInterval;
    private bool _resumed;

    public PausableTimer(double interval) : base(interval)
    {
        _initialInterval = interval;
        Elapsed += OnElapsed;
        _stopwatch = new Stopwatch();
    }

    public new void Start()
    {
        ResetStopwatch();
        base.Start();
    }

    private void OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
        if (_resumed)
        {
            _resumed = false;
            Stop();
            Interval = _initialInterval;
            Start();
        }

        ResetStopwatch();
    }

    private void ResetStopwatch()
    {
        _stopwatch.Reset();
        _stopwatch.Start();
    }

    public void Pause()
    {
        Stop();
        _stopwatch.Stop();
        RemainingAfterPause = Interval - _stopwatch.Elapsed.TotalMilliseconds;
    }

    public void Resume()
    {
        _resumed = true;
        Interval = RemainingAfterPause;
        RemainingAfterPause = 0;
        Start();
    }

}
