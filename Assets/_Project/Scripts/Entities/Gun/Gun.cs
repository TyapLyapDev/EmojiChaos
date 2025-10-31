using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunVisual _visual;
    [SerializeField] private Transform _rotatingModel;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _shotDelay;

    private Aimer _aim;
    private IntervalRunner _runner;
    private Shooter _shooter;
    private bool _isInitialized;

    public event Action<Bullet> BulletActivated;

    public bool IsActive => gameObject.activeInHierarchy;

    private void OnDestroy() =>
        _runner.Dispose();

    public void Initialize(Pool<Bullet> bulletPool, EnemyRegistry enemyRegistry, Action<Bullet> bulletActivated)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

        if (_visual == null)
            throw new NullReferenceException(nameof(_visual));

        if (_rotatingModel == null)
            throw new NullReferenceException(nameof(_rotatingModel));

        if (_bulletStartPosition == null)
            throw new NullReferenceException(nameof(_bulletStartPosition));

        if(_shotDelay <= 0)
            throw new ArgumentOutOfRangeException(nameof(_shotDelay), "Значение должно быть больше нуля");

        _visual.Initialize();
        _runner = new(OnShootTick);
        _aim = new(_rotatingModel);
        _shooter = new(bulletPool, enemyRegistry, _bulletStartPosition, bulletActivated);

        _isInitialized = true;
    }

    public void Activate(int carType, int bulletCount, Color color)
    {
        ValidateInitialization(nameof(Activate));

        if (bulletCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount), "Значение должно быть больше нуля");

        _shooter.Activate(bulletCount, carType, color);
        _visual.SetColor(color);
        _visual.SetBulletCount(bulletCount);
        _aim.ResetRotation();
        gameObject.SetActive(true);

        _runner.StartRunning(_shotDelay);
    }

    private void Deactivate()
    {
        _runner.StopRunning();
        gameObject.SetActive(false);
    }

    private void OnShootTick()
    {
        if(_shooter.TryShoot(out Bullet bullet))
        {
            _aim.AimAtTarget(bullet.Target.transform);
            _visual.SetBulletCount(_shooter.BulletCount);
            BulletActivated?.Invoke(bullet);
        }

        if (_shooter.IsActive == false)
            Deactivate();
    }

    private void ValidateInitialization(string methodName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызыван перед инициализацией. Сначала вызовите{nameof(Initialize)}");
    }
}