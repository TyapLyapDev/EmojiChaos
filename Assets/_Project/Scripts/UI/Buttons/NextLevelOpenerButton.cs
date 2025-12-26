using YG;

public class NextLevelOpenerButton : ButtonClickHandler<NextLevelOpenerButton> 
{
    protected override void OnClick()
    {
        base.OnClick();

        if (YG2.saves.SavesData.IsNoAds == false)
            YG2.InterstitialAdvShow();
    }
}