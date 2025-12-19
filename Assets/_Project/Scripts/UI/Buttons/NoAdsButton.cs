using YG;

public class NoAdsButton : ButtonClickHandler<NoAdsButton> 
{
    protected override void OnClick()
    {
        base.OnClick();
        YG2.BuyPayments(Constants.PurchasingNoAds);
    }
}