using DG.Tweening;
using System;
using UnityEngine;

public class Gun : InitializingWithConfigBehaviour<GunParam>
{
    private const float HiddingTime = 0.25f;

    [SerializeField] private GunVisual _visual;
    [SerializeField] private Transform _rotatingModel;
    [SerializeField] private Transform _bulletStartPosition;

    private GunParam _config;
    private Aimer _aim;
    private IntervalRunner _runner;
    private Tween _deactivateTween;
    private int _id;

    public Transform Center => _rotatingModel;

    public bool IsAvailable => _config.Shooter.IsHaveBullet == false;

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

        _id = carType;
        _deactivateTween?.Kill();
        _config.Shooter.Activate(bulletCount, carType);
        _visual.SetColor(color);
        _visual.DisplayBulletCount(bulletCount);
        _aim.ResetRotation();
        gameObject.SetActive(true);
        _runner.StartRunning(_config.TimeReload);
        Audio.Sfx.PlayGunInstalled();
    }

    protected override void OnInitialize(GunParam config)
    {
        _config = config;

        _visual.Initialize();

        _runner = new(OnShootTick);
        _aim = new(_rotatingModel);
        _config.Shooter.SetStartPosition(_bulletStartPosition);
    }

    private void Deactivate() =>
        gameObject.SetActive(false);

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

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Vector3 position = transform.position + Vector3.up * 0.3f + Vector3.forward * -0.14f;
        string text = _id.ToString();

        float bgRadius = 0.1f;
        UnityEditor.Handles.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        UnityEditor.Handles.DrawSolidDisc(position, Camera.current?.transform.forward ?? Vector3.forward, bgRadius);

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(position, Camera.current?.transform.forward ?? Vector3.forward, bgRadius);

        GUIStyle style = new()
        {
            fontSize = 12,
            normal = { textColor = Color.yellow },
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        UnityEditor.Handles.Label(position, text, style);
#endif
    }
}