using System;
using System.Collections.Generic;
using EmojiChaos.UI.ShopContainer.Card.Enum;

namespace EmojiChaos.Data
{
    [Serializable]
    public class ShopData
    {
        public ShopEntityItemType EntityType;
        public List<ShopCardItemButtonType> ButtonTypes = new ();
    }
}