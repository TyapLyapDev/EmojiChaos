using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedDirector : IDisposable
{
    private readonly MonoBehaviour _mono;
    private readonly EnemySpawner _spawner;
    private readonly List<Enemy> _enemies = new();
    private readonly float _speed;

    private bool _isRunning;

    private Coroutine _coroutine;

    public EnemySpeedDirector(MonoBehaviour mono, EnemySpawner spawner, float speed)
    {
        _mono = mono;
        _spawner = spawner;
        _speed = speed;

        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();

        _isRunning = false;

        if (_coroutine != null)
            _mono.StopCoroutine(_coroutine);


    }

    public void Run()
    {
        _isRunning = true;
        _coroutine = _mono.StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (_isRunning)
        {
            Move();

            yield return null;
        }
    }

    private void Move()
    {
        float deltaTime = Time.deltaTime;
        List<Enemy> enemies = new(_enemies);

        foreach (Enemy enemy in enemies)
            if (enemy != null)
                enemy.Move(_speed, deltaTime);
    }

    private void Subscribe() =>
        _spawner.Spawned += OnEnemySpawned;

    private void Unsubscribe()
    {
        _spawner.Spawned -= OnEnemySpawned;

        foreach (Enemy enemy in _enemies)
            if (enemy != null)
                enemy.Deactivated -= UnsubscribeEnemy;

        _enemies.Clear();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (enemy == null)
            return;

        _enemies.Add(enemy);
        enemy.Deactivated += UnsubscribeEnemy;
    }

    private void UnsubscribeEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        _enemies.Remove(enemy);
        enemy.Deactivated -= UnsubscribeEnemy;
    }
}