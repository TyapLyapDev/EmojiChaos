using EmojiChaos.Core.Abstract.UI;
using EmojiChaos.UI.TutorialSpace;
using UnityEngine;

namespace EmojiChaos.UI.Buttons
{
    public class TutorialSkipButton : ButtonClickHandler<TutorialSkipButton>
    {
        [SerializeField] private Tutorial _tutorial;

        private void Awake() =>
            Initialize();

        protected override void OnClick()
        {
            base.OnClick();
            _tutorial.Complete();
        }
    }
}