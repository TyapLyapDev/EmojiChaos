namespace EmojiChaos.UI.Buttons
{
    using Audio;
    using Core.Abstract.UI;

    public class PauseButton : ButtonClickHandler<PauseButton>
    {
        protected override void OnClick() =>
            Audio.Sfx.PlayPause();
    }
}