using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarAlgorithm
{
    private const float TieBreaker = 0.0001f;

    private readonly PathContext _pathContext;

    private readonly SortedDictionary<float, Queue<SplineNode>> _openSet = new();
    private readonly Dictionary<SplineNode, SplineNode> _cameFrom = new();
    private readonly Dictionary<SplineNode, float> _costFromStart = new();
    private readonly Dictionary<SplineNode, float> _estimatedTotalCost = new();
    private readonly HashSet<SplineNode> _openSetLookup = new();

    public AStarAlgorithm(PathContext pathContext)
    {
        _pathContext = pathContext ?? throw new ArgumentNullException(nameof(pathContext));
    }

    public List<SplineNode> FindPath(VirtualNodes virtualNodes)
    {
        InitializeSearch(virtualNodes);

        while (_openSetLookup.Count > 0)
        {
            SplineNode current = ExtractLowestCostNode();

            if (current == virtualNodes.Goal)
                return ReconstructPath(_cameFrom, current);

            EvaluateNeighbors(current, virtualNodes.Goal);
        }

        return new List<SplineNode>();
    }

    private void InitializeSearch(VirtualNodes virtualNodes)
    {
        ClearCollections();
        float initialCost = Vector3.Distance(virtualNodes.Start.WorldPosition, virtualNodes.Goal.WorldPosition);
        EnqueueNode(virtualNodes.Start, initialCost);
        _costFromStart[virtualNodes.Start] = 0;
        _estimatedTotalCost[virtualNodes.Start] = initialCost;
    }

    private void EvaluateNeighbors(SplineNode current, SplineNode goal)
    {
        foreach (var neighborPair in current.ConnectedNodes)
        {
            SplineNode neighbor = neighborPair.Key;
            float storedDistance = neighborPair.Value;

            float adjustedDistance = _pathContext.ApplyDirectionPenalty(current, neighbor, storedDistance);
            float tentativeGScore = _costFromStart[current] + adjustedDistance + TieBreaker;

            if (ShouldUpdateNode(neighbor, tentativeGScore))
                UpdateNode(current, neighbor, tentativeGScore, goal);
        }
    }

    private void UpdateNode(SplineNode current, SplineNode neighbor, float tentativeGScore, SplineNode goal)
    {
        _cameFrom[neighbor] = current;
        _costFromStart[neighbor] = tentativeGScore;
        float newEstimatedCost = tentativeGScore + Vector3.Distance(neighbor.WorldPosition, goal.WorldPosition);
        _estimatedTotalCost[neighbor] = newEstimatedCost;
        EnqueueNode(neighbor, newEstimatedCost);
    }

    private bool ShouldUpdateNode(SplineNode neighbor, float tentativeGScore) =>
        _costFromStart.ContainsKey(neighbor) == false || tentativeGScore < _costFromStart[neighbor];

    private void EnqueueNode(SplineNode node, float cost)
    {
        if (_openSet.ContainsKey(cost) == false)
            _openSet[cost] = new Queue<SplineNode>();

        _openSet[cost].Enqueue(node);
        _openSetLookup.Add(node);
    }

    private SplineNode ExtractLowestCostNode()
    {
        if (_openSet.Count == 0)
            return null;

        var firstPair = _openSet.First();
        var queue = firstPair.Value;
        SplineNode node = queue.Dequeue();

        if (queue.Count == 0)
            _openSet.Remove(firstPair.Key);

        _openSetLookup.Remove(node);
        return node;
    }

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

    private void ClearCollections()
    {
        _openSet.Clear();
        _openSetLookup.Clear();
        _cameFrom.Clear();
        _costFromStart.Clear();
        _estimatedTotalCost.Clear();
    }
}