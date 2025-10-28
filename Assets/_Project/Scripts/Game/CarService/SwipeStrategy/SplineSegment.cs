using UnityEngine.Splines;

public readonly struct SplineSegment
{
    private readonly Spline _spline;
    private readonly float _startT;
    private readonly float _endT;
    private readonly SplineNode _startNode;
    private readonly SplineNode _endNode;
    private readonly bool _isTransition;

    public SplineSegment(Spline spline, float startT, float endT, SplineNode startNode, SplineNode endNode, bool isTransition = false)
    {
        _spline = spline;
        _startT = startT;
        _endT = endT;
        _startNode = startNode;
        _endNode = endNode;
        _isTransition = isTransition;
    }

    public Spline Spline => _spline;

    public float StartT => _startT;

    public float EndT => _endT;

    public SplineNode StartNode => _startNode;

    public SplineNode EndNode => _endNode;

    public bool IsTransition => _isTransition;
}