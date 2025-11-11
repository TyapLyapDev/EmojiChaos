using System;
using UnityEngine;

public class Shooter
{
    private readonly Pool<Bullet> _bulletPool;
    private readonly EnemyRegistryToAttack _enemyRegistry;
    private readonly BulletSpeedDirector _bulletSpeedDirector;

    private Transform _bulletStartPosition;
    private int _bulletCount;
    private int _bulletType;

    public Shooter(Pool<Bullet> bulletPool, EnemyRegistryToAttack enemyRegistry, BulletSpeedDirector bulletSpeedDirector)
    {
        _bulletPool = bulletPool ?? throw new ArgumentNullException(nameof(bulletPool));
        _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));
        _bulletSpeedDirector = bulletSpeedDirector ?? throw new ArgumentNullException(nameof(bulletSpeedDirector));
    }

    public event Action Completed;

    public int BulletCount => _bulletCount;

    private bool IsHaveBullet => _bulletCount > 0;

    public void SetStartPosition(Transform position)
    {
        if (position == null)
            throw new ArgumentNullException(nameof(position));

        _bulletStartPosition = position;
    }

    public void Activate(int bulletCount, int bulletType)
    {
        if (bulletCount < 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount), " оличество патронов не может быть отрицательным");

        _bulletCount = bulletCount;
        _bulletType = bulletType;
    }        

    public bool TryShoot(out Bullet bullet)
    {
        bullet = null;

        if (IsHaveBullet == false)
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
        if(bullet == null)
            throw new ArgumentNullException(nameof(bullet));

        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        bullet.Activate(enemy, _bulletStartPosition.position);
        _bulletCount--;

        if (IsHaveBullet == false)
            Completed?.Invoke();

        _bulletSpeedDirector?.RegisterBullet(bullet);
    }
}