using TMPro;
using UnityEngine;
using YG;

#if UNITY_EDITOR
using YG.EditorScr;
#endif

using YG.Utils.Pay;

public class PurchasePanel : PanelBase
{
    [SerializeField] private ImageLoadYG purchaseImageLoad;
    [SerializeField] private ImageLoadYG currencyImageLoad;
    [SerializeField] private TMP_Text _tittle;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private PurchaseApplierButton _purchaseButton;

    private InApp _info;

    private void OnEnable()
    {
        YandexGameConnector.LangSwitched += OnSwitchLanguage;
        OnSwitchLanguage(YandexGameConnector.Lang);

        _purchaseButton.Clicked += OnPurchaseClicked;

        UpdateEntries(YG2.PurchaseByID(_info.Id));        
    }

    private void OnDisable()
    {
        YandexGameConnector.LangSwitched -= OnSwitchLanguage;
        _purchaseButton.Clicked -= OnPurchaseClicked;
    }

    public void SetInfo(InApp info)
    {
        _info = info;
        OnSwitchLanguage(YandexGameConnector.Lang);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _purchaseButton.Initialize();
    }

    private void UpdateEntries(Purchase data)
    {
        if (data == null)
        {
            Debug.LogError($"No product with ID found: {_info.Id}");
            return;
        }

        _price.text = data.priceValue;

        if (purchaseImageLoad)
        {
#if UNITY_EDITOR
            if (data.imageURI == InfoYG.DEMO_IMAGE)
                purchaseImageLoad.Load(ServerInfo.saveInfo.purchaseImage);
            else
                purchaseImageLoad.Load(data.imageURI);
#else
                purchaseImageLoad.Load(data.imageURI);
#endif
        }

        if (currencyImageLoad && data.currencyImageURL != string.Empty && data.currencyImageURL != null)
            currencyImageLoad.Load(data.currencyImageURL);
    }

    private void OnSwitchLanguage(string lang)
    {
        if (_info == null)
            return;

        SetText(_info.Tittle, lang, _tittle);
        SetText(_info.Description, lang, _description);
    }

    private void SetText(LanguageTextsSet textsSet, string lang, TMP_Text text)
    {
        LangParams langParams = textsSet.GetByLang(lang);

        text.font = langParams.Font;
        text.fontMaterial = langParams.Preset;
        text.text = langParams.Text;
    }

    private void OnPurchaseClicked(PurchaseApplierButton button) =>
        YandexGameConnector.BuyPayments(_info.Id);
}