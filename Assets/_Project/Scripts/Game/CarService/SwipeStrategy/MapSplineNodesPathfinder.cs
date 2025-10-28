using System.Collections.Generic;
using UnityEngine;

public class MapSplineNodesPathfinder
{
    private readonly IReadOnlyList<SplineNode> _nodes;

    public MapSplineNodesPathfinder(IReadOnlyList<SplineNode> nodes)
    {
        _nodes = nodes;
    }

    public SplineNode FindNearestNode(Vector3 worldPosition)
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

    public List<SplineNode> FindPath(SplineNode startNode, SplineNode endNode) =>
        AStarSearch(startNode, endNode);

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