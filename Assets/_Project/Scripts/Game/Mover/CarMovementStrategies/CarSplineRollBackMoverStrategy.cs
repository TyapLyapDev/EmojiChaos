using System;
using UnityEngine;

public class CarSplineRollBackMoverStrategy : IMovementStrategy
{
    private const float AngularSpeed = 150f;
    private const float RotationThreshold = 20f;
    private const float MinDistanceToSplineStart = 1f;

    private readonly Transform _transform;
    private readonly Action _destinationReached;

    private readonly Vector3 _targetPosition;
    private readonly Quaternion _targetRotation;

    public CarSplineRollBackMoverStrategy(Transform transform, SplineSegment segment, Action destinationReached)
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
        float angleToTargetRotation = Quaternion.Angle(_transform.rotation, _targetRotation);

        if (angleToTargetRotation < RotationThreshold || toTarget.sqrMagnitude > MinDistanceToSplineStart * MinDistanceToSplineStart)
        {
            _destinationReached?.Invoke();

            return;
        }

        _transform.SetPositionAndRotation(
            CalculatePosition(deltaDistance),
            CalculateRotation(deltaDistance));
    }

    private Vector3 CalculatePosition(float deltaDistance) =>
        _transform.position - _transform.forward * deltaDistance;

    private Quaternion CalculateRotation(float deltaDistance) =>
        Quaternion.RotateTowards(_transform.rotation, _targetRotation, AngularSpeed * deltaDistance);
}