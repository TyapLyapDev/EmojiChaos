using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Shop/TempCardInfo")]
public abstract class ShopCardInfo : ScriptableObject
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