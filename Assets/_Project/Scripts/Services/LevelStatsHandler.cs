using System;

public class LevelStatsHandler : IDisposable
{
    private readonly VictoryHandler _victoryHandler;
    private readonly DefeatHandler _defeatHandler;
    private readonly StarsCounter _starsCounter;
    private readonly LevelScore _levelScore;

    public LevelStatsHandler(EnemiesSpeedDirector enemySpeedDirector, 
        StarsCounter starCounter, 
        EnemiesCounter enemiesCounter,
        int level)
    {
        _starsCounter = starCounter ?? throw new ArgumentNullException(nameof(starCounter));

        _victoryHandler = new(enemiesCounter);
        _defeatHandler = new(enemySpeedDirector, _starsCounter);
        _levelScore = new(enemiesCounter, level);

        Subscribe();
    }

    public event Action Victory;
    public event Action Defeat;

    public int StarCount => _starsCounter.StarCount;

    public int Score => _levelScore.Score;

    public int FinalScore => _levelScore.Score * _starsCounter.StarCount * 2;

    public void Dispose()
    {
        Unsubscribe();
        _victoryHandler?.Dispose();
        _defeatHandler?.Dispose();
        _starsCounter?.Dispose();
        _levelScore?.Dispose();
    }

    private void Subscribe()
    {
        _victoryHandler.Victory += OnVictory;
        _defeatHandler.Defeat += OnDefeat;
    }

    private void Unsubscribe()
    {
        if (_victoryHandler != null)
            _victoryHandler.Victory -= OnVictory;

        if (_defeatHandler != null)
            _defeatHandler.Defeat -= OnDefeat;
    }

    private void OnVictory()
    {
        ProcessVictory();
        Unsubscribe();
    }

    private void OnDefeat()
    {
        ProcessDefeat();
        Unsubscribe();
    }

    private void ProcessVictory()
    {
        SetRemainingStarsToEnjoy();
        Victory?.Invoke();
    }

    private void ProcessDefeat() =>
        Defeat?.Invoke();

    private void SetRemainingStarsToEnjoy()
    {
        foreach (Star star in _starsCounter.Stars)
            if (star != null)
                star.SetEnjoy();
    }
}