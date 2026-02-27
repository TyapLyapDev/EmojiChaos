using System;
using TMPro;
using UnityEngine;

public class LanguageSwitchHandlerWithParam : MonoBehaviour
{
    [SerializeField] private LanguageTextsSet _texts;

    private TMP_Text _text;
    private string _param = "0";

    private void Awake()
    {
        if (TryGetComponent(out _text) == false)
            throw new NullReferenceException($"Object: {name}, NullComponent: {nameof(_text)}");
    }

    private void OnEnable()
    {
        YandexGameConnector.LangSwitched += SwitchLanguage;
        SwitchLanguage(YandexGameConnector.Lang);
    }

    private void OnDisable() =>
        YandexGameConnector.LangSwitched -= SwitchLanguage;

    public void SetParam(string param) =>
        _param = param;

    private void SwitchLanguage(string lang)
    {
        LangParams langParams = _texts.GetByLang(lang);

        _text.font = langParams.Font;
        _text.fontMaterial = langParams.Preset;
        _text.text = string.Format(langParams.Text, _param);
    }
}