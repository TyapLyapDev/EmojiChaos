using UnityEngine;

public class CarRollBackMoverStrategy : IMovementStrategy
{
    private const float Bisector = 2;
    private const float SpeedMultiplier = 0.3f;

    private readonly Transform _transform;
    private readonly Collider _selfCollider;
    private readonly int _direction;

    private readonly float _sphereRadius;
    private readonly float _forwardOffset;

    public CarRollBackMoverStrategy(Transform transform, BoxCollider self, int direction)
    {
        _transform = transform;
        _selfCollider = self;
        _direction = direction;

        _sphereRadius = self.size.x * transform.lossyScale.x / Bisector;
        _forwardOffset = self.size.z * transform.lossyScale.z / Bisector + _sphereRadius;
    }

    public event System.Action<CarRollBackMoverStrategy> Stopped;

    public void Move(float deltaDistance)
    {
        deltaDistance *= SpeedMultiplier;
        Vector3 direction = _direction * _transform.forward;

        if (IsCollision(deltaDistance, direction) && IsCollision(deltaDistance, -direction) == false)
        {
            _transform.position -= _direction * deltaDistance * _transform.forward;

            return;
        }

        Stopped?.Invoke(this);
    }

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