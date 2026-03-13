using System;
using System.Linq;
using EmojiChaos.Core.Abstract.MonoBehaviourWrapper;
using EmojiChaos.UI.Buttons;
using EmojiChaos.UtilsSpace.Static;
using UnityEngine;

namespace EmojiChaos.Lang
{
    public class LanguageSwitcher : InitializingBehaviour
    {
        private readonly string[] _languages = new[] { Constants.LangRu, Constants.LangEn, Constants.LangTr };

        [SerializeField] private PreviousLanguageButton _previousLanguageButton;
        [SerializeField] private NextLanguageButton _nextLanguageButton;

        private void OnDestroy()
        {
            if (_previousLanguageButton != null)
                _previousLanguageButton.Clicked -= OnPreviousLanguagedClicked;

            if (_nextLanguageButton != null)
                _nextLanguageButton.Clicked -= OnNextLanguagedClicked;
        }

        protected override void OnInitialize()
        {
            _previousLanguageButton.Initialize();
            _nextLanguageButton.Initialize();

            _previousLanguageButton.Clicked += OnPreviousLanguagedClicked;
            _nextLanguageButton.Clicked += OnNextLanguagedClicked;
        }

        private int GetCurrentLanguageIndex()
        {
            string currentLanguage = YandexGameConnector.Lang;

            if (_languages.Contains(currentLanguage) == false)
                throw new Exception($"The current language {currentLanguage} is not in the list");

            return Array.IndexOf(_languages, currentLanguage);
        }

        private void OnPreviousLanguagedClicked(PreviousLanguageButton button)
        {
            int index = GetCurrentLanguageIndex();
            index = (index - 1 + _languages.Length) % _languages.Length;
            YandexGameConnector.SwitchLanguage(_languages[index]);
        }

        private void OnNextLanguagedClicked(NextLanguageButton button)
        {
            int index = GetCurrentLanguageIndex();
            index = (index + 1) % _languages.Length;
            YandexGameConnector.SwitchLanguage(_languages[index]);
        }
    }
}