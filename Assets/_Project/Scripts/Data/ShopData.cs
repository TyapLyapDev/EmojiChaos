using System;
using System.Collections.Generic;

namespace EmojiChaos.Data
{
    using UI.ShopContainer.Card.Enum;

    [Serializable]
    public class ShopData
    {
        public ShopEntityItemType EntityType;
        public List<ShopCardItemButtonType> ButtonTypes = new ();
    }
}