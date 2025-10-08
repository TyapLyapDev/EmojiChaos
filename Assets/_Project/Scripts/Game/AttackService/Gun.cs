using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunVisual _visual;
    [SerializeField] private Transform _modelToRotation;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _delay;

    private List<Enemy> _enemies = new();
    private WaitForSeconds _wait;
    private Coroutine _shootCoroutine;
    private Pool<Bullet> _bulletPool;
    private Color _color;
    private int _bulletCount;
    private int _id;

    public event Action Finished;

    private void OnEnable() =>
        _shootCoroutine = StartCoroutine(Shooting());

    public void Initialize(Pool<Bullet> bulletPool, List<Enemy> enemies)
    {
        _bulletPool = bulletPool;
        _enemies = enemies;
        _visual.Initialize();
        _wait = new(_delay);
    }

    public void Activate(int id, int bulletCount, Color color)
    {
        if (bulletCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount), "Значение должно быть больше нуля");

        _id = id;
        _bulletCount = bulletCount;
        _color = color;
        _visual.SetColor(color);
        _visual.SetText(bulletCount);
        gameObject.SetActive(true);
        _modelToRotation.rotation = Quaternion.identity;
    }

    private IEnumerator Shooting()
    {
        while (_bulletCount > 0)
        {
            yield return _wait;

            Shoot();
        }

        Finished?.Invoke();
    }

    private void Shoot()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null && enemy.Id == _id)
            {
                if (_bulletPool.TryGet(out Bullet bullet))
                {
                    _modelToRotation.LookAt(enemy.transform);
                    bullet.transform.position = _bulletStartPosition.position;
                    bullet.Activate(enemy, _color);
                    _bulletCount--;
                    _visual.SetText(_bulletCount);
                    _enemies.Remove(enemy);
                }

                return;
            }
        }
    }
}