public class ProgressLevelOpenerButton : ButtonClickHandler<ProgressLevelOpenerButton>
{
    protected override void OnClick()
    {
        Audio.Sfx.PlayLevelSelected();
    }
}