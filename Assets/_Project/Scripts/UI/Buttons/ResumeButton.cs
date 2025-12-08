public class ResumeButton : ButtonClickHandler<ResumeButton> 
{
    protected override void OnClick() =>
        Audio.Sfx.PlayResume();
}