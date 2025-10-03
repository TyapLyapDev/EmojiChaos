using System;
using System.Collections.Generic;

public class WaveSequencer
{
    private readonly Queue<Crowd> _waveQueue;
    private Crowd _currentWave;

    public WaveSequencer(List<Crowd> crowdSequence)
    {
        if (crowdSequence == null)
            throw new ArgumentNullException(nameof(crowdSequence));

        _waveQueue = new Queue<Crowd>(crowdSequence);
    }

    public event Action<Crowd> WaveStarted;
    public event Action<Crowd> WaveCompleted;

    public Crowd CurrentWave => _currentWave;

    public bool TryStartNextWave()
    {
        if (_currentWave != null)
        {
            WaveCompleted?.Invoke(_currentWave);
            _currentWave = null;
        }

        if (_waveQueue.Count == 0)
            return false;
        
        _currentWave = _waveQueue.Dequeue();
        WaveStarted?.Invoke(_currentWave);

        return true;
    }
}