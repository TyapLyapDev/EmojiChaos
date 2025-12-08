using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LanguageSprite : MonoBehaviour
{
    [SerializeField] private Sprite _ru;
    [SerializeField] private Sprite _en;
    [SerializeField] private Sprite _tr;

    private Image _image;

    private void Awake()
    {
        if (TryGetComponent(out _image) == false)
            throw new NullReferenceException($"Object: {name}, NullComponent: {nameof(_image)}");
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
        _image.sprite = lang switch
        {
            Constants.LangRu => _ru,
            Constants.LangTr => _tr,
            _ => _en
        };
    }
}