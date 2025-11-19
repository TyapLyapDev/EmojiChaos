using System;

public class VirtualNodes
{
    private SplineNode _start;
    private SplineNode _goal;

    public VirtualNodes(SplineNode start, SplineNode goal)
    {
        _start = start ?? throw new ArgumentNullException(nameof(start));
        _goal = goal ?? throw new ArgumentNullException(nameof(goal));
    }

    public SplineNode Start => _start;

    public SplineNode Goal => _goal;
}