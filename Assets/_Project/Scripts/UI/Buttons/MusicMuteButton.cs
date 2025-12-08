public class MusicMuteButton : MuteButtonBase<MusicMuteButton>
{
    protected override void OnSliderChanged(float value)
    {
        base.OnSliderChanged(value);

        if (IsZero)
            Audio.Music.SetMute();
        else
            Audio.Music.ResetMute();
    }
}