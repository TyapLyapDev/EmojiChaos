using System;
using YG;
using YG.Utils.LB;
using YG.Utils.Pay;

namespace YG
{
    public partial class SavesYG
    {
        public SavesData SavesData = new();
    }
}

public static class YandexGameConnector
{
    static YandexGameConnector()
    {
        YG2.onGetSDKData += OnGetSDKData;
        YG2.onRewardAdv += OnRewardAdv;
        YG2.onPurchaseSuccess += OnPurchaseSuccess;
        YG2.onSwitchLang += OnSwitchLang;
        YG2.onGetLeaderboard += OnGetLeaderboard;
    }

    public static event Action GettedSDKData;
    public static event Action<string> AdvRewarded;
    public static event Action<string> SuccessPurchased;
    public static event Action<string> LangSwitched;
    public static event Action<LBData> GettedLeaderboard;

    public static string Lang => YG2.lang;

    public static bool IsAuthorized => YG2.player.auth;

    public static string PlayerId => YG2.player.id;

    public static string PlayerName => YG2.player.name;

    public static bool IsPlayerAuth => YG2.player.auth;

    public static string PlayerPhoto => YG2.player.photo;

    public static SavesData SavesData => YG2.saves.SavesData;

    public static bool HasScore => YG2.saves.SavesData.Score > 0;

    public static void OpenAuthDialog() =>
        YG2.OpenAuthDialog();

    public static void SwitchLanguage(string text) =>
        YG2.SwitchLanguage(text);

    public static void ConsumePurchases(InApp noAds, InApp additionalSlot)
    {
        foreach (Purchase purchase in YG2.purchases)
        {
            if (purchase.consumed == false)
            {
                if (purchase.id == noAds.Id)
                {
                    YG2.saves.SavesData.IsNoAds = true;
                    YG2.ConsumePurchaseByID(purchase.id, true);
                }
                else if (purchase.id == additionalSlot.Id)
                {
                    YG2.saves.SavesData.IsPurchsingRack = true;
                    YG2.ConsumePurchaseByID(purchase.id, true);
                }
            }
        }
    }

    public static void RewardedAdvShow(string id) =>
        YG2.RewardedAdvShow(id);

    public static void ReviewShow() =>
        YG2.ReviewShow();

    public static void GameLabelShowDialog() =>
        YG2.GameLabelShowDialog();

    public static void InterstitialAdvShow() =>
            YG2.InterstitialAdvShow();

    public static bool HasDataLeaderboard(LBData lbData) =>
        lbData.entries != InfoYG.NO_DATA;

    public static void GetLeaderboard(string nameLB, int quatityTop, int quantityAround, string photSizeLB) =>
        YG2.GetLeaderboard(nameLB, quatityTop, quantityAround, photSizeLB);

    public static void SetLeaderboard(string nameLB) =>
        YG2.SetLeaderboard(nameLB, YG2.saves.SavesData.Score);

    public static void StickyAdActivity(bool isOn) =>
        YG2.StickyAdActivity(isOn);

    public static void BuyPayments(string id) =>
        YG2.BuyPayments(id);

    public static void SaveProgress() => 
        YG2.SaveProgress();

    private static void OnGetSDKData() =>
        GettedSDKData?.Invoke();

    private static void OnRewardAdv(string id) =>
        AdvRewarded?.Invoke(id);

    private static void OnPurchaseSuccess(string id) =>
        SuccessPurchased?.Invoke(id);

    private static void OnSwitchLang(string lang) =>
        LangSwitched?.Invoke(lang);

    private static void OnGetLeaderboard(LBData data) =>
        GettedLeaderboard?.Invoke(data);
}