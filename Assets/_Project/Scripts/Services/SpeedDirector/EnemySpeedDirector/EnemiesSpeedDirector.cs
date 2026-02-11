using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class EnemiesSpeedDirector : IDisposable
{
    private const float BackwardSpeedMultiplier = 2f;

    private readonly EnemySpawner _spawner;
    private readonly List<EnemyMovementInfo> _enemies = new();
    private readonly CompositeDisposable _disposables = new();
    private readonly Portal _portal;
    private readonly float _speed;
    private bool _isPause;

    public EnemiesSpeedDirector(EnemySpawner enemySpawner, Portal portal, float speed)
    {
        _spawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
        _portal = portal != null ? portal : throw new ArgumentNullException(nameof(_portal));

        if (speed <= 0)
            throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be greater than zero");

        _speed = speed;

        _spawner.Spawned += OnEnemySpawned;
    }

    public event Action<float> FirstEnemyProgressChanged;
    public event Action FirstEnemyFinished;

    public void Dispose()
    {
        if (_spawner != null)
            _spawner.Spawned -= OnEnemySpawned;

        List<EnemyMovementInfo> enemies = new(_enemies);

        foreach (EnemyMovementInfo enemy in enemies)
            if (enemy.Enemy != null)
                OnEnemyKilled(enemy.Enemy);

        _enemies.Clear();
        _disposables?.Dispose();
    }

    public void Run()
    {
        Observable.EveryUpdate().
            Where(_ => _enemies.Count > 0).
            Subscribe(_ => UpdateMovement()).
            AddTo(_disposables);
    }

    public void Pause() =>
        _isPause = true;

    public void Resume() =>
        _isPause = false;

    private void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));        

        if (_enemies.Count > 0)
        { 
            EnemyMovementInfo lastEnemyInfo = _enemies.Last();
            float distance = CalculateDistance( lastEnemyInfo.Enemy, enemy);

            if (lastEnemyInfo.Distance == 0)
                lastEnemyInfo.SetDistance(distance);
        }

        EnemyMovementInfo currentEnemytInfo = new(enemy, 0);
        _enemies.Add(currentEnemytInfo);
        enemy.Killed += OnEnemyKilled;
    }

    private void UpdateMovement()
    {
        if (_isPause)
            return;

        float baseDeltaDistance = _speed * Time.deltaTime;
        List<EnemyMovementInfo> enemies = _enemies.Where(enemy => enemy != null).ToList();

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            EnemyMovementInfo currentEnemyInfo = enemies[i];
            Enemy currentEnemy = currentEnemyInfo.Enemy;
            float currentDeltaDistance = baseDeltaDistance;
            int nextEnemyIndex = i + 1;

            if(nextEnemyIndex < enemies.Count)
            {
                EnemyMovementInfo nextEnemyInfo = enemies[nextEnemyIndex];
                Enemy nextEnemy = enemies[nextEnemyIndex].Enemy;
                float actualDistance = CalculateDistance(currentEnemy, nextEnemy);
                float requiredDistance = currentEnemyInfo.Distance;

                if (actualDistance > requiredDistance)
                {
                    currentDeltaDistance *= -BackwardSpeedMultiplier;
                    currentDeltaDistance = Mathf.Min(currentDeltaDistance, actualDistance - requiredDistance);
                }
            }

            currentEnemy.Move(currentDeltaDistance);
        }

        float firstEnemyProgress = enemies[0].Enemy.Progress;
        FirstEnemyProgressChanged?.Invoke(firstEnemyProgress);

        if (Mathf.Approximately(firstEnemyProgress, 1f))
            FirstEnemyFinished?.Invoke();
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        EnemyMovementInfo enemyToRemove = _enemies.Find(info => info?.Enemy == enemy);

        if (enemyToRemove != null)
            _enemies.Remove(enemyToRemove);

        enemy.Killed -= OnEnemyKilled;
    }

    private float CalculateDistance(Enemy first, Enemy second)
    {
        if (first == null)
            throw new ArgumentNullException(nameof(first));

        if (second == null)
            throw new ArgumentNullException(nameof(second));

        return first.SplineDistance - second.SplineDistance;
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        RegisterEnemy(enemy);
    }
}