using System;
using UnityEngine;

public abstract class CarStraightRollMoverBaseStrategy : IMovementStrategy
{
    private const float SizeDivider = 2;
    private const float SpeedMultiplier = 0.3f;

    private readonly Transform _transform;
    private readonly Collider _selfCollider;
    private readonly Action _completed;

    private readonly float _sphereRadius;
    private readonly float _forwardOffset;

    public CarStraightRollMoverBaseStrategy(Transform transform, BoxCollider self, Action completed)
    {
        _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
        _selfCollider = self != null ? self : throw new ArgumentNullException(nameof(self));
        _completed = completed;

        _sphereRadius = self.size.x * transform.lossyScale.x / SizeDivider;
        _forwardOffset = self.size.z * transform.lossyScale.z / SizeDivider + _sphereRadius;
    }

    protected Transform Transform => _transform;

    public void Move(float deltaDistance)
    {
        deltaDistance *= SpeedMultiplier;
        Vector3 direction = GetDirection();

        if (IsCollision(deltaDistance, direction) && IsCollision(deltaDistance, -direction) == false)
        {
            _transform.position -= direction * deltaDistance;

            return;
        }

        _completed?.Invoke();
    }

    protected abstract Vector3 GetDirection();

    private bool IsCollision(float deltaDistance, Vector3 direction)
    {
        Vector3 startPosition = _transform.position + _transform.up * _sphereRadius;
        float distance = _forwardOffset + deltaDistance;

        RaycastHit[] hits = new RaycastHit[10];
        int hitCount = Physics.SphereCastNonAlloc(
            startPosition,
            _sphereRadius,
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
                return true;
        }

        return false;
    }
}