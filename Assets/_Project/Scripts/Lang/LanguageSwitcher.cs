using System;
using System.Linq;
using UnityEngine;
using YG;

public class LanguageSwitcher : InitializingBehaviour
{
    [SerializeField] private PreviousLanguageButton _previousLanguageButton;
    [SerializeField] private NextLanguageButton _nextLanguageButton;
    
    private readonly string[] _languages = new[] { Constants.LangRu, Constants.LangEn, Constants.LangTr };

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
        string currentLanguage = YG2.lang;

        if (_languages.Contains(currentLanguage) == false)
            throw new Exception($"язык {currentLanguage} отсутствует в списке доступных");

        return Array.IndexOf(_languages, currentLanguage);
    }

    private void OnPreviousLanguagedClicked(PreviousLanguageButton _)
    {
        int index = GetCurrentLanguageIndex();
        index = (index - 1 + _languages.Length) % _languages.Length;
        YG2.SwitchLanguage(_languages[index]);
    }

    private void OnNextLanguagedClicked(NextLanguageButton _)
    {
        int index = GetCurrentLanguageIndex();
        index = (index + 1) % _languages.Length;
        YG2.SwitchLanguage(_languages[index]);
    }
}