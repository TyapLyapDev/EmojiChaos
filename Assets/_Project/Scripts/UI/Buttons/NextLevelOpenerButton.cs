using YG;

public class NextLevelOpenerButton : ButtonClickHandler<NextLevelOpenerButton> 
{
    protected override void OnClick()
    {
        base.OnClick();

        if (YG2.saves.SavesData.IsNoAds == false)
            YG2.InterstitialAdvShow();

        if (YG2.saves.SavesData.LevelProgress >= 2)
            YG2.ReviewShow();

        if (YG2.saves.SavesData.LevelProgress >= 4)
            YG2.GameLabelShowDialog();
    }
}