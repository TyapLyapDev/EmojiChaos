using EmojiChaos.Core.Abstract.UI;
using EmojiChaos.ScriptableObect;
using UnityEngine;

namespace EmojiChaos.UI.Buttons
{
    public class SlotPurchasingButton : ButtonClickHandler<SlotPurchasingButton>
    {
        [SerializeField] private InApp _inApp;

        public InApp InApp => _inApp;
    }
}