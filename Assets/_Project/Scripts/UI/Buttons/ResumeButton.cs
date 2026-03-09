namespace EmojiChaos.UI.Buttons
{
    using Audio;
    using Core.Abstract.UI;

    public class ResumeButton : ButtonClickHandler<ResumeButton>
    {
        protected override void OnClick() =>
            Audio.Sfx.PlayResume();
    }
}