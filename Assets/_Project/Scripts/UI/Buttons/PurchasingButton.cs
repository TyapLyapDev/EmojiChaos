using YG;

public class PurchasingButton : ButtonClickHandler<PurchasingButton>
{
    protected override void OnClick()
    {
        base.OnClick();
        YG2.BuyPayments(Constants.PurchasingRack);
    }
}