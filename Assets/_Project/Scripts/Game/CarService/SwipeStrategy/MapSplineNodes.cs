using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MapSplineNodes
{
    private readonly MapSplineNodesBuilder _builder;
    private readonly MapSplineNodesPathfinder _pathfinder;
    private readonly Transform _splineContainerTransform;

    public MapSplineNodes(SplineContainer splineContainer)
    {
        _builder = new MapSplineNodesBuilder(splineContainer);
        _pathfinder = new MapSplineNodesPathfinder(_builder.Nodes);
        _splineContainerTransform = splineContainer.transform;
    }

    public Transform SplineContainerTransform => _splineContainerTransform;

    public IReadOnlyList<SplineNode> Nodes => _builder.Nodes;

    public IReadOnlyList<SplineConnection> Connections => _builder.Connections;

    public List<SplineSegment> GetPathSegments(Vector3 startPosition, Vector3 targetPosition)
    {
        SplineNode startNode = FindNearestNode(startPosition);
        SplineNode targetNode = FindNearestNode(targetPosition);
        List<SplineNode> path = _pathfinder.FindPath(startNode, targetNode);

        return BuildSegmentsFromPath(path);
    }

    private SplineNode FindNearestNode(Vector3 worldPosition) =>
        _pathfinder.FindNearestNode(worldPosition);

    private List<SplineSegment> BuildSegmentsFromPath(List<SplineNode> path)
    {
        List<SplineSegment> segments = new();

        if (path.Count < 2)
            return segments;

        for (int i = 0; i < path.Count - 1; i++)
        {
            SplineNode currentNode = path[i];
            SplineNode nextNode = path[i + 1];

            if (currentNode.SplineIndex == nextNode.SplineIndex)
            {
                SplineSegment segment = new(
                    currentNode.Spline,
                    GetTFromKnotIndex(currentNode),
                    GetTFromKnotIndex(nextNode),
                    currentNode,
                    nextNode);

                segments.Add(segment);
            }
            else
            {
                SplineSegment transitionSegment = new(
                    currentNode.Spline,
                    GetTFromKnotIndex(currentNode),
                    GetTFromKnotIndex(currentNode),
                    currentNode,
                    currentNode,
                    true);

                segments.Add(transitionSegment);
            }
        }

        return segments;
    }

    private float GetTFromKnotIndex(SplineNode node)
    {
        Spline spline = node.Spline;
        if (spline.Closed)
            return node.KnotIndex / (float)spline.Count;
        else
            return node.KnotIndex / (float)(spline.Count - 1);
    }
}