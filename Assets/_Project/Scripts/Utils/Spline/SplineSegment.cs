using UnityEngine;
using UnityEngine.Splines;

public class SplineSegment
{
    private readonly Spline _spline;
    private readonly Transform _splineContainerTransform;
    private float _startProgress;
    private float _endProgress;
    private readonly bool _isReversed;

    public SplineSegment(Spline spline,
        Transform splineContainerTransform,
        float startProgress,
        float endProgress,
        bool isReversed)
    {
        _spline = spline;
        _splineContainerTransform = splineContainerTransform;
        _startProgress = startProgress;
        _endProgress = endProgress;
        _isReversed = isReversed;
    }

    public Spline Spline => _spline;

    public float StartProgress => _startProgress;

    public float EndProgress => _endProgress;

    public float CalculateLength()
    {
        float startProgress = _startProgress;
        float endProgress = _endProgress;
        float length = _spline.GetLength() * Mathf.Abs(endProgress - startProgress);

        return length;
    }

    public void SetStartProgress(float progress) =>
        _startProgress = progress;

    public void SetEndProgress(float progress) =>
        _endProgress = progress;

    public Vector3 GetWorldPosition(float splineProgress)
    {
        Vector3 localPosition = _spline.EvaluatePosition(splineProgress);
        Vector3 worldPosition = _splineContainerTransform.TransformPoint(localPosition);

        return worldPosition;
    }

    public Vector3 GetWorldTangent(float splineProgress)
    {
        Vector3 localTangent = _spline.EvaluateTangent(splineProgress);
        Vector3 worldTangent = _splineContainerTransform.TransformDirection(localTangent);

        if (_isReversed)
            worldTangent = -worldTangent;

        if (worldTangent.sqrMagnitude > Mathf.Epsilon)
            worldTangent = worldTangent.normalized;

        return worldTangent;
    }

    public void ShowInfo()
    {
        Debug.Log($"SplineSegment Info: " +
                  $"Spline Index: {_spline.GetHashCode()}, " +
                  $"Start Progress: {_startProgress}, " +
                  $"End Progress: {_endProgress}, " +
                  $"Is Reversed: {_isReversed}");
    }
}