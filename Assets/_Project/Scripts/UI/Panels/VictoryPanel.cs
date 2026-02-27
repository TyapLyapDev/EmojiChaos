using UnityEngine;

public class VictoryPanel : PanelBase
{
    [SerializeField] private LevelScoreDisplay _scoreDisplay;
    [SerializeField] private LanguageSwitchHandlerWithParam _level;
    [SerializeField] private GameObject[] _stars;

    private ScoreAccrualAnimator _scoreAccrualAnimator;
    private StarBonusCalculator _starBonusCalculator;

    private void OnDestroy()
    {
        if (_scoreAccrualAnimator != null)
        {
            _scoreAccrualAnimator.ScoreUpdated -= UpdateScoreDisplay;
            _scoreAccrualAnimator.Completed -= OnCompletedAccrual;
            _scoreAccrualAnimator.StopAccrual();
        }

        if (_starBonusCalculator != null)
        {
            _starBonusCalculator.OnBonusUpdated -= OnStarBonusUpdated;
            _starBonusCalculator.StopBonus();
        }
    }

    public void Activate(int targetScore, int starsCount, int level)
    {
        _level.SetParam((level + 1).ToString());
        _scoreAccrualAnimator = new(targetScore);

        foreach (GameObject star in _stars)
            star.SetActive(false);

        _scoreAccrualAnimator.ScoreUpdated += UpdateScoreDisplay;
        _scoreAccrualAnimator.Completed += OnCompletedAccrual;

        if (starsCount > 0)
        {
            _starBonusCalculator = new(targetScore, starsCount);
            _starBonusCalculator.OnBonusUpdated += OnStarBonusUpdated;
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _scoreDisplay.Initialize();
    }

    protected override void OnShow()
    {
        base.OnShow();
        _scoreAccrualAnimator.StartAccrual();
        Audio.Sfx.PlayVictory();
    }

    private void UpdateScoreDisplay(int score) =>
        _scoreDisplay.SetScore(score);

    private void OnStarBonusUpdated(int score)
    {
        foreach (GameObject star in _stars)
        {
            if (star.activeSelf)
                continue;

            _scoreDisplay.SetBonus(score);
            star.SetActive(true);

            return;
        }
    }

    private void OnCompletedAccrual() =>
        _starBonusCalculator?.StartBonus();
}