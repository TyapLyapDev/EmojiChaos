using YG;

public class AdvertisingButton : ButtonClickHandler<AdvertisingButton> 
{
    protected override void OnClick()
    {
        base.OnClick();
        YG2.RewardedAdvShow(Constants.RewardRack + GetInstanceID());
    }
}