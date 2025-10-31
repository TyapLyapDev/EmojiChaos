using System;
using UnityEngine;

public class Shooter
{
    private readonly Pool<Bullet> _bulletPool;
    private readonly EnemyRegistry _enemyRegistry;
    private readonly Transform _bulletStartPosition;
    private readonly Action<Bullet> _bulletActivated;

    private Color _bulletColor;
    private int _bulletCount;
    private int _bulletType;

    public Shooter(Pool<Bullet> bulletPool, EnemyRegistry enemyRegistry, Transform bulletStartPosition, Action<Bullet> bulletActivated)
    {
        _bulletPool = bulletPool ?? throw new ArgumentNullException(nameof(bulletPool));
        _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));
        _bulletStartPosition = bulletStartPosition != null ? bulletStartPosition : throw new ArgumentNullException(nameof(bulletStartPosition));
        _bulletActivated = bulletActivated ?? throw new ArgumentNullException(nameof(bulletActivated));
    }

    public bool IsActive => _bulletCount > 0;

    public int BulletCount => _bulletCount;

    public void Activate(int bulletCount, int bulletType, Color bulletColor)
    {
        if (bulletCount < 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount), " оличество патронов не может быть отрицательным");

        _bulletCount = bulletCount;
        _bulletType = bulletType;
        _bulletColor = bulletColor;
    }        

    public bool TryShoot(out Bullet bullet)
    {
        bullet = null;

        if (IsActive == false)
            return false;

        if (_bulletPool.TryGive(out bullet) == false)
            return false;

        if (_enemyRegistry.TryGiveEnemy(_bulletType, out Enemy enemy) == false)
        {
            _bulletPool.Return(bullet);

            return false;
        }

        PerformShot(bullet, enemy);

        return true;
    }

    private void PerformShot(Bullet bullet, Enemy enemy)
    {
        bullet.Activate(enemy, _bulletColor, _bulletStartPosition.position);
        _bulletCount--;

        _bulletActivated?.Invoke(bullet);
    }
}