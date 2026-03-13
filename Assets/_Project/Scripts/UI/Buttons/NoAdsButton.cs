using EmojiChaos.Core.Abstract.UI;
using EmojiChaos.ScriptableObect;
using UnityEngine;

namespace EmojiChaos.UI.Buttons
{
    public class NoAdsButton : ButtonClickHandler<NoAdsButton>
    {
        [SerializeField] private InApp _inApp;

        public InApp InApp => _inApp;
    }
}