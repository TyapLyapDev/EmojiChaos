using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineNode
{
    private readonly Spline _spline;
    private readonly Vector3 _worldPosition;
    private readonly float _progress;
    private readonly Dictionary<SplineNode, float> _connectedNodes = new();

    public SplineNode(Spline spline, Vector3 worldPosition, float progress)
    {
        _spline = spline ?? throw new ArgumentNullException(nameof(spline));
        _worldPosition = worldPosition;
        _progress = progress;
    }

    public Spline Spline => _spline;

    public Vector3 WorldPosition => _worldPosition;

    public float Progress => _progress;

    public IReadOnlyDictionary<SplineNode, float> ConnectedNodes => _connectedNodes;

    public void AddConnectedNode(SplineNode node, float distance)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));

        if (_connectedNodes.ContainsKey(node) == false)
            _connectedNodes[node] = distance;
    }
}