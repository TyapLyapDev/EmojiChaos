using System;
using UnityEngine;

public abstract class CarStraightMoverBaseStrategy : IMovementStrategy
{
    private const float SizeDivider = 2;

    private readonly Transform _transform;
    private readonly BoxCollider _selfCollider;
    private readonly Action<Vector3> _obstacleCollision;
    private readonly Action<CarSplineContainer, Vector3> _roadDetected;

    private float _collisionSphereRadius;
    private float _collisionCheckDistance;

    public CarStraightMoverBaseStrategy(Transform transform,
        BoxCollider self,
        Action<Vector3> obstacleCollision,
        Action<CarSplineContainer, Vector3> roadDetected)
    {
        _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
        _selfCollider = self != null ? self : throw new ArgumentNullException(nameof(self));
        _obstacleCollision = obstacleCollision;
        _roadDetected = roadDetected;

        CalculateCollisionDimensions();
    }

    protected Transform Transform => _transform;

    public void Move(float deltaDistance)
    {
        Vector3 direction = GetDirection();

        if (IsCollision(deltaDistance, direction))
            return;

        _transform.position += deltaDistance * direction;
    }

    protected abstract Vector3 GetDirection();

    private bool IsCollision(float deltaDistance, Vector3 direction)
    {
        Vector3 startPosition = _transform.position + _transform.up * _collisionSphereRadius;
        float distance = _collisionCheckDistance + deltaDistance;

        RaycastHit[] hits = new RaycastHit[10];

        int hitCount = Physics.SphereCastNonAlloc(
            startPosition,
            _collisionSphereRadius,
            direction,
            hits,
            distance
        );

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider == _selfCollider)
                continue;

            if (hit.collider.TryGetComponent(out IObstacle _))
            {
                _obstacleCollision?.Invoke(hit.point);

                return true;
            }

            if (hit.collider.TryGetComponent(out CarSplineContainer carRoad))
                _roadDetected?.Invoke(carRoad, hit.point);
        }

        return false;
    }

    private void CalculateCollisionDimensions()
    {
        _collisionSphereRadius = _selfCollider.size.x * _transform.lossyScale.x / SizeDivider;
        _collisionCheckDistance = _selfCollider.size.z * _transform.lossyScale.z / SizeDivider - _collisionSphereRadius;
    }
}