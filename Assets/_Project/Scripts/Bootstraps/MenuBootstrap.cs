using UnityEngine;
using YG;
using YG.Utils.Pay;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private MenuUIHandler _menuUIHandler;

    private void Start()
    {
        ConsumePurchases();

        _menuUIHandler.Initialize(new MenuUiConfig(
            new Saver(Utils.CalculateLevelCountInProject())));

        Audio.Music.PlayMenu();

        if (YG2.saves.SavesData.ShowedAuthDialog == false)
        {
            YG2.OpenAuthDialog();
            YG2.saves.SavesData.ShowedAuthDialog = true;
        }
    }

    private void ConsumePurchases()
    {
        foreach (Purchase purchase in YG2.purchases)
        {
            if (purchase.consumed == false)
            {
                if (purchase.id == Constants.PurchasingNoAds)
                {
                    YG2.saves.SavesData.IsNoAds = true;
                    YG2.ConsumePurchaseByID(purchase.id, true);
                }
                else if (purchase.id == Constants.PurchasingRack)
                {
                    YG2.saves.SavesData.IsPurchsingRack = true;
                    YG2.ConsumePurchaseByID(purchase.id, true);
                }
            }
        }
    }
}