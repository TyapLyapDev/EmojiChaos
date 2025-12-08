using TMPro;
using UnityEngine;

public class LevelScoreDisplay : InitializingBehaviour
{
    private static readonly int s_levelScoreBonusAddingId = Animator.StringToHash("LevelScoreBonusAdding");

    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private Animator _animator;

    protected override void OnInitialize() { }

    public void SetBonus(int score)
    {
        SetScore(score);
        _animator.Play(s_levelScoreBonusAddingId, -1, 0f);
        Audio.Sfx.PlayStarCollected();
    }

    public void SetScore(int score)
    {
        _score.text = score.ToString();
        Audio.Sfx.PlayScoreIncreased();
    }
}