using System;

public class VictoryHandler : IDisposable
{
    private readonly EnemiesCounter _enemyCounter;

    public VictoryHandler(EnemiesCounter enemyCounter)
    {
        _enemyCounter = enemyCounter ?? throw new ArgumentNullException(nameof(enemyCounter));

        _enemyCounter.AllEnemiesDefeated += OnAllEnemiesDefeated;
    }

    public event Action Victory;

    public void Dispose()
    {
        if (_enemyCounter != null)
            _enemyCounter.AllEnemiesDefeated -= OnAllEnemiesDefeated;
    }

    private void OnAllEnemiesDefeated() =>
        Victory?.Invoke();
}