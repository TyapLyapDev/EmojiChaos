using System;
using System.Collections;
using UnityEngine;

public class SpawnCoordinator
{
    private readonly EnemyFormationCalculator _offsetCalculator;
    private readonly SpawnStrategyRegistry _spawnStrategy;
    private readonly MonoBehaviour _coroutineRunner;

    private Coroutine _currentSpawningCoroutine;

    public event Action<Enemy> EnemySpawned;
    public event Action<Crowd> WaveSpawningStarted;
    public event Action<Crowd> WaveSpawningCompleted;

    public SpawnCoordinator(MonoBehaviour coroutineRunner)
    {
        if(coroutineRunner == null)
            throw new ArgumentNullException(nameof(coroutineRunner));

        _coroutineRunner = coroutineRunner;

        _offsetCalculator = new();
        _spawnStrategy = new();
    }

    public bool IsSpawning => _currentSpawningCoroutine != null;

    public void StartSpawningSequence(WaveSequencer waveSequencer, EnemyPoolRegistry poolRegistry, float gameSpeed)
    {
        StopSpawningSequence();
        _currentSpawningCoroutine = _coroutineRunner.StartCoroutine(SpawningProcess(waveSequencer, poolRegistry, gameSpeed));
    }

    public void StopSpawningSequence()
    {
        if (_currentSpawningCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_currentSpawningCoroutine);
            _currentSpawningCoroutine = null;
        }
    }

    private IEnumerator SpawningProcess(WaveSequencer waveSequencer, EnemyPoolRegistry poolManager, float gameSpeed)
    {
        while (waveSequencer.TryStartNextWave())
        {
            Crowd currentWave = waveSequencer.CurrentWave;
            WaveSpawningStarted?.Invoke(currentWave);

            yield return _coroutineRunner.StartCoroutine(SpawnWave(currentWave, poolManager, gameSpeed));

            WaveSpawningCompleted?.Invoke(currentWave);
        }
    }

    private IEnumerator SpawnWave(Crowd crowd, EnemyPoolRegistry poolRegisty, float gameSpeed)
    {
        if (crowd == null) 
            yield break;

        int currentIndex = 0;
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
            if (poolRegisty.TryGetEnemy(crowd.Id, out Enemy enemy))
            {
                enemy.SetPositionOffset(offsets[spawnOrder[currentIndex]]);
                enemy.gameObject.SetActive(true);
                currentIndex = (currentIndex + 1) % offsets.Length;
                remainingEnemies--;
                bool isLastInLine = (currentIndex % countLines == 0) && (remainingEnemies > 0);
                EnemySpawned?.Invoke(enemy);

                yield return isLastInLine ? waitLine : waitRow;
            }
        }

        yield return waitLine;
    }
}