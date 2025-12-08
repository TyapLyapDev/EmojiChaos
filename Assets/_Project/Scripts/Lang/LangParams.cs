using System;
using TMPro;
using UnityEngine;

[Serializable]
public class LangParams
{
    [SerializeField, TextArea] private string _text = "текст";
    [SerializeField] private TMP_FontAsset _font;
    [SerializeField] private Material _preset;

    public string Text => _text;

    public TMP_FontAsset Font => _font;

    public Material Preset => _preset;
}