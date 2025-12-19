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
    private readonly float _speed;
    private bool _isPause;

    public EnemiesSpeedDirector(EnemySpawner enemySpawner, float speed)
    {
        _spawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));

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

        float forwardDistance;

        if (_enemies.Count == 0)
            forwardDistance = 0;
        else
            forwardDistance = CalculateDistance(_enemies.Last().Enemy, enemy);

        EnemyMovementInfo enemyMovementInfo = new(enemy, forwardDistance);
        _enemies.Add(enemyMovementInfo);
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

            if (currentEnemyInfo?.Enemy == null)
                continue;

            float currentDeltaDistance = baseDeltaDistance;
            int nextEnemyIndex = i + 1;

            if (nextEnemyIndex < enemies.Count && enemies[nextEnemyIndex] != null)
            {
                Enemy nextEnemy = enemies[nextEnemyIndex].Enemy;
                float actualDistance = CalculateDistance(currentEnemyInfo.Enemy, nextEnemy);
                float requiredDistance = enemies[nextEnemyIndex].RequiredDistance;

                if (actualDistance > requiredDistance)
                {
                    currentDeltaDistance *= -BackwardSpeedMultiplier;
                    currentDeltaDistance = Mathf.Min(currentDeltaDistance, actualDistance - requiredDistance);
                }
            }

            currentEnemyInfo.Enemy.Move(currentDeltaDistance);
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