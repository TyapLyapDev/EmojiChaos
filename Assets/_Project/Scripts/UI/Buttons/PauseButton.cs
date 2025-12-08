public class PauseButton : ButtonClickHandler<PauseButton> 
{
    protected override void OnClick() =>
        Audio.Sfx.PlayPause();
}