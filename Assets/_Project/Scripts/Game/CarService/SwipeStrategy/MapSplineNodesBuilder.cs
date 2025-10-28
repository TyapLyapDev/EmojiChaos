using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class MapSplineNodesBuilder
{
    private const float ConnectionThreshold = 0.005f;

    private readonly SplineContainer _splineContainer;
    private readonly List<SplineNode> _nodes = new();
    private readonly List<SplineConnection> _connections = new();

    public MapSplineNodesBuilder(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer;
        BuildMap();
    }

    public IReadOnlyList<SplineNode> Nodes => _nodes;

    public IReadOnlyList<SplineConnection> Connections => _connections;

    private void BuildMap()
    {
        IReadOnlyList<Spline> splines = _splineContainer.Splines;
        CollectAllNodes(splines);
        FindConnections();
        BuildConnectionGraph();
    }

    private void CollectAllNodes(IReadOnlyList<Spline> splines)
    {
        _nodes.Clear();

        for (int splineIndex = 0; splineIndex < splines.Count; splineIndex++)
        {
            Spline spline = splines[splineIndex];

            for (int knotIndex = 0; knotIndex < spline.Count; knotIndex++)
            {
                BezierKnot knot = spline[knotIndex];
                Vector3 worldPosition = _splineContainer.transform.TransformPoint(knot.Position);
                SplineNode splineNode = new(_nodes.Count, splineIndex, knotIndex, worldPosition, knot.Position, spline);
                _nodes.Add(splineNode);
            }
        }
    }

    private void FindConnections()
    {
        _connections.Clear();

        List<SplineNode> unprocessedNodes = new(_nodes);

        while (unprocessedNodes.Count > 0)
        {
            SplineNode currentNode = unprocessedNodes[0];
            unprocessedNodes.RemoveAt(0);

            List<SplineNode> connectedNodes = FindConnectedNodes(currentNode, unprocessedNodes);

            foreach (SplineNode connectedNode in connectedNodes)
            {
                float distance = Vector3.Distance(currentNode.WorldPosition, connectedNode.WorldPosition);
                SplineConnection splineConnection = new(currentNode, connectedNode, distance);
                _connections.Add(splineConnection);
                unprocessedNodes.Remove(connectedNode);
            }
        }
    }

    private List<SplineNode> FindConnectedNodes(SplineNode referenceNode, List<SplineNode> nodeList)
    {
        List<SplineNode> connectedNodes = new();

        foreach (SplineNode node in nodeList)
        {
            if (node.SplineIndex == referenceNode.SplineIndex)
                continue;

            float distance = Vector3.Distance(referenceNode.WorldPosition, node.WorldPosition);

            if (distance <= ConnectionThreshold)
                connectedNodes.Add(node);
        }

        return connectedNodes;
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

        if (node.KnotIndex > 0)
        {
            SplineNode prevNode = FindNode(node.SplineIndex, node.KnotIndex - 1);

            if (prevNode != null && node.ConnectedNodes.Contains(prevNode) == false)
                node.AddConnectedNode(prevNode);
        }

        if (node.KnotIndex < knotCount - 1)
        {
            SplineNode nextNode = FindNode(node.SplineIndex, node.KnotIndex + 1);

            if (nextNode != null && node.ConnectedNodes.Contains(nextNode) == false)
                node.AddConnectedNode(nextNode);
        }

        if (spline.Closed && knotCount > 1)
        {
            if (node.KnotIndex == 0)
            {
                SplineNode lastNode = FindNode(node.SplineIndex, knotCount - 1);

                if (lastNode != null && node.ConnectedNodes.Contains(lastNode) == false)
                    node.AddConnectedNode(lastNode);
            }
            else if (node.KnotIndex == knotCount - 1)
            {
                SplineNode firstNode = FindNode(node.SplineIndex, 0);

                if (firstNode != null && node.ConnectedNodes.Contains(firstNode) == false)
                    node.AddConnectedNode(firstNode);
            }
        }
    }

    private SplineNode FindNode(int splineIndex, int knotIndex) =>
        _nodes.Find(node => node.SplineIndex == splineIndex && node.KnotIndex == knotIndex);
}