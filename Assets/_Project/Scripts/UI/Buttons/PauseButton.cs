using EmojiChaos.AudioSpace;
using EmojiChaos.Core.Abstract.UI;

namespace EmojiChaos.UI.Buttons
{
    public class PauseButton : ButtonClickHandler<PauseButton>
    {
        protected override void OnClick() =>
            Audio.Sfx.PlayPause();
    }
}