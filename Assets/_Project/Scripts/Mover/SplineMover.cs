using System;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMover
{
    private readonly Transform _transform;
    private readonly SplineOffsetCalculator _offsetCalculator;
    private readonly SideOffsetHandler _offsetHandler;

    private float _currentDistance;

    public SplineMover(SplineContainer splineContainer, Transform transform)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        if (transform == null)
            throw new ArgumentNullException(nameof(transform));

        _transform = transform;
        _offsetCalculator = new(splineContainer);
        _offsetHandler = new();

        Reset();
    }

    public void Reset()
    {
        _currentDistance = 0f;
        _offsetHandler.Reset();
        UpdateTransformPosition();
    }

    public void SetSideOffset(float offset) =>
        _offsetHandler.SetTargetOffset(offset);

    public void Move(float speed, float deltaTime)
    {
        if (deltaTime < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime), "ќѕј„ »,  ќ—я ! «начение не может быть меньше нул€");

        float newDistance = _currentDistance + speed * deltaTime;
        float splineLength = _offsetCalculator.SplineLength;

        if (newDistance <= 0 || newDistance >= splineLength)
            return;

        _currentDistance = Mathf.Clamp(newDistance, 0, splineLength);
        _offsetHandler.Update(deltaTime);
        UpdateTransformPosition();
    }

    private void UpdateTransformPosition() =>
        _transform.position = _offsetCalculator.CalculatePosition(_currentDistance, _offsetHandler.CurrentOffset);
}