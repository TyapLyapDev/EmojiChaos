using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Shop
{
    public class TabFactory : MonoBehaviour
    {
        [SerializeField] private TabButton _buttonPrefab;
        [SerializeField] private Transform _buttonContent;
        [SerializeField] private TabPanel _panelPrefab;
        [SerializeField] private Transform _panelContent;

        private readonly List<TabPanel> _tabPanels = new();
        private readonly List<TabButton> _tabButtons = new();

        public IReadOnlyList<TabPanel> TabPanels => _tabPanels;

        public IReadOnlyList<TabButton> TabButtons => _tabButtons;

        public void CreateTabs(ShopCardInfos[] infos, Action<TabButton> onTabClick, Action<TabPanel, Card> onCardClick)
        {
            ClearAll();

            foreach (ShopCardInfos info in infos)
            {
                TabPanel tabPanel = Instantiate(_panelPrefab, _panelContent);
                TabButton tabButton = Instantiate(_buttonPrefab, _buttonContent);

                tabButton.Init(tabPanel, info.TabName);
                tabPanel.Init(info.CardInfos, info.EntityType);

                tabButton.Clicked += onTabClick;
                tabPanel.CardClicked += onCardClick;

                _tabPanels.Add(tabPanel);
                _tabButtons.Add(tabButton);
            }

            if (_tabButtons.Count > 0)
                onTabClick?.Invoke(_tabButtons[0]);
        }

        public void ActivateTab(TabButton selectedTab)
        {
            foreach (TabButton tab in _tabButtons)
                tab.SetSelectStatus(tab == selectedTab);
        }

        public void ApplySavedData(Saver saver)
        {
            foreach (TabPanel tabPanel in _tabPanels)
            {
                IReadOnlyList<ShopCardItemButtonType> buttonTypes = saver.GetShopButtonTypes(tabPanel.EntityType);
                
                for (int i = 0; i < buttonTypes.Count; i++)
                    tabPanel.SelCardType(i, buttonTypes[i]);

                if (saver.IsNoAds)
                    tabPanel.DisableAds();
            }
        }

        private void ClearAll()
        {
            _tabPanels.Clear();
            _tabButtons.Clear();
            Utils.ClearContent(_buttonContent);
            Utils.ClearContent(_panelContent);
        }
    }
}