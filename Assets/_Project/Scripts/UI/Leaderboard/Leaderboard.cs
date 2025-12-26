using TMPro;
using UnityEngine;
using YG;
using YG.Utils.LB;

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
            LeaderboardYGMediator.Instance.DataUpdated += OnUpdatedData;
    }

    private void OnDisable()
    {
        if (LeaderboardYGMediator.Instance != null)
        {
            LeaderboardYGMediator.Instance.DataUpdated -= OnUpdatedData;
            Debug.Log("LiaderboardDestroyed");
        }
    }

    public void Activate()
    {
        if (LeaderboardYGMediator.Instance != null)
            OnUpdatedData(LeaderboardYGMediator.Instance.Data);
    }

    protected override void OnInitialize() { }

    private void OnUpdatedData(LBData lbData)
    {
        bool isNoData = lbData.entries == InfoYG.NO_DATA;
        _noDataText.SetActive(isNoData);

        if (isNoData)
        {
            DeactivateAllLines();

            return;
        }

        int linesToActivate = Mathf.Min(lbData.players.Length, _lines.Length);
        bool isPlayerInTop = false;

        for (int i = 0; i < linesToActivate; i++)
        {
            LBPlayerData player = lbData.players[i];

            bool isCurrentPlayer = player.uniqueID == YG2.player.id;

            LeaderboardLineConfig config = new(
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

        if (isPlayerInTop || YG2.player.auth == false)
        {
            _lineOutTop.Deactivate();
            _split.SetActive(false);
        }
        else
        {
            LBCurrentPlayerData player = lbData.currentPlayer;

            LeaderboardLineConfig config = new(
                uniqueId: YG2.player.id,
                rank: player.rank,
                nickname: YG2.player.name,
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
    }
}