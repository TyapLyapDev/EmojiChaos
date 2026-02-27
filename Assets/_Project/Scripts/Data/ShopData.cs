using System;
using System.Collections.Generic;

[Serializable]
public class ShopData
{
    public ShopEntityItemType EntityType;
    public List<ShopCardItemButtonType> ButtonTypes = new();
}