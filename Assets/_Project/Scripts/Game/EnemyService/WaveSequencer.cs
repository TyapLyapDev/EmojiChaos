using System;
using System.Collections.Generic;

public class WaveSequencer
{
    private readonly Queue<Crowd> _waveQueue;

    public WaveSequencer(List<Crowd> crowdSequence)
    {
        if (crowdSequence == null)
            throw new ArgumentNullException(nameof(crowdSequence));

        _waveQueue = new Queue<Crowd>(crowdSequence);
    }

    public bool TryGiveNextWave(out Crowd crowd)
    {
        crowd = null;

        if (_waveQueue.Count == 0)
            return false;

        crowd = _waveQueue.Dequeue();

        return true;
    }
}