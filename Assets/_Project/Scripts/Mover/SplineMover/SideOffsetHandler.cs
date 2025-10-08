using UnityEngine;

public class SideOffsetHandler
{
    private const float OffsetLerpDuration = 0.5f;

    private float _targetOffset;
    private float _currentOffset;
    private float _lerpTime;
    private bool _isInitialized;

    public float CurrentOffset => _currentOffset;

    public void SetTargetOffset(float targetOffset)
    {
        _targetOffset = targetOffset;
        _isInitialized = true;
        _lerpTime = 0f;
        _currentOffset = 0;
    }

    public void Update(float deltaTime)
    {
        if (_isInitialized == false)
            return;

        _lerpTime += deltaTime;
        float progress = Mathf.Clamp01(_lerpTime / OffsetLerpDuration);
        _currentOffset = Mathf.Lerp(0f, _targetOffset, progress);

        if (progress >= 1f)
        {
            _isInitialized = false;
            _currentOffset = _targetOffset;
        }
    }

    public void Reset()
    {
        _targetOffset = 0f;
        _currentOffset = 0f;
        _lerpTime = 0f;
        _isInitialized = false;
    }
}