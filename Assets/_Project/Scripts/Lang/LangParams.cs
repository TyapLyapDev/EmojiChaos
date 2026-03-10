using System;
using UnityEngine;
using TMPro;

namespace EmojiChaos.Lang
{
    [Serializable]
    public class LangParams
    {
        [SerializeField][TextArea] private string _text = "text";
        [SerializeField] private TMP_FontAsset _font;
        [SerializeField] private Material _preset;

        public string Text => _text;

        public TMP_FontAsset Font => _font;

        public Material Preset => _preset;
    }
}