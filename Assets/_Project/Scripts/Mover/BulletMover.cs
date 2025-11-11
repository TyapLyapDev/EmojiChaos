using System;
using UnityEngine;

public class BulletMover
{
    private const float Threshold = 0.001f;

    private readonly Transform _transform;

    private Transform _target;

    public BulletMover(Transform transform)
    {
        _transform = transform;
    }

    public event Action TargetReached;

    public void Move(float deltaSpeed)
    {
        if (_target == null)
            return;

        if(deltaSpeed < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaSpeed), "Значение должно быть положительным");

        _transform.LookAt(_target);

        Vector3 direction = _target.position - _transform.position;
        float sqrDistance = direction.sqrMagnitude;

        if (sqrDistance <= Threshold * Threshold)
        {
            TargetReached?.Invoke();

            return;
        }

        float distance = Mathf.Sqrt(sqrDistance);
        float moveDistance = Mathf.Min(deltaSpeed, distance);

        _transform.position += direction * (moveDistance / distance);

        if (moveDistance >= distance - Threshold)
            TargetReached?.Invoke();
    }

    public void SetStartPosition(Vector3 position) =>
        _transform.position = position;

    public void SetTarget(Transform target)
    {
        if(target == null)
            throw new ArgumentNullException(nameof(target));

        _target = target;
        _transform.LookAt(_target);
    }

    public void ResetTarget() =>
        _target = null;
}