using UnityEngine;
using UnityEngine.Splines;

public class SplineDistanceCalculator
{
    private const float DirectionChangeThreshold = 0.5f;

    public float CalculateMinDistance(Spline spline, float p1, float p2)
    {
        float absDiff = Mathf.Abs(p1 - p2);
        float minDiff = Mathf.Min(absDiff, 1f - absDiff);

        return minDiff * spline.GetLength();
    }

    public float CalculateDirectedDistance(Spline spline, float from, float to, float expected)
    {
        float delta = to - from;
        float directedDelta;

        if (expected > 0)
            directedDelta = delta >= 0 ? delta : delta + 1f;
        else
            directedDelta = delta <= 0 ? -delta : 1f - delta;

        return directedDelta * spline.GetLength();
    }

    public float CalculateExpectedDirection(float startProgress, float endProgress)
    {
        float progressDelta = endProgress - startProgress;
        float absProgressDelta = Mathf.Abs(progressDelta);

        if (absProgressDelta > DirectionChangeThreshold)
            return -Mathf.Sign(progressDelta);
        else
            return Mathf.Sign(progressDelta);
    }

    public float CalculateDistance(SplineNode nodeA, SplineNode nodeB)
    {
        if (nodeA.Spline != nodeB.Spline)
            return Vector3.Distance(nodeA.WorldPosition, nodeB.WorldPosition);

        return CalculateMinDistance(nodeA.Spline, nodeA.Progress, nodeB.Progress);
    }
}