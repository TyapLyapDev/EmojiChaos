using DG.Tweening;
using System;
using UnityEngine;

public class Gun : InitializingWithConfigBehaviour<GunConfig>
{
    private const float HiddingTime = 0.35f;

    [SerializeField] private GunVisual _visual;
    [SerializeField] private Transform _rotatingModel;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _shotDelay;

    private GunConfig _config;
    private Aimer _aim;
    private IntervalRunner _runner;
    private Tween _deactivateTween;

    public event Action ShootingCompleted;

    public bool IsActive => gameObject.activeInHierarchy;

    private void OnDestroy()
    {
        _runner?.Dispose();
        _deactivateTween?.Kill();
    }

    public void Activate(int carType, int bulletCount, Color color)
    {
        ValidateInit(nameof(Activate));

        if (bulletCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount), "Значение должно быть больше нуля");

        _deactivateTween?.Kill();
        _config.Shooter.Activate(bulletCount, carType);
        _visual.SetColor(color);
        _visual.DisplayBulletCount(bulletCount);
        _aim.ResetRotation();
        gameObject.SetActive(true);

        Audio.Sfx.PlayGunInstalled();

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
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        ShootingCompleted?.Invoke();
    }

    private void OnShootTick()
    {
        if (_config.Shooter.TryShoot(out Bullet bullet))
        {
            IHittable enemyTarget = bullet.Target;

            if (enemyTarget != null)
                _aim.AimAtTarget(enemyTarget.CenterBody);

            if (_config.Shooter.IsHaveBullet)
            {
                _visual.SetShooting();
                _visual.DisplayBulletCount(_config.Shooter.BulletCount);
                Audio.Sfx.PlayGunShot();
            }
            else
            {
                HandleShootingCompleted();
            }

            _config.ParticleShower.ShowSmoke(_bulletStartPosition.position, _bulletStartPosition.rotation);
        }
    }

    private void HandleShootingCompleted()
    {
        _runner.StopRunning();
        _visual.SetHidding();
        _deactivateTween = DOVirtual.DelayedCall(HiddingTime, Deactivate).SetUpdate(UpdateType.Normal, false);
        Audio.Sfx.PlayGunDisapperence();
    }
}