using EmojiChaos.AudioSpace;
using EmojiChaos.Core.Abstract.UI;

namespace EmojiChaos.UI.Buttons
{
    public class ProgressLevelOpenerButton : ButtonClickHandler<ProgressLevelOpenerButton>
    {
        protected override void OnClick()
        {
            Audio.Sfx.PlayLevelSelected();
        }
    }
}