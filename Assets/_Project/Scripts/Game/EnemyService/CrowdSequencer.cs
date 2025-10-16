using System;
using System.Collections.Generic;

public class CrowdSequencer
{
    private readonly Queue<Crowd> _crowdsQueue;

    public CrowdSequencer(List<Crowd> crowdSequence)
    {
        if (crowdSequence == null)
            throw new ArgumentNullException(nameof(crowdSequence));

        _crowdsQueue = new(crowdSequence);
    }

    public bool TryGiveNextCrowd(out Crowd crowd)
    {
        crowd = null;

        if (_crowdsQueue.Count == 0)
            return false;

        crowd = _crowdsQueue.Dequeue();

        return true;
    }
}