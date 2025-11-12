using System;
using UnityEngine;

public class Bullet : InitializingBehaviour, IPoolable<Bullet>
{
    [SerializeField] private BulletVisual _visual;

    private BulletMover _mover;
    private IHittable _target;

    public event Action<Bullet> Deactivated;

    public IHittable Target => GetSafeReference(_target);

    private void OnDestroy()
    {
        if (_mover != null)
            _mover.TargetReached -= OnTargetReached;

        UnsubscribeFromTarget();
    }

    public void Activate(IHittable target, Vector3 startPosition)
    {
        ValidateInit(nameof(Activate));

        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (target.IsActive == false)
            throw new InvalidOperationException("Невозможно активировать пулю при неактивной цели");

        _target = target;
        _visual.SetColor(target.Color);
        _mover.SetStartPosition(startPosition);
        _mover.SetTarget(_target.Center);

        _target.Disappeared += OnTargetDeactivated;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        ValidateInit(nameof(Deactivate));

        gameObject.SetActive(false);
        UnsubscribeFromTarget();
        _mover.ResetTarget();

        Deactivated?.Invoke(this);
    }

    public void Move(float deltaDistance)
    {
        ValidateInit(nameof(Move));

        _mover.Move(deltaDistance);
    }

    protected override void OnInitialize()
    {
        _visual.Initialize();
        _mover = new(transform);
        _mover.TargetReached += OnTargetReached;
    }

    private void OnTargetReached()
    {
        _target?.Kill();

        Deactivate();
    }

    private void OnTargetDeactivated(IHittable hittable) =>
        Deactivate();

    private void UnsubscribeFromTarget()
    {
        if (_target == null)
            return;

        _target.Disappeared -= OnTargetDeactivated;
        _target = null;
    }
}