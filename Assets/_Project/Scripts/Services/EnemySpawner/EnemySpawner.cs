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
    private bool _isPause;

    public EnemySpawner(Pool<Enemy> pool, TypeColorRandomizer colorRandomizer, float speed)
    {
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        _colorRandomizer = colorRandomizer ?? throw new ArgumentNullException(nameof(colorRandomizer));

        if(speed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be greater than zero");

        _gameSpeed = speed;
    }

    public event Action<Enemy> Spawned;

    public void Dispose()
    {
        Spawned = null;
    }

    public void Pause() =>
        _isPause = true;

    public void Resume() =>
        _isPause = false;

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
            while(_isPause)
                yield return null;

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