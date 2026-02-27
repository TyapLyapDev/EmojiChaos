using TMPro;
using UnityEngine;

namespace UI.Shop
{
    public class ButtonText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private LanguageTextsSet _textsSet;

        private void OnEnable()
        {
            YandexGameConnector.LangSwitched += OnSwitchLanguage;
            OnSwitchLanguage(YandexGameConnector.Lang);
        }

        private void OnDisable() =>
            YandexGameConnector.LangSwitched -= OnSwitchLanguage;

        public void SetText(LanguageTextsSet textsSet)
        {
            _textsSet = textsSet;
            OnSwitchLanguage(YandexGameConnector.Lang);
        }

        private void OnSwitchLanguage(string lang)
        {
            if (_textsSet == null)
                return;

            LangParams langParams = _textsSet.GetByLang(lang);
            _text.font = langParams.Font;
            _text.fontMaterial = langParams.Preset;
            _text.text = langParams.Text;
        }
    }
}