using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class EnemySpeedDirector : System.IDisposable
{
    private const float ReverseMovementMultiplier = 2.5f;

    private readonly List<EnemyMovementInfo> _enemies = new();
    private readonly CompositeDisposable _disposables = new();
    private readonly float _speed;

    public EnemySpeedDirector(float speed)
    {
        _speed = speed;        
    }

    public void Dispose()
    {
        List<EnemyMovementInfo> enemies = new(_enemies);

        foreach (EnemyMovementInfo enemy in enemies)
            OnEnemyDeactivated(enemy.Enemy);

        _enemies.Clear();
        _disposables?.Dispose();
    }

    public void StartRun()
    {
        Observable.EveryUpdate().
            Where(_ => _enemies.Count > 0).
            Subscribe(_ => UpdateMovement()).
            AddTo(_disposables);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        float forwardDistance;

        if (_enemies.Count == 0)
            forwardDistance = 0;
        else
            forwardDistance = CalculateDistance(_enemies.Last().Enemy, enemy);

        EnemyMovementInfo enemyMovementInfo = new(enemy, forwardDistance);
        _enemies.Add(enemyMovementInfo);
        enemy.Deactivated += OnEnemyDeactivated;
    }

    private void UpdateMovement()
    {
        float baseDeltaDistance = _speed * Time.deltaTime;

        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            EnemyMovementInfo currentEnemyInfo = _enemies[i];

            if (currentEnemyInfo?.Enemy == null)
                continue;

            float currentDeltaDistance = baseDeltaDistance;
            int nextEnemyIndex = i + 1;

            if (_enemies[i] != null)
            {
                if (nextEnemyIndex < _enemies.Count && _enemies[nextEnemyIndex] != null)
                {
                    Enemy nextEnemy = _enemies[nextEnemyIndex].Enemy;
                    float actualDistance = CalculateDistance(currentEnemyInfo.Enemy, nextEnemy);
                    float requiredDistance = _enemies[nextEnemyIndex].RequiredDistance;

                    if (actualDistance > requiredDistance)
                    {
                        currentDeltaDistance *= -ReverseMovementMultiplier;
                        currentDeltaDistance = Mathf.Min(currentDeltaDistance, actualDistance - requiredDistance);
                    }
                }

                currentEnemyInfo.Enemy.Move(currentDeltaDistance);
            }
        }
    }

    private void OnEnemyDeactivated(Enemy enemy)
    {
        if (enemy == null)
            throw new System.ArgumentNullException(nameof(enemy));

        EnemyMovementInfo enemyToRemove = _enemies.Find(info => info?.Enemy == enemy);

        if (enemyToRemove != null)
            _enemies.Remove(enemyToRemove);

        enemy.Deactivated -= OnEnemyDeactivated;
    }

    private float CalculateDistance(Enemy first, Enemy second)
    {
        if (first == null)
            throw new System.ArgumentNullException(nameof(first));

        if (second == null)
            throw new System.ArgumentNullException(nameof(second));

        return first.SplineDistance - second.SplineDistance;
    }
}