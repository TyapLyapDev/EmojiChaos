using System.Collections.Generic;
using UnityEngine;

public class AttackSystem
{
    private readonly List<SlotAttack> _slots;
    private readonly List<Enemy> _enemies = new();
    private readonly Pool<Bullet> _bulletPool;

    public AttackSystem(List<SlotAttack> slots, Bullet prefab)
    {
        _slots = slots;
        _bulletPool = new(prefab, OnBulletCreated, null);

        foreach (SlotAttack slot in _slots)
            if(slot != null)
                slot.Initialize(_bulletPool, _enemies);
    }

    public bool TryApply(int id, int bulletCount, Color color)
    {
        foreach (SlotAttack slot in _slots)
        {
            if (slot.IsAvailable)
            {
                slot.Apply(id, bulletCount, color);

                return true;
            }
        }

        return false;
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
        enemy.Deactivated += RemoveEnemy;
    }

    public void RemoveEnemy(Enemy enemy) 
    {
        enemy.Deactivated -= RemoveEnemy;
        _enemies.Remove(enemy);
    }

    private void OnBulletCreated(Bullet bullet) =>
        bullet.Initialize();
}