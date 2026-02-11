using TMPro;
using UnityEngine;
using YG;

namespace UI.Shop
{
    public class ButtonText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private LanguageTextsSet _textsSet;

        private void OnEnable()
        {
            YG2.onSwitchLang += OnSwitchLanguage;
            OnSwitchLanguage(YG2.lang);
        }

        private void OnDisable() =>
            YG2.onSwitchLang -= OnSwitchLanguage;

        public void SetText(LanguageTextsSet textsSet)
        {
            _textsSet = textsSet;
            OnSwitchLanguage(YG2.lang);
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