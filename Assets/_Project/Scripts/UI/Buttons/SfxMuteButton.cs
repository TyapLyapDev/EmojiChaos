using EmojiChaos.AudioSpace;
using EmojiChaos.Core.Abstract;

namespace EmojiChaos.UI.Buttons
{
    public class SfxMuteButton : MuteButtonBase<SfxMuteButton>
    {
        protected override void OnSliderChanged(float value)
        {
            base.OnSliderChanged(value);

            if (IsZero)
                Audio.Sfx.SetMute();
            else
                Audio.Sfx.ResetMute();
        }
    }
}