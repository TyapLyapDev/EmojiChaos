using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private BulletVisual _visual;

    private BulletMover _mover;
    private Enemy _enemyTarget;
    private bool _isInitialized;

    public event Action<Bullet> Deactivated;

    public Transform Target => _enemyTarget.BulletTarget;

    private void OnDestroy()
    {
        _isInitialized = false;

        if (_mover != null)
        {
            _mover.TargetReached -= OnTargetReached;
            _mover = null;
        }

        UnsubscribeFromTarget();
    }

    public void Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

        _visual.Initialize();
        _mover = new(transform);
        _mover.TargetReached += OnTargetReached;

        _isInitialized = true;
    }

    public void Activate(Enemy enemyTarget, Vector3 startPosition)
    {
        ValidateInitialization(nameof(Activate));

        if (enemyTarget == null)
            throw new ArgumentNullException(nameof(enemyTarget));

        if (enemyTarget.IsActive == false)
            throw new InvalidOperationException("Невозможно активировать пулю при неактивной цели");

        _enemyTarget = enemyTarget;
        _visual.SetColor(enemyTarget.Color);
        _mover.SetStartPosition(startPosition);
        _mover.SetTarget(_enemyTarget.BulletTarget);

        _enemyTarget.Deactivated += OnTargetDeactivated;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        ValidateInitialization(nameof(Deactivate));

        gameObject.SetActive(false);
        UnsubscribeFromTarget();
        _mover.ResetTarget();

        Deactivated?.Invoke(this);
    }

    public void Move(float deltaDistance)
    {
        ValidateInitialization(nameof(Move));

        _mover.Move(deltaDistance);
    }

    private void OnTargetReached()
    {
        if (_enemyTarget != null)
            _enemyTarget.Kill();

        Deactivate();
    }

    private void OnTargetDeactivated(Enemy _) =>
        Deactivate();

    private void UnsubscribeFromTarget()
    {
        if (_enemyTarget == null)
            return;

        _enemyTarget.Deactivated -= OnTargetDeactivated;
        _enemyTarget = null;
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите {nameof(Initialize)}");
    }
}