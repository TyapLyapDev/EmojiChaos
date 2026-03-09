using UnityEngine;

namespace EmojiChaos.UI.Buttons
{
    using Core.Abstract.UI;
    using ScriptableObect;

    public class SlotPurchasingButton : ButtonClickHandler<SlotPurchasingButton>
    {
        [SerializeField] private InApp _inApp;

        public InApp InApp => _inApp;
    }
}