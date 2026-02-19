using UI.Shop;
using UnityEngine;

public class ShopPanel : PanelBase
{
    [SerializeField] private TabFactory _tabFactory;
    [SerializeField] private ShopCardInfos[] _infos;
    
    private Saver _saver;
    private CardSelector _cardSelector;
    private CardClickProcessor _clickProcessor;    
    private AdsRewardListener _adsListener;

    private void OnDestroy()
    {
        _clickProcessor?.Dispose();
        _adsListener?.Dispose();
    }

    private void OnEnable() =>
        _adsListener.Subscribe();

    private void OnDisable() =>
        _adsListener.Unsubscribe();

    public void Initialize(Saver saver)
    {
        _saver = saver;
        Initialize();
        _cardSelector = new (_saver, _tabFactory);
        _adsListener = new(_cardSelector);
        _clickProcessor = new (_cardSelector, _adsListener.ShowRewardedAd);

        _tabFactory.CreateTabs(_infos, OnTabClick, OnCardClick);
        _tabFactory.ApplySavedData(_saver);
        _saver.AddShopCardInfos(_infos);
    }

    private void OnTabClick(UI.Shop.TabButton tabButton) =>
        _tabFactory.ActivateTab(tabButton);

    private void OnCardClick(TabPanel tabPanel, Card card) =>
        _clickProcessor.ProcessCardClick(tabPanel, card);
}