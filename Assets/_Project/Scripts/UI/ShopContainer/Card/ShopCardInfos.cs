using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Shop/CardInfos")]
public class ShopCardInfos : ScriptableObject
{
    [SerializeField] private ShopEntityItemType _entityType;
    [SerializeField] private LanguageTextsSet _tabName;
    [SerializeField] private ShopCardInfo[] _cardInfos;

    public ShopEntityItemType EntityType => _entityType;

    public LanguageTextsSet TabName => _tabName;

    public IReadOnlyList<ShopCardInfo> CardInfos => _cardInfos.ToList();
}