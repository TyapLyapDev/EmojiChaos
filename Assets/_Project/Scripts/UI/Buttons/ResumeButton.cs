using EmojiChaos.AudioSpace;
using EmojiChaos.Core.Abstract.UI;

namespace EmojiChaos.UI.Buttons
{
    public class ResumeButton : ButtonClickHandler<ResumeButton>
    {
        protected override void OnClick() =>
            Audio.Sfx.PlayResume();
    }
}