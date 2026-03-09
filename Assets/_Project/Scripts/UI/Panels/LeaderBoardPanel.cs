using UnityEngine;

namespace EmojiChaos.UI.Panels
{
    using Core.Abstract.UI;
    using Leaderboard;

    public class LeaderBoardPanel : PanelBase
    {
        [SerializeField] private Leaderboard _leaderboard;

        protected override void OnInitialize ( )
        {
            base.OnInitialize ( );
            _leaderboard.Initialize ( );
        }
    }
}