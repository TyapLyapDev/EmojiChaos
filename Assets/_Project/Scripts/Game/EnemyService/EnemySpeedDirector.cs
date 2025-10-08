using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedDirector : IDisposable
{
    private readonly MonoBehaviour _runner;
    private readonly List<Enemy> _enemies = new();
    private readonly float _speed;

    private bool _isRunning;

    private Coroutine _coroutine;

    public EnemySpeedDirector(MonoBehaviour runner, float speed)
    {
        _runner = runner;
        _speed = speed;
    }

    public void Dispose()
    {
        List<Enemy> enemies = new(_enemies);

        foreach (Enemy enemy in enemies)
            UnsubscribeEnemy(enemy);

        _enemies.Clear();

        _isRunning = false;

        if (_coroutine != null)
            _runner.StopCoroutine(_coroutine);
    }

    public void Run()
    {
        _isRunning = true;
        _coroutine = _runner.StartCoroutine(Moving());
    }

    public void AddEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        _enemies.Add(enemy);
        enemy.Deactivated += UnsubscribeEnemy;
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

    private void UnsubscribeEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        _enemies.Remove(enemy);
        enemy.Deactivated -= UnsubscribeEnemy;
    }
}