using TMPro;
using UnityEngine;

public class Leaderboard : InitializingBehaviour
{
    [SerializeField] private Transform _context;
    [SerializeField] private TextMeshProUGUI _noDataText;
    [SerializeField] private LeaderboardLine[] _lines;
    [SerializeField] private GameObject _split;
    [SerializeField] private LeaderboardLine _lineOutTop;

    private void OnEnable()
    {
        if (LeaderboardYGMediator.Instance != null)
        {
            LeaderboardYGMediator.Instance.DataUpdated += OnUpdatedData;
            OnUpdatedData(LeaderboardYGMediator.Instance.Data);
        }
    }

    private void OnDisable()
    {
        if (LeaderboardYGMediator.Instance != null)
            LeaderboardYGMediator.Instance.DataUpdated -= OnUpdatedData;
    }

    protected override void OnInitialize() { }

    private void OnUpdatedData(YG.Utils.LB.LBData lbData)
    {
        bool hasData = YandexGameConnector.HasDataLeaderboard(lbData);
        _noDataText.SetActive(hasData == false);

        if (hasData == false)
        {
            DeactivateAllLines();

            return;
        }

        if(lbData.players == null)
        {
            DeactivateAllLines();
            Debug.Log("lbData.players is null");

            return;
        }

        int linesToActivate = Mathf.Min(lbData.players.Length, _lines.Length);
        bool isPlayerInTop = false;

        for (int i = 0; i < linesToActivate; i++)
        {
            YG.Utils.LB.LBPlayerData player = lbData.players[i];

            bool isCurrentPlayer = player.uniqueID == YandexGameConnector.PlayerId;

            LeaderboardLineParam config = new(
                uniqueId: player.uniqueID,
                rank: player.rank,
                nickname: player.name,
                score: player.score,
                isCurrentPlayer: isCurrentPlayer);

            _lines[i].Activate(config);

            if (isCurrentPlayer)
                isPlayerInTop = true;
        }

        for (int i = linesToActivate; i < _lines.Length; i++)
            _lines[i].Deactivate();

        if (isPlayerInTop || YandexGameConnector.IsPlayerAuth == false || YandexGameConnector.HasScore == false)
        {
            _lineOutTop.Deactivate();
            _split.SetActive(false);
        }
        else
        {
            YG.Utils.LB.LBCurrentPlayerData player = lbData.currentPlayer;

            LeaderboardLineParam config = new(
                uniqueId: YandexGameConnector.PlayerId,
                rank: player.rank,
                nickname: YandexGameConnector.PlayerName,
                score: player.score,
                isCurrentPlayer: true);

            _split.SetActive(true);
            _lineOutTop.Activate(config);
        }
    }

    private void DeactivateAllLines()
    {
        foreach (LeaderboardLine line in _lines)
            line.Deactivate();

        _lineOutTop.Deactivate();
        _split.SetActive(false);
    }
}