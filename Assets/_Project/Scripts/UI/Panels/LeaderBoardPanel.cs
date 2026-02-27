using UnityEngine;

public class LeaderBoardPanel : PanelBase 
{
    [SerializeField] private Leaderboard _leaderboard;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _leaderboard.Initialize();
    }
}