using System;
using TMPro;
using UnityEngine;
using YG;

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
        YG2.onSwitchLang += OnSwitchLanguage;
        OnSwitchLanguage(YG2.lang);
    }

    private void OnDisable() =>
        YG2.onSwitchLang -= OnSwitchLanguage;

    private void OnSwitchLanguage(string lang)
    {
        LangParams langParams = _texts.GetByLang(lang);

        _text.font = langParams.Font;
        _text.fontMaterial = langParams.Preset;
        _text.text = langParams.Text;
    }
}