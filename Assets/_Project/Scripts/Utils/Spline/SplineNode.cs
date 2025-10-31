using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineNode
{
    private readonly int _id;
    private readonly int _splineIndex;
    private readonly int _knotIndex;
    private readonly Vector3 _localPosition;
    private readonly Vector3 _worldPosition;
    private readonly Spline _spline;
    private readonly List<SplineNode> _connectedNodes = new();
    private readonly float _progress;

    public SplineNode(int id, int splineIndex, int knotIndex, Vector3 localPosition, Vector3 worldPosition, Spline spline)
    {
        _id = id;
        _splineIndex = splineIndex;
        _knotIndex = knotIndex;
        _localPosition = localPosition;
        _worldPosition = worldPosition;
        _spline = spline;
        _progress = GetProgressFromKnotIndex();
    }

    public int Id => _id;

    public int SplineIndex => _splineIndex;

    public int KnotIndex => _knotIndex;

    public Vector3 LocalPosition => _localPosition;

    public Vector3 WorldPosition => _worldPosition;

    public Spline Spline => _spline;

    public float Progress => _progress;

    public IReadOnlyList<SplineNode> ConnectedNodes => _connectedNodes.AsReadOnly();

    public void AddConnectedNode(SplineNode node)
    {
        if (_connectedNodes.Contains(node) == false)
            _connectedNodes.Add(node);
    }

    public void ClearConnectedNodes() =>
        _connectedNodes.Clear();

    private float GetProgressFromKnotIndex()
    {
        if (_spline.Count <= 1)
            return 0f;

        SplineUtility.GetNearestPoint(_spline, _localPosition, out float3 _, out float progress);

        return progress;
    }
}