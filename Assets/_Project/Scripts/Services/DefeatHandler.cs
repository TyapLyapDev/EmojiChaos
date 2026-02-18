using System;

public class DefeatHandler : IDisposable
{
    private readonly EnemiesMovementDirector _enemiesSpeedDirector;
    private readonly StarsCounter _starsCounter;

    public DefeatHandler(EnemiesMovementDirector enemiesSpeedDirector, StarsCounter starsCounter)
    {
        _enemiesSpeedDirector = enemiesSpeedDirector ?? throw new ArgumentNullException(nameof(enemiesSpeedDirector));
        _starsCounter = starsCounter ?? throw new ArgumentNullException(nameof(starsCounter));

        Subscribe();
    }

    public event Action Defeat;

    public void Dispose() =>
        Unsubscribe();

    private void Subscribe()
    {
        _starsCounter.StarsLeft += OnStarsLeft;
        _enemiesSpeedDirector.FirstEnemyFinished += OnEnemyFirstFinished;
    }

    private void Unsubscribe()
    {
        if (_starsCounter != null)
            _starsCounter.StarsLeft -= OnStarsLeft;

        if (_enemiesSpeedDirector != null)
            _enemiesSpeedDirector.FirstEnemyFinished -= OnEnemyFirstFinished;
    }

    private void OnStarsLeft() =>
        Defeat?.Invoke();

    private void OnEnemyFirstFinished() =>
        Defeat?.Invoke();
}