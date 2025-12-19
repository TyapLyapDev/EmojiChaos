using YG;

public class RestartButton : ButtonClickHandler<RestartButton> 
{
    protected override void OnClick()
    {
        base.OnClick();

        if (YG2.saves.SavesData.IsNoAds == false)
            YG2.InterstitialAdvShow();
    }
}