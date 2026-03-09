using System;
using System.Linq;
using UnityEngine;

namespace EmojiChaos.Lang
{
    using Core.Abstract.MonoBehaviourWrapper;
    using UI.Buttons;
    using Utils.Static;

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
                throw new Exception($"���� {currentLanguage} ����������� � ������ ���������");

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