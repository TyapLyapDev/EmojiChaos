using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class NodeGraphBuilder
{
    private const float ConnectionThreshold = 0.3f;

    private readonly List<SplineNode> _nodes = new();
    private readonly SplineContainer _splineContainer;
    private readonly SplinePositionFinder _positionFinder;
    private readonly SplineDistanceCalculator _distanceCalculator;

    public NodeGraphBuilder(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer != null ? splineContainer : throw new System.ArgumentNullException(nameof(splineContainer));
        _positionFinder = new SplinePositionFinder(splineContainer);
        _distanceCalculator = new SplineDistanceCalculator();

        BuildGraph();
    }

    public IReadOnlyList<SplineNode> Nodes => _nodes;

    private void BuildGraph()
    {
        _nodes.Clear();
        _nodes.AddRange(CreateKeyNodes());
        _nodes.AddRange(CreateTransitionNodes());
        ConnectNeighborNodes();
    }

    private List<SplineNode> CreateKeyNodes()
    {
        List<SplineNode> keyNodes = new();
        IReadOnlyList<Spline> splines = _splineContainer.Splines;

        foreach (Spline spline in splines)
        {
            Vector3 startWorldPosition = _positionFinder.GetWorldPosition(spline, 0f);
            keyNodes.Add(new SplineNode(spline, startWorldPosition, 0f));

            if (spline.Closed == false)
            {
                Vector3 endWorldPosition = _positionFinder.GetWorldPosition(spline, 1f);
                keyNodes.Add(new SplineNode(spline, endWorldPosition, 1f));
            }
        }

        return keyNodes;
    }

    private List<SplineNode> CreateTransitionNodes()
    {
        List<SplineNode> transitionNodes = new();
        IReadOnlyList<Spline> splines = _splineContainer.Splines;

        foreach (SplineNode existingNode in _nodes)
            foreach (Spline otherSpline in splines)
                if (ShouldSkipSpline(existingNode, otherSpline) == false)
                    TryCreateTransitionConnection(existingNode, otherSpline, transitionNodes);

        return transitionNodes;
    }

    private bool ShouldSkipSpline(SplineNode existingNode, Spline otherSpline) =>
        existingNode.Spline == otherSpline;

    private void TryCreateTransitionConnection(SplineNode existingNode, Spline otherSpline, List<SplineNode> transitionNodes)
    {
        if (_positionFinder.TryFindNearestProgressOnSpline(otherSpline, existingNode.WorldPosition, out float nearestProgress) == false)
            return;

        Vector3 nearestWorld = _positionFinder.GetWorldPosition(otherSpline, nearestProgress);

        if (IsWithinConnectionThreshold(existingNode.WorldPosition, nearestWorld) == false)
            return;

        SplineNode transitionNode = GetOrCreateTransitionNode(otherSpline, nearestWorld, nearestProgress, transitionNodes);
        CreateTransitionRelationship(existingNode, transitionNode);
    }

    private bool IsWithinConnectionThreshold(Vector3 positionA, Vector3 positionB)
    {
        float euclideanDistance = Vector3.Distance(positionA, positionB);

        return euclideanDistance <= ConnectionThreshold;
    }

    private SplineNode GetOrCreateTransitionNode(Spline spline, Vector3 worldPosition, float progress, List<SplineNode> transitionNodes)
    {
        SplineNode existingTransitionNode = transitionNodes.FirstOrDefault(n => n.Spline == spline && Mathf.Abs(n.Progress - progress) < 0.001f);

        if (existingTransitionNode != null)
            return existingTransitionNode;

        SplineNode newTransitionNode = new(spline, worldPosition, progress);
        transitionNodes.Add(newTransitionNode);

        return newTransitionNode;
    }

    private void CreateTransitionRelationship(SplineNode fromNode, SplineNode toNode)
    {
        float distance = Vector3.Distance(fromNode.WorldPosition, toNode.WorldPosition);
        AddRelationship(fromNode, toNode, distance);
    }

    private void ConnectNeighborNodes()
    {
        foreach (Spline spline in _splineContainer.Splines)
        {
            List<SplineNode> splineNodes = _nodes
                .Where(n => n.Spline == spline)
                .OrderBy(n => n.Progress)
                .ToList();

            for (int i = 0; i < splineNodes.Count - 1; i++)
            {
                float splineDistance = _distanceCalculator.CalculateDistance(splineNodes[i], splineNodes[i + 1]);
                AddRelationship(splineNodes[i], splineNodes[i + 1], splineDistance);
            }

            if (spline.Closed && splineNodes.Count > 1)
            {
                float splineDistance = _distanceCalculator.CalculateDistance(splineNodes[0], splineNodes[^1]);
                AddRelationship(splineNodes[0], splineNodes[^1], splineDistance);
            }
        }
    }

    private void AddRelationship(SplineNode firstNode, SplineNode secondNode, float distance)
    {
        firstNode.AddConnectedNode(secondNode, distance);
        secondNode.AddConnectedNode(firstNode, distance);
    }
}