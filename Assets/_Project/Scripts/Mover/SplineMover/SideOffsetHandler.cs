using UnityEngine;

public class SideOffsetHandler
{
    private const float InterpolationDuration = 0.5f;

    private float _targetOffset;
    private float _currentOffset;
    private float _startOffset;
    private float _interpolationTime;
    private bool _isInterpolationActive;

    public float CurrentOffset => _currentOffset;

    public void Reset()
    {
        _targetOffset = 0f;
        _currentOffset = 0f;
        _startOffset = 0f;
        _interpolationTime = 0f;
        _isInterpolationActive = false;
    }

    public void SetTargetOffset(float targetOffset)
    {
        _targetOffset = targetOffset;
        _startOffset = _currentOffset;
        _interpolationTime = 0f;
        _isInterpolationActive = true;
    }

    public void Update(float deltaTime)
    {
        if (_isInterpolationActive == false)
            return;

        _interpolationTime += deltaTime;
        float progress = Mathf.Clamp01(_interpolationTime / InterpolationDuration);
        _currentOffset = Mathf.Lerp(_startOffset, _targetOffset, progress);

        if (progress >= 1f)
            CompleteInterpolation();
    }

    private void CompleteInterpolation()
    {
        _currentOffset = _targetOffset;
        _startOffset = _targetOffset;
        _isInterpolationActive = false;
    }
}