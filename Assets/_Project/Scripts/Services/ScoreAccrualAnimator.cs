using DG.Tweening;
using System;
using UnityEngine;

public class ScoreAccrualAnimator
{
    private const float DelayBeforeAccrual = 1.5f;
    private const float MaxDuration = 2.5f;
    private const float SpeedAccrualRate = 100f;

    private readonly int _targetScore;

    private Sequence _sequence;
    private int _currentScore;

    public ScoreAccrualAnimator(int targetScore)
    {
        _targetScore = targetScore;
    }

    public event Action<int> ScoreUpdated;
    public event Action Completed;

    public void StartAccrual()
    {
        StopAccrual();

        _currentScore = 0;
        float duration = CalculateDuration();

        _sequence = DOTween.Sequence();
        _sequence.AppendInterval(DelayBeforeAccrual);

        _sequence.Append(DOTween.To(() => _currentScore, x =>
        {
            _currentScore = x;
            ScoreUpdated?.Invoke(_currentScore);
        }, _targetScore, duration)
        .SetEase(Ease.Linear));

        _sequence.SetUpdate(true);
        _sequence.OnComplete(OnComplete);

        _sequence.Play();
    }

    public void StopAccrual()
    {
        _sequence?.Kill();
        _sequence = null;
    }

    private float CalculateDuration()
    {
        float timeBySpeed = _targetScore / SpeedAccrualRate;

        return Mathf.Min(timeBySpeed, MaxDuration);
    }

    private void OnComplete()
    {
        StopAccrual();
        _currentScore = _targetScore;
        ScoreUpdated?.Invoke(_currentScore);
        Completed?.Invoke();
    }
}