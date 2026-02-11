using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/ShopCardInfo")]
public class ShopCardInfos : ScriptableObject
{
    [SerializeField] private ShopEntityItemType _entityType;
    [SerializeField] private LanguageTextsSet _tabName;
    [SerializeField] private ShopCardInfo[] _cardInfos;

    public ShopEntityItemType EntityType => _entityType;

    public LanguageTextsSet TabName => _tabName;

    public IReadOnlyList<ShopCardInfo> CardInfos => _cardInfos.ToList();
}

[Serializable]
public class ShopCardInfo
{
    [SerializeField] private ShopCardItemButtonType _type;
    [SerializeField] private string _revardedAdvId;
    [SerializeField] private Sprite _preview;
    [SerializeField] private LanguageTextsSet _tittle;
    [SerializeField] private LanguageTextsSet _description;

    public ShopCardItemButtonType Type => _type;

    public Sprite Preview => _preview;

    public LanguageTextsSet Tittle => _tittle;

    public LanguageTextsSet Description => _description;

    public string RevardedAdvId => _revardedAdvId;
}