using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class NodesPathfinder
{
    private readonly Transform _splineContainerTransform;
    private readonly IReadOnlyList<SplineNode> _nodes;
    private readonly SplineContainer _splineContainer;

    public NodesPathfinder(SplineContainer splineContainer, IReadOnlyList<SplineNode> nodes)
    {
        _splineContainer = splineContainer;
        _splineContainerTransform = splineContainer.transform;
        _nodes = nodes;
    }

    public List<SplineSegment> GetPathSegments(Vector3 startPosition, Vector3 targetPosition)
    {
        ProgressOnSpline startSplinePosition = FindNearestPointOnAnySpline(startPosition);
        ProgressOnSpline targetSplinePosition = FindNearestPointOnAnySpline(targetPosition);

        List<SplineNode> path = FindPath(startPosition, targetPosition);
        List<SplineSegment> segments = BuildSegmentsFromPath(path);

        if (segments.Count > 0)
        {
            if (startSplinePosition.Spline != null)
                segments[0] = ModifySegmentBoundary(segments[0], startSplinePosition, true);

            if (targetSplinePosition.Spline != null)
                segments[^1] = ModifySegmentBoundary(segments[^1], targetSplinePosition, false);
        }

        return segments;
    }

    private SplineNode FindNearestNode(Vector3 worldPosition)
    {
        SplineNode nearestNode = null;
        float minDistance = float.MaxValue;

        foreach (SplineNode node in _nodes)
        {
            float distance = Vector3.Distance(worldPosition, node.WorldPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    private List<SplineNode> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) =>
        AStarSearch(FindNearestNode(startWorldPosition), FindNearestNode(endWorldPosition));

    private ProgressOnSpline FindNearestPointOnAnySpline(Vector3 worldPosition)
    {
        Vector3 localPosition = _splineContainerTransform.InverseTransformPoint(worldPosition);
        Ray ray = new(localPosition + Vector3.up * 10f, Vector3.down);

        float minDistance = float.MaxValue;
        Spline nearestSpline = null;
        float nearestSplineProgress = 0f;

        foreach (Spline spline in _splineContainer.Splines)
        {
            SplineUtility.GetNearestPoint(
                spline,
                ray,
                out float3 point,
                out float splineProgress
            );

            float distance = Vector3.Distance(localPosition, point);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSpline = spline;
                nearestSplineProgress = splineProgress;
            }
        }

        return new ProgressOnSpline(
            nearestSpline,
            nearestSplineProgress
        );
    }

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
                float startProgress = currentNode.Progress;
                float endProgress = nextNode.Progress;
                bool isReversed = false;

                if (currentNode.Id + 1 != nextNode.Id)
                {
                    isReversed = true;

                    if (currentNode.Spline.Closed && startProgress == 0)
                        startProgress = 1;
                }                

                SplineSegment segment = new(
                    currentNode.Spline,
                    _splineContainerTransform,
                    startProgress,
                    endProgress,
                    isReversed);

                segments.Add(segment);
            }
        }

        return segments;
    }

    private SplineSegment ModifySegmentBoundary(SplineSegment segment, ProgressOnSpline position, bool isStart)
    {
        if (segment.Spline != position.Spline)
            return segment;

        float newProgress = position.Progress;

        if (IsProgressBetween(newProgress, segment.StartProgress, segment.EndProgress) == false)
            return segment;

        if (isStart)
            segment.SetStartProgress(newProgress);
        else
            segment.SetEndProgress(newProgress);

        return segment;
    }

    private bool IsProgressBetween(float progress, float startProgress, float endProgress)
    {
        if (startProgress <= endProgress)
            return progress >= startProgress && progress <= endProgress;
        else
            return progress <= startProgress && progress >= endProgress;
    }

    private List<SplineNode> AStarSearch(SplineNode start, SplineNode goal)
    {
        List<SplineNode> openSet = new() { start };
        Dictionary<SplineNode, SplineNode> cameFrom = new();
        Dictionary<SplineNode, float> costFromStart = new();
        Dictionary<SplineNode, float> estimatedTotalCost = new();

        costFromStart[start] = 0;
        estimatedTotalCost[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            SplineNode current = openSet[0];

            foreach (SplineNode node in openSet)
                if (estimatedTotalCost.ContainsKey(node) && (!estimatedTotalCost.ContainsKey(current) || estimatedTotalCost[node] < estimatedTotalCost[current]))
                    current = node;

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (SplineNode neighbor in current.ConnectedNodes)
            {
                float tentativeGScore = costFromStart[current] + Vector3.Distance(current.WorldPosition, neighbor.WorldPosition);

                if (costFromStart.ContainsKey(neighbor) == false || tentativeGScore < costFromStart[neighbor])
                {
                    cameFrom[neighbor] = current;
                    costFromStart[neighbor] = tentativeGScore;
                    estimatedTotalCost[neighbor] = costFromStart[neighbor] + Heuristic(neighbor, goal);

                    if (openSet.Contains(neighbor) == false)
                        openSet.Add(neighbor);
                }
            }
        }

        return new();
    }

    private float Heuristic(SplineNode a, SplineNode b) =>
        Vector3.Distance(a.WorldPosition, b.WorldPosition);

    private List<SplineNode> ReconstructPath(Dictionary<SplineNode, SplineNode> cameFrom, SplineNode current)
    {
        List<SplineNode> path = new() { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }
}