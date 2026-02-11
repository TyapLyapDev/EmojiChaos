namespace UI.Shop
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

        public void SelectCard(TabPanel tabPanel, Card card)
        {
            tabPanel.SelectCard(card);
            _saver.SetShopCards(tabPanel.EntityType, tabPanel.ButtonTypes);
            _saver.Save();
        }

        public bool TryUnlockWithReward(string rewardId)
        {
            foreach (var tabPanel in _tabFactory.TabPanels)
            {
                if (tabPanel.TryGetCard(rewardId, out Card card))
                {
                    SelectCard(tabPanel, card);

                    return true;
                }
            }

            return false;
        }
    }
}