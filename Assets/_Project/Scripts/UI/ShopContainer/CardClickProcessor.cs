using System;
using EmojiChaos.UI.ShopContainer.Card;
using EmojiChaos.UI.ShopContainer.Card.Enum;

namespace EmojiChaos.UI.ShopContainer
{
    public class CardClickProcessor : IDisposable
    {
        private readonly CardSelector _selector;

        private Action<string> _needShowAds;

        public CardClickProcessor(CardSelector selector, Action<string> needShowAds)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _needShowAds = needShowAds;
        }

        public void Dispose() =>
            _needShowAds = null;

        public void ProcessCardClick(TabPanel tabPanel, ShopCard card)
        {
            switch (card.Type)
            {
                case ShopCardItemButtonType.Selected:
                    return;

                case ShopCardItemButtonType.Opened:
                    _selector.SelectCard(tabPanel, card);
                    break;

                case ShopCardItemButtonType.NeedViewAds:
                    _needShowAds?.Invoke(card.RevardedAdvId);
                    break;
            }
        }
    }
}