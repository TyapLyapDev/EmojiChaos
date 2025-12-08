using System;
using TMPro;
using UnityEngine;
using YG;

public class LanguageText : MonoBehaviour
{
    [SerializeField] private LangParams _ru;
    [SerializeField] private LangParams _en;
    [SerializeField] private LangParams _tr;

    private TMP_Text _text;

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

    private void SwitchLanguage(string lang)
    {
        LangParams langParams = lang switch
        {
            Constants.LangRu => _ru,
            Constants.LangTr => _tr,
            _ => _en
        };

        _text.font = langParams.Font;
        _text.fontMaterial = langParams.Preset;
        _text.text = langParams.Text;
    }
}