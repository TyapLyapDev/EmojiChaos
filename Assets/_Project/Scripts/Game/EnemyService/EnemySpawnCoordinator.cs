using System;
using System.Collections;
using UnityEngine;

public class EnemySpawnCoordinator
{
    private readonly EnemyFormationCalculator _offsetCalculator;
    private readonly SpawnStrategyRegistry _spawnStrategy;
    private readonly MonoBehaviour _runner;

    private Coroutine _currentSpawningCoroutine;    

    public EnemySpawnCoordinator(MonoBehaviour coroutineRunner)
    {
        if (coroutineRunner == null)
            throw new ArgumentNullException(nameof(coroutineRunner));

        _runner = coroutineRunner;

        _offsetCalculator = new();
        _spawnStrategy = new();
    }

    public event Action<Enemy> Spawned;

    public void StartSpawningSequence(WaveSequencer waveSequencer, Pool<Enemy> pool, float gameSpeed)
    {
        if(waveSequencer == null)
            throw new ArgumentNullException(nameof(waveSequencer));

        if(pool == null)
            throw new ArgumentNullException(nameof(pool));

        StopSpawningSequence();
        _currentSpawningCoroutine = _runner.StartCoroutine(SpawningProcess(waveSequencer, pool, gameSpeed));
    }

    public void StopSpawningSequence()
    {
        if (_currentSpawningCoroutine != null)
        {
            _runner.StopCoroutine(_currentSpawningCoroutine);
            _currentSpawningCoroutine = null;
        }
    }

    private IEnumerator SpawningProcess(WaveSequencer waveSequencer, Pool<Enemy> pool, float gameSpeed)
    {
        while (waveSequencer.TryGiveNextWave(out Crowd crowd))
            yield return _runner.StartCoroutine(SpawnWave(crowd, pool, gameSpeed));
    }

    private IEnumerator SpawnWave(Crowd crowd, Pool<Enemy> pool, float gameSpeed)
    {
        if (crowd == null)
            yield break;

        int offsetIndex = 0;
        int countLines = crowd.CountLines;

        float[] offsets = _offsetCalculator.Calculate(countLines, crowd.StepOffset);
        int[] spawnOrder = _spawnStrategy.CalculateSpawnOrder(crowd.SpawnOrder, countLines);
        float lineDelay = crowd.DelayLine / gameSpeed;
        float rowDelay = crowd.DelayRow / gameSpeed;

        WaitForSeconds waitLine = new(lineDelay);
        WaitForSeconds waitRow = new(rowDelay);

        int remainingEnemies = crowd.Quantity;

        while (remainingEnemies > 0)
        {
            bool isLastInLine = false;

            if (pool.TryGet(out Enemy enemy))
            {
                enemy.SetPositionOffset(offsets[spawnOrder[offsetIndex]]);
                enemy.gameObject.SetActive(true);
                enemy.SetId(crowd.Id);
                offsetIndex = (offsetIndex + 1) % offsets.Length;
                remainingEnemies--;
                isLastInLine = (offsetIndex % countLines == 0) && (remainingEnemies > 0);
                Spawned?.Invoke(enemy);
            }

            yield return isLastInLine ? waitLine : waitRow;
        }

        yield return waitLine;
    }
}