using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner
{
    private readonly Pool<Enemy> _pool;
    private readonly TypeColorRandomizer _colorRandomizer;
    private readonly EnemyFormationCalculator _formationCalculator = new();
    private readonly SpawnStrategyRegistry _spawnStrategy = new();
    private readonly float _gameSpeed;

    public EnemySpawner(Pool<Enemy> pool, TypeColorRandomizer colorRandomizer, float gameSpeed)
    {
        _pool = pool;
        _colorRandomizer = colorRandomizer ?? throw new ArgumentNullException(nameof(colorRandomizer));
        _gameSpeed = gameSpeed;
    }

    public event Action<Enemy> Spawned;

    public void Dispose()
    {

    }

    public IEnumerator SpawnCrowd(Crowd crowd)
    {
        if (crowd == null)
            yield break;

        int offsetIndex = 0;
        int countLines = crowd.CountLines;

        float[] offsets = _formationCalculator.Calculate(countLines, crowd.StepOffset);
        int[] spawnOrder = _spawnStrategy.CalculateSpawnOrder(crowd.SpawnOrder, countLines);
        float lineDelay = crowd.DelayLine / _gameSpeed;
        float rowDelay = crowd.DelayRow / _gameSpeed;

        WaitForSeconds waitLine = new(lineDelay);
        WaitForSeconds waitRow = new(rowDelay);

        int remainingEnemies = crowd.Quantity;

        while (remainingEnemies > 0)
        {
            bool isLastInLine = false;

            if (_colorRandomizer.TryGetColor(crowd.Id, out Color color))
            {
                if (_pool.TryGive(out Enemy enemy))
                {
                    enemy.Activate(crowd.Id,
                        offsets[spawnOrder[offsetIndex]],
                        color);

                    offsetIndex = (offsetIndex + 1) % offsets.Length;
                    remainingEnemies--;
                    isLastInLine = (offsetIndex % countLines == 0) && (remainingEnemies > 0);
                    Spawned?.Invoke(enemy);
                }
            }

            yield return isLastInLine ? waitLine : waitRow;
        }

        yield return waitLine;
    }
}