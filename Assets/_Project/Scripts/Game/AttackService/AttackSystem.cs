using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : IDisposable
{
    private readonly List<AttackSlot> _slots;
    private readonly Pool<Bullet> _bulletPool;
    private readonly EnemyRegistry _enemyRegistry = new();
    private readonly BulletSpeedDirector _bulletSpeedDirector = new();

    public AttackSystem(List<AttackSlot> slots, Bullet prefab)
    {
        _slots = slots ?? throw new ArgumentNullException(nameof(slots));
        _bulletPool = new(prefab, OnBulletCreated, null);

        foreach (AttackSlot slot in _slots)
            if(slot != null)
                slot.Initialize(_bulletPool, _enemyRegistry, OnBulletActivated);
    }

    public void Dispose()
    {
        _bulletSpeedDirector.Dispose();
        _enemyRegistry.Dispose();
    }

    public bool TryInstallGun(int carType, int bulletCount, Color color)
    {
        foreach (AttackSlot slot in _slots)
            if (slot.TryActivateGun(carType, bulletCount, color))
                return true;

        return false;
    }

    public bool TryGetAvailableSlot(out AttackSlot attackSlot)
    {
        foreach (AttackSlot slot in _slots)
        {
            if (slot.IsAvailable)
            {
                attackSlot = slot;

                return true;
            }
        }            

        attackSlot = null;

        return false;
    }

    public void RegisterEnemy(Enemy enemy) =>
        _enemyRegistry.RegisterEnemy(enemy);

    private void OnBulletCreated(Bullet bullet) =>
        bullet.Initialize();

    private void OnBulletActivated(Bullet bullet) =>
        _bulletSpeedDirector.RegisterBullet(bullet);
}