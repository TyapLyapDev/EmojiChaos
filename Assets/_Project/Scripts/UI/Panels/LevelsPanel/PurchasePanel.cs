using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PurchasePanel : PanelBase
{
    [SerializeField] private Image _preview;
    [SerializeField] private TMP_Text _tittle;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private LanguageSwitchHandlerWithParam _price;
    [SerializeField] private PurchaseApplierButton _purchaseButton;

    private InApp _info;

    private void OnEnable()
    {
        YG2.onSwitchLang += OnSwitchLanguage;
        OnSwitchLanguage(YG2.lang);

        _purchaseButton.Clicked += OnPurchaseClicked;
    }

    private void OnDisable()
    {
        YG2.onSwitchLang -= OnSwitchLanguage;
        _purchaseButton.Clicked -= OnPurchaseClicked;
    }

    public void SetInfo(InApp info)
    {
        _info = info;
        OnSwitchLanguage(YG2.lang);
        _preview.sprite = info.Preview;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _purchaseButton.Initialize();
    }

    private void OnSwitchLanguage(string lang)
    {
        SetText(_info.Tittle, lang, _tittle);
        SetText(_info.Description, lang, _description);
        _price.SetParam(_info.Price.ToString());
    }

    private void SetText(LanguageTextsSet textsSet, string lang, TMP_Text text)
    {
        LangParams langParams = textsSet.GetByLang(lang);

        text.font = langParams.Font;
        text.fontMaterial = langParams.Preset;
        text.text = langParams.Text;
    }

    private void OnPurchaseClicked(PurchaseApplierButton button)
    {
        YG2.BuyPayments(_info.Id);
    }
}