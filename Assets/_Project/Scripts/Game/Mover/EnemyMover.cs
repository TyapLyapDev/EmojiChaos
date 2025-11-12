using System;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyMover
{
    private const float PositionInterpolationSpeed = 10;

    private readonly Transform _transform;
    private readonly SplineOffsetCalculator _offsetCalculator;
    private readonly SideOffsetHandler _sideOffsetHandler;

    private float _currentDistance;

    public EnemyMover(SplineContainer splineContainer, Transform transform)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        if (transform == null)
            throw new ArgumentNullException(nameof(transform));

        _transform = transform;
        _offsetCalculator = new(splineContainer);
        _sideOffsetHandler = new();

        Reset();
    }

    public float CurrentDistance => _currentDistance;

    public void Reset()
    {
        _currentDistance = 0f;
        _sideOffsetHandler.Reset();
        _transform.position = CalculateTargetPosition();
    }

    public void SetSideOffset(float offset) =>
        _sideOffsetHandler.SetTargetOffset(offset);

    public void Move(float deltaDistance)
    {
        UpdateSideOffset(deltaDistance);
        UpdateDistance(deltaDistance);
        UpdatePosition(deltaDistance);
    }

    private void UpdateSideOffset(float deltaDistance) =>
        _sideOffsetHandler.Update(deltaDistance);

    private void UpdateDistance(float distanceDelta)
    {
        _currentDistance += distanceDelta;
        _currentDistance = Mathf.Clamp(_currentDistance, 0f, _offsetCalculator.SplineLength);
    }

    private void UpdatePosition(float deltaDistance)
    {
        Vector3 targetPosition = CalculateTargetPosition();
        float interpolationFactor = deltaDistance * PositionInterpolationSpeed;
        _transform.position = Vector3.Lerp(_transform.position, targetPosition, interpolationFactor);
    }

    private Vector3 CalculateTargetPosition() =>
        _offsetCalculator.CalculatePosition(_currentDistance, _sideOffsetHandler.CurrentOffset);
}