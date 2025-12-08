using DG.Tweening;
using System;

public class StarBonusCalculator
{
    private const float DelayBeforeBonus = 0.5f;
    private const float DelayBetweenSteps = 0.75f;

    private readonly int _baseScore;
    private readonly int _starsCount;

    private Sequence _bonusSequence;
    private int _currentMultiplier;

    public StarBonusCalculator(int baseScore, int starsCount)
    {
        _baseScore = baseScore;
        _starsCount = starsCount;
        _currentMultiplier = 1;
    }

    public event Action<int> OnBonusUpdated;
    public event Action OnBonusComplete;

    public void StartBonus()
    {
        StopBonus();

        _currentMultiplier = 1;
        _bonusSequence = DOTween.Sequence();
        _bonusSequence.AppendInterval(DelayBeforeBonus);

        for (int i = 0; i < _starsCount; i++)
        {
            _bonusSequence.AppendCallback(() =>
            {
                _currentMultiplier *= 2;
                int newScore = _baseScore * _currentMultiplier;
                OnBonusUpdated?.Invoke(newScore);
            });

            if (i < _starsCount - 1)
                _bonusSequence.AppendInterval(DelayBetweenSteps);
        }

        _bonusSequence.OnComplete(() => OnBonusComplete?.Invoke()).SetUpdate(true);
    }

    public void StopBonus()
    {
        _bonusSequence?.Kill();
        _bonusSequence = null;
    }
}