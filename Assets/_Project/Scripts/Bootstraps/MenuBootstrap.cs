using EmojiChaos.AudioSpace;
using EmojiChaos.ScriptableObect;
using EmojiChaos.Services.Core;
using EmojiChaos.Services.Save;
using EmojiChaos.UI.Menu;
using EmojiChaos.UtilsSpace.Static;
using UnityEngine;

namespace EmojiChaos.Bootstraps
{
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