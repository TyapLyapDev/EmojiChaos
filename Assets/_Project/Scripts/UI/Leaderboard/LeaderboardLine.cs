using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardLine : MonoBehaviour
{
    [SerializeField] private Image[] _bakgrounds;
    [SerializeField] private TextMeshProUGUI _rank;
    [SerializeField] private AvatarImage _avatatImage;
    [SerializeField] private TextMeshProUGUI _nickname;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private Image[] _backgrounds;
    [SerializeField] private GameObject _currentPlayerFrame;
    [SerializeField] private Color _initialBackgroundColor;
    [SerializeField] private Color _currentPlayerBackgoundColor;


    private LeaderboardLineParam _config;

    public void Activate(LeaderboardLineParam config)
    {
        _config = config;

        _rank.text = _config.Rank;
        _avatatImage.Activate(_config.UniqueId);
        _nickname.text = _config.Nickname;
        _score.text = _config.Score;
        ProcessCurrentPlayerStatus();

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _rank.text = string.Empty;
        _avatatImage.Deactivate();
        _nickname.text = string.Empty;
        _score.text = string.Empty;

        gameObject.SetActive(false);
    }

    private void ProcessCurrentPlayerStatus()
    {
        bool isCurrentPlayer = _config.IsCurrentPlayer;

        _currentPlayerFrame.SetActive(isCurrentPlayer);
        SetBackgroundColor(isCurrentPlayer ? _currentPlayerBackgoundColor : _initialBackgroundColor);
    }

    private void SetBackgroundColor(Color color)
    {
        foreach (Image image in _backgrounds)
            image.color = color;
    }
}