using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/In App")]
public class InApp : ScriptableObject
{
    [SerializeField] private Sprite _preview;
    [SerializeField] private LanguageTextsSet _tittle;
    [SerializeField] private LanguageTextsSet _description;
    [SerializeField] private string _id;
    [SerializeField] private int _price;

    public Sprite Preview => _preview;

    public LanguageTextsSet Tittle => _tittle;

    public LanguageTextsSet Description => _description;

    public int Price => _price;

    public string Id => _id;
}