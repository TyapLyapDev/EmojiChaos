namespace EmojiChaos.UI.Buttons
{
    using Audio;
    using Core.Abstract.UI;

    public class ProgressLevelOpenerButton : ButtonClickHandler<ProgressLevelOpenerButton>
    {
        protected override void OnClick()
        {
            Audio.Sfx.PlayLevelSelected();
        }
    }
}