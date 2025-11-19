using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class AStarPathfinder
{
    private readonly SplineContainer _splineContainer;
    private readonly SplinePositionFinder _positionFinder;
    private readonly SplineDistanceCalculator _distanceCalculator;
    private readonly AStarAlgorithm _pathFinder;
    private readonly PathContext _pathContext;

    public AStarPathfinder(SplineContainer splineContainer, IReadOnlyList<SplineNode> nodes)
    {
        _splineContainer = splineContainer != null ? splineContainer : throw new ArgumentNullException(nameof(splineContainer));

        _positionFinder = new SplinePositionFinder(splineContainer);
        _distanceCalculator = new SplineDistanceCalculator();
        _pathContext = new PathContext(nodes, _distanceCalculator);
        _pathFinder = new AStarAlgorithm(_pathContext);
    }

    public List<SplineSegment> GetPath(Vector3 startPosition, Vector3 targetPosition)
    {
        if (_positionFinder.TryFindNearestProgressOnAnySpline(startPosition, out float startProgress, out Spline nearestStartSpline) == false)
            throw new InvalidOperationException($"Не удалось найти ближайший сплайн для {nameof(startPosition)}");

        if (_positionFinder.TryFindNearestProgressOnAnySpline(targetPosition, out float targetProgress, out Spline nearestTargetSpline) == false)
            throw new InvalidOperationException($"Не удалось найти ближайший сплайн для {nameof(targetPosition)}");

        return FindPath(nearestStartSpline, startProgress, nearestTargetSpline, targetProgress);
    }

    public List<SplineSegment> FindPath(Spline startSpline, float startProgress, Spline goalSpline, float goalProgress)
    {
        if (startSpline == null || goalSpline == null)
            return new();

        _pathContext.InitializePathContext(startSpline, startProgress, goalSpline, goalProgress);
        VirtualNodes virtualNodes = _pathContext.CreateVirtualNodes(_positionFinder, startSpline, startProgress, goalSpline, goalProgress);
        List<SplineNode> nodePath = _pathFinder.FindPath(virtualNodes);

        if (nodePath.Count > 0)
        {
            SegmentBuilder segmentBuilder = new(_splineContainer.transform, _pathContext);

            return segmentBuilder.BuildSegments(nodePath, startSpline, startProgress, goalSpline, goalProgress);
        }

        return new();
    }
}