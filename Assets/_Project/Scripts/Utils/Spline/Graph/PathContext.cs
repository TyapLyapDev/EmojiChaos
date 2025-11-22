using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class PathContext
{
    private const float NoPenalty = 1f;
    private const float DirectionPenaltyMultiplier = 2f;

    private readonly IReadOnlyList<SplineNode> _nodes;
    private readonly SplineDistanceCalculator _distanceCalculator;

    private Spline _commonSpline;
    private float _expectedDirection;

    public PathContext(IReadOnlyList<SplineNode> nodes, SplineDistanceCalculator distanceCalculator)
    {
        _nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
        _distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));
    }

    public void InitializePathContext(Spline startSpline, float startProgress, Spline goalSpline, float goalProgress)
    {
        if (startSpline == goalSpline && startSpline.Closed)
        {
            _commonSpline = startSpline;
            _expectedDirection = _distanceCalculator.CalculateExpectedDirection(startProgress, goalProgress);
        }
        else
        {
            _commonSpline = null;
            _expectedDirection = 0f;
        }
    }

    public VirtualNodes CreateVirtualNodes(
        SplinePositionFinder positionFinder,
        Spline startSpline, float startProgress,
        Spline goalSpline, float goalProgress)
    {
        SplineNode startNode = new(startSpline, positionFinder.GetWorldPosition(startSpline, startProgress), startProgress);
        SplineNode goalNode = new(goalSpline, positionFinder.GetWorldPosition(goalSpline, goalProgress), goalProgress);

        ConnectVirtualNode(startNode, _nodes.Where(n => n.Spline == startSpline).ToList(), startSpline, startProgress, true);
        ConnectVirtualNode(goalNode, _nodes.Where(n => n.Spline == goalSpline).ToList(), goalSpline, goalProgress, false);

        return new VirtualNodes(startNode, goalNode);
    }

    public float GetAdjustedProgress(Spline spline, float fromProgress, float toProgress)
    {
        if (_commonSpline != null && _commonSpline == spline)
            return AdjustProgressForDirection(spline, fromProgress, toProgress, _expectedDirection);
        else
            return toProgress;
    }

    public float ApplyDirectionPenalty(SplineNode current, SplineNode neighbor, float storedDistance)
    {
        if (current.Spline == neighbor.Spline && current.Spline.Closed)
        {
            float directionPenalty = CalculateDirectionPenaltyMultiplier(current, neighbor);

            return storedDistance * directionPenalty;
        }

        return storedDistance;
    }

    private float AdjustProgressForDirection(Spline spline, float fromProgress, float toProgress, float expectedDirection)
    {
        if (spline.Closed == false)
            return toProgress;

        float directDiff = toProgress - fromProgress;

        if (Mathf.Approximately(directDiff, 0f))
            return toProgress;

        float sign = Mathf.Sign(directDiff);

        if (sign != expectedDirection)
            return toProgress - sign;

        return toProgress;
    }

    private void ConnectVirtualNode(SplineNode sourceNode, List<SplineNode> targetNodes,
        Spline spline, float referenceProgress, bool isSourceFirst)
    {
        foreach (SplineNode targetNode in targetNodes)
        {
            float distance = _commonSpline != null
                ? _distanceCalculator.CalculateDirectedDistance(spline,
                    isSourceFirst ? referenceProgress : targetNode.Progress,
                    isSourceFirst ? targetNode.Progress : referenceProgress,
                    _expectedDirection)
                : _distanceCalculator.CalculateMinDistance(spline,
                    isSourceFirst ? referenceProgress : targetNode.Progress,
                    isSourceFirst ? targetNode.Progress : referenceProgress);

            if (isSourceFirst)
                sourceNode.AddConnectedNode(targetNode, distance);
            else
                targetNode.AddConnectedNode(sourceNode, distance);
        }
    }

    private float CalculateDirectionPenaltyMultiplier(SplineNode current, SplineNode neighbor)
    {
        if (_commonSpline == null || current.Spline != _commonSpline)
            return NoPenalty;

        float currentToNeighborDirection = Mathf.Sign(neighbor.Progress - current.Progress);

        if (currentToNeighborDirection != _expectedDirection)
            return DirectionPenaltyMultiplier;

        return NoPenalty;
    }
}