using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public partial class MapSplineNodes
{
    private readonly NodeGraphBuilder _graphBuilder;
    private readonly AStarPathfinder _pathfinder;

    public MapSplineNodes(SplineContainer splineContainer)
    {
        if(splineContainer == null)
            throw new ArgumentException(nameof(splineContainer));

        _graphBuilder = new(splineContainer);
        _pathfinder = new(splineContainer, _graphBuilder.Nodes);
    }

    public List<SplineSegment> GetPath(Vector3 startPosition, Vector3 targetPosition) =>
        _pathfinder.GetPath(startPosition, targetPosition);
}