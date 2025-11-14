using System;
using UnityEngine;

public class Gun : InitializingWithConfigBehaviour<GunConfig>
{
    [SerializeField] private GunVisual _visual;
    [SerializeField] private Transform _rotatingModel;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _shotDelay;

    private GunConfig _config;
    private Aimer _aim;
    private IntervalRunner _runner;

    public event Action ShootingCompleted;

    public bool IsActive => gameObject.activeInHierarchy;

    private void OnDestroy()
    {
        if (_config.Shooter != null)
            _config.Shooter.Completed -= OnShootingCompleted;

        _runner?.Dispose();
    }

    public void Activate(int carType, int bulletCount, Color color)
    {
        ValidateInit(nameof(Activate));

        if (bulletCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount), "Значение должно быть больше нуля");

        _config.Shooter.Activate(bulletCount, carType);
        _visual.SetColor(color);
        _visual.DisplayBulletCount(bulletCount);
        _aim.ResetRotation();
        gameObject.SetActive(true);

        _runner.StartRunning(_shotDelay);
    }

    protected override void OnInitialize(GunConfig config)
    {
        _config = config;

        if (_shotDelay <= 0)
            throw new ArgumentOutOfRangeException(nameof(_shotDelay), "Значение должно быть больше нуля");

        _visual.Initialize();
        _runner = new(OnShootTick);
        _aim = new(_rotatingModel);
        _config.Shooter.SetStartPosition(_bulletStartPosition);
        _config.Shooter.Completed += OnShootingCompleted;
    }

    private void Deactivate()
    {
        _runner.StopRunning();
        gameObject.SetActive(false);
    }

    private void OnShootTick()
    {
        if (_config.Shooter.TryShoot(out Bullet bullet))
        {
            IHittable enemyTarget = bullet.Target;

            if (enemyTarget != null)
                _aim.AimAtTarget(enemyTarget.Center);

            _visual.DisplayBulletCount(_config.Shooter.BulletCount);
            _config.ParticleShower.ShowSmoke(_bulletStartPosition.position, _bulletStartPosition.rotation);
        }
    }

    private void OnShootingCompleted()
    {
        _config.ParticleShower.ShowBlood(transform.position, transform.rotation, _visual.Color);
        Deactivate();
        ShootingCompleted?.Invoke();
    }
}