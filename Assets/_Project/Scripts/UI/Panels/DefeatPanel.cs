using UnityEngine;

namespace EmojiChaos.UI.Panels
{
    using Audio;
    using Core.Abstract.UI;
    using Lang;

    public class DefeatPanel : PanelBase
    {
        [SerializeField] private LanguageSwitchHandlerWithParam _level;

        public void Activate(int level) =>
            _level.SetParam((level + 1).ToString());

        protected override void OnShow()
        {
            base.OnShow();
            Audio.Sfx.PlayFail();
        }
    }
}