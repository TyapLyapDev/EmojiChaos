using UnityEngine;

namespace EmojiChaos.Bootstraps
{
    using Audio;
    using ScriptableObect;
    using Services.Core;
    using Services.Save;
    using UI.Menu;
    using Utils.Static;

    public class MenuBootstrap : MonoBehaviour
    {
        [SerializeField] private MenuUIHandler _menuUIHandler;
        [SerializeField] private InApp _noAds;
        [SerializeField] private InApp _additionalSlot;

        private void Start()
        {
            YandexGameConnector.ConsumePurchases(_noAds, _additionalSlot);

            _menuUIHandler.Initialize(new MenuUiParam(
                new Saver(Utils.CalculateLevelCountInProject()),
                SceneLoader.Instance));

            Audio.Music.UnPause();
            Audio.Music.PlayMenu();
        }
    }
}