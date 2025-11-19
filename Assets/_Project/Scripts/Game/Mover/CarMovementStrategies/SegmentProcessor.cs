using System;
using System.Collections.Generic;

public class SegmentProcessor
{
    private readonly List<SplineSegment> _path;
    private float _currentSegmentProgress;

    public SegmentProcessor(List<SplineSegment> path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public bool HasPath => _path.Count > 0;

    public void ProcessMovement(float remainingDistance)
    {
        float newRemainingDistance = remainingDistance;

        while (newRemainingDistance > 0 && _path.Count > 0)
        {
            SplineSegment currentSegment = _path[0];
            float segmentLength = currentSegment.Length;

            if (segmentLength <= 0)
            {
                RemoveCurrentSegment();
                continue;
            }

            float progressDelta = newRemainingDistance / segmentLength;
            float newProgress = _currentSegmentProgress + progressDelta;

            if (newProgress >= 1f)
            {
                float usedProgress = 1f - _currentSegmentProgress;
                float usedDistance = usedProgress * segmentLength;
                newRemainingDistance -= usedDistance;
                RemoveCurrentSegment();
            }
            else
            {
                _currentSegmentProgress = newProgress;
                newRemainingDistance = 0;
            }
        }
    }

    private void RemoveCurrentSegment()
    {
        if (_path.Count > 0)
        {
            _path.RemoveAt(0);
            _currentSegmentProgress = 0;
        }
    }

    public SplineSegment GetCurrentSegment() => _path.Count > 0 ? _path[0] : null;

    public float GetCurrentProgress() => _currentSegmentProgress;
}