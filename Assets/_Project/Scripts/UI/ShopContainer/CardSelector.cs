using EmojiChaos.Services.Save;
using EmojiChaos.UI.ShopContainer.Card;

namespace EmojiChaos.UI.ShopContainer
{
    public class CardSelector
    {
        private readonly Saver _saver;
        private readonly TabFactory _tabFactory;

        public CardSelector(Saver saver, TabFactory tabFactory)
        {
            _saver = saver;
            _tabFactory = tabFactory;
        }

        public void SelectCard(TabPanel tabPanel, ShopCard card)
        {
            tabPanel.SelectCard(card);
            _saver.SetShopCards(tabPanel.EntityType, tabPanel.ButtonTypes);
            _saver.Save();
        }

        public void DisableAds()
        {
            foreach (TabPanel tabPanel in _tabFactory.TabPanels)
                tabPanel.DisableAds();
        }

        public bool TryUnlockWithReward(string rewardId, bool needSelect)
        {
            foreach (TabPanel tabPanel in _tabFactory.TabPanels)
            {
                if (tabPanel.TryGetCard(rewardId, out ShopCard card))
                {
                    if (needSelect)
                        SelectCard(tabPanel, card);

                    return true;
                }
            }

            return false;
        }
    }
}