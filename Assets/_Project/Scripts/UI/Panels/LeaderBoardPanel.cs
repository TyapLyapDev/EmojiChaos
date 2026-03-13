using EmojiChaos.Core.Abstract.UI;
using UnityEngine;

namespace EmojiChaos.UI.Panels
{
    public class LeaderBoardPanel : PanelBase
    {
        [SerializeField] private Leaderboard.Leaderboard _leaderboard;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _leaderboard.Initialize();
        }
    }
}