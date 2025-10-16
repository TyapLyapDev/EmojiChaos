using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class BulletSpeedDirector : System.IDisposable
{
    private const float Speed = 8;

    private readonly List<Bullet> _bullets = new();
    private readonly CompositeDisposable _disposables = new();

    public BulletSpeedDirector()
    {
        Observable.EveryUpdate()
            .Where(_ => _bullets.Count > 0)
            .Subscribe(_ => UpdateMovement())
            .AddTo(_disposables);
    }

    public void Dispose()
    {
        List<Bullet> bullets = _bullets;

        foreach (Bullet bullet in bullets)
            if (bullet != null)
                bullet.Deactivated -= OnBulletDeactivated;

        _bullets.Clear();
        _disposables?.Dispose();
    }

    public void RegisterBullet(Bullet bullet)
    {
        if (bullet == null)
            throw new System.ArgumentNullException(nameof(bullet));

        if (_bullets.Contains(bullet))
            throw new System.InvalidOperationException($"{nameof(RegisterBullet)} Попытка повторной регистрации пули");

        _bullets.Add(bullet);
        bullet.Deactivated += OnBulletDeactivated;
    }

    private void UpdateMovement()
    {
        float deltaDistance = Speed * Time.deltaTime;

        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            Bullet bullet = _bullets[i];

            if (bullet == null)
            {
                _bullets.RemoveAt(i);
                continue;
            }

            bullet.Move(deltaDistance);
        }
    }

    private void OnBulletDeactivated(Bullet bullet)
    {
        if (bullet == null)
            throw new System.ArgumentNullException(nameof(bullet));

        bullet.Deactivated -= OnBulletDeactivated;
        _bullets.Remove(bullet);
    }
}