using System;

public class LevelScore : IDisposable
{
    private readonly EnemiesCounter _enemiesCounter;
    private readonly int _level;

    private int _score;

    public LevelScore(EnemiesCounter enemiesCounter, int level)
    {
        _enemiesCounter = enemiesCounter ?? throw new ArgumentNullException(nameof(enemiesCounter));

        if(level <= 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        _level = level;
        _enemiesCounter.Killed += OnEnemyKilled;
    }

    public int Score => _score;

    public void Dispose()
    {
        if (_enemiesCounter != null)
            _enemiesCounter.Killed -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy _) =>
        _score += _level;
}