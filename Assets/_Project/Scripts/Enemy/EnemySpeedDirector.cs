using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedDirector : IDisposable
{
    private readonly MonoBehaviour _mono;
    private readonly EnemySpawner _spawner;
    private readonly List<Enemy> _enemies = new();
    private readonly WaitForEndOfFrame _waitForEndOfFrame = new();
    private readonly float _speed;

    private Coroutine _coroutine;

    public EnemySpeedDirector(MonoBehaviour mono, EnemySpawner spawner, float speed)
    {
        _mono = mono;
        _spawner = spawner;
        _speed = speed;
        
        Subscribe();        
    }

    public void Run() =>
        _coroutine = _mono.StartCoroutine(Moving());

    public void Dispose()
    {
        Unsubscribe();

        if(_coroutine != null)
            _mono.StopCoroutine(_coroutine);
    }

    private IEnumerator Moving()
    {
        while (_mono.enabled)
        {
            foreach (Enemy enemy in _enemies)
                enemy.Move(_speed, Time.deltaTime);

            yield return _waitForEndOfFrame;
        }        
    }

    private void Subscribe() =>
        _spawner.Spawned += OnEnemySpawned;

    private void Unsubscribe()
    {
        _spawner.Spawned -= OnEnemySpawned;

        foreach (Enemy enemy in _enemies)
            enemy.Deactivated -= UnsubscribeEnemy;
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        _enemies.Add(enemy);
        enemy.Deactivated += UnsubscribeEnemy;
    }

    private void UnsubscribeEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        enemy.Deactivated -= UnsubscribeEnemy;
    }
}