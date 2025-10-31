public class SplineConnection
{
    private readonly SplineNode _fromNode;
    private readonly SplineNode _toNode;
    private readonly float _distance;

    public SplineConnection(SplineNode fromNode, SplineNode toNode, float distance)
    {
        _fromNode = fromNode;
        _toNode = toNode;
        _distance = distance;
    }

    public SplineNode FromNode => _fromNode;

    public SplineNode ToNode => _toNode;

    public float Distance => _distance;
}