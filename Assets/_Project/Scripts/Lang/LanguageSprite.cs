using System;
using UnityEngine;
using UnityEngine.UI;

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
        YandexGameConnector.LangSwitched += SwitchLanguage;
        SwitchLanguage(YandexGameConnector.Lang);
    }

    private void OnDisable() =>
        YandexGameConnector.LangSwitched -= SwitchLanguage;

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