using System;
using UnityEngine;

namespace EmojiChaos.Lang
{
    using Utils.Static;

    [Serializable]
    public class LanguageTextsSet
    {
        [SerializeField] private LangParams _ru;
        [SerializeField] private LangParams _en;
        [SerializeField] private LangParams _tr;

        public LangParams GetByLang(string lang)
        {
            return lang switch
            {
                Constants.LangRu => _ru,
                Constants.LangTr => _tr,
                _ => _en
            };
        }
    }
}