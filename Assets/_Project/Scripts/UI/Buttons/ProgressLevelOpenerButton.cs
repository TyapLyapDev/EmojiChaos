using YG;

public class ProgressLevelOpenerButton : ButtonClickHandler<ProgressLevelOpenerButton>
{
    protected override void OnClick()
    {
        Audio.Sfx.PlayLevelSelected();

        if (YG2.saves.SavesData.IsNoAds == false)
            YG2.InterstitialAdvShow();
    }
}