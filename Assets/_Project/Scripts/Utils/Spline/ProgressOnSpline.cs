using UnityEngine.Splines;

public readonly struct ProgressOnSpline
{
    private readonly Spline _spline;
    private readonly float _progress;

    public ProgressOnSpline(Spline spline, float splineProgress)
    {
        _spline = spline;
        _progress = splineProgress;
    }

    public Spline Spline => _spline;

    public float Progress => _progress;
}