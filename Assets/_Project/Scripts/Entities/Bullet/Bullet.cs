using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private BulletVisual _visual;

    private BulletMover _mover;
    private Enemy _target;
    private bool _isInitialized;
    private bool _isDeactivating;

    public event Action<Bullet> Deactivated;

    public Enemy Target => _target;

    private void OnDestroy()
    {
        _isInitialized = false;

        if (_mover != null)
            _mover.TargetReached -= OnTargetReached;

        UnsubscribeFromTarget();
    }

    public void Move(float deltaDistance)
    {
        if (_isInitialized == false)
            return;

        _mover.Move(deltaDistance);
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

    public void Activate(Enemy target, Color color, Vector3 startPosition)
    {
        ValidateInitialization(nameof(Activate));

        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (target.IsActive == false)
            throw new InvalidOperationException("Невозможно активировать пулю при неактивной цели");

        _target = target;
        _visual.SetColor(color);
        _mover.SetStartPosition(startPosition);
        _mover.SetTarget(_target.transform);

        target.Deactivated += OnTargetDeactivated;

        _isDeactivating = false;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        ValidateInitialization(nameof(Deactivate));

        if (_isDeactivating)
            return;

        _isDeactivating = true;

        gameObject.SetActive(false);
        UnsubscribeFromTarget();
        _mover.ResetTarget();

        Deactivated?.Invoke(this);
    }

    private void OnTargetReached()
    {
        if (_target != null && _target.IsActive)
            _target.Deactivate();
    }

    private void OnTargetDeactivated(Enemy _) =>
        Deactivate();

    private void UnsubscribeFromTarget()
    {
        if (_target != null)
        {
            _target.Deactivated -= OnTargetDeactivated;
            _target = null;
        }
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите{nameof(Initialize)}");
    }
}