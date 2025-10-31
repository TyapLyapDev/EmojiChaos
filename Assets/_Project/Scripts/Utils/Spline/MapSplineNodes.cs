using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public partial class MapSplineNodes
{
    private readonly SplineNodesBuilder _builder;
    private readonly NodesPathfinder _pathfinder;

    public MapSplineNodes(SplineContainer splineContainer)
    {
        _builder = new(splineContainer);
        _pathfinder = new(splineContainer, _builder.Nodes);
    }

    public List<SplineSegment> GetPathSegments(Vector3 startPosition, Vector3 targetPosition) =>
        _pathfinder.GetPathSegments(startPosition, targetPosition);
}