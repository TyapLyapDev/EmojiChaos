using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineNode
{
    private readonly int _id;
    private readonly int _splineIndex;
    private readonly int _knotIndex;
    private readonly Vector3 _worldPosition;
    private readonly Vector3 _localPosition;
    private readonly Spline _spline;
    private readonly List<SplineNode> _connectedNodes = new();

    public SplineNode(int id, int splineIndex, int knotIndex, Vector3 worldPosition, Vector3 localPosition, Spline spline)
    {
        _id = id;
        _splineIndex = splineIndex;
        _knotIndex = knotIndex;
        _worldPosition = worldPosition;
        _localPosition = localPosition;
        _spline = spline;
    }

    public int Id => _id;

    public int SplineIndex => _splineIndex;

    public int KnotIndex => _knotIndex;

    public Vector3 WorldPosition => _worldPosition;

    public Vector3 LocalPosition => _localPosition;

    public Spline Spline => _spline;

    public IReadOnlyList<SplineNode> ConnectedNodes => _connectedNodes.AsReadOnly();

    public void AddConnectedNode(SplineNode node)
    {
        if (_connectedNodes.Contains(node) == false)
            _connectedNodes.Add(node);
    }

    public void ClearConnectedNodes() =>
        _connectedNodes.Clear();

    public override string ToString() =>
        $"Node {Id} (Spline {SplineIndex}, Knot {KnotIndex})";
}