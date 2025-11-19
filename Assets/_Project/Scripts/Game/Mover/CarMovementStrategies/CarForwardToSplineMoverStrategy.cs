using System;
using UnityEngine;

public class CarForwardToSplineMoverStrategy : IMovementStrategy
{
    private const float PositionThreshold = 0.005f;
    private const float MovementSpeedMultiplier = 0.8f;
    private const float AngularSpeed = 150f;

    private readonly Transform _transform;
    private readonly Action _destinationReached;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    public CarForwardToSplineMoverStrategy(Transform transform, SplineSegment segment, Action destinationReached)
    {
        _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));

        if (segment == null)
            throw new ArgumentNullException(nameof(segment));

        _destinationReached = destinationReached;
        _targetPosition = segment.GetWorldPositionBySegmentProgress(0);

        _targetRotation = segment.GetWorldTangentBySegmentProgress(0) != Vector3.zero
            ? Quaternion.LookRotation(segment.GetWorldTangentBySegmentProgress(0))
            : _transform.rotation;
    }

    public void Move(float deltaDistance)
    {
        Vector3 toTarget = _targetPosition - _transform.position;

        if (toTarget.sqrMagnitude <= PositionThreshold * PositionThreshold)
        {
            _destinationReached?.Invoke();

            return;
        }

        _transform.SetPositionAndRotation(
            CalculatePosition(deltaDistance),
            CalculateRotation(deltaDistance));
    }

    private Vector3 CalculatePosition(float deltaDistance) =>
        Vector3.MoveTowards(_transform.position, _targetPosition, MovementSpeedMultiplier * deltaDistance);

    private Quaternion CalculateRotation(float deltaDistance) =>
        Quaternion.RotateTowards(_transform.rotation, _targetRotation, AngularSpeed * deltaDistance);
}