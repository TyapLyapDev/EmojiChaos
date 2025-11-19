using System;
using System.Collections.Generic;
using UnityEngine;

public class CarSplineMoverStrategy : IMovementStrategy
{
    private const float MovementSmoothSpeedMultiplier = 1f;
    private const float RotationSmoothSpeedMultiplier = 140;
    private const float SqrThrieshold = 0.005f;
    private const float StartMargin = 0.2f;

    private readonly Transform _transform;
    private readonly Action _destinationReached;
    private readonly SegmentProcessor _segmentProcessor;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;

    public CarSplineMoverStrategy(Transform transform, List<SplineSegment> path, Action destinationReached)
    {
        _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
        _destinationReached = destinationReached;
        _segmentProcessor = new SegmentProcessor(path ?? throw new ArgumentNullException(nameof(path)));

        _segmentProcessor.ProcessMovement(StartMargin);
    }

    public void Move(float deltaDistance)
    {
        if (_segmentProcessor.HasPath == false && (_transform.position - _targetPosition).sqrMagnitude <= SqrThrieshold)
        {
            _destinationReached?.Invoke();
            return;
        }

        _segmentProcessor.ProcessMovement(deltaDistance);
        UpdateTargetTransform();
        ApplySmoothTransform(deltaDistance);
    }

    private void UpdateTargetTransform()
    {
        SplineSegment currentSegment = _segmentProcessor.GetCurrentSegment();

        if (currentSegment == null)
            return;

        _targetPosition = currentSegment.GetWorldPositionBySegmentProgress(_segmentProcessor.GetCurrentProgress());

        Vector3 worldTangent = currentSegment.GetWorldTangentBySegmentProgress(_segmentProcessor.GetCurrentProgress());
        _targetRotation = Quaternion.LookRotation(worldTangent);
    }

    private void ApplySmoothTransform(float deltaDistance)
    {
        Vector3 newPosition = Vector3.MoveTowards(
            _transform.position,
            _targetPosition,
            deltaDistance * MovementSmoothSpeedMultiplier
        );

        Quaternion newRotation = Quaternion.RotateTowards(
            _transform.rotation,
            _targetRotation,
            deltaDistance * RotationSmoothSpeedMultiplier
        );

        _transform.SetPositionAndRotation(newPosition, newRotation);
    }
}