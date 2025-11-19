using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SegmentBuilder
{
    private readonly Transform _splineContainerTransform;
    private readonly PathContext _nodeManager;

    public SegmentBuilder(Transform splineContainerTransform, PathContext nodeManager)
    {
        _splineContainerTransform = splineContainerTransform != null
            ? splineContainerTransform
            : throw new ArgumentNullException(nameof(splineContainerTransform));

        _nodeManager = nodeManager ?? throw new ArgumentNullException(nameof(nodeManager));
    }

    public List<SplineSegment> BuildSegments(List<SplineNode> nodePath,
        Spline startSpline, float startProgress,
        Spline goalSpline, float goalProgress)
    {
        if (nodePath.Count < 3)
            return new List<SplineSegment>();

        RemoveVirtualNodes(nodePath);
        List<SplineSegment> segments = new();
        AddStartSegment(segments, nodePath, startSpline, startProgress);
        AddIntermediateSegments(segments, nodePath);
        AddEndSegment(segments, nodePath, goalSpline, goalProgress);

        return segments;
    }

    private void RemoveVirtualNodes(List<SplineNode> nodePath)
    {
        nodePath.RemoveAt(0);
        nodePath.RemoveAt(nodePath.Count - 1);
    }

    private void AddStartSegment(List<SplineSegment> segments, List<SplineNode> nodePath, Spline startSpline, float startProgress)
    {
        float adjustedFirstTo = _nodeManager.GetAdjustedProgress(startSpline, startProgress, nodePath[0].Progress);
        segments.Add(new SplineSegment(startSpline, _splineContainerTransform, startProgress, adjustedFirstTo));
    }

    private void AddIntermediateSegments(List<SplineSegment> segments, List<SplineNode> nodePath)
    {
        for (int i = 0; i < nodePath.Count - 1; i++)
        {
            SplineNode current = nodePath[i];
            SplineNode next = nodePath[i + 1];

            if (current.Spline != next.Spline)
                continue;

            float adjustedNext = _nodeManager.GetAdjustedProgress(current.Spline, current.Progress, next.Progress);
            segments.Add(new SplineSegment(current.Spline, _splineContainerTransform, current.Progress, adjustedNext));
        }
    }

    private void AddEndSegment(List<SplineSegment> segments, List<SplineNode> nodePath, Spline goalSpline, float goalProgress)
    {
        float adjustedEnd = _nodeManager.GetAdjustedProgress(goalSpline, nodePath[^1].Progress, goalProgress);
        segments.Add(new SplineSegment(goalSpline, _splineContainerTransform, nodePath[^1].Progress, adjustedEnd));
    }
}