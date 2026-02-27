using System;
using TMPro;
using UnityEngine;

public class LanguageSwitchHandler : MonoBehaviour
{
    [SerializeField] private LanguageTextsSet _texts;

    private TMP_Text _text;

    private void Awake()
    {
        if (TryGetComponent(out _text) == false)
            throw new NullReferenceException($"Object: {name}, NullComponent: {nameof(_text)}");
    }

    private void OnEnable()
    {
        YandexGameConnector.LangSwitched += OnSwitchLanguage;
        OnSwitchLanguage(YandexGameConnector.Lang);
    }

    private void OnDisable() =>
        YandexGameConnector.LangSwitched -= OnSwitchLanguage;

    private void OnSwitchLanguage(string lang)
    {
        LangParams langParams = _texts.GetByLang(lang);

        _text.font = langParams.Font;
        _text.fontMaterial = langParams.Preset;
        _text.text = langParams.Text;
    }
}