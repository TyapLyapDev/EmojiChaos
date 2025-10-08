using System;
using UnityEngine;

public class BulletMover
{
    private const float Threshold = 0.001f;

    private readonly Transform _transform;

    public BulletMover(Transform transform)
    {
        _transform = transform;
    }

    public event Action TargetReached;

    public void Move(Transform target, float deltaSpeed)
    {
        if (target == null)
            return;

        _transform.LookAt(target);

        Vector3 direction = target.position - _transform.position;
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
}