using System;
using TMPro;
using UnityEngine;
using YG;

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
        YG2.onSwitchLang += SwitchLanguage;
        SwitchLanguage(YG2.lang);
    }

    private void OnDisable() =>
        YG2.onSwitchLang -= SwitchLanguage;

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