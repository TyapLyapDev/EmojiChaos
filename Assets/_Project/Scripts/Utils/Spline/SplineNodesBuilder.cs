using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SplineNodesBuilder
{
    private const float ConnectionThreshold = 0.005f;

    private readonly SplineContainer _splineContainer;
    private readonly List<SplineNode> _nodes = new();
    private readonly List<SplineConnection> _connections = new();
    private Dictionary<(int splineIndex, int knotIndex), SplineNode> _nodeMap;

    public SplineNodesBuilder(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer;
        BuildMap();
    }

    public IReadOnlyList<SplineNode> Nodes => _nodes;

    private void BuildMap()
    {
        IReadOnlyList<Spline> splines = _splineContainer.Splines;
        CollectAllNodes(splines);
        FindConnections();
        BuildConnectionGraph();
    }

    private void CollectAllNodes(IReadOnlyList<Spline> splines)
    {
        _nodeMap = new();
        _nodes.Clear();

        for (int splineIndex = 0; splineIndex < splines.Count; splineIndex++)
        {
            Spline spline = splines[splineIndex];

            for (int knotIndex = 0; knotIndex < spline.Count; knotIndex++)
            {
                int nodeId = _nodes.Count;
                Vector3 localPosition = spline[knotIndex].Position;
                Vector3 worldPosition = _splineContainer.transform.TransformPoint(localPosition);
                SplineNode splineNode = new(nodeId, splineIndex, knotIndex, localPosition, worldPosition, spline);
                _nodes.Add(splineNode);
                _nodeMap[(splineIndex, knotIndex)] = splineNode;
            }
        }
    }

    private void FindConnections()
    {
        _connections.Clear();

        for (int i = 0; i < _nodes.Count; i++)
        {
            SplineNode nodeA = _nodes[i];

            for (int j = i + 1; j < _nodes.Count; j++)
            {
                SplineNode nodeB = _nodes[j];
                float distance = Vector3.Distance(nodeA.WorldPosition, nodeB.WorldPosition);

                if (distance <= ConnectionThreshold)
                {
                    SplineConnection connection = new(nodeA, nodeB, distance);
                    _connections.Add(connection);
                }
            }
        }
    }

    private void BuildConnectionGraph()
    {
        foreach (SplineNode node in _nodes)
        {
            node.ClearConnectedNodes();

            foreach (SplineConnection connection in _connections)
            {
                if (connection.FromNode.Id == node.Id)
                    node.AddConnectedNode(connection.ToNode);
                else if (connection.ToNode.Id == node.Id)
                    node.AddConnectedNode(connection.FromNode);
            }

            AddSplineNeighbors(node);
        }
    }

    private void AddSplineNeighbors(SplineNode node)
    {
        Spline spline = _splineContainer.Splines[node.SplineIndex];
        int knotCount = spline.Count;

        if (knotCount <= 1)
            return;

        int prevKnotIndex = (node.KnotIndex - 1 + knotCount) % knotCount;
        int nextKnotIndex = (node.KnotIndex + 1) % knotCount;

        if (spline.Closed)
        {
            TryAddNeighbor(node, prevKnotIndex);
            TryAddNeighbor(node, nextKnotIndex);

            return;
        }

        if (node.KnotIndex > 0)
            TryAddNeighbor(node, prevKnotIndex);

        if (node.KnotIndex < knotCount - 1)
            TryAddNeighbor(node, nextKnotIndex);
    }

    private void TryAddNeighbor(SplineNode node, int neighborKnotIndex)
    {
        if (node.KnotIndex == neighborKnotIndex)
            return;

        SplineNode neighbor = FindNode(node.SplineIndex, neighborKnotIndex);

        if (neighbor != null && IsConnected(node, neighbor) == false)
            node.AddConnectedNode(neighbor);
    }

    private bool IsConnected(SplineNode nodeA, SplineNode nodeB)
    {
        return _connections.Any(conn =>
            (conn.FromNode == nodeA && conn.ToNode == nodeB) ||
            (conn.FromNode == nodeB && conn.ToNode == nodeA));
    }

    private SplineNode FindNode(int splineIndex, int knotIndex)
    {
        _nodeMap.TryGetValue((splineIndex, knotIndex), out SplineNode node);

        return node;
    }
}